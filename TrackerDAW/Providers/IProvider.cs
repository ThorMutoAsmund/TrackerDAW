﻿using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public interface IProvider : ISampleProvider
    {
        string Title { get; }
        double Offset { get; }
    }
}
