using ServerContainerManager.Shared.Utils.Enums;

namespace ServerContainerManager.Shared.Utils
{
    public sealed record Actor
    {
        public Guid? Id { get; private set; }
        public ActorType ActorType { get; private set; }

        private Actor() { } //EF

        private Actor(Guid? id, ActorType actorType)
        {
            Id = id;
            ActorType = actorType;
        }

        public static Actor FromUser(Guid userId)
        {
            if(userId == Guid.Empty)
                throw new ArgumentException("Invalid empty User Id.", nameof(userId));

            return new Actor(userId, ActorType.User);
        }

        public static Actor System() => new (null, ActorType.System); 
    }
}
