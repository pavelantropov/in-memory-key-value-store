namespace KeyValueStore.Host.Extensions;

public static class StringExtensions
{
    public static bool IsCommand(this string message, Command command)
    {
        return message.Equals(nameof(Command.Exit), StringComparison.OrdinalIgnoreCase);
    }
}
