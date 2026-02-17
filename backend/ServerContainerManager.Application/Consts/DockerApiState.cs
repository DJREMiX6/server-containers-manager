namespace ServerContainerManager.Application.Consts
{
    public static class DockerApiState
    {
        public const string Created = "created";
        public const string Running = "running";
        public const string Paused = "paused";
        public const string Restarting = "restarting";
        public const string Exited = "exited";
        public const string Removing = "removing";
        public const string Dead = "dead";
    }
}
