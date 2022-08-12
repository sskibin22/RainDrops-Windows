using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RainDrops.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Sprites
{
    internal class RainDrop : Drop
    {
        public RainDrop(Dictionary<string, Animation> animations) : base(animations)
        {
            PH = 7;
            animationNum = 0;
        }
    }
}
