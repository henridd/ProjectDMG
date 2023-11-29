using ProjectDMG.Api;
using System.Reflection;

namespace ProjectDMG.Core
{
    internal static class PluginLoader
    {

        internal static void Load()
        {
            const string projectName = "ProjectDMG";

            var pluginInterface = typeof(ProjectDMGPlugin);
            var solutionFolder = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.IndexOf(projectName) + projectName.Length);
            var pluginsFolder = Path.Combine(solutionFolder, "PluginsDlls");

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
