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
    internal class LightningBolt : Sprite
    {
        public bool IsActive { get; set; }
        private bool lightFlag;
        private float flashTimer;
        public LightningBolt(Dictionary<string, Animation> animations) : base(animations)
        {
            Origin = new Vector2(_animationManager.CurrentAnimation.FrameWidth / 2, _animationManager.CurrentAnimation.FrameHeight);
            Position = new Vector2(RainDropsGame.ScreenWidth / 2, RainDropsGame.ScreenHeight - GameState.phBarHeight - GameState.cupHeight/2);
            IsActive = false;
            Scale = 1f;
            Layer = 0.84f;
            lightFlag = true;
            flashTimer = 0;
        }
        public void SetXPosition(float x)
        {
            X = x;
        }

        public override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                flashTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if(flashTimer > 83.335f)
                {
                    flashTimer = 0;
                    if (lightFlag)
                    {
                        GameState.bg.ChangeOpacity(0.3f);
                        lightFlag = false;
                    }
                    else
                    {
                        GameState.bg.ChangeOpacity(1f);
                        lightFlag = true;
                    }  
                }
                if (_animationManager.PlayOnce(_animations["default"], gameTime))
                {
                    IsActive = false;
                    GameState.bg.ChangeOpacity(1f);
                    lightFlag = true;
                    flashTimer = 0f;
                }
            }  
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsActive)
                _animationManager.Draw(spriteBatch);
        }
    }
}
