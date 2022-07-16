using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Tools
{
    internal class SpriteSheetData
    {
        public List<string> Keys { get; internal set; }
        public List<(int x, int y)> Coordinates { get; internal set; }
        public List<(int width, int height)> FrameSize { get; internal set; }

        public SpriteSheetData()
        {
            Keys = new List<string>();
            Coordinates = new List<(int x, int y)>();
            FrameSize = new List<(int width, int height)>();
        }

    }
}
