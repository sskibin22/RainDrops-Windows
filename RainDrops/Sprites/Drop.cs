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
        public float Value { get; protected set; }
        public float DropSpeed { get; set; }

        private float timer;
        private bool switchScale;
        protected Drop(GraphicsDevice graphics, Texture2D texture, float rotation, float scale, float layer) : base(graphics, texture, rotation, scale, layer)
        {
            //Position = new Vector2(RainDropsGame.Random.Next((int)(texture.Width * scale / 2), (int)(RainDropsGame.ScreenWidth - texture.Width * scale / 2)), -texture.Height * scale / 2);
            //DropSpeed = RainDropsGame.Random.Next(200, 220);
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Position.Y += Convert.ToInt32(DropSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            if (this.PH == 0 || this.PH == 14)
            {
                if (timer > 10f)
                {
                    timer = 0f;
                    if (this.scale <= 0.75f)
                    {
                        switchScale = true;
                    }
                    if (this.scale >= 1.5f)
                    {
                        switchScale = false;
                    }

                    if (switchScale)
                    {
                        this.scale += .02f;
                    }
                    else
                    {
                        this.scale -= .02f;
                    }
                    

                }
            }
            if (Rect.Bottom >= RainDropsGame.ScreenHeight)
                IsRemoved = true;
        }
       
        protected bool IsDropCollision(Sprite sprite)
        {
            return this.Position.Y + this.Rect.Height/2 > sprite.Position.Y - sprite.Rect.Height/2 &&
                ((this.Position.X + this.Rect.Width/2 <= sprite.Position.X + sprite.Rect.Width/2 && this.Position.X + this.Rect.Width/2 >= sprite.Position.X - sprite.Rect.Width/2) ||
                (this.Position.X - this.Rect.Width/2 >= sprite.Position.X - sprite.Rect.Width/2 && this.Position.X - this.Rect.Width/2 <= sprite.Position.X + sprite.Rect.Width/2));
        }

    }
}
