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
    internal class RainDrop : Drop
    {
        public RainDrop(Dictionary<string, Animation> animations) : base(animations)
        {
            PH = 7;
            IsGlowing = false;
        }
        protected override void UpdateAnimation(GameTime gameTime)
        {
            _animationManager.Play(_animations["standardRain"]);
            _animationManager.Update(gameTime);
        }
        protected override void RemoveDrop()
        {
            if (Rect.Bottom >= RainDropsGame.ScreenHeight)
            {
                IsRemoved = true;
                GameState.rainDropCount--;
                GameState.dropCount--;
                GameState.lifeCount--;
                GameState.statManager.IncreaseMissedDropTotal();
                if (GameState.lifeCount >= 0)
                    GameState.lives[GameState.lifeCount].IsRemoved = true;
            }
            
        }


    }
}
