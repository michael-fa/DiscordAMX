using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace discordamx.Scripting
{
    public class DiscordEmbedBuilder : IDisposable
    {
        private DSharpPlus.Entities.DiscordEmbedBuilder m_Builder;
        public int m_ID;
        bool disposed = false;
        public DiscordMessage m_MessageID = null!;

        public DiscordEmbedBuilder(string szTitle, string szDescription = "")
        {
            var embed = new DSharpPlus.Entities.DiscordEmbedBuilder
            {
                Title = szTitle,
                Description = szDescription
            };

            m_Builder = embed;
            Program.m_Embeds.Add(this);
            m_ID = Program.m_Embeds.Count;  
        }

        public void SetThumbnail(string szUrl, int iH = 0, int iW = 0)
        {
            m_Builder.WithThumbnail(szUrl, iH, iW);
        }

        public void SetAuthor(string szAuthor, string szUrl = null!)
        {
            m_Builder.WithAuthor(szAuthor, szUrl);
        }

        public void SetFooter(string szText, string szIconUrl)
        {
            m_Builder.WithFooter(szText, szIconUrl);
        }

        public void SetColor(DiscordColor color)
        {
            m_Builder.WithColor(color);
        }

        public void SetImageUrl(string szUrl)
        {
            m_Builder.WithImageUrl(szUrl);
        }
        public void SetUrl(string szUrl)
        {
            m_Builder.WithUrl(szUrl);
        }

        public void AddText(string szTitle, string szText, bool bInline = false)
        {
            m_Builder.AddField(szTitle, szText, bInline);
        }

        public void ToggleTimestamp()
        {
            if (this.m_Builder.Timestamp == null) this.m_Builder.Timestamp = DateTime.Now;
            else this.m_Builder.Timestamp = null;
        }

        public DiscordMessage Send(DiscordChannel channel)
        {
#pragma warning disable CS0168 // Variable ist deklariert, wird jedoch niemals verwendet
            try
            {
                m_MessageID = channel.SendMessageAsync(this.m_Builder).Result;
                return m_MessageID;
            }
            catch (Exception ex)
            {
                return null!;
            }
#pragma warning restore CS0168 // Variable ist deklariert, wird jedoch niemals verwendet
        }

        public DiscordMessage Update()
        {
            try
            {
                this.m_MessageID.ModifyAsync(this.m_Builder.Build());
                return m_MessageID;
            }
            catch (Exception ex)
            {
                return null!;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                }

                Program.m_Embeds.Remove(this);
                disposed = true;
            }
        }
    }
}
