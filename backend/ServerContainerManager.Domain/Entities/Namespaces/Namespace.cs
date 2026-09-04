using ErrorOr;
using ServerContainerManager.Domain.Entities.Auth;
using ServerContainerManager.Domain.Entities.Containers;
using ServerContainerManager.Domain.Entities.Namespaces.Errors;

namespace ServerContainerManager.Domain.Entities.Namespaces
{
    public sealed class Namespace
    {
        private List<AppUser> _associatedUsers = [];
        private List<Container> _associatedContainers = [];

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public IReadOnlyList<AppUser> AssociatedUsers => _associatedUsers;
        public IReadOnlyList<Container> AssociatedContainers => _associatedContainers;

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

        public ErrorOr<Success> UpdateAssociatedUsers(ICollection<AppUser> associatedUsers)
        {
            _associatedUsers = [.. associatedUsers];
            return Result.Success;
        }

        public ErrorOr<Success> UpdateAssociatedContainers(ICollection<Container> associatedContainers)
        {
            _associatedContainers = [.. associatedContainers];
            return Result.Success;
        }
    }
}
