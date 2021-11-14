using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;


namespace TrackerDAW
{
    public class Watchers
    {
        public List<string> SamplesList { get; private set; } = new List<string>();
        public List<string> ScriptsList { get; private set; } = new List<string>();

        public event Action<List<string>> SamplesListChanged;
        public event Action<List<string>> ScriptsListChanged;

        private FileSystemWatcher samplesWatcher;
        private FileSystemWatcher scriptsWatcher;
        private bool rescanScriptsHalted;

        public Watchers()
        {
            Song.SongChanged += (song, action) =>
            {
                if (action == SongChangedAction.Opened)
                {
                    ConfigureWatchers();
                    RescanSamples();
                    RescanSripts();
                }
                else if(action == SongChangedAction.Closed)
                {
                    StopWatchers();
                }
            };
        }

        private void ConfigureWatchers()
        {
            // Samples
            var context = SynchronizationContext.Current;
            this.samplesWatcher = new FileSystemWatcher()
            {
                Path = Env.Song.SamplesPath,
                NotifyFilter = NotifyFilters.FileName,
                Filter = "*.*",
            };
            this.samplesWatcher.Changed += (source, e) => context.Post(val => RescanSamples(), source);
            this.samplesWatcher.Created += (source, e) => context.Post(val => RescanSamples(), source);
            this.samplesWatcher.Deleted += (source, e) => context.Post(val => RescanSamples(), source);
            this.samplesWatcher.Renamed += (source, e) => context.Post(val => RescanSamples(), source);

            this.samplesWatcher.EnableRaisingEvents = true;

            // Scripts
            this.scriptsWatcher = new FileSystemWatcher()
            {
                Path = Env.Song.ScriptsPath,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
                Filter = "*.cs",
            };
            this.scriptsWatcher.Changed += (source, e) => context.Post(val => RescanSripts(true), source);
            this.scriptsWatcher.Created += (source, e) => context.Post(val => RescanSripts(), source);
            this.scriptsWatcher.Deleted += (source, e) => context.Post(val => RescanSripts(), source);
            this.scriptsWatcher.Renamed += (source, e) => context.Post(val => RescanSripts(), source);

            this.scriptsWatcher.EnableRaisingEvents = true;
        }

        private void StopWatchers()
        {
            if (this.samplesWatcher != null)
            {
                this.samplesWatcher.EnableRaisingEvents = false;
                this.samplesWatcher = null;
                this.scriptsWatcher.EnableRaisingEvents = false;
                this.scriptsWatcher = null;

                this.SamplesList.Clear();
                this.ScriptsList.Clear();
            }
        }

        private void RescanSamples()
        {
            try
            {
                this.SamplesList = Directory.EnumerateFiles(Env.Song.SamplesPath, "*.*").Select(x => System.IO.Path.GetFileName(x)).ToList();
            }
            catch (Exception)
            {
                this.SamplesList.Clear();
            }
            this.SamplesListChanged?.Invoke(this.SamplesList);
        }

        private void RescanSripts(bool halt = false)
        {
            if (halt)
            {
                if (this.rescanScriptsHalted)
                {
                    return;
                }

                this.rescanScriptsHalted = true;

                var timer1 = new System.Windows.Forms.Timer();
                timer1.Interval = 1000;
                timer1.Enabled = true;
                timer1.Tick += (s, e) =>
                {
                    this.rescanScriptsHalted = false;
                    timer1.Enabled = false;
                    timer1.Stop();
                };
                timer1.Start();
            }

            try
            {
                this.ScriptsList = Directory.EnumerateFiles(Env.Song.ScriptsPath, "*.cs").Select(x => System.IO.Path.GetFileName(x)).ToList();
            }
            catch (Exception)
            {
                this.ScriptsList.Clear();
            }
            this.ScriptsListChanged?.Invoke(this.ScriptsList);
        }
    }
}
