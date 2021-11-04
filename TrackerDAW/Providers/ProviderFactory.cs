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
        private static Dictionary<string, Type> providerClasses = new Dictionary<string, Type>();
       
        static ProviderFactory()
        {
            providerClasses.Add(ProviderInfo.EmptyProviderInfo.Name, typeof(EmptyProvider));
            providerClasses.Add(ProviderInfo.DefaultSongProviderInfo.Name, typeof(DefaultSongProvider));
            providerClasses.Add(ProviderInfo.DefaultPatternProviderInfo.Name, typeof(DefaultPatternProvider));
            providerClasses.Add(ProviderInfo.DefaultTrackProviderInfo.Name, typeof(DefaultTrackProvider));
            providerClasses.Add(ProviderInfo.DefaultSampleProviderInfo.Name, typeof(DefaultSampleProvider));
            providerClasses.Add(ProviderInfo.DefaultCompositionProviderInfo.Name, typeof(DefaultCompositionProvider));
        }

        public static Type GetProviderClass(ProviderInfo providerInfo)
        {
            if (providerInfo != null && providerClasses.ContainsKey(providerInfo.Name))
            {
                return providerClasses[providerInfo.Name];
            }

            return null;
        } 
    }
}
