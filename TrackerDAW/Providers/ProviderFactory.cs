using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public static class ProviderFactory
    {        
        public static ProviderInfo EmptyProviderInfo = ProviderInfo.CreateDefaultProvider<EmptyProvider>(0);
        public static ProviderInfo DefaultSongProviderInfo = ProviderInfo.CreateDefaultProvider<DefaultSongProvider>(DefaultSongProvider.Version);
        public static ProviderInfo DefaultPatternProviderInfo = ProviderInfo.CreateDefaultProvider<DefaultPatternProvider>(DefaultPatternProvider.Version);
        public static ProviderInfo DefaultTrackProviderInfo = ProviderInfo.CreateDefaultProvider<DefaultTrackProvider>(DefaultTrackProvider.Version);
        public static ProviderInfo DefaultSampleProviderInfo = ProviderInfo.CreateDefaultProvider<DefaultSampleProvider>(DefaultSampleProvider.Version);

        private static Dictionary<string, Type> providerClasses = new Dictionary<string, Type>();
        
        private static Dictionary<object, IProvider> providers = new Dictionary<object, IProvider>();


        static ProviderFactory()
        {
            providerClasses.Add(DefaultSongProviderInfo.Name, typeof(DefaultSongProvider));
            providerClasses.Add(DefaultPatternProviderInfo.Name, typeof(DefaultPatternProvider));
            providerClasses.Add(DefaultTrackProviderInfo.Name, typeof(DefaultTrackProvider));
            providerClasses.Add(DefaultSampleProviderInfo.Name, typeof(DefaultSampleProvider));
        }

        public static void AddDefaultProviders(Song song)
        {
            //song.Providers.Add(DefaultSongProviderInfo);
            //song.Providers.Add(DefaultPatternProviderInfo);
            //song.Providers.Add(DefaultTrackProviderInfo);
            //song.Providers.Add(DefaultSampleProviderInfo);
        }

        public static ISampleProvider InitFromSong(Song song)
        {
            providers.Clear();
            return Create(song, song, song.ProviderInfo, new ProviderData()
            {
            });
        }

        public static ISampleProvider InitFromPattern(Song song, Pattern pattern)
        {
            providers.Clear();
            
            return Create(song, pattern, pattern.ProviderInfo, new ProviderData()
            {
                { ProviderData.PatternKey, pattern },
                { ProviderData.OffsetKey, 0d },
            });
        }

        public static ISampleProvider Create(Song song, object source, ProviderInfo providerInfo, ProviderData providerData)
        {
            if (providers.ContainsKey(source))
            {
                return providers[source];
            }

            var providerClass = GetProviderClass(providerInfo);

            IProvider provider;

            if (providerClass == null)
            {
                provider = new EmptyProvider(song, $"Provider class {providerInfo.Name} not found");
            }
            else
            {
                provider = Activator.CreateInstance(providerClass, song, providerData) as IProvider;

                if (provider == null)
                {
                    provider = new EmptyProvider(song, $"Object instantiated from {providerInfo.Name} is not an IProvider");
                }
            }

            providers[source] = provider;

            return provider;
        }

        private static Type GetProviderClass(ProviderInfo providerInfo)
        {
            if (providerClasses.ContainsKey(providerInfo.Name))
            {
                return providerClasses[providerInfo.Name];
            }

            return null;
        }
    }
}
