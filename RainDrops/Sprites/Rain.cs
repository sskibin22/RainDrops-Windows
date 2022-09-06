using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RainDrops.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Sprites
{
    internal class Rain : Sprite
    {
        public Rain(Texture2D texture) : base(texture)
        {
            Layer = 0.81f;
        }
        public override void Update(GameTime gameTime)
        {
            Position += Velocity;
            if (Rect.Top > RainDropsGame.ScreenHeight - GameState.phBarHeight)
            {
                IsRemoved = true;
            }
        }
    }
}
