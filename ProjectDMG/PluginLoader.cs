using ProjectDMG.Api;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ProjectDMG
{
    internal static class PluginLoader
    {
        internal static void Load()
        {
            var pluginInterface = typeof(ProjectDMGPlugin);
            var solutionFolder = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.IndexOf("ProjectDMG\\bin"));
            var pluginsFolder = Path.Combine(solutionFolder, "PluginsDlls");

            if (Directory.Exists(pluginsFolder))
            {
                var dllFiles = Directory.GetFiles(pluginsFolder, "*.dll");

                foreach (string dllFile in dllFiles)
                {
                    try
                    {
                        var assembly = Assembly.LoadFrom(dllFile);
                        foreach (var pluginType in assembly.GetTypes().Where(t => pluginInterface.IsAssignableFrom(t) && !t.IsAbstract))
                            ((ProjectDMGPlugin)Activator.CreateInstance(pluginType)).Run();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error loading assembly {Path.GetFileName(dllFile)}: {ex.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Plugins folder not found.");
            }
        }
    }
}
