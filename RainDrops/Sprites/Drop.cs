using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RainDrops.Models;
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
        protected int animationNum;
        protected List<string> animationKeys = new() { "rainDrop", "acidDrop", "alkDrop"};
        protected Drop(Dictionary<string, Animation> animations) : base(animations)
        {
            Layer = 0.85f;

        }

        public override void Update(GameTime gameTime)
        {
            if(PH == 0)
            {
                _animationManager.Play(_animations["acidGlow"]);
            }
            else if(PH == 14)
            {
                _animationManager.Play(_animations["alkGlow"]);
            }
            else
            {
                _animationManager.Play(_animations[animationKeys[animationNum]]);
            }
            _animationManager.Update(gameTime);
            
            Velocity.Y += (DropSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
            Position += Velocity;

            if (Rect.Bottom >= RainDropsGame.ScreenHeight)
            {
                IsRemoved = true;
                GameState.dropCount--;
                if(PH == 7)
                {
                    GameState.lifeCount--;
                    GameState.statManager.IncreaseMissedDropTotal();
                    if(GameState.lifeCount >= 0)
                        GameState.lives[GameState.lifeCount].IsRemoved = true;
                }
            }      
        }
        public bool IsCupCollision(Cup cup)
        {
            return Rect.Bottom > cup.Rect.Top + 10 &&
                Rect.Bottom < cup.Rect.Top + 30 &&
                (Position.X) >= (cup.Position.X - ((cup.Rect.Width) / 2)) &&
                (Position.X) <= (cup.Position.X + ((cup.Rect.Width) / 2));
        }
    }
}
