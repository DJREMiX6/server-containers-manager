using ErrorOr;
using ServerContainerManager.Domain.Entities.Containers.Enums;
using ServerContainerManager.Domain.Entities.Containers.ValueObjects;
using ServerContainerManager.Domain.Entities.Namespaces;
using System.Text.RegularExpressions;

namespace ServerContainerManager.Domain.Entities.Containers
{
    public sealed class Container
    {
        private static readonly Regex DockerContainerIdCharactersRegex = new ("[a-f0-9]", RegexOptions.Compiled);

        private List<Label> _labels = [];
        private List<Port> _ports = [];
        private List<Namespace> _namespaces = [];

        public string Id { get; private set; }
        public string Name { get; private set; }
        public ContainerState State { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public IReadOnlyCollection<Label> Labels => _labels;
        public IReadOnlyCollection<Port> Ports => _ports;
        public IReadOnlyCollection<Namespace> Namespaces => _namespaces;

        private Container() { } //EF

        private Container(
            string id,
            string name,
            ContainerState state,
            DateTime createdAt,
            List<Label> labels,
            List<Port> ports,
            List<Namespace> namespaces)
        {
            Id = id;
            Name = name;
            State = state;
            CreatedAt = createdAt;
            _labels = [.. labels];
            _ports = [.. ports];
            _namespaces = [.. namespaces];
        }

        public static ErrorOr<Container> Create(
            string dockerId,
            string name,
            ContainerState state,
            DateTime createdAt,
            List<Label> labels,
            List<Port> ports,
            List<Namespace> namespaces)
        {
            var trimmedDockerId = dockerId.Trim();
            var trimmedName = name.Trim().TrimStart('/'); // Container's names starts with '/', the TrimStart('/') removes it

            var errors = new List<Error>();

            var dockerIdValidationResult = ValidateDockerId(trimmedDockerId);
            if(dockerIdValidationResult.IsError)
                errors.AddRange(dockerIdValidationResult.Errors);

            if (string.IsNullOrEmpty(trimmedName) || trimmedName.Length < 3)
                errors.Add(Error.Validation($"{nameof(Container)}.{nameof(Create)}", "Name must be at least 3 characters long."));

            if(!Enum.IsDefined(state))
                errors.Add(Error.Validation($"{nameof(Container)}.{nameof(Create)}", "State must be a valid container state."));

            if (errors.Count > 0)
                return errors;

            return new Container(trimmedDockerId, trimmedName, state, createdAt, labels, ports, namespaces);
        }

        private static ErrorOr<Success> ValidateDockerId(string dockerId)
        {
            if (string.IsNullOrWhiteSpace(dockerId))
                return Error.Validation($"{nameof(Container)}.{nameof(ValidateDockerId)}", "Docker container ID cannot be null or empty.");
            if (dockerId.Length != 64)
                return Error.Validation($"{nameof(Container)}.{nameof(ValidateDockerId)}", "Docker container ID must be 64 characters long.");
            if(!DockerContainerIdCharactersRegex.IsMatch(dockerId))
                return Error.Validation($"{nameof(Container)}.{nameof(ValidateDockerId)}", "Invalid Docker container ID format.");

            return Result.Success;
        }

        public ErrorOr<Success> Rename(string name)
        {
            name = name.Trim()[1..];

            if (string.IsNullOrEmpty(name) || name.Length < 3)
                return Error.Validation($"{nameof(Container)}.{nameof(Rename)}", "Name must be at least 3 characters long.");

            Name = name;
            return Result.Success;
        }

        public ErrorOr<Success> UpdateState(ContainerState state)
        {
            if (!Enum.IsDefined(state))
                return Error.Validation($"{nameof(Container)}.{nameof(UpdateState)}", "State must be a valid container state.");

            State = state;
            return Result.Success;
        }

        public ErrorOr<Success> UpdateLabels(IList<Label> labels)
        {
            _labels = [.. labels];
            return Result.Success;
        }

        public ErrorOr<Success> UpdatePorts(IList<Port> ports)
        {
            _ports = [.. ports];
            return Result.Success;
        }

        public ErrorOr<Success> AssignNamespaces(List<Namespace> namespaces)
        {
            if (namespaces.Count == 0) return Error.Validation($"{nameof(Container)}.{nameof(AssignNamespaces)}", "Namespaces list cannot be empty");

            namespaces.RemoveAll(n => _namespaces.Contains(n));

            _namespaces.AddRange(namespaces);

            return Result.Success;
        }

        public ErrorOr<Success> UnassignNamespaces(List<Namespace> namespaces)
        {
            if(namespaces.Count == 0) return Error.Validation($"{nameof(Container)}.{nameof(UnassignNamespaces)}", "Namespaces list cannot be empty");

            var errors = namespaces
                .Where(n => !_namespaces.Contains(n))
                .Select(n => Error.Validation($"{nameof(Container)}.{nameof(UnassignNamespaces)}", $"Namespace {n.Name} is not assigned to current Container"))
                .ToList();
            if (errors.Count > 0) return errors;

            _namespaces.RemoveAll(namespaces.Contains);

            return Result.Success;
        }
    }
}
