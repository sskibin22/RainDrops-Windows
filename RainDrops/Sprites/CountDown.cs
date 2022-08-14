using Microsoft.Xna.Framework;
using RainDrops.Models;
using RainDrops.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Sprites
{
    internal class CountDown : Sprite
    {
        private float timer = 0f;
        public CountDown(Dictionary<string, Animation> animations) : base(animations)
        {
            Position = new Vector2(RainDropsGame.ScreenWidth / 2, RainDropsGame.ScreenHeight / 2);
            Scale = 2f;
        }
        public override void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(timer < 1000f)
            {
                _animationManager.Play(_animations["three"]);
            }
            else if(timer >= 1000f && timer < 2000f)
            {
                _animationManager.Play(_animations["two"]);
            }
            else if (timer >= 2000f && timer < 3000f)
            {
                _animationManager.Play(_animations["one"]);
            }
            else if (timer >= 3000f && timer < 4000f)
            {
                _animationManager.Play(_animations["rain"]);
                Origin = new Vector2(_animationManager.CurrentAnimation.FrameWidth / 2, _animationManager.CurrentAnimation.FrameHeight / 2);
            }
            else
            {
                IsRemoved = true;
                GameState.hasStarted = true;
            }
            _animationManager.Update(gameTime);
        }

        public void Reset()
        {
            _animationManager.Play(_animations["three"]);
            Origin = new Vector2(_animationManager.CurrentAnimation.FrameWidth / 2, _animationManager.CurrentAnimation.FrameHeight / 2);
            timer = 0f;
            IsRemoved = false;
        }
    }
}
