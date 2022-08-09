﻿using Microsoft.Xna.Framework;
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
            Type = "rainSplash";
        }

        public override void Update(GameTime gameTime)
        {
            _animationManager.position.X = GameState.cup.Position.X;
            _animationManager.position.Y = GameState.cup.Position.Y*GameState.splashHeightMult;
            _animationManager.Play(_animations[Type]);

            if (_animationManager.PlayOnce(gameTime))
            {
                IsRemoved = true;
            }
            _animationManager.Update(gameTime);
        }
    }
}