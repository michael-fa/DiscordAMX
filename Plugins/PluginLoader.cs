using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DiscordAmxPlugin;

namespace discordamx.Plugin
{
    static class PluginLoader
    {
        static public List<IPlugin> plugins = null!;
        static public async void LoadPluginsAsync()
        {
            plugins = new List<IPlugin>();

            await Task.Run(() => {
                //this is somewhat stupid I am sorry otherwise it would lock up
               
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Plugins"))
                {
                    string[] dllFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Plugins/", "*.dll");

                    foreach (var dllFile in dllFiles)
                    {
                        try
                        {
                            Assembly assembly = Assembly.LoadFrom(dllFile);

                            var types = assembly.GetTypes()
                                .Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsInterface);

                            foreach (var type in types)
                            {
                                var plugin = (IPlugin)Activator.CreateInstance(type)!;
                                plugins.Add(plugin);
                                plugin.Initialize();
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to load plugin from {dllFile}: {ex.Message}");
                        }
                    }
                }

                int count = 0;
                while (plugins.Count == 0 && count < 10)
                {
                    Thread.Sleep(100);
                }
            });
        }
        static public async void UnloadPlugins()
        {
            await Task.Run(() => {
                if (plugins.Count == 0) return;
                foreach (var plugin in plugins)
                {
                    plugin.OnUnload();
                }
                //All this assembly proxy appdomain shit is for later. I guess you can catch a shutdown from inside the plugin code somehow \0/
            });
        }
    }
}
