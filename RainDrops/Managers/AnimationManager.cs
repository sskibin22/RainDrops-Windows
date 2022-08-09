using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RainDrops.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Managers
{
    internal class AnimationManager
    {
        private Animation _animation;

        private float _timer;
        private float _timer2;
        public Vector2 position;
        public Animation CurrentAnimation { get { return _animation; } }
        public float Layer { get; set; }
        public Vector2 Origin { get; set; }
        //public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public AnimationManager(Animation animation)
        {
            _animation = animation;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_animation.Texture, position, new Rectangle(_animation.CurrentFrame * _animation.FrameWidth, 0, _animation.FrameWidth, _animation.FrameHeight), Color.White, Rotation, Origin, _animation.Scale, SpriteEffects.None, Layer);
        }

        public void Play(Animation animation)
        {
            if (_animation == animation)
            {
                return;
            }
            _animation = animation;
            _animation.CurrentFrame = 0;
            _timer = 0;
        }

        public bool PlayOnce(GameTime gameTime)
        {
            _timer2 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_timer2 > CurrentAnimation.FrameCount * CurrentAnimation.FrameSpeed)
            {
                Stop();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Stop()
        {
            _timer = 0f;
            //_animation.CurrentFrame = 0;
        }

        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_timer > _animation.FrameSpeed)
            {
                _timer = 0f;
                _animation.CurrentFrame++;
                if (_animation.CurrentFrame >= _animation.FrameCount)
                {
                    _animation.CurrentFrame = 0;
                }
            }
        }
    }
}
