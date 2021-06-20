
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public class EmptyProvider : IProvider
    {
        public static readonly EmptyProvider Instance = new EmptyProvider();
        public string Title => "empty";

        private EmptyProvider()
        {
        }
    }
}
