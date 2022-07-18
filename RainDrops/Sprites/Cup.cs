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
        private int frameCount = 0;
        public int dropsCaught = 0;
        public int acidDropsCaught = 0;
        public int alkDropsCaught = 0;
        public Cup(GraphicsDevice graphics, Texture2D texture, float rotation, float scale, float layer) : base(graphics, texture, rotation, scale, layer)
        {
            Position = new Vector2(RainDropsGame.ScreenWidth / 2, RainDropsGame.ScreenHeight - (texture.Height / 2) - RainDropsGame.phBarHeight);
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            Move();
            foreach(var sprite in sprites)
            {
                if(sprite is Cup)
                    continue;

                if (IsCupCollision(sprite) && sprite is RainDrop)
                {
                    frameCount++;
                    texture = RainDropsGame.rainCupTextures[frameCount];
                    var rainDrop = sprite as RainDrop;
                    dropsCaught++;
                    sprite.IsRemoved = true;
                }
                if (IsCupCollision(sprite) && sprite is AcidRainDrop)
                {
                    var acidDrop = sprite as AcidRainDrop;
                    //dropsCaught++;
                    acidDropsCaught++;
                    sprite.IsRemoved = true;
                    MovePHSelector(acidDrop);
                }
                if (IsCupCollision(sprite) && sprite is AlkRainDrop)
                {
                    var alkDrop = sprite as AlkRainDrop;
                    //dropsCaught++;
                    alkDropsCaught++;
                    sprite.IsRemoved = true;
                    MovePHSelector(alkDrop);
                }
            }
        }

        private void Move()
        {
            Position.X = Mouse.GetState().Position.X;
            Position.X = MathHelper.Clamp(Position.X, Rect.Width / 2, RainDropsGame.ScreenWidth - Rect.Width/2);
        }

        protected bool IsCupCollision(Sprite sprite)
        {
            return sprite.Rect.Bottom > this.Rect.Top + 15 &&
                sprite.Rect.Bottom < this.Rect.Top + 30 &&
                (sprite.Position.X) >= (this.Position.X - ((this.texture.Width * this.scale) / 2)) &&
                (sprite.Position.X) <= (this.Position.X + ((this.texture.Width * this.scale) / 2));

        }
        //Move PH selector a certain amount of pixels along the x-axis given the PH level of the drop caught
        private void MovePHSelector(Drop drop)
        {
            switch (drop.PH)
            {
                case 14:
                    RainDropsGame.phSelect.phSelected += 3;
                    if (RainDropsGame.phSelect.phSelected > 14)
                        RainDropsGame.phSelect.phSelected = 14;
                    break;
                case 11:
                    if(RainDropsGame.phSelect.phSelected != 14)
                        RainDropsGame.phSelect.phSelected += 1;                  
                    break;
                case 3:
                    if (RainDropsGame.phSelect.phSelected != 0)
                        RainDropsGame.phSelect.phSelected -= 1;
                    break;
                case 0:
                    RainDropsGame.phSelect.phSelected -= 3;
                    if (RainDropsGame.phSelect.phSelected < 0)
                        RainDropsGame.phSelect.phSelected = 0;
                    break;
                default:
                    break;
            }
        }
    }
}
