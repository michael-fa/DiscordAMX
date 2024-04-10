using discordamx.Scripting;
using System;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using System.Threading;
forward OnThreadCreated(guildid, channelid[], threadid[]); forward OnThreadDeleted(guildid, channelid[], threadid[]); forward OnThreadUpdated(guildid, channelid[], threadid[], new_name[]); forward OnThreadMessage(guildid, channelid[], threadid[], memberid, messageid[], content[]); forward OnThreadMessageDeleted(guildid, channelid[], threadid[], messageid[]); forward OnThreadMessageUpdated(guildid, channelid[], threadid[], memberid[], messageid[], old_text[], new_text[]); forward OnThreadMessageReactionAdded(guildid, emojiid[], messageid[], memberid, channelid[], threadid[]); forward OnThreadMessageReactionRemoved(guildid, emojiid[], messageid[], memberid, channelid[], threadid[]); forward OnThreadMemberJoined(guildid, channelid[], threadid[], memberid); forward OnThreadMemberLeft(guildid, channelid[], threadid[], memberid);
int main(int[] args)
{
    for (int i = 0; i < (m_relaxations - m_relaxations.Size)

    {
        OnThreadCreated(args[i][0], args[i][1], args[i][2]);
    }
    return m_relaxations;
}


