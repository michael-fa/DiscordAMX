using System;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using discordamx.Scripting;
using System.Runtime.CompilerServices;

namespace discordamx {
    public class Logger
    {
        public string m_FileName = null!;

        public Logger()
        {

            m_FileName = DateTime.Now.ToString().Replace(':', '-').Replace('/', '-') + ".txt";
        }

        public void Write(string message, Script _source = null!)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                using (StreamWriter fs = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", m_FileName), true))
                {
                    if (_source == null) fs.WriteLineAsync("[" + DateTime.Now + "] " + message);
                    else
                    {
                        string[] scrname = _source.m_amxFile.Split("/");
                        fs.WriteLineAsync("<" + scrname[scrname.Length - 1] + ">" + "[" + DateTime.Now + "] " + message);
                    }
                    fs.Close();
                }
                Console.WriteLine(message);

            }
            catch (Exception ex)
            {
                Console.WriteLine("--------------------\n" + ex.InnerException + "\n" + ex.Message + "\n    ---- AT:\n" + ex.Source + "\n    ----DATA:\n" + ex.Data);
            }
        }

        public void Error(string message, Script _source = null!)
        {

            try
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                using (StreamWriter fs = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", m_FileName), true))
                {
                    if (_source == null) fs.WriteLineAsync("[" + DateTime.Now + "] " + "[ERROR] " + message);
                    else
                    {
                        string[] scrname = _source.m_amxFile.Split("/");
                        fs.WriteLineAsync("<" + scrname[scrname.Length - 1] + ">" + "[" + DateTime.Now + "] " + "[ERROR] " + message);
                    }
                    fs.Close();
                }
                Console.WriteLine("[ERROR]" + message);

            }
            catch (Exception ex)
            {
                Console.WriteLine("--------------------\n" + ex.InnerException + "\n" + ex.Message + "\n    ---- AT:\n" + ex.Source + "\n    ----DATA:\n" + ex.Data);
            }
        }

        public void Warning(string message, Script _source = null!)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                using (StreamWriter fs = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", m_FileName), true))
                {
                    if (_source == null) fs.WriteLineAsync("[" + DateTime.Now + "] " + "[WARNING] " + message);
                    else
                    {
                        string[] scrname = _source.m_amxFile.Split("/");
                        fs.WriteLineAsync("<" + scrname[scrname.Length - 1] + ">" + "[" + DateTime.Now + "] " + "[WARNING] " + message);
                    }
                    fs.Close();
                }
                Console.WriteLine("[WARNING]" + message);

            }
            catch (Exception ex)
            {
                Console.WriteLine("--------------------\n" + ex.InnerException + "\n" + ex.Message + "\n    ---- AT:\n" + ex.Source + "\n    ----DATA:\n" + ex.Data);
            }
        }

        public void Debug(string message, Script _source = null!)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                using (StreamWriter fs = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", m_FileName), true))
                {
                    if (_source == null) fs.WriteLineAsync("[" + DateTime.Now + "] " + "[DEBUG] " + message);
                    else
                    {
                        string[] scrname = _source.m_amxFile.Split("/");
                        fs.WriteLineAsync("<" + scrname[scrname.Length - 1] + ">" + "[" + DateTime.Now + "] " + "[DEBUG] " + message);
                    }
                    fs.Close();
                }
                Console.WriteLine("[DEBUG]" + message);

            }
            catch (Exception ex)
            {
                Console.WriteLine("--------------------\n" + ex.InnerException + "\n" + ex.Message + "\n    ---- AT:\n" + ex.Source + "\n    ----DATA:\n" + ex.Data);
            }
        }

        public void Exception(@Exception e, Script _source = null!)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                using (StreamWriter fs = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Logs/" + m_FileName, true))
                {
                    if (_source == null) fs.WriteLineAsync("[" + DateTime.Now + "]" + " [EXCEPTION] " + e.Message + "\n" + e.Source + "\n" + e.InnerException + "\n" + e.StackTrace + "\n---------------------------------------\n");
                    else
                    {
                        string[] scrname = _source.m_amxFile.Split("/");
                        fs.WriteLineAsync("<" + scrname[scrname.Length - 1] + ">" + "[" + DateTime.Now + "]" + " [EXCEPTION] " + e.Message + "\n" + e.Source + "\n" + e.InnerException + "\n" + e.StackTrace + "\n---------------------------------------\n");
                    }
                    fs.Close();
                }
                Console.WriteLine("---------------------------------------\n[" + DateTime.Now + "]" + " [EXCEPTION] " + e.Message + "\n" + e.Source + "\n" + e.InnerException + "\n" + e.StackTrace + "\n---------------------------------------\n");

            }
            catch (Exception ex)
            {
                Console.WriteLine("--------------------\n" + ex.InnerException + "\n" + ex.Message + "\n    ---- AT:\n" + ex.Source + "\n    ----DATA:\n" + ex.Data);
            }
        }
    }

    /*public static class Log
    {
        public static string fileName = DateTime.Now.ToString().Replace(':', '-').Replace('/', '-') + ".txt";

        public static bool Safe_Append(string text)
        {

            /* try
             {
                 new Thread(() =>
                     File.AppendAllText(@Environment.CurrentDirectory + "/Logs/" + fileName, text + "\n")
                 ).Start();
                 return true;
             }
             catch { return false; }

            return false;
        }
        public static void WriteLine(string _msg)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(_msg);
            if (_msg.Length > 0) Safe_Append(_msg);
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

            if (_msg.Length > 0) Safe_Append(_msg);

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
            if (_msg.Length > 0) Safe_Append(_msg);

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
            
            if (_msg.Length > 0) Safe_Append(_msg);

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
            if (_msg.Length > 0) Safe_Append(_msg);

            Console.ForegroundColor = ConsoleColor.White;



#endif
        }


        public static void Exception(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("---------------------------------------\n[EXCEPTION] " + e.Message + "\n" + e.Source + "\n" + e.InnerException + "\n" + e.StackTrace + "\n---------------------------------------\n");
            Safe_Append("---------------------------------------\n[EXCEPTION] " + e.Message + "\n" + e.Source + "\n" + e.InnerException + "\n" + e.StackTrace + "\n-------------------------------------- -\n");

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Exception(@Exception e, Script _source = null!)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            if (_source == null)
            {
                Console.WriteLine("---------------------------------------\n[EXCEPTION] " + e.Message + "\n" + e.Source + "\n" + e.InnerException + "\n" + e.StackTrace + "\n---------------------------------------\n");
                Safe_Append("---------------------------------------\n[EXCEPTION] " + e.Message + "\n" + e.Source + "\n" + e.InnerException + "\n" + e.StackTrace + "\n-------------------------------------- -\n");
            }
            else
            {
                Console.WriteLine("---------------------------------------\n[EXCEPTION] [" + _source.m_amxFile + "] " + e.Message + "\n" + e.Source + "\n" + e.InnerException + "\n" + e.StackTrace + "\n---------------------------------------\n");
                Safe_Append("---------------------------------------\n[EXCEPTION] [" + _source.m_amxFile + "] " + e.Message + "\n" + e.Source + "\n" + e.InnerException + "\n" + e.StackTrace + "\n-------------------------------------- -\n");

            }


            Console.ForegroundColor = ConsoleColor.White;
        }
    }*/
}