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
            Program.m_Plugins = new List<Plugins.Plugin>();
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


            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Scripts/"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Scripts/");

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Logs/"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Logs/");

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Plugins/"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Plugins/");

            //check if main config exists..
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "discordamx.properties"))
            {
                //Set defaults
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "discordamx.properties", "discord-bot-token=changeme\n#This option is optional, since every .amx file inside /scripts/ is automatically loaded. This only loadsa script first(!!) if you wish to.\nmain-script=");
            }


            m_Properties = new Properties(AppDomain.CurrentDomain.BaseDirectory + "discordamx.properties");
            
            if (m_Properties != null)
            {

                Program.dConfig.Token = m_Properties.get("discord-bot-token");

                //Start the discord bot
                Discord.Bot.RunAsync(dConfig).GetAwaiter().GetResult();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("[INFO] Waiting for first guild data download to finish..");
                while (!Program.m_ScriptingInited)
                {
                    Console.Write(".");
                    Thread.Sleep(900);   
                }
                Console.Write("\n");


                foreach (string fl in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Plugins/"))
                {
                    if (!fl.EndsWith(".dll")) continue;
                    Log.Info("\n---------------------------------------------\n" +
                                                   "[CORE] Found plugin: '" + fl + "' !");
                    Log.Info("\n---------------------------------------------");
                    new Plugins.Plugin(fl);
                }


                if (!String.IsNullOrEmpty(m_Properties.get("main-script")) || !String.IsNullOrWhiteSpace(m_Properties.get("main-script")))
                {
                    //Load a specific script first.
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Scripts/" + m_Properties.get("main-script") + ".amx"))
                    {
                        Scripting.Manager.m_InitScriptName = m_Properties.get("main-script");
                        Scripting.Manager.m_InitScript = new Scripting.Script(AppDomain.CurrentDomain.BaseDirectory + "Scripts/" + m_Properties.get("main-script") + ".amx");
                        Scripting.Manager.m_InitScript.m_Amx.ExecuteMain();
                    }
                }


            }
            else
            {
                StopEverything();
                return;
            }


            /*
            int idx = 0;
            foreach (string arg in args)
            {
                switch (arg)
                {
                    case "-mainscript":

                        try
                        {
                            if (args[idx + 1] != null && args[idx + 1].Length > 0 && !_mainScriptLoaded)
                            {
                                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Scripts/" + args[idx + 1] + ".amx"))
                                {
                                    Console.WriteLine("mainscript forced");
                                    Scripting.Manager.m_InitScript = new Scripting.Script(AppDomain.CurrentDomain.BaseDirectory + "Scripts/" + args[idx + 1] + ".amx"); //Priority (force) calling the main script, if given.
                                    Scripting.Manager.m_InitScript.m_Amx.ExecuteMain();
                                    _mainScriptLoaded = true;
                                }
                            }
                        }
                        catch
                        {
                            Log.Error("Main script defined incorrectly!");
                        }

                        break;

                }

                idx++;
            }*/

            
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
