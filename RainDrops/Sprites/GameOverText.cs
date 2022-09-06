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
    internal class GameOverText : Sprite
    {
        public bool isActive;
        public GameOverText(Dictionary<string, Animation> animations) : base(animations)
        {
            isActive = false;
            Position = new Vector2(RainDropsGame.ScreenWidth / 2, (RainDropsGame.ScreenHeight / 2) - 170); ;
            Scale = 1.5f;
            Layer = 1f;
        }

        public override void Update(GameTime gameTime)
        {
            _animationManager.Play(_animations["default"]);
            _animationManager.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (isActive)
                _animationManager.Draw(spriteBatch);
        }
    }
}
