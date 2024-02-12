using DiscordAmxPlugin;
using discordamx;
using AMXWrapperCore;
using discordamx.Scripting;

namespace DcAmxTestPlugin
{
    public class MyGoodPlugin : IPlugin
    {
        public void Initialize()
        {
            Program.m_Logger.Write("Plugin Example by 'name here' has been loaded.");
        }

        public string[] GetNatives()
        {
            return new string[] { "TestNative1", "TestNative2" };
        }

        public string GetName()
        {
            return ("MyGoodPlugin v1");
        }

        public static int TestNative1(AMX amx1, AMXArgumentList args1, Script caller_script)
        {
            Console.WriteLine("So far, so good.");
            return 1;
        }

        public static int TestNative2(AMX amx1, AMXArgumentList args1, Script caller_script)
        {
            Console.WriteLine("Rain is wet. Water is wet. Wet is moisture. Moisture is hydration. Hydration is good.    ");
            return 1;
        }

        public void OnUnload()
        {

        }
    }
}
