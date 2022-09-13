using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Models
{
    internal class Animation
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public int CurrentFrame { get; set; }
        public int FrameCount { get; set; }
        public int FrameHeight { get { return Texture.Height / Rows; } }
        public float FrameSpeed { get; set; }
        public int FrameWidth { get { return Texture.Width / Cols; } }
        public bool IsLooping { get; set; }
        public Texture2D Texture { get; private set; }
        public Animation(Texture2D texture, int frameCount)
        {
            Texture = texture;
            FrameCount = frameCount;
            Rows = 1;
            Cols = FrameCount;
            IsLooping = true;
            CurrentFrame = 0; 

        }
        public Animation(Texture2D texture, int frameCount, int rows, int cols)
        {
            Texture = texture;
            FrameCount = frameCount;
            Rows = rows;
            Cols = cols;
            Row = 0;
            Col = 0;
            IsLooping = true;
            CurrentFrame = 0;
        }
    }
}
