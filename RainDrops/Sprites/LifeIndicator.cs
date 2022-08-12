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
    internal class LifeIndicator : Sprite
    {
        public LifeIndicator(Dictionary<string, Animation> animations) : base(animations)
        {

        }

        public override void Update(GameTime gameTime)
        {
            _animationManager.Play(_animations["lifeDrop"]);
            _animationManager.Update(gameTime);
        }
    }
}
