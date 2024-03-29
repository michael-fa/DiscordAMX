﻿using System;
using System.Collections.Generic;
using DSharpPlus;
using DSharpPlus.Entities;


namespace discordamx.Scripting
{
    public class Guild
    {
        public int m_ID;
        public DiscordGuild m_DCGuild;
        public List<Scripting.Member> m_ScriptMembers = null!;

        public Guild(DiscordGuild _dcg)
        {
            m_ScriptMembers = new List<Scripting.Member>();

            m_DCGuild = _dcg;
            Program.m_ScriptGuilds.Add(this);

            m_ID = Program.m_ScriptGuilds.Count;
        }
        
    }
}
