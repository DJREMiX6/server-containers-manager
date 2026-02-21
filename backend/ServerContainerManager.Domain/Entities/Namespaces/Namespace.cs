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

        public static Namespace Create(string name)
        {
            return new Namespace(Guid.NewGuid(), name);
        }
    }
}
