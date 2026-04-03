using ErrorOr;

namespace ServerContainerManager.Domain.Entities.Containers.ValueObjects
{
    public sealed record Label
    {
        public string Key { get; private set; }
        public string Value { get; private set; }

        private Label() { } // EF

        private Label(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public static ErrorOr<Label> Create(string key, string value)
        {
            var trimmedKey = key.Trim();
            var trimmedValue = value.Trim();

            var errors = new List<Error>();

            if (string.IsNullOrEmpty(trimmedKey))
                errors.Add(Error.Validation($"{nameof(Label)}.{nameof(Create)}", "Key cannot be null or empty"));

            if (string.IsNullOrEmpty(trimmedValue))
                errors.Add(Error.Validation($"{nameof(Label)}.{nameof(Create)}", "Value cannot be null or empty"));

            return new Label(trimmedKey, trimmedValue);
        }
    }
}
