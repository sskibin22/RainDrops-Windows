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
        }
        private void Move()
        {
            //int normX = Mouse.GetState().Position.X / RainDropsGame.ScreenWidth;
            //int normY = Mouse.GetState().Position.Y / RainDropsGame.ScreenHeight;
            //RainDropsGame.mousePos.X = normX * RainDropsGame.window.ClientBounds.Width;
            //RainDropsGame.mousePos.Y = normY * RainDropsGame.window.ClientBounds.Height;

            X = RainDropsGame.scaledMouse.X;
            X = MathHelper.Clamp(Position.X, Rect.Width / 2, RainDropsGame.ScreenWidth - Rect.Width/2);
            
        }
    }
}
