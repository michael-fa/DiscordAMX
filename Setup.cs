using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMXWrapperCore;
using System.Threading.Tasks;
using System.Diagnostics;
using DSharpPlus;
using System.Runtime.InteropServices;
using discordamx.Discord;
using DSharpPlus.Entities;
using discordamx.Utils;
using discordamx.Scripting;
using Microsoft.VisualBasic;
using System.IO;
using static System.Collections.Specialized.BitVector32;
using System.IO.Pipes;
using discordamx.Plugin;

namespace discordamx
{
    internal static partial class Program
    {
        //Coreinfo
        static bool m_isWindows;
        static bool m_isLinux;

        static void Setup(string[] args)
        {
            if (m_Setup) return;



            Program.m_ScriptGuilds = new List<Scripting.Guild>();
            
            m_DmUsers = new List<DiscordChannel>();
            Program.m_Embeds = new List<Scripting.DiscordEmbedBuilder>();

            Console.ForegroundColor = ConsoleColor.Yellow;
#if DEBUG

            Log.Warning(" RUNNING IN DEBUG MODE with user " + Environment.UserName);
#endif
            Console.ForegroundColor = ConsoleColor.White;

            Log.Info(" -> DiscordAMX BETA 2 © 2024 - www.fanter.eu <-");
           

            //Environment - Set the OS
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) m_isWindows = true;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) m_isLinux = true;
            else StopEverything();

            if (m_isWindows) Log.Info("INIT: -> Running on Windows.");
            else if (m_isLinux) Log.Info("INIT: Running on Linux. (Make sure you are always up to date!");

            //Handle commands.
            Console.CancelKeyPress += delegate {
                StopEverything();
                return;
            };

            Program.dConfig = new DiscordConfiguration()
            {
                TokenType = TokenType.Bot,
                AlwaysCacheMembers = false,
                MessageCacheSize = 4065,
                Intents = DiscordIntents.DirectMessageReactions
             | DiscordIntents.DirectMessages
             | DiscordIntents.GuildMessageReactions
             | DiscordIntents.MessageContents
             | DiscordIntents.GuildBans
             | DiscordIntents.GuildEmojis
             | DiscordIntents.GuildInvites
             | DiscordIntents.GuildMembers
             | DiscordIntents.GuildMessages
             | DiscordIntents.Guilds
             | DiscordIntents.GuildVoiceStates
             | DiscordIntents.GuildWebhooks,
                AutoReconnect = true
            };


            


            //check if main config exists..
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "discordamx.properties"))
            {
                //Set defaults
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "discordamx.properties", "#You need to know how to create a bot with the Discord Developer Portal website. Use the internet for tutorials. Add your own bot's token below:\ndiscord-bot-token=changeme");
            }


            m_Properties = new Properties(AppDomain.CurrentDomain.BaseDirectory + "discordamx.properties");
            
            if (m_Properties != null)
            {

                Program.dConfig.Token = m_Properties.get("discord-bot-token");

                //Start the discord bot
                Bot.RunAsync(dConfig).GetAwaiter().GetResult();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Log.Debug("Waiting for first guild data download to finish..");
                while (!Program.m_ScriptingInited)
                {
                    Thread.Sleep(900);   
                }
                Console.Write("\n");


            }
            else
            {
                StopEverything();
                return;
            }

            //Load the plugins
            PluginLoader.LoadPluginsAsync();



            //Load all the other files.
            Scripting.Manager.LoadFiles();
          
            //Handle key commands.
            Console.CancelKeyPress += delegate {
                StopEverything();
                return;
            };

            m_Setup = true;
        }
    }
}
