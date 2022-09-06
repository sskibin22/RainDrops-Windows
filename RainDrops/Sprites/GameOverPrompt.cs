using Microsoft.Xna.Framework;
using RainDrops.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Sprites
{
    internal class GameOverPrompt : Sprite
    {
        public GameOverPrompt(Dictionary<string, Animation> animations) : base(animations)
        {
            Position = new Vector2(RainDropsGame.ScreenWidth / 2, RainDropsGame.ScreenHeight / 2);
            Layer = 0.95f;
            Scale = 0f;
            Opacity = 0.8f;
        }

        public override void Update(GameTime gameTime)
        {
            _animationManager.Play(_animations["shine"]);
            _animationManager.Update(gameTime);

            if (Scale < 1f)
            {
                Scale += 0.01f;        
            }
            else
            {
                return;
            }
        }
    }
}
