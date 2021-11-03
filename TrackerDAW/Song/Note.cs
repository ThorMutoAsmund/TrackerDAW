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
        [JsonIgnore] public string NameWithDefaultValue => String.IsNullOrEmpty(this.Name) ? "(untitled)" : this.Name;
        
        public Note()
        {
            this.Offset = 0d;
        }

        public Note(double offset, string name = "", string content = "")
        {
            this.Offset = offset;
            this.Name = name;
            this.Content = content;
        }

        public override Part Clone()
        {
            return new Note(this.Offset, this.Name, this.Content);
        }

        public override float GetLength()
        {
            return 3f;
        }
    }
}
