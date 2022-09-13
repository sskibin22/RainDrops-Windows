using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RainDrops.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
        private float _timer3;
        public Animation CurrentAnimation { get { return _animation; } }
        public float Layer { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Position { get; set; }
        public float Scale { get; set; }
        public float Rotation { get; set; }
        public Color Color { get; set; }
        public float Opacity { get; set; }
        public AnimationManager(Animation animation)
        {
            _animation = animation;
            Opacity = 1f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(CurrentAnimation.Rows < 2)
            {
                spriteBatch.Draw(_animation.Texture, Position, new Rectangle(_animation.CurrentFrame * _animation.FrameWidth, 0, _animation.FrameWidth, _animation.FrameHeight), Color * Opacity, Rotation, Origin, Scale, SpriteEffects.None, Layer);
            }
            else
            {
                spriteBatch.Draw(_animation.Texture, Position, new Rectangle(_animation.Col * _animation.FrameWidth, _animation.Row * _animation.FrameHeight, _animation.FrameWidth, _animation.FrameHeight), Color * Opacity, Rotation, Origin, Scale, SpriteEffects.None, Layer);
            }
        }

        public void Play(Animation animationKey)
        {
            if (_animation == animationKey)
            {
                return;
            }
            _animation = animationKey;
            _animation.CurrentFrame = 0;
            _animation.Row = 0;
            _animation.Col = 0;
            _timer = 0;
        }

        public bool PlayOnce(Animation animation, GameTime gameTime)
        {
            if (_animation != animation)
            {
                _animation = animation;
                _animation.CurrentFrame = 0;
                _animation.Row = 0;
                _animation.Col = 0;
                _timer2 = 0;
            }

            if (_animation.Rows < 2)
            {
                _timer2 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_timer2 >= _animation.FrameSpeed)
                {
                    _timer2 = 0f;
                    _animation.CurrentFrame++;
                    if (_animation.CurrentFrame >= _animation.FrameCount)
                    {
                        Stop();
                        _timer2 = 0f;
                        return true;
                    }
                }
                return false;
            }
            else
            {
                _timer2 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_timer2 >= _animation.FrameSpeed)
                {
                    _timer2 = 0f;
                    _animation.CurrentFrame++;
                    _animation.Col++;
                    if (_animation.Col >= _animation.Cols)
                    {
                        _animation.Col = 0;
                        _animation.Row++;
                    }
                    if (_animation.CurrentFrame >= _animation.FrameCount)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public bool PlayOnceBackwards(Animation animation, GameTime gameTime)
        {
            if (_animation != animation)
            {
                _animation = animation;
                _animation.CurrentFrame = _animation.FrameCount;
                _timer3 = 0;
            }

            _timer3 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_timer3 > _animation.FrameSpeed)
            {
                _timer3 = 0f;
                if (_animation.CurrentFrame > 0)
                {
                    _animation.CurrentFrame--;
                }
                else
                {
                    Stop();
                    _timer3 = 0f;
                    return true;

                }
            }
            return false;
        }

        public void Stop()
        {
            _timer = 0f;
            _animation.CurrentFrame = 0;
        }
        public void NextFrame()
        {
            if (CurrentAnimation.CurrentFrame < CurrentAnimation.FrameCount - 1)
            {
                CurrentAnimation.CurrentFrame++;
            }
        }
        public void SetAnimation(Animation animation)
        {
            if (_animation == animation)
            {
                return;
            }
            _animation = animation;
        }

        public void Update(GameTime gameTime)
        {
            if (_animation.Rows < 2)
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
            else
            {
                _timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_timer > _animation.FrameSpeed)
                {
                    _timer = 0f;
                    _animation.CurrentFrame++;
                    _animation.Col++;
                    if(_animation.Col >= _animation.Cols)
                    {
                        _animation.Col = 0;
                        _animation.Row++;
                    }
                    if (_animation.CurrentFrame >= _animation.FrameCount)
                    {
                        _animation.CurrentFrame = 0;
                        _animation.Row = 0;
                        _animation.Col = 0;
                    }
                }
            }
        }
    }
}
