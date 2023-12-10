using ProjectDMG.Api;
using System.Reflection;

namespace ProjectDMG.Core
{
    internal static class PluginLoader
    {
        internal static IEnumerable<ProjectDMGPlugin> Load()
        {
            const string projectName = "ProjectDMG";

            var pluginInterface = typeof(ProjectDMGPlugin);
            var solutionFolder = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.IndexOf(projectName) + projectName.Length);
            var pluginsFolder = Path.Combine(solutionFolder, "PluginsDlls");

            var loadedPlugins = new List<ProjectDMGPlugin>();
            if (Directory.Exists(pluginsFolder))
            {
                var dllFiles = Directory.GetFiles(pluginsFolder, "*.dll");

                foreach (string dllFile in dllFiles)
                {
                    try
                    {
                        // TODO: It doesn't work when the assembly has NuGet packages.
                        var assembly = Assembly.LoadFrom(dllFile);
                        foreach (var pluginType in assembly.GetTypes().Where(t => pluginInterface.IsAssignableFrom(t) && !t.IsAbstract))
                        {
                            var plugin = (ProjectDMGPlugin)Activator.CreateInstance(pluginType);
                            loadedPlugins.Add(plugin);
                            plugin.Run();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error loading assembly {Path.GetFileName(dllFile)}: {ex.Message}");
                    }
                }

                return loadedPlugins;
            }
            else
            {
                Console.WriteLine("Plugins folder not found.");
                return Enumerable.Empty<ProjectDMGPlugin>();
            }
        }
    }
}
