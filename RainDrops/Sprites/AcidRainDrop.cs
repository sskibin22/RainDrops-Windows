using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RainDrops.Models;
using RainDrops.States;
using RainDrops.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Sprites
{
    internal class AcidRainDrop : Drop
    {
        public AcidRainDrop(Dictionary<string, Animation> animations) : base(animations)
        {
            WeightedRandomExecutor wre = new WeightedRandomExecutor(
                new WeightedRandomParam(() => { IsGlowing = true; PH = 0; }, 20), // 20% chance for PH to be 0
                new WeightedRandomParam(() => { IsGlowing = false; PH = 3; }, 80));   // 80% chance for PH to be 3
            wre.Execute();
        }
        protected override void UpdateAnimation(GameTime gameTime)
        {
            if (IsGlowing)
            {
                _animationManager.Play(_animations["glowingAcid"]);
            }
            else
            {
                _animationManager.Play(_animations["standardAcid"]);
            }
            _animationManager.Update(gameTime);
        }

        protected override void RemoveDrop()
        {
            if (Rect.Bottom >= RainDropsGame.ScreenHeight)
            {
                IsRemoved = true;
                GameState.dropCount--;
            }
        }

        
    }
}
