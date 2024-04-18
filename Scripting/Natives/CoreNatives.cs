using AMXWrapperCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace discordamx.Scripting.Natives
{
    public static class CoreNatives
    {
        public static int Loadscript(AMX amx1, AMXArgumentList args1, Script caller_script)
        {
            if (args1.Length != 1) return 0;
            if (args1[0].AsString().Length == 0)
            {
                Program.m_Logger.Error(" [command] You did not specify a correct script file!", caller_script);
                return 0;
            }

            if (!File.Exists("Scripts/" + args1[0].AsString() + ".amx"))
            {
                Program.m_Logger.Error(" [command] The script file " + args1[0].AsString() + ".amx does not exist in /Scripts/ folder.", caller_script);
                return 0;
            }

            foreach (Script x in Manager.m_Scripts)
            {
                if (x.m_amxFile.Equals(args1[0].AsString())) //There is a better way, but still; we can always do or catch a unhandled error here.
                {
                    Program.m_Logger.Error(" [command] Script " + args1[0].AsString() + " is already loaded!");
                    return 0;
                }
            }

            Script scr = new Script(args1[0].AsString());
            AMXPublic pub = scr.m_Amx.FindPublic("OnInit");
            if (pub != null) pub.Execute();

            return 1;
        }

        public static int Unloadscript(AMX amx1, AMXArgumentList args1, Script caller_script)
        {
            if (args1.Length != 1) return 1;
            if (args1[0].AsString().Length == 0)
            {
                Program.m_Logger.Error(" [command] You did not specify a correct script file!");
                return 0;
            }

            foreach (Script sc in Scripting.Manager.m_Scripts)
            {
                if (sc.m_amxFile.Equals(args1[0].AsString()))
                {   
                    AMXPublic pub = sc.m_Amx.FindPublic("OnUnload");
                    if (pub != null) pub.Execute();
                    sc.m_Amx.Dispose();
                    sc.m_Amx = null!;
                    Scripting.Manager.m_Scripts.Remove(sc);
                    Program.m_Logger.Write("[CORE] Script '" + args1[0].AsString() + "' unloaded.");
                    return 1;
                }
            }
            Program.m_Logger.Error(" [command] The script '" + args1[0].AsString() + "' is not running.");
            return 1;
        }

        public static int CallRemoteFunction(AMX amx1, AMXArgumentList args1, Script caller_script)
        {
            try
            {
                if (args1.Length == 1)
                {
                    if (args1[0].AsString().Length < 2)
                        return 0;

                    AMXPublic tmp = null;
                    foreach (Script scr in Manager.m_Scripts)
                    {
                        tmp = scr.m_Amx.FindPublic(args1[0].AsString());
                        if (tmp != null) tmp.Execute();
                    }
                    return 1;
                }
                else if (args1.Length > 3)
                {
                    if (args1.Length != args1[1].AsString().Length)
                        return 1;

                    int count = (args1.Length - 1);

                    AMXPublic p = null;
                    List<CellPtr> Cells = new List<CellPtr>();

                    //Important so the format ( ex "iissii" ) is aligned with the arguments pushed to the callback, not being reversed
                    string reversed_format = Utils.Scripting.Reverse(args1[1].AsString());

                    foreach (Script scr in Manager.m_Scripts)
                    {
                        if (scr.Equals(caller_script)) continue;
                        p = scr.m_Amx.FindPublic(args1[0].AsString());
                        if (p == null) continue;
                        foreach (char x in reversed_format.ToCharArray())
                        {
                            //if (count == 1) break;
                            switch (x)
                            {
                                case 'i':
                                    {
                                        p.AMX.Push(args1[count].AsIntPtr());
                                        count--;
                                        break;
                                    }
                                case 'f':
                                    {
                                        p.AMX.Push((float)args1[count].AsCellPtr().Get().AsFloat());
                                        count--;
                                        break;
                                    }

                                case 's':
                                    {
                                        Cells.Add(p.AMX.Push(args1[count].AsString()));
                                        count--;
                                        break;
                                    }
                            }
                        }
                        //Reset our arg index counter
                        count = (args1.Length - 1);
                        p.Execute();


                        foreach (CellPtr cell in Cells)
                        {
                            p.AMX.Release(cell);
                        }
                        GC.Collect();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.m_Logger.Exception(ex);
            }
            return 1;
        }


        public static int gettimestamp(AMX amx1, AMXArgumentList args1, Script caller_script)
        {
            return (Int32)DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        public static int SetTimer(AMX amx1, AMXArgumentList args1, Script caller_script)
        {
            if (args1[2].AsInt32() > 1 || args1[2].AsInt32() < 0)
            {
                Program.m_Logger.Error("SetTimer: Argument 'repeating' is boolean. Please pass 0 or 1 only!");
                return 0;
            }

            try
            {
                ScriptTimer timer = new ScriptTimer(args1[1].AsInt32(), Convert.ToBoolean(args1[2].AsInt32()), args1[0].AsString(), caller_script);
            }
            catch (Exception ex)
            {
                Program.m_Logger.Exception(ex, caller_script);
            }
            return (Program.m_ScriptTimers.Count);
        }

        public static int SetTimerEx(AMX amx1, AMXArgumentList args1, Script caller_script)
        {
            if (args1.Length < 5)
                return 1; // Return an error code or handle the case where there are insufficient arguments

            try
            {
                int duration = args1[1].AsInt32(); // Assuming duration is the first argument
                bool repeat = Convert.ToBoolean(args1[2].AsInt32()); // Assuming repeat is the second argument
                string callback = args1[0].AsString(); // Assuming callback is the third argument
                string format = args1[3].AsString(); // Assuming format is the fourth argument

                // Extracting arguments based on the format string
                object[] parsedArgs = new object[format.Length];
                int argIndex = 4; // Start from index 4 of AMXArgumentList
                for (int i = 0; i < format.Length; i++)
                {
                    switch (format[i])
                    {
                        case 'i':
                            parsedArgs[i] = args1[argIndex].AsInt32(); // Assuming integers start from index 4
                            argIndex++;
                            break;
                        case 'f':
                            parsedArgs[i] = args1[argIndex].AsFloat(); // Assuming floats start from index 5
                            argIndex++;
                            break;
                        case 's':
                            parsedArgs[i] = args1[argIndex].AsString(); // Assuming strings start from index 6
                            argIndex++;
                            Console.WriteLine(Convert.ToString(parsedArgs[argIndex]));
                            break;
                        default:
                            // Handle unsupported format specifier
                            break;
                    }
                }

                // Now call the ScriptTimer function with parsed arguments
                new ScriptTimer(duration, repeat, callback, caller_script, format, parsedArgs);
                return 1; 
            }
            catch (Exception ex)
            {
                Program.m_Logger.Exception(ex, caller_script);
                return -1; // Return an error code if an exception occurs
            }
        }

        public static int KillTimer(AMX amx1, AMXArgumentList args1, Script caller_script)
        {
            foreach (ScriptTimer scrt in Program.m_ScriptTimers)
            {
                if (scrt != null)
                {
                    if (scrt.ID == args1[0].AsInt32())
                    {
                        scrt.KillTimer();
                        return 1;
                    }
                }
            }
            return 1;
        }

        public static int DC_GetBotPing(AMX amx1, AMXArgumentList args1, Script caller_script)
        {
            try
            {
                return Discord.Bot.Client.Ping;
            }
            catch (Exception ex)
            {
                Program.m_Logger.Exception(ex, caller_script);
                Program.m_Logger.Error("In native 'DC_GetBotPing'! (could be m_Discord->Client Null reference)", caller_script);
            } 
            return 1;
        }
    }
}
