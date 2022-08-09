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
    internal abstract class Drop : Sprite
    {
        public int PH { get; protected set; }
        public float DropSpeed { get; set; }

        private float timer;
        private float velocity;
        private bool switchScale;
        protected Drop(Texture2D texture) : base(texture)
        {
            //Position = new Vector2(RainDropsGame.Random.Next((int)(texture.Width * scale / 2), (int)(RainDropsGame.ScreenWidth - texture.Width * scale / 2)), -texture.Height * scale / 2);
            //DropSpeed = RainDropsGame.Random.Next(200, 220);
            //velocity = DropSpeed;
        }

        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            velocity += (DropSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
            position.Y += Convert.ToInt32(velocity);
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
            {
                IsRemoved = true;
                if(this.PH == 7)
                {
                    GameState.lifeCount--;
                    if(GameState.lifeCount >= 0)
                        GameState.lives[GameState.lifeCount].IsRemoved = true;
                }
            }      
        }
        public bool IsCupCollision(Cup cup)
        {
            return Rect.Bottom > cup.Rect.Top + 15 &&
                Rect.Bottom < cup.Rect.Top + 30 &&
                (Position.X) >= (cup.Position.X - ((cup.Rect.Width * cup.Scale) / 2)) &&
                (Position.X) <= (cup.Position.X + ((cup.Rect.Width * cup.Scale) / 2));

        }
        protected bool IsDropCollision(Sprite sprite)
        {
            return this.Position.Y + this.Rect.Height/2 > sprite.Position.Y - sprite.Rect.Height/2 &&
                ((this.Position.X + this.Rect.Width/2 <= sprite.Position.X + sprite.Rect.Width/2 && this.Position.X + this.Rect.Width/2 >= sprite.Position.X - sprite.Rect.Width/2) ||
                (this.Position.X - this.Rect.Width/2 >= sprite.Position.X - sprite.Rect.Width/2 && this.Position.X - this.Rect.Width/2 <= sprite.Position.X + sprite.Rect.Width/2));
        }

    }
}
