using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RainDrops.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RainDrops.States;

namespace RainDrops.Sprites
{
    internal class PowerUp : Drop
    {
        public PowerUp(Dictionary<string, Animation> animations) : base(animations)
        {
            PH = -1;
            IsGlowing = false;
        }
        protected override void Move(GameTime gameTime)
        {
            Velocity.Y += (DropSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds);
            Position += Velocity;
        }
        protected override void UpdateAnimation(GameTime gameTime)
        {
            _animationManager.Play(_animations["lightningShield"]);
            _animationManager.Update(gameTime);
        }
        protected override void RemoveDrop()
        {
            if (Rect.Bottom >= RainDropsGame.ScreenHeight)
            {
                IsRemoved = true;
            }
        }
    }
}
