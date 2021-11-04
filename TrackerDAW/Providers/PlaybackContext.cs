﻿using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public class PlaybackContext : ISampleProvider
    {
        public Song Song { get; private set; }
        private ISampleProvider SampleProvider { get; set; }
        public WaveFormat WaveFormat => this.Song.WaveFormat;
        public int SamplePosition { get; set; }

        private Dictionary<object, IProvider> providers = new Dictionary<object, IProvider>();
        
        private PlaybackContext(Song song)
        {
            this.Song = song;
            this.SamplePosition = 0;
        }

        public static PlaybackContext CreateFromSong(Song song, double offset = 0d)
        {
            var context = new PlaybackContext(song);

            context.SampleProvider = context.CreateProvider(song, song.ProviderInfo, new ProviderData()
            {
                { ProviderDataKey.IStartAt, (int)-(offset*song.SampleRate) },
            });

            return context;
        }

        public static PlaybackContext CreateFromPattern(Song song, Pattern pattern, double offset = 0d)
        {
            var context = new PlaybackContext(song);

            context.SampleProvider = context.CreateProvider(pattern, pattern.ProviderInfo, new ProviderData()
            {
                { ProviderDataKey.Pattern, pattern },
                { ProviderDataKey.IStartAt, (int)-(offset * song.SampleRate) },
            });

            return context;
        }
         
        public IProvider CreateProvider(object source, ProviderInfo providerInfo, ProviderData providerData)
        {
            if (this.providers.ContainsKey(source))
            {
                return this.providers[source];
            }

            var providerClass = ProviderFactory.GetProviderClass(providerInfo);

            IProvider provider;

            if (providerClass == null)
            {
                provider = new EmptyProvider(this, $"Provider class {providerInfo.Name} not found");
            }
            else
            {
                provider = Activator.CreateInstance(providerClass, this, providerData) as IProvider;

                if (provider == null)
                {
                    provider = new EmptyProvider(this, $"Provider class {providerInfo.Name} instantiation failed");
                }
            }

            this.providers[source] = provider;

            return provider;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            var result = this.SampleProvider.Read(buffer, offset, count);
            
            this.SamplePosition += count;
            
            return result;
        }
    }
}
