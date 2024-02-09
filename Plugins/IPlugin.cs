using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordAmxPlugin
{
    public interface IPlugin
    {
        void Initialize();
        string[] GetNatives();
        string GetName();

    }
}
