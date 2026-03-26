using ErrorOr;
using ServerContainerManager.Domain.Entities.Namespaces.Errors;

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
            name = name.Trim();

            if (string.IsNullOrEmpty(name) || name.Length < 3)
                return NamespaceValidationErrors.NameTooShort();

            return new Namespace(Guid.NewGuid(), name);
        }
    }
}
