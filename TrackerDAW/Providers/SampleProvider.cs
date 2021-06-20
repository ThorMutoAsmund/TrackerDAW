using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public class SampleProvider : IProvider
    {
        private string sampleName;

        public string Title => this.sampleName;

        public SampleProvider(string sampleName)
        {
            this.sampleName = sampleName;
        }
    }
}
