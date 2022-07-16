using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Tools
{
    internal class SpriteSheetDataReader
    {
        public SpriteSheetData SpriteSheetData { get; private set; }
        public int FrameCount { get; private set; }

        public SpriteSheetDataReader(string filepath)
        {
            string[]? lines = File.ReadAllLines(filepath);
            FrameCount = lines.Length;
            SpriteSheetData = new SpriteSheetData();
            foreach (string line in lines)
            {
                string[]? data = line.Split(';');
                SpriteSheetData.Keys.Add(data[0]);
                (int x, int y) coords;
                coords.x = Int32.Parse(data[1]);
                coords.y = Int32.Parse(data[2]);
                (int width, int height) sizes;
                sizes.width = Int32.Parse(data[3]);
                sizes.height = Int32.Parse(data[4]);
                SpriteSheetData.Coordinates.Add(coords);
                SpriteSheetData.FrameSize.Add(sizes);
            }
        }
    }
}
