using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechNickelDirector
{
    class Cue
    {
        public string FullFilename { get; set; }
        public string FileName => Path.GetFileName(FullFilename);
        public override string ToString()
        {
            return FullFilename;
        }
    }
}
