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
    internal abstract class Drop : Sprite
    {
        public int PH { get; protected set; }
        public float DropSpeed { get; set; }
        public bool IsGlowing { get; protected set; }

        protected Drop(Dictionary<string, Animation> animations) : base(animations)
        {
            Layer = 0.85f;
            Scale = GameState.dropScale;
        }

        public override void Update(GameTime gameTime)
        {    
            UpdateAnimation(gameTime);
            Move(gameTime);
            RemoveDrop();
        }
        protected abstract void Move(GameTime gameTime);
        protected abstract void UpdateAnimation(GameTime gameTime);
        protected abstract void RemoveDrop();
        public bool IsCupCollision(Cup cup)
        {
            return Rect.Bottom > cup.Rect.Top + 10 &&
                Rect.Bottom < cup.Rect.Top + 30 &&
                (Position.X) >= (cup.Position.X - ((cup.Rect.Width) / 2)) &&
                (Position.X) <= (cup.Position.X + ((cup.Rect.Width) / 2));
        }
    }
}
