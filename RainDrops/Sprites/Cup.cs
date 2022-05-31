using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Sprites
{
    internal class Cup : Sprite
    {
        public int dropsCaught = 0;
        public int phScore = 0;
        public Cup(GraphicsDevice graphics, Texture2D texture, float rotation, float scale, float layer) : base(graphics, texture, rotation, scale, layer)
        {
            Position = new Vector2(RainDropsGame.ScreenWidth / 2, RainDropsGame.ScreenHeight - texture.Height / 2);
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Move();
            foreach(var sprite in sprites)
            {
                if(sprite is Cup)
                    continue;

                if (this.IsCupCollision(sprite) && sprite is RainDrop)
                {
                    var rainDrop = sprite as RainDrop;
                    dropsCaught++;
                    phScore += rainDrop.PH;
                    sprite.IsRemoved = true;
                }
                if (this.IsCupCollision(sprite) && sprite is AcidRainDrop)
                {
                    var acidDrop = sprite as AcidRainDrop;
                    dropsCaught++;
                    phScore -= acidDrop.PH;
                    sprite.IsRemoved = true;
                }
                if (this.IsCupCollision(sprite) && sprite is AlkRainDrop)
                {
                    var alkDrop = sprite as AlkRainDrop;
                    dropsCaught++;
                    phScore -= alkDrop.PH;
                    sprite.IsRemoved = true;
                }


            }
        }

        private void Move()
        {
            Position.X = Mouse.GetState().Position.X;
            Position.X = MathHelper.Clamp(Position.X, Rect.Width / 2, RainDropsGame.ScreenWidth - Rect.Width/2);
        }
    }
}
