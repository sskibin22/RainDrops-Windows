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
    internal class SplashAnimation : Sprite
    {
        public string Type { get; set; }
        public SplashAnimation(Dictionary<string, Animation> animations) : base(animations)
        {
            Scale = 1f;
            Type = "rainSplash";
        }

        public override void Update(GameTime gameTime)
        {
            X = GameState.cup.Position.X;
            Y = GameState.cup.Position.Y * GameState.splashHeightMult;
            //_animationManager.Play(_animations[Type]);

            if (_animationManager.PlayOnce(_animations[Type], gameTime))
            {
                IsRemoved = true;
            }
            //_animationManager.Update(gameTime);
        }
    }
}
