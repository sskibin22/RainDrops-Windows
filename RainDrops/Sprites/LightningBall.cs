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
    internal class LightningBall : Sprite
    {
        private int index = 1;
        public bool IsActive { get; set; }
        public LightningBall(Dictionary<string, Animation> animations) : base(animations)
        {
            Position = new Vector2(RainDropsGame.ScreenWidth / 2, RainDropsGame.ScreenHeight - GameState.phBarHeight - GameState.cupHeight/2); 
            Scale = 1f;
            Layer = 0.95f;
        }
        public void SetPosition()
        {
            index = RainDropsGame.Random.Next(1, GameState.numDropCols-2);
            X = GameState.dropCols[index];
        }

        public override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                _animationManager.Play(_animations["default"]);
                _animationManager.Update(gameTime);
            }
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsActive)
                _animationManager.Draw(spriteBatch);
        }
    }
}
