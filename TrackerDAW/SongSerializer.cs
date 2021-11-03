using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;

namespace TrackerDAW
{
    public class PartSpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(Part).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null;
            return base.ResolveContractConverter(objectType);
        }
    }

    public class BaseConverter : JsonConverter
    {
        private static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new PartSpecifiedConcreteClassConverter() };

        public override bool CanWrite => false;
        public override bool CanConvert(Type objectType) => objectType == typeof(Part);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var objType = jObject[nameof(Part.ObjType)].Value<int>();
            
            switch (objType)
            {
                case Composition.ObjTypeValue:
                    return JsonConvert.DeserializeObject<Composition>(jObject.ToString(), SpecifiedSubclassConversion);
                case Note.ObjTypeValue:
                    return JsonConvert.DeserializeObject<Note>(jObject.ToString(), SpecifiedSubclassConversion);
                case Sample.ObjTypeValue:
                    return JsonConvert.DeserializeObject<Sample>(jObject.ToString(), SpecifiedSubclassConversion);
                default:
                    throw new JsonSerializationException($"Cannot deserialize part with ObjType {objType}");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }
    }

    public class SongSerializer
    {
        public static int CurrentVersion = 1;

        public Song Song { get; set; }

        private static JsonConverter[] Converters = { new BaseConverter() };
        public SongSerializer()
        {
        }

        private void WriteToFile(string projectFilePath)
        {
            this.Song.Version = CurrentVersion;

            string json = JsonConvert.SerializeObject(this, Formatting.Indented);

            File.WriteAllText(projectFilePath, json);
        }


        public static SongSerializer FromFile(string projectFilePath)
        {
            var json = File.ReadAllText(projectFilePath);
            return JsonConvert.DeserializeObject<SongSerializer>(json, new JsonSerializerSettings() { Converters = Converters });
        }

        public static void ToFile(Song song)
        {
            new SongSerializer()
            {
                Song = song,
            }.WriteToFile(song.ProjectFilePath);
        }
    }
}
