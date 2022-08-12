using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RainDrops.Models;
using RainDrops.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Sprites
{
    internal class Cup : Sprite
    {
        public Cup(Dictionary<string, Animation> animations) : base(animations)
        {
            Scale = 0.9f;
            Position = new Vector2(RainDropsGame.ScreenWidth / 2, RainDropsGame.ScreenHeight - (Rect.Height / 2) - GameState.phBarHeight);
        }

        public override void Update(GameTime gameTime)
        {
            Move();
        }
        public void IncreaseFrame()
        {
            _animationManager.NextFrame();
        }
        public void Reset()
        {
            _animationManager.Stop();
            X = RainDropsGame.ScreenWidth / 2;
        }
        private void Move()
        {
            X = Mouse.GetState().Position.X;
            X = MathHelper.Clamp(Position.X, Rect.Width / 2, RainDropsGame.ScreenWidth - Rect.Width/2);
            
        }
    }
}
