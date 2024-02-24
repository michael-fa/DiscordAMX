﻿using AMXWrapperCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public AMXArgumentList m_AmxArgs;
        int m_msWait;
        string m_Func;
        Script m_ParentScript;
        System.Threading.Timer m_Timer;
        AMXPublic m_AMXCallback;
        public string m_ArgFrmt;
        public AMXArgumentList m_Args;
        public ScriptTimer(int interval, bool rep, string funcCall, Script arg_parent_Script, string _ArgsFrm, AMXArgumentList _args)
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
            m_ArgFrmt = _ArgsFrm;
            m_Args = _args;


            Program.m_ScriptTimers.Add(this);
            this.ID = Program.m_ScriptTimers.Count;

            System.Threading.TimerCallback TimerDelegate =
            new System.Threading.TimerCallback(OnTimedEvent);


            m_Timer = new System.Threading.Timer(TimerDelegate, null, m_msWait, m_msWait);
            Program.m_Logger.Debug("Initialised Script-Timer (\"" + m_Func + "\") !", arg_parent_Script);
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
            //m_ArgFrmt = "Cx00A01"; //Identify later as no timerex.



            Program.m_ScriptTimers.Add(this);
            this.ID = Program.m_ScriptTimers.Count;

            System.Threading.TimerCallback TimerDelegate =
            new System.Threading.TimerCallback(OnTimedEvent);


            m_Timer = new System.Threading.Timer(TimerDelegate, null, m_msWait, m_msWait);
            Program.m_Logger.Debug("Initialised Script-Timer (\"" + m_Func + "\") !", arg_parent_Script);
        }

        public void OnTimedEvent(Object state)
        {
            try
            {
                m_AMXCallback = m_ParentScript.m_Amx.FindPublic(m_Func);
                //Utils.Log.Debug("length: " + m_Args.Length + "frmtstr " + m_ArgFrmt);
                List<CellPtr> _list = new List<CellPtr>();
                int count = (m_Args.Length -1);
                if (m_Args.Length != m_ArgFrmt.Length + 4)
                {
                    m_AMXCallback.AMX.RaiseError(AMXError.ParameterError);
                    return;
                }
                if(count == 3) //the end of args passed
                {
                    m_AMXCallback.Execute();
                    Program.m_Logger.Debug("Script-Timer invoked \"" + m_Func + "\"", m_ParentScript);
                    return;
                }


                foreach (char x in Utils.Scripting.Reverse(m_ArgFrmt).ToCharArray())
                {
                    switch (x)
                    {
                        case 'i':
                            {
                                m_AMXCallback.AMX.Push(m_Args[count].AsIntPtr());
                                count--;
                                continue;
                            }
                        case 'f':
                            {
                                m_AMXCallback.AMX.Push((float)m_Args[count].AsCellPtr().Get().AsFloat());
                                count--;
                                continue;
                            }
                        case 's':
                            {
                                _list.Add(m_AMXCallback.AMX.Push(m_Args[count].AsString()));
                                count--;
                                continue;
                            }
                    }
                }
                m_AMXCallback.Execute();

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

            //No repeating?
            if (!m_Repeat)
            {
                m_Timer.Change(Timeout.Infinite, Timeout.Infinite);
                this.m_Active = false;
            }

        }


        public bool KillTimer()
        {
            if (!this.m_Active) return false;


            m_Timer.Change(Timeout.Infinite, Timeout.Infinite);
            this.m_Active = false;
            this.m_ArgFrmt = "";

            return true;
        }
    }
}
