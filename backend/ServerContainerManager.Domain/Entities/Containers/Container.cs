using ErrorOr;
using ServerContainerManager.Domain.Entities.Containers.Enums;
using ServerContainerManager.Domain.Entities.Containers.ValueObjects;
using ServerContainerManager.Domain.Entities.Namespaces;
using ServerContainerManager.Shared.Utils;
using System.Text.RegularExpressions;

namespace ServerContainerManager.Domain.Entities.Containers
{
    public sealed class Container : AuditableEntity<string>
    {
        private static readonly Regex DockerContainerIdCharactersRegex = new ("[a-f0-9]", RegexOptions.Compiled);

        private List<Label> _labels = [];
        private List<Port> _ports = [];
        private List<Namespace> _namespaces = [];

        public string Id { get; private set; }
        public string Name { get; private set; }
        public ContainerState State { get; private set; }
        public IReadOnlyCollection<Label> Labels => _labels;
        public IReadOnlyCollection<Port> Ports => _ports;
        public IReadOnlyCollection<Namespace> Namespaces => _namespaces;

        private Container() : base(Actor.System(), DateTime.Now) { } //EF

        private Container(
            string id,
            string name,
            ContainerState state,
            List<Label> labels,
            List<Port> ports,
            List<Namespace> namespaces,
            Actor actor,
            DateTime now,
            DateTime? createdAt) : base(actor, createdAt ?? now)
        {
            Id = id;
            Name = name;
            State = state;
            _labels = [.. labels];
            _ports = [.. ports];
            _namespaces = [.. namespaces];
        }

        public static ErrorOr<Container> Create(
            string dockerId,
            string name,
            ContainerState state,
            List<Label> labels,
            List<Port> ports,
            List<Namespace> namespaces,
            Actor actor,
            DateTime now,
            DateTime? createdAt = null)
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

            return new Container(trimmedDockerId, trimmedName, state, labels, ports, namespaces, actor, now, createdAt);
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

        public ErrorOr<Success> Start(Actor actor, DateTime now)
        {
            return UpdateState(ContainerState.Running, actor, now);
        }

        public ErrorOr<Success> Rename(string name, Actor actor, DateTime now)
        {
            name = name.Trim()[1..];

            if (string.IsNullOrEmpty(name) || name.Length < 3)
                return Error.Validation($"{nameof(Container)}.{nameof(Rename)}", "Name must be at least 3 characters long.");

            if(name == Name)
                return Result.Success;

            Name = name;

            Touch(actor, now);
            return Result.Success;
        }

        public ErrorOr<Success> UpdateState(ContainerState state, Actor actor, DateTime now)
        {
            if (!Enum.IsDefined(state))
                return Error.Validation($"{nameof(Container)}.{nameof(UpdateState)}", "State must be a valid container state.");

            if(state == State)
                return Result.Success;

            State = state;

            Touch(actor, now);
            return Result.Success;
        }

        public ErrorOr<Success> UpdateLabels(IList<Label> labels, Actor actor, DateTime now)
        {
            _labels = [.. labels];

            Touch(actor, now);
            return Result.Success;
        }

        public ErrorOr<Success> UpdatePorts(IList<Port> ports, Actor actor, DateTime now)
        {
            _ports = [.. ports];

            Touch(actor, now);
            return Result.Success;
        }

        public ErrorOr<Success> UpdateNamespaces(IList<Namespace> namespaces, Actor actor, DateTime now)
        {
            if (namespaces.Count == 0) return Error.Validation($"{nameof(Container)}.{nameof(UpdateNamespaces)}", "Namespaces list cannot be empty");

            var namespacesSet = new HashSet<Namespace>(namespaces);

            _namespaces = [.. namespacesSet];

            Touch(actor, now);
            return Result.Success;
        }
    }
}
