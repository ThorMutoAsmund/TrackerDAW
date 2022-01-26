using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public class Note : Part
    {
        public const int ObjTypeValue = 2;

        [JsonProperty]
        public override int ObjType
        {
            get => ObjTypeValue;
            set { }
        }
        [JsonProperty] public string Content { get; set; }
        [JsonProperty] public double Length { get; set; }
        [JsonIgnore] public string NameWithDefaultValue => String.IsNullOrEmpty(this.Name) ? "(untitled)" : this.Name;

        /// <summary>
        /// Must not assign anything due to JSON deserialization
        /// </summary>
        private Note()
        {
        }

        public static Note CreateNew(double offset, double length, string name = "", string content = "")
        {
            return new Note()
            {
                Offset = offset,
                Length = length,
                Name = name,
                Content = content
            };
        }

        public override Part Clone()
        {
            return Note.CreateNew(this.Offset, this.Length, this.Name, this.Content);
        }

        public override double GetLength()
        {
            return this.Length;
        }
    }
}
