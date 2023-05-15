using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RainDrops.Sprites;
using RainDrops.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Events
{
    internal class LightningEvent
    {
        public float EventTimer { get; private set; }
        public bool EventActive { get; private set; }
        private LightningBolt _lightningBolt;
        private LightningBall _lightningBall;
        private float ballTimer;
        private int boltLimit;
        private int start;
        private bool shieldUsed = false;

        public LightningEvent(LightningBolt lightningBolt, LightningBall lightningBall)
        {
            start = 0;
            EventActive = false;
            SetEventTimer();
            ballTimer = 0f;
            _lightningBolt = lightningBolt;
            _lightningBall = lightningBall;

            
        }
        private void SetEventTimer()
        {
            EventTimer = RainDropsGame.Random.Next(10000, 20000);
        }
        public void Start()
        {
            if(start == 0)
            {
                EventActive = true;
                _lightningBall.SetPosition();
                _lightningBolt.SetXPosition(_lightningBall.Position.X);
                boltLimit = RainDropsGame.Random.Next(1000, 3000);
                _lightningBall.IsActive = true;
            }
            start = 1;
        }

        public void Stop()
        {
            SetEventTimer();
            start = 0;
            ballTimer = 0f;
            _lightningBall.IsActive = false;
            _lightningBolt.IsActive = false;
        }

        public void Update(GameTime gameTime, Cup cup)
        {
            if (EventActive)
            {   
                _lightningBall.Update(gameTime);
                ballTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if(ballTimer >= boltLimit)
                {
                    _lightningBolt.IsActive = true;
                    _lightningBolt.Update(gameTime);
                    if (_lightningBall.Rect.Intersects(cup.Rect))
                    {
                        if (cup.lightningShield)
                        {
                            _lightningBolt.cupShielded = true;
                            GameState.sparksEmitter.isActive = true;
                            shieldUsed = true;
                            //cup.lightningShield = false;
                        }
                        else
                        {
                            _lightningBolt.cupShielded = false;
                            cup.Stun();
                        }
                    }
                    else
                    {
                        GameState.sparksEmitter.isActive = false;
                        _lightningBolt.cupShielded = false;
                    }
                    if (_lightningBolt.IsActive == false)
                    {
                        if (shieldUsed)
                        {
                            shieldUsed = false;
                            cup.lightningShield = false;
                            GameState.deBuffManager.DeactivateLightningShieldBuff();
                        }
                        GameState.sparksEmitter.isActive = false;
                        EventActive = false;
                    }
                }
            }
            else
            {
                GameState.sparksEmitter.isActive = false;
            }

        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _lightningBall.Draw(gameTime, spriteBatch);
            _lightningBolt.Draw(gameTime, spriteBatch);
        }
    }
}
