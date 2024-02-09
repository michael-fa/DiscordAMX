using discordamx.Scripting;
using DSharpPlus;
using DSharpPlus.Entities;
using System.Runtime.InteropServices;
using System.Text;

namespace discordamx
{

    internal static partial class Program
    {
        public static string VERSION = "0.0.2";

        public static bool                                  m_Run = true;
        public static Properties                            m_Properties = null!;
        private static ConsoleColor                         m_DefForegrColor;
        private static bool                                 m_Setup = false;
        public static DiscordConfiguration                  dConfig = null!;
        public static bool                                  m_ScriptingInited = false;
        public static List<DiscordChannel>                  m_DmUsers = null;
        public static List<Scripting.DiscordEmbedBuilder>   m_Embeds = null;
        public static List<Scripting.Guild>                 m_ScriptGuilds = null!;


        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler = null!;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool Handler(CtrlType sig)
        {
            switch (sig)
            {
                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:
                    StopEverything();

                    return false;
                default:
                    return false;
            }
        }

        static void Main(string[] args)
        {

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Scripts/"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Scripts/");

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Logs/"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Logs/");

            if (!File.Exists(@Environment.CurrentDirectory + "/Logs/" + Log.fileName))
                File.Create(@Environment.CurrentDirectory + "/Logs/" + Log.fileName);

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Plugins/"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Plugins/");

            Console.OutputEncoding = System.Text.Encoding.Unicode;
            m_DefForegrColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;

            //Set the console handler, catching events such as close.76
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            Setup(args);

            while (m_Run)
            {
                //Listen for input - handle it locally - trigger script event too.

                ProcessLocalInput();
                Thread.Sleep(100);
            }

        }

        public static void StopEverything(int errcode = 0)
        {
            m_Run = false;


            foreach (Script script in Manager.m_Scripts)
            {
                Manager.UnloadScript(script);
            }
            Console.ForegroundColor = m_DefForegrColor;
            Environment.Exit(errcode);
        }
    }
}
