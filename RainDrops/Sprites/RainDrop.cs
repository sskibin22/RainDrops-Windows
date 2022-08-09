using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Sprites
{
    internal class RainDrop : Drop
    {
        public RainDrop(Texture2D texture) : base(texture)
        {
            PH = 7;
        }
        


    }
}
