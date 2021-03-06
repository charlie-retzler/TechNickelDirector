﻿using System.IO;

namespace TechNickelDirector
{
    public class Cue
    {
        public string FullFilename { get; set; }
        public string FileName => Path.GetFileName(FullFilename);
        public override string ToString()
        {
            return FullFilename;
        }
    }
}
