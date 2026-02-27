using KeyValueStore.Host.Enums;

namespace KeyValueStore.Host.Extensions;

public static class StringExtensions
{
    extension(string message)
    {
        public bool IsCommand(out Command command)
        {
            return Enum.TryParse(message, true, out command);
        }

        public bool IsCommand(Command command)
        {
            return message.Equals(command.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
