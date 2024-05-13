using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

public class CommandManager
{
    private Dictionary<string, Command> _commands = new Dictionary<string, Command>();
    private int _nextCommandId = 0;

    public int RegisterCommand(string name, List<string> arguments, string description, string callback)
    {
        var command = new Command(name, arguments, description, callback);

        _commands.Add(name, command);

        return _nextCommandId++;
    }

    public void SetCommandDescription(string name, string description)
    {
        if (_commands.ContainsKey(name))
        {
            _commands[name].Description = description;
        }
    }

    // Additional methods for unregistering, modifying, and executing commands could be added here
}
