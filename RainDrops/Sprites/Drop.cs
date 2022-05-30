using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Sprites
{
    internal abstract class Drop : Sprite
    {
        public int PH { get; protected set; }
        public float DropSpeed { get; protected set; }
        protected Drop(GraphicsDevice graphics, Texture2D texture, float rotation, float scale, float layer) : base(graphics, texture, rotation, scale, layer)
        {
            Position = new Vector2(RainDropsGame.Random.Next((int)(texture.Width * scale / 2), (int)(RainDropsGame.ScreenWidth - texture.Width * scale / 2)), -texture.Height * scale / 2);
            DropSpeed = RainDropsGame.Random.Next(150, 250);
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Position.Y += Convert.ToInt32(DropSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (Rect.Bottom >= RainDropsGame.ScreenHeight)
                IsRemoved = true;
        }
    }
}
