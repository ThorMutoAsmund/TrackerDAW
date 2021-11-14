using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace TrackerDAW
{
    public static class IDEIntegration
    {
        private const string SolutionGuidKey = @"$solutionGuid$";
        private const string ProjectGuidKey = @"$projectGuid$";
        private const string ProjectTypeGuidKey = @"$projectTypeGuid$";

        private static Process CurrentProcess;

        public static void OpenSourceFile(string fullScriptsPath, string scriptFileName)
        {
            var csSolutionPath = Path.Combine(fullScriptsPath, Env.CsSolutionFileName);
            var filePath = Path.Combine(Env.Song.ScriptsPath, scriptFileName);

            if (IDEIntegration.CurrentProcess == null)
            {
                IDEIntegration.CurrentProcess = Process.Start(Env.VSCodePath, $"{fullScriptsPath} {filePath}");
            }
            else
            {
                Process.Start(Env.VSCodePath, $"{filePath}");
            }
        }

        public static void CreateBlankProject(string projectPath)
        {
            // See if file exists
            var csProjectPath = Path.Combine(projectPath, Env.ScriptsFolder, Env.CsProjectFileName);
            if (File.Exists(csProjectPath))
            {
                return;
            }

            var csSolutionPath = Path.Combine(projectPath, Env.ScriptsFolder, Env.CsSolutionFileName);
            var libMainPath = Path.Combine(projectPath, Env.ScriptsFolder, Env.LibMainFileName);

            try
            {
                // Get content
                var solutionTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Env.ResourcesSystemFolder, Env.CsSolutionFileName);
                var projectTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Env.ResourcesSystemFolder, Env.CsProjectFileName);
                var libMainTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Env.ResourcesSystemFolder, Env.LibMainFileName);

                // Write new files
                var solutionGuid = Guid.NewGuid();
                var projectGuid = Guid.NewGuid();
                var projectTypeGuid = Guid.NewGuid();
                var solutionTemplateText = File.ReadAllText(solutionTemplatePath).Replace(SolutionGuidKey, solutionGuid.ToString())
                    .Replace(ProjectTypeGuidKey, projectTypeGuid.ToString()).Replace(ProjectGuidKey, projectGuid.ToString());
                File.WriteAllText(csSolutionPath, solutionTemplateText);

                var projectTemplateText = File.ReadAllText(projectTemplatePath).Replace(ProjectGuidKey, projectGuid.ToString());
                File.WriteAllText(csProjectPath, projectTemplateText);

                var libMainTemplateText = File.ReadAllText(libMainTemplatePath);
                File.WriteAllText(libMainPath, libMainTemplateText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        //private static bool IsCurrentProcessRunning()
        //{
        //    var processes = Process.GetProcessesByName(Env.DevEnvProcessName);
        //    return processes.Any(p => p.Id == CurrentProcess.Id);
        //}

        public static void TestBuildProject(string scriptsPath)
        {
            var csProjectPath = Path.Combine(scriptsPath, Env.CsProjectFileName);

            Microsoft.Build.Evaluation.Project p = new Microsoft.Build.Evaluation.Project(csProjectPath);
            p.SetGlobalProperty("Configuration", "Release");

            var logger = new StringLogger();
            if (!p.Build(logger))
            {
                MessageBox.Show(logger.Log, "Build error", MessageBoxButton.OK);
            }

            Microsoft.Build.Evaluation.ProjectCollection.GlobalProjectCollection.UnloadAllProjects();
        }
    }
}
