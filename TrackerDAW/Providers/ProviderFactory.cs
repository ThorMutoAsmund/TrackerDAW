using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TrackerDAW
{
    public static class ProviderFactory
    {        
        public static Dictionary<string, ProviderRegistration> ProviderRegistrations { get; private set; } = new Dictionary<string, ProviderRegistration>();
       
        static ProviderFactory()
        {
            Song.SongChanged += Song_SongChanged;
        }

        private static void Song_SongChanged(Song song, SongChangedAction action)
        {
            if (action == SongChangedAction.Opened)
            {
                RefreshProviders();
            }
            else if (action == SongChangedAction.Closed)
            {
                ProviderRegistrations.Clear();
            }
            //if (song == null)
            //{
            //    this.DataContext = null;
            //}
        }

        public static void RefreshProviders()
        {
            ProviderRegistrations.Clear();

            AddProvider<EmptyProvider>(ProviderInfo.EmptyProviderInfo);
            AddProvider<DefaultSongProvider>(ProviderInfo.DefaultSongProviderInfo);
            AddProvider<DefaultPatternProvider>(ProviderInfo.DefaultPatternProviderInfo);
            AddProvider<DefaultTrackProvider>(ProviderInfo.DefaultTrackProviderInfo);
            AddProvider<DefaultSampleProvider>(ProviderInfo.DefaultSampleProviderInfo);
            AddProvider<DefaultCompositionProvider>(ProviderInfo.DefaultCompositionProviderInfo);
        }

        public static void AddProvider<T>(ProviderInfo providerInfo)
        {
            ProviderRegistrations.Add(providerInfo.Name, new ProviderRegistration()
            {
                Name = typeof(T).Name,
                Type = typeof(T),
                ProviderInfo = providerInfo
            });            
        }

        public static Type GetProviderClass(ProviderInfo providerInfo)
        {
            if (providerInfo != null && ProviderRegistrations.ContainsKey(providerInfo.Name))
            {
                return ProviderRegistrations[providerInfo.Name].Type;
            }

            return null;
        }
        public static ProviderRegistration GetProviderRegistration(ProviderInfo providerInfo)
        {
            if (providerInfo != null && ProviderRegistrations.ContainsKey(providerInfo.Name))
            {
                return ProviderRegistrations[providerInfo.Name];
            }

            return null;
        }

        public static void CreateBlankProviderScript(string className)
        {
            // Get content
            var blankProviderScriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Providers", "BlankProvider.cs");

            try
            {
                var blankProviderText = File.ReadAllText(blankProviderScriptPath);

                var newFileName = $"{className}.cs";
                var newProviderScriptPath = Path.Combine(Env.Song.ScriptsPath, newFileName);
                if (File.Exists(newProviderScriptPath))
                {
                    MessageBox.Show($"Script with the name '{newFileName}' already exists");
                    return;
                }

                File.WriteAllText(newProviderScriptPath, blankProviderText);                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

        }
    }

    public class ProviderRegistration
    {
        public string Name { get; set; }
        public ProviderInfo ProviderInfo { get; set; }
        public Type Type { get; set; }
    }
}
