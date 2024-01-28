using AMXWrapperCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


//Note
//Unload/Reload should check for
//scriptfile hash Differences because of name problems
namespace discordamx.Scripting
{
    static class Manager
    {
        private static bool m_Inited;
        public static List<Script> m_Scripts = new List<Script>();
        public static string m_InitScriptName = null!;
        public static Script m_InitScript = null!;

        public static void LoadFiles()
        {
           
            Script scr = null!;
            foreach (string x in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"\Scripts\"))
            {
                if (!x.Contains(".amx")) continue;
                if (x.Equals(m_InitScript)) continue; //the main file is reserved to be loaded first.
                try
                {
                    scr = new Script(x);

                    m_Scripts.Add(scr!);

                    using (var md5 = MD5.Create())
                    {
                        using (var stream = File.OpenRead(x))
                        {
                            scr.m_Hash = md5.ComputeHash(stream);
                        }
                    }

                    Log.Debug("Loaded Script + " + x + " with hash =  " + BitConverter.ToString(scr.m_Hash).Replace("-", "").ToLowerInvariant() + "!");
                    var p = scr.m_Amx.FindPublic("OnInit");
                    if (p != null) p.Execute();
                }
                catch (Exception ex)
                {
                    Log.Exception(ex);
                }


              
            }
        }

        public static void UnloadScript(Script script)
        {
            if (script.m_Amx != null)
            {
                script.StopAllTimers();
                var p = script.m_Amx.FindPublic("OnUnload");
                if (p != null) p.Execute();

                script.m_Amx.Dispose();
                script.m_Amx = null!;
            }
            m_Scripts.Remove(script);
        }
    }
}
