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
            });
        }
    }
}
