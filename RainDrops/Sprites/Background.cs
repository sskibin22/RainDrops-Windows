using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RainDrops.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Sprites
{
    internal class Background : Sprite
    {
        public Background(Dictionary<string, Animation> animations) : base(animations)
        {
            Layer = 0f;
            Scale = 1f;
            Position = new Vector2(RainDropsGame.ScreenWidth / 2, RainDropsGame.ScreenHeight / 2);
        }
        public Background(Texture2D texture) : base(texture)
        {
            Layer = 0f;
            Scale = 1f;
            Position = new Vector2(RainDropsGame.ScreenWidth / 2, RainDropsGame.ScreenHeight / 2);
        }

        public void ChangeOpacity(float op)
        {
            Opacity = op;
        }

        public override void Update(GameTime gameTime)
        {
            if(_animations != null)
            {
                _animationManager.Play(_animations["default"]);
                _animationManager.Update(gameTime);
            }
        }
    }
}
