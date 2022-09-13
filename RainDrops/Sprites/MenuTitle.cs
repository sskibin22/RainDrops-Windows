using Microsoft.Xna.Framework;
using RainDrops.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Sprites
{
    internal class MenuTitle : Sprite
    {
        private bool startUpDone;
        public MenuTitle(Dictionary<string, Animation> animations) : base(animations)
        {
            Origin = new Vector2(_animationManager.CurrentAnimation.FrameWidth / 2, 0);
            Position = new Vector2(RainDropsGame.ScreenWidth / 2, 0);
            Layer = 1f;
            Scale = 1f;
            Opacity = 1f;
            startUpDone = false;
        }
        public override void Update(GameTime gameTime)
        {
            if (startUpDone)
            {
                _animationManager.Play(_animations["loop"]);
                _animationManager.Update(gameTime);
            }
            else
            {
                if (_animationManager.PlayOnce(_animations["startUp"], gameTime))
                {
                    startUpDone = true;
                    _animationManager.Play(_animations["loop"]);
                    _animationManager.Update(gameTime);
                }
            }
        }
    }
}
