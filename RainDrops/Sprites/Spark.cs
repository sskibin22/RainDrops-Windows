using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RainDrops.States;

namespace RainDrops.Sprites
{
    internal class Spark : Sprite
    {
        private float timer = 0;
        public int lifespan;
        public Spark(Texture2D texture) : base(texture)
        {
            Layer = 0.85f;
        }
        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer >= lifespan)
            {
                IsRemoved = true;
                timer = 0;
            }
            
            X += (Velocity.X * (timer/1000));
            Y -= (float)((Velocity.Y * (timer / 1000)) + (-4 * Math.Pow((timer / 1000), 2)));
            Opacity -= 0.009f;
            
        }
    }
}
