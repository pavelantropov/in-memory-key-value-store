using KeyValueStore.Host.Enums;

namespace KeyValueStore.Host.Extensions;

public static class StringExtensions
{
    extension(string message)
    {
        public bool IsCommand(out Command command)
        {
            var isCommand = Enum.TryParse(message, true, out command);
            return isCommand && command != Command.None;
        }

        public bool IsCommand(Command command)
        {
            return message.Equals(command.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
