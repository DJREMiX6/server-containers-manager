using ErrorOr;
using ServerContainerManager.Domain.Entities.Containers.Enums;
using ServerContainerManager.Domain.Entities.Containers.Errors;
using ServerContainerManager.Domain.Entities.Containers.ValueObjects;
using ServerContainerManager.Domain.Entities.Namespaces;
using ServerContainerManager.Shared.Utils;
using ServerContainerManager.Shared.Utils.Enums;
using ServerContainerManager.Shared.Utils.Errors;
using System.Text.RegularExpressions;

namespace ServerContainerManager.Domain.Entities.Containers
{
    public sealed class Container : AuditableEntity<string>
    {
        private static readonly Regex DockerContainerIdCharactersRegex = new ("[a-f0-9]", RegexOptions.Compiled);

        private List<Label> _labels = [];
        private List<Port> _ports = [];
        private List<Namespace> _namespaces = [];

        public string Name { get; private set; }
        public ContainerState State { get; private set; }
        public IReadOnlyCollection<Label> Labels => _labels;
        public IReadOnlyCollection<Port> Ports => _ports;
        public IReadOnlyCollection<Namespace> Namespaces => _namespaces;

        private Container() : base(Actor.System(), DateTime.Now) { } //EF

        private Container(
            string id,
            string name,
            ContainerState state,
            List<Label> labels,
            List<Port> ports,
            List<Namespace> namespaces,
            Actor actor,
            DateTime createdAt) : base(actor, createdAt)
        {
            Id = id;
            Name = name;
            State = state;
            _labels = [.. labels];
            _ports = [.. ports];
            _namespaces = [.. namespaces];
        }

        public static ErrorOr<Container> Create(
            string dockerId,
            string name,
            ContainerState state,
            List<Label> labels,
            List<Port> ports,
            List<Namespace> namespaces,
            Actor actor,
            DateTime now,
            DateTime? createdAt = null)
        {
            dockerId = dockerId.Trim();
            name = name.Trim().TrimStart('/'); // Container's names starts with '/', the TrimStart('/') removes it

            var errors = new List<Error>();

            var dockerIdValidationResult = ValidateDockerId(dockerId);
            if(dockerIdValidationResult.IsError)
                errors.AddRange(dockerIdValidationResult.Errors);

            if (string.IsNullOrEmpty((string)name) || name.Length < 3)
                errors.Add(ContainerValidationErrors.NameTooShort());

            if(!Enum.IsDefined(state))
                errors.Add(ContainerValidationErrors.InvalidState());

            if (errors.Count > 0)
                return errors;

            return new Container(dockerId, name, state, labels, ports, namespaces, actor, createdAt ?? now);
        }

        private static ErrorOr<Success> ValidateDockerId(string dockerId)
        {
            if (string.IsNullOrWhiteSpace(dockerId))
                return ContainerValidationErrors.NullOrEmptyId();
            if (dockerId.Length != 64)
                return ContainerValidationErrors.InvalidIdLength();
            if (!DockerContainerIdCharactersRegex.IsMatch(dockerId))
                return ContainerValidationErrors.InvalidIdFormat();

            return Result.Success;
        }

        public ErrorOr<Success> Start(Actor actor, DateTime now)
        {
            if (State == ContainerState.Running || State == ContainerState.Paused || State == ContainerState.Restarting)
                return ContainerErrors.AlreadyRunning(Id);

            if (State == ContainerState.Removing)
                return ContainerErrors.Removing(Id);

            return UpdateState(ContainerState.Running, actor, now);
        }

        public ErrorOr<Success> Stop(Actor actor, DateTime now)
        {
            if (State != ContainerState.Running || State != ContainerState.Paused)
                return ContainerErrors.NotRunning(Id);

            return UpdateState(ContainerState.Exited, actor, now);
        }

        public ErrorOr<Success> Restart(Actor actor, DateTime now)
        {
            if (State != ContainerState.Running)
                return ContainerErrors.NotRunning(Id);

            return UpdateState(ContainerState.Restarting, actor, now);
        }

        public ErrorOr<Success> Pause(Actor actor, DateTime now)
        {
            if (State != ContainerState.Running)
                return ContainerErrors.NotRunning(Id);

            return UpdateState(ContainerState.Paused, actor, now);
        }

        public ErrorOr<Success> Resume(Actor actor, DateTime now)
        {
            if (State != ContainerState.Paused)
                ContainerErrors.NotPaused(Id);

            return UpdateState(ContainerState.Running, actor, now);
        }

        public ErrorOr<Success> Kill(Actor actor, DateTime now)
        {
            if (State != ContainerState.Running && State != ContainerState.Paused)
                return ContainerErrors.NotRunning(Id);

            return UpdateState(ContainerState.Exited, actor, now);
        }

        public ErrorOr<Success> Rename(string name, Actor actor, DateTime now)
        {
            name = name.Trim()[1..];

            if (string.IsNullOrEmpty(name) || name.Length < 3)
                return ContainerValidationErrors.NameTooShort();

            if(name == Name)
                return Result.Success;

            Name = name;

            Touch(actor, now);
            return Result.Success;
        }
        
        public ErrorOr<Success> UpdateState(ContainerState state, Actor actor, DateTime now)
        {
            if (!Enum.IsDefined(state))
                return ContainerValidationErrors.InvalidState();

            if(state == State)
                return Result.Success;
            
            /* This enables Auditing for restarting since when restaring a container it passes through two different states:
             * - Restarting
             * - Running/Exited (based on the result)
             * Without this check the second state would override the UpdatedBy with a System actor since it will come from the Reconciliator.
             */
            if (State == ContainerState.Restarting && Updated.By.ActorType == ActorType.User)  
                actor = Updated.By;

            State = state;

            Touch(actor, now);
            return Result.Success;
        }

        public ErrorOr<Success> UpdateLabels(IList<Label> labels, Actor actor, DateTime now)
        {
            _labels = [.. labels];

            Touch(actor, now);
            return Result.Success;
        }

        public ErrorOr<Success> UpdatePorts(IList<Port> ports, Actor actor, DateTime now)
        {
            _ports = [.. ports];

            Touch(actor, now);
            return Result.Success;
        }

        public ErrorOr<Success> UpdateNamespaces(IList<Namespace> namespaces, Actor actor, DateTime now)
        {
            if (namespaces.Count == 0) return ContainerValidationErrors.EmptyNamespaces();

            var namespacesSet = new HashSet<Namespace>(namespaces);

            _namespaces = [.. namespacesSet];

            Touch(actor, now);
            return Result.Success;
        }
    }
}
