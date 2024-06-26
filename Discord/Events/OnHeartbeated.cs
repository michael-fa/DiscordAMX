﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;
using DSharpPlus.Interactivity.Extensions;
using AMXWrapperCore;
using System.Diagnostics;

namespace discordamx.Discord.Events
{
    public static class OnHeartbeated
    {
        public static Task Execute(DiscordClient c, HeartbeatEventArgs e)
        {
            if(Program.m_ScriptingInited)
            {
                if (Scripting.Manager.m_Scripts[0].m_Amx == null) return Task.CompletedTask;


                AMXPublic p = Scripting.Manager.m_Scripts[0].m_Amx.FindPublic("OnHeartbeat");

                if (p != null)
                {
                    p.AMX.Push(e.Ping);
                    p.Execute();
                }
                else return Task.FromResult(0);

            }
            return Task.CompletedTask;
        }
    }
}
