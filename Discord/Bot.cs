using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;

namespace discordamx.Discord
{



    public static class Bot
    {
        public static DiscordClient Client { get; private set; }
        //public static InteractivityExtension Interactivity { get; private set; }
        //public static CommandsNextExtension Commands { get; private set; }
        


        public static async Task RunAsync(DiscordConfiguration dConfig)
        {


            //Try to create a new discord client, this is in the scope of the DC+' code.. 
            try
            {
                Client = new DiscordClient(dConfig);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.Source + "\n" + ex.StackTrace);
                Program.m_Logger.Exception(ex);
                Program.StopEverything();
            }


            //Liten to all the Discord Events
            
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
            


            /*var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { "?" },
                EnableMentionPrefix = true,
             
                EnableDms = true
            };
            Commands = Client.UseCommandsNext(commandsConfig);
            */
            Client.UseInteractivity(new InteractivityConfiguration
            {

            });


            //Finally, connect the bot. Also, in the scope of DC+' code.
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
