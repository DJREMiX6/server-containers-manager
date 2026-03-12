using ErrorOr;

namespace ServerContainerManager.Domain.Entities.Namespaces
{
    public sealed class Namespace
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        private Namespace() { } // EF

        private Namespace(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public static ErrorOr<Namespace> Create(string name)
        {
            if (string.IsNullOrEmpty(name) || name.Length < 3)
                return Error.Validation($"{nameof(Namespace)}.{nameof(Create)}", "Namespace name must be at least 3 characters long");

            return new Namespace(Guid.NewGuid(), name);
        }
    }
}
