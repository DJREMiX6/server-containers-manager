using ErrorOr;
using ServerContainerManager.Domain.Entities.Namespaces;

namespace ServerContainerManager.Domain.Entities.Containers
{
    public sealed class Container
    {
        private List<Namespace> _namespaces = [];

        public string Id { get; private set; }
        public IReadOnlyCollection<Namespace> Namespaces => _namespaces;

        private Container() { } //EF

        private Container(string id, List<Namespace> namespaces)
        {
            Id = id;
            _namespaces = namespaces;
        }

        public static ErrorOr<Container> Create(string dockerId, List<Namespace> namespaces) 
        {
            if (string.IsNullOrWhiteSpace(dockerId))
                return Error.Validation($"{nameof(Container)}.{nameof(Create)}", "Docker container ID is required.");

            return new Container(dockerId, namespaces);
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
