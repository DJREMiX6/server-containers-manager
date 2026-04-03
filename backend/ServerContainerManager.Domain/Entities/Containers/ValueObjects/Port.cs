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
            if (publicPort == 0)
                return Error.Validation($"", "public port cannot be zero");

            if (privatePort == 0)
                return Error.Validation($"", "private port cannot be zero");

            return new Port(publicPort, privatePort);
        }
    }
}
