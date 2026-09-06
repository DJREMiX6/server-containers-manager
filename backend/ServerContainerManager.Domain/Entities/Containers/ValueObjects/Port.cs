using ErrorOr;

namespace ServerContainerManager.Domain.Entities.Containers.ValueObjects
{
    public sealed record Port
    {
        public ushort Public { get; private set; }
        public ushort Private { get; private set; }

        private Port() { }

        private Port(ushort publicPort, ushort privatePort)
        {
            Public = publicPort;
            Private = privatePort;
        }

        public static ErrorOr<Port> Create(ushort publicPort, ushort privatePort)
        {
            if (publicPort == null)
                throw new ArgumentNullException(nameof(publicPort));

            if (privatePort == null)
                throw new ArgumentNullException(nameof(privatePort));

            return new Port(publicPort, privatePort);
        }
    }
}
