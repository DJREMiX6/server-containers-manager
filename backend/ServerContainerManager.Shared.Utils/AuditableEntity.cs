namespace ServerContainerManager.Shared.Utils
{
    public abstract class AuditableEntity<TId> : Entity<TId>
    {
        public AuditInfo Created { get; private set; }
        public AuditInfo Updated { get; private set; }

        protected AuditableEntity(Actor actor, DateTime dateTime)
        {
            var auditInfo = new AuditInfo()
            {
                At = dateTime,
                By = actor,
            };

            Created = auditInfo;
            Updated = auditInfo;
        }

        protected void Touch(Actor actor, DateTime dateTime)
        {
            var auditInfo = new AuditInfo() 
            { 
                At = dateTime, 
                By = actor 
            };

            Updated = auditInfo;
        }
    }
}
