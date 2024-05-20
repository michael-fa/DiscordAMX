using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

public class Command : BaseCommandModule
{
    public string Name { get; set; }
    public List<string> Arguments { get; set; }
    public string Callback { get; set; }
    public string Description { get; set; }

    public Command(string name, List<string> arguments, string description, string callbck)
    {
        Name = name;
        Arguments = arguments;
        Callback = callbck;
        Description = description;
    }

    // Override the AfterExecutionAsync method
    public override async Task AfterExecutionAsync(CommandContext ctx)
    {
        // Custom logic after command execution
        await ctx.Channel.SendMessageAsync($"Command '{ctx.Command.Name}' executed.");
    }

    // You can also override BeforeExecutionAsync if you need pre-execution logic
    public override async Task BeforeExecutionAsync(CommandContext ctx)
    {
        // Custom logic before command execution
        await ctx.Channel.SendMessageAsync($"Command '{ctx.Command.Name}' is about to be executed.");
    }
}
