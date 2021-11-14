using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public class ProviderRegistrationAttribute : Attribute
    {
        public int Version { get; set; }
        public ProviderRegistrationAttribute(int version)
        {
            this.Version = version;
        }
    }
}
