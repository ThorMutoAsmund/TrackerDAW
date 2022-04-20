using System;
using System.Collections.Generic;
using System.Windows;

namespace TrackerDAW
{
    public static class Env
    {
        public static event Action<bool> DirtyChanged;
        public static event Action<Pattern> SelectedPatternChanged;
        public static event Action ApplicationEnded;
        public static event Action<string> OutputAdded;
        public static event Action<TimeSpan> TimeChanged;

        public static MainWindow MainWindow;
        public static string AppName = "TrackerDAW";
        public static string ApplicationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static string DevEnvProcessName = "devenv";
        public static string DevEnvPath = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\devenv.exe";
        public static string VSCodePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Programs\Microsoft VS Code\Code.exe";
        public static string SamplesFolder = "Samples";
        public static string ScriptsFolder = "Scripts";
        public static string ProvidersSystemFolder = "Providers";
        public static string ResourcesSystemFolder = "Resources";
        public static string ProjectFileName = "Project";
        public static string DefaultProjectFileName = "default_project";
        public static string DefaultProjectName = "Default project";
        public static string NewProjectName = "New project";
        public static string BlankProviderFileName = "BlankProvider.cs";
        public static string CsProjectFileName = "Project.csproj";
        public static string CsSolutionFileName = "Project.sln";
        public static string CsOutputFolder = "Bin";
        public static string CsOutputDllFileName = "UserLib.dll";
        public static string LibMainFileName = "LibMain.cs";
        public static double DefaultPatternLength = 10d;
        public static int DefaultSampleRate = 44100;
        public static int DefaultNumberOfPatterns = 1;
        public static int DefaultNumberOfTracks = 4;
        public static double DefaultBPS = 120d;
        public static double DefaultPartLength = 3d;
        public static Song Song;
        public static int SelectedPatternIdx;
        public static Pattern SelectedPattern;
        public static bool HasChanges;
        public static string LastProjectPath;
        public static double Trackheight = 35d;
        public static double TrackPixelsPerSecond = 100d;
        public static double PlayPosition = 0d;
        public static Watchers Watchers { get; private set; } = new Watchers();
        public static List<string> RecentFiles { get; } = new List<string>();

        public static void OnApplicationEnded()
        {
            ApplicationEnded?.Invoke();
        }

        public static void OnDirtyChanged(bool dirty)
        {
            if (dirty != HasChanges)
            {
                HasChanges = dirty;

                DirtyChanged?.Invoke(HasChanges);
            }
        }

        public static void OnTimeChanged(TimeSpan time)
        {
            TimeChanged?.Invoke(time);
        }

        public static void OnSelectedPatternChanged(int idx)
        {
            Env.SelectedPatternIdx = idx;
            Env.SelectedPattern = idx >= 0 && idx < Env.Song.Patterns.Count ? Env.Song.Patterns[idx] : null;

            SelectedPatternChanged?.Invoke(Env.SelectedPattern);
        }

        public static void AddRecentFile(string projectPath)
        {
            LastProjectPath = projectPath;

            if (!RecentFiles.Contains(projectPath))
            {
                RecentFiles.Add(projectPath);
            }
        }

        public static string SampleRateToString(double value)
        {
            return value.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture);
        }
        public static string GainToString(double value)
        {
            return value.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string BPSToString(double value)
        {
            return value.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string TimeToString(double value)
        {
            return value.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string TimeToStringPrecision(double value, int precision)
        {
            return value.ToString($"0.{"0000".Substring(0,precision)}", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static bool TryParseDouble(string s, out double result)
        {
            return double.TryParse(s.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out result);
        }

        public static void AddOutput(string message)
        {
            OutputAdded?.Invoke(message);
        }
        public static void AddOutput(string title, string message)
        {
            OutputAdded?.Invoke($"{title} {message}");
        }
    }
}
