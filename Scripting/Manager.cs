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
//load/Reload should check for
//script file hash Differences because of name problems
namespace discordamx.Scripting
{
    static class Manager
    {
        private static bool m_Inited;
        public static List<Script> m_Scripts = new List<Script>();

        public static void LoadFiles()
        {

            Script scr = null!;
            byte[] _hash = null!;
            int first = 0;
            foreach (string x in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"\Scripts\"))
            {
                if (!Path.GetExtension(x).Equals(".amx", StringComparison.OrdinalIgnoreCase)) continue;


                //Pregen the hash, make sure its not equal to the main script already loaded.
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(x))
                    {
                        _hash = md5.ComputeHash(stream);
                    }
                }

                try
                {
                    scr = new Script(x);
                    scr.m_Hash = _hash;
                    m_Scripts.Add(scr);
                    Log.Debug("Loaded Script " + x + " with hash =  " + BitConverter.ToString(scr.m_Hash).Replace("-", "").ToLowerInvariant() + "!");
                    if (first == 0) scr.m_Amx.ExecuteMain(); //Only for the first file.

                    first = 1;
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
