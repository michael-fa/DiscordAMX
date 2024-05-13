public class Command
{
    public string Name { get; set; }
    public List<string> Arguments { get; set; }
    public string Description { get; set; }
    public string Callback { get; set; }

    public Command(string name, List<string> arguments, string description, string callbck)
    {
        Name = name;
        Arguments = arguments;
        Description = description;
        Callback = callbck;
    }
}
