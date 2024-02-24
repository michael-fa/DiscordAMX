using System;
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
    public static class MemberJoinLeave
    {
        public static Task Join(DiscordClient c, GuildMemberAddEventArgs arg)
        {
            AMXPublic p = null!;
            foreach (Scripting.Script scr in Scripting.Manager.m_Scripts)
            {
                p = scr.m_Amx.FindPublic("OnMemberJoin");
                if (p != null)
                {
                    p.AMX.Push(Utils.Scripting.ScrMemberDCMember_ID(arg.Member, Utils.Scripting.DCGuild_ScrGuild(arg.Guild)));
                    p.AMX.Push(arg.Guild.Id);
                    p.Execute();
                    GC.Collect();
                }
                p = null!;
            }

            var targetGuild = arg.Guild;
            var relevantGuild = Program.m_ScriptGuilds.FirstOrDefault(gld => gld.m_DCGuild == targetGuild);

            if (relevantGuild != null)
            {
                relevantGuild.m_ScriptMembers?.Add(new Scripting.Member(arg.Member, relevantGuild));
            }

            return Task.CompletedTask;
        }

        public static Task Leave(DiscordClient c, GuildMemberRemoveEventArgs arg)
        {
            if (arg.Member == Discord.Bot.Client.CurrentUser) return Task.CompletedTask;

            AMXPublic p = null!;
            foreach (Scripting.Script scr in Scripting.Manager.m_Scripts)
            {
                p = scr.m_Amx.FindPublic("OnMemberLeft");
                if (p != null)
                {
                    p.AMX.Push(Utils.Scripting.ScrMemberDCMember_ID(arg.Member, Utils.Scripting.DCGuild_ScrGuild(arg.Guild)));
                    p.AMX.Push(arg.Guild.Id.ToString());
                    p.Execute();
                    GC.Collect();
                }
                p = null!;
            }

            
           

            Utils.Scripting.DCMember_ScrMember(arg.Member, Utils.Scripting.DCGuild_ScrGuild(arg.Guild)).Remove();

            return Task.CompletedTask;
        }

    }
}
