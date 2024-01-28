using System;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using discordamx.Scripting;
using System.Runtime.CompilerServices;

namespace discordamx {

    public static class Log
    {
        public static string fileName = DateTime.Now.ToString().Replace(':', '-').Replace('/', '-') + ".txt";

        public static void WriteLine(string _msg)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(_msg);
            if (_msg.Length > 0) File.AppendAllText(@Environment.CurrentDirectory + "/Logs/" + fileName, _msg + "\n");

            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Info(string _msg, Script _source = null!)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            if (_source != null)
            {
                string[] scrname = _source.m_amxFile.Split("/");
                Console.WriteLine("[INFO] [" + scrname[scrname.Length - 1] + "] " + _msg);
            }
            else Console.WriteLine("[INFO] " + _msg);

            if (_msg.Length > 0) File.AppendAllText(@Environment.CurrentDirectory + "/Logs/"+ fileName, _msg + "\n");

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Error(string _msg, Script _source = null!)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (_source != null)
            {
                string[] scrname = _source.m_amxFile.Split("/");
                Console.WriteLine("[ERROR] [" + scrname[scrname.Length - 1] + "] " + _msg);
            }
            else Console.WriteLine("[ERROR] " + _msg);
            if (_msg.Length > 0) File.AppendAllText(@Environment.CurrentDirectory + "/Logs/" + fileName, _msg + "\n");

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Warning(string _msg, Script _source = null!)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if (_source != null)
            {
                string[] scrname = _source.m_amxFile.Split("/");
                Console.WriteLine("[WARNING] [" + scrname[scrname.Length - 1] + "] " + _msg);
            }
            else Console.WriteLine("[WARNING] " + _msg);
            if (_msg.Length > 0) File.AppendAllText(@Environment.CurrentDirectory + "/Logs/"+ fileName, _msg + "\n");

            Console.ForegroundColor = ConsoleColor.White;
        }


        public static void Debug(string _msg, Script _source = null!)
        {
#if DEBUG

            Console.ForegroundColor = ConsoleColor.Magenta;
            if (_source != null)
            {
                string[] scrname = _source.m_amxFile.Split("/");
                Console.WriteLine("[DEBUG] [" + scrname[scrname.Length - 1] + "] " + _msg);
                System.Diagnostics.Debug.WriteLine("Utils.Log: <" + scrname[scrname.Length - 1] + "> " + _msg);
            }
            else
            {
                Console.WriteLine("[DEBUG] " + _msg);
                System.Diagnostics.Debug.WriteLine("Utils.Log: " + _msg);
            }
            if (_msg.Length > 0) File.AppendAllText("Logs/"+ fileName, _msg + "\n");

            Console.ForegroundColor = ConsoleColor.White;



#endif
        }


        public static void Exception(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("---------------------------------------\n[EXCEPTION] " + e.Message + "\n" + e.Source + "\n" + e.InnerException + "\n" + e.StackTrace + "\n---------------------------------------\n");
            File.AppendAllText(@Environment.CurrentDirectory + "/Logs/"+ fileName, "---------------------------------------\n[EXCEPTION] " + e.Message + "\n" + e.Source + "\n" + e.InnerException + "\n" + e.StackTrace + "\n-------------------------------------- -\n");

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Exception(@Exception e, Script _source = null!)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            if (_source == null)
            {
                Console.WriteLine("---------------------------------------\n[EXCEPTION] " + e.Message + "\n" + e.Source + "\n" + e.InnerException + "\n" + e.StackTrace + "\n---------------------------------------\n");
                File.AppendAllText(@Environment.CurrentDirectory + "/Logs/"+ fileName, "---------------------------------------\n[EXCEPTION] " + e.Message + "\n" + e.Source + "\n" + e.InnerException + "\n" + e.StackTrace + "\n-------------------------------------- -\n");
            }
            else
            {
                Console.WriteLine("---------------------------------------\n[EXCEPTION] [" + _source.m_amxFile + "] " + e.Message + "\n" + e.Source + "\n" + e.InnerException + "\n" + e.StackTrace + "\n---------------------------------------\n");
                File.AppendAllText(@Environment.CurrentDirectory + "/Logs/"+ fileName, "---------------------------------------\n[EXCEPTION] [" + _source.m_amxFile + "] " + e.Message + "\n" + e.Source + "\n" + e.InnerException + "\n" + e.StackTrace + "\n-------------------------------------- -\n");

            }


            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}