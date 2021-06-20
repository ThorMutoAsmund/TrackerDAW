using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public static class Env
    {
        public static event Action<bool> DirtyChanged;
        public static event Action<Pattern> SelectedPatternChanged;

        public static string AppName = "TrackerDAW";
        public static string ApplicationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static string SamplesFolder = "Samples";
        public static string ScriptsFolder = "Scripts";
        public static string ProjectFileName = "Project";
        public static string DefaultProjectFileName = "default_project";
        public static string DefaultProjectName = "Default project";
        public static double DefaultPatternLength = 10d;
        public static double DefaultSampleRate = 44100d;
        public static int DefaultNumberOfPatterns = 1;
        public static int DefaultNumberOfTracks = 4;
        public static double DefaultPartHeight = 24d;
        public static double DefaultBPS = 120d;
        public static Song Song;
        public static int SelectedPatternIdx;
        public static Pattern SelectedPattern;
        public static bool HasChanges;
        public static string LastProjectPath;
        public static double TrackPixelsPerSecond = 100d;
        public static Watchers Watchers { get; private set; } = new Watchers();
        public static List<string> RecentFiles { get; } = new List<string>();
        public static void OnDirtyChanged(bool dirty)
        {
            if (dirty != HasChanges)
            {
                HasChanges = dirty;

                DirtyChanged?.Invoke(HasChanges);
            }
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

    }
}
