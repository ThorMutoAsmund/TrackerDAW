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
    public class ProviderFactory
    {
        public static ProviderFactory Default { get; } = new ProviderFactory();
        public Dictionary<string, ProviderRegistration> ProviderRegistrations { get; private set; } = new Dictionary<string, ProviderRegistration>();
       
        public ProviderFactory()
        {
            Song.SongChanged += Song_SongChanged;
        }

        private void Song_SongChanged(Song song, SongChangedAction action)
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

        public void RefreshProviders()
        {
            ProviderRegistrations.Clear();

            foreach (Type type in System.Reflection.Assembly.GetExecutingAssembly().GetTypes())
            {
                var customAttributes = type.GetCustomAttributes(typeof(ProviderRegistrationAttribute), true);
                if (customAttributes.Length > 0 && customAttributes[0] is ProviderRegistrationAttribute providerRegistrationAttribute)
                {
                    AddProvider(type, providerRegistrationAttribute.Version);
                }
            }
            //AddProvider<EmptyProvider>(ProviderInfo.EmptyProviderInfo);
            //AddProvider<DefaultSongProvider>(ProviderInfo.DefaultSongProviderInfo);
            //AddProvider<DefaultPatternProvider>(ProviderInfo.DefaultPatternProviderInfo);
            //AddProvider<DefaultTrackProvider>(ProviderInfo.DefaultTrackProviderInfo);
            //AddProvider<DefaultSampleProvider>(ProviderInfo.DefaultSampleProviderInfo);
            //AddProvider<DefaultCompositionProvider>(ProviderInfo.DefaultCompositionProviderInfo);
        }

        public void AddProvider<T>(int version)
        {
            AddProvider(typeof(T), version);
        }

        public void AddProvider(Type type, int version)
        {
            ProviderRegistrations.Add(type.FullName, new ProviderRegistration()
            {
                Name = type.Name,
                Type = type,
                Version = version
            });
        }

        public Type GetProviderClass(ProviderInfo providerInfo)
        {
            if (providerInfo != null && ProviderRegistrations.ContainsKey(providerInfo.Type))
            {
                return ProviderRegistrations[providerInfo.Type].Type;
            }

            return null;
        }
        
        public ProviderRegistration GetProviderRegistration(ProviderInfo providerInfo)
        {
            if (providerInfo != null && ProviderRegistrations.ContainsKey(providerInfo.Type))
            {
                return ProviderRegistrations[providerInfo.Type];
            }

            return null;
        }

        public void CreateBlankProviderScript(string className)
        {
            // Get content
            var blankProviderScriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Env.ProvidersSystemFolder, Env.BlankProviderFileName);

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
        //public ProviderInfo ProviderInfo { get; set; }
        public Type Type { get; set; }
        public int Version { get; set; }

        public ProviderInfo ToProviderInfo()
        {
            return ProviderInfo.CreateNew($"{this.Type}", this.Version);
        }
    }
}
