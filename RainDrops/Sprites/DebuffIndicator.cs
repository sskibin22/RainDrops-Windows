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
    internal class DebuffIndicator : Sprite
    {
        public bool isActive;
        public DebuffIndicator(Dictionary<string, Animation> animations) : base(animations)
        {
            isActive = false;   
        }
        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                _animationManager.Play(_animations["default"]);
                _animationManager.Update(gameTime);
            }
            else
            {
                _animationManager.Stop();
            }
            
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(isActive)
                _animationManager.Draw(spriteBatch);
        }
    }
}
