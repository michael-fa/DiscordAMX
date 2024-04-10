using AMXWrapperCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace discordamx.Scripting
{
    public class  ScriptTimer
    {
        public int ID;
        public bool m_Active = false;
        public bool m_Repeat = true;
        int m_msWait;
        string m_Func;
        Script m_ParentScript;
        System.Timers.Timer m_Timer;
        AMXPublic m_AMXCallback;
        public string m_ArgFrmt;
        public object[] m_Args;
        private object lockObject;


        public ScriptTimer(int interval, bool rep, string funcCall, Script arg_parent_Script, string _ArgsFrm, object[] _args)
        {
            m_ParentScript = arg_parent_Script;
           

            m_msWait = interval;
            Console.WriteLine(funcCall + " has to be called nigga.");
            m_Func = funcCall;
            m_Active = true;
            m_Repeat = rep;
            m_ArgFrmt = _ArgsFrm;
            m_Args = _args;
            lockObject = new object();

            m_AMXCallback = m_ParentScript.m_Amx.FindPublic(funcCall);
            if (m_AMXCallback == null)
            {
                return;
            }

            int count = m_ArgFrmt.Length - 1;
            char[] reversedFormat = Utils.Scripting.Reverse(m_ArgFrmt).ToCharArray();
            List<CellPtr> Cells = new List<CellPtr>();

            Program.m_ScriptTimers.Add(this);
            this.ID = Program.m_ScriptTimers.Count;


            //m_Timer = new System.Threading.Timer(TimerDelegate, null, m_msWait, m_msWait);
            m_Timer = new System.Timers.Timer(m_msWait);
            m_Timer.Elapsed += OnTimedEvent;
            m_Timer.AutoReset = m_Repeat;
            m_Timer.Start();
            Program.m_Logger.Debug("Initialised Script-Timer (\"" + m_Func + "\") !", arg_parent_Script);
            return;
        }

        public ScriptTimer(int interval, bool rep, string funcCall, Script arg_parent_Script)
        {
            m_ParentScript = arg_parent_Script;
            m_AMXCallback = m_ParentScript.m_Amx.FindPublic(funcCall);
            if (m_AMXCallback == null)
            {
                return;
            }
            m_msWait = interval;
            m_Func = funcCall;
            m_Active = true;
            m_Repeat = rep;
            lockObject = new object();


            Program.m_ScriptTimers.Add(this);
            this.ID = Program.m_ScriptTimers.Count;

            m_Timer = new System.Timers.Timer(m_msWait);
            m_Timer.Elapsed += OnTimedEvent;
            m_Timer.AutoReset = m_Repeat;
            m_Timer.Start();

            Program.m_Logger.Debug("Initialised Script-Timer (\"" + m_Func + "\") !", arg_parent_Script);
        }

        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            lock (lockObject)
            {
                Console.WriteLine("LOOP");
                try
                {
                    m_AMXCallback = m_ParentScript.m_Amx.FindPublic(m_Func);
                    if (m_AMXCallback == null) { return; }
                    List<CellPtr> _list = new List<CellPtr>();
                    //int count = (m_Args.Length - 1);

                    if (m_Args == null) goto justcall;
                    if (m_Args.Length != m_ArgFrmt.Length)
                    {
                        Console.WriteLine("LOOP2");
                        m_AMXCallback.AMX.RaiseError(AMXError.ParameterError);
                        return;
                    }
                    Console.WriteLine("LOOP3");
                    if (m_Args.Length == 3) //the end of args passed
                    {
                        m_AMXCallback.Execute();
                        Program.m_Logger.Debug("Script-Timer invoked \"" + m_Func + "\"", m_ParentScript);
                        return;
                    }
                    Console.WriteLine("Lop, " + m_Args.Length + ", " + m_ArgFrmt.Length);
                    Console.WriteLine("LOOP4");

                    for (int i = 0; i < m_ArgFrmt.Length; i++)
                    {
                        switch (m_ArgFrmt[i])
                        {
                            case 'i':
                                {
                                    m_AMXCallback.AMX.Push((int)m_Args[i]);
                                    break;
                                }
                            case 'f':
                                {
                                    m_AMXCallback.AMX.Push((float)m_Args[i]);
                                    break;
                                }
                            case 's':
                                {
                                    _list.Add(m_AMXCallback.AMX.Push((string)m_Args[i]));
                                    break;
                                }
                        }
                    }

                    justcall:
                    // Assuming m_AMXCallback is an instance of a class and Execute is a method of that class.
                    Type amxCallbackType = m_AMXCallback.GetType();
                    MethodInfo executeMethod = amxCallbackType.GetMethod("Execute");

                    if (executeMethod != null)
                    {
                        // Optionally, create an instance of the class if the method is not static.
                        // If m_AMXCallback is already an instance, skip this step.
                        // object instance = Activator.CreateInstance(amxCallbackType);

                        // Invoke the method.
                        executeMethod.Invoke(m_AMXCallback, null);
                    }
                    else
                    {
                        // Method not found.
                        Console.WriteLine("Execute method not found.");
                    }


                    foreach (CellPtr cell in _list)
                    {
                        m_AMXCallback.AMX.Release(cell);
                    }
                    GC.Collect();

                    Program.m_Logger.Debug("Script-TimerEx invoked \"" + m_Func + "\"", m_ParentScript);
                }
                catch (Exception ex)
                {
                    Program.m_Logger.Exception(ex, m_ParentScript);
                }
            }

        }



        public bool KillTimer()
        {
            if (!this.m_Active) return false;


            m_Timer.Stop();
            this.m_Active = false;
            this.m_ArgFrmt = "";

            return true;
        }
    }
}
