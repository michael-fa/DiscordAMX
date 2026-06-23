using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.VoiceNext;

namespace discordamx.Discord
{
    public static class Bot
    {
        public static DiscordClient Client { get; private set; }
        public static InteractivityExtension Interactivity { get; private set; }
        public static CommandsNextExtension Commands { get; private set; }
        public static VoiceNextExtension Voice { get; private set; }

        public static async Task RunAsync(DiscordConfiguration dConfig)
        {
            try
            {
                Client = new DiscordClient(dConfig);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.Source + "\n" + ex.StackTrace);
                Program.m_Logger.Exception(ex);
                Program.StopEverything();
                return;
            }

            // Discord Events
            Client.GuildDownloadCompleted += Events.GuildActions.DownloadCompleted;
            Client.Heartbeated += Events.OnHeartbeated.Execute;
            Client.GuildMemberAdded += Events.MemberJoinLeave.Join;
            Client.GuildMemberRemoved += Events.MemberJoinLeave.Leave;
            Client.MessageCreated += Events.MessageActions.MessageAdded;
            Client.MessageUpdated += Events.MessageActions.MessageUpdated;
            Client.MessageDeleted += Events.MessageActions.MessageDeleted;
            Client.MessageReactionAdded += Events.MessageActions.ReactionAdded;
            Client.MessageReactionRemoved += Events.MessageActions.ReactionRemoved;
            Client.GuildCreated += Events.GuildActions.GuildAdded;
            Client.GuildDeleted += Events.GuildActions.GuildRemoved;
            Client.GuildUpdated += Events.GuildActions.GuildUpdated;
            Client.ChannelCreated += Events.GuildActions.ChannelCreated;
            Client.ChannelDeleted += Events.GuildActions.ChannelDeleted;
            Client.ChannelUpdated += Events.GuildActions.ChannelUpdated;
            Client.ThreadCreated += Events.GuildActions.ThreadCreated;
            Client.ThreadDeleted += Events.GuildActions.ThreadDeleted;
            Client.GuildMemberUpdated += Events.GuildActions.UserUpdated;
            Client.ThreadUpdated += Events.GuildActions.ThreadUpdated;
            Client.ThreadMembersUpdated += Events.GuildActions.ThreadMembersUpdated;

            // CommandsNext
            var cmds = new CommandsNextConfiguration
            {
                StringPrefixes = new[] { "?" },
                EnableMentionPrefix = true,
                EnableDms = true
            };
            Commands = Client.UseCommandsNext(cmds);

            // Interactivity (requires NuGet: DSharpPlus.Interactivity)
            Interactivity = Client.UseInteractivity(new InteractivityConfiguration());

            // VoiceNext
            Voice = Client.UseVoiceNext(new VoiceNextConfiguration
            {
                EnableIncoming = false
            });

            try
            {
                await Client.ConnectAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.Source + "\n" + ex.StackTrace + ex.InnerException);
                Program.m_Logger.Exception(ex);
                Program.StopEverything();
            }
        }

        public static async Task DisconnectAsync()
        {
            await Client.DisconnectAsync();
        }
    }
}
