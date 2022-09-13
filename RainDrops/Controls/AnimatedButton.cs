using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RainDrops.Models;

namespace RainDrops.Controls
{
    internal class AnimatedButton : Component
    {
        private Animation _animation;
        private bool _isHovering;
        private float timer;
        private float playTimer;
        public EventHandler Click;
        public bool IsActive { get; set; }
        public bool Play { get; set; }
        public bool Clicked { get; private set; }
        public float Opacity { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }
        public float Layer { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; private set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X - ((int)Origin.X), (int)Position.Y - (int)Origin.Y, _animation.FrameWidth, _animation.FrameHeight);
            }
        }

        public AnimatedButton(Animation animation)
        {
            _animation = animation;
            timer = 0f;
            playTimer = 0f;
            IsActive = true;
            Play = false;
            Clicked = false;
            Opacity = 1f;
            Rotation = 0f;
            Scale = 1f;
            Layer = 1f;
            Origin = new Vector2(_animation.FrameWidth / 2, _animation.FrameHeight / 2);
        }
        public void Reset()
        {
            playTimer = 0f;
            timer = 0f;
            IsActive = true;
            Clicked = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                if (Play)
                {
                    playTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (playTimer > _animation.FrameSpeed)
                    {
                        playTimer = 0f;
                        _animation.CurrentFrame++;
                        if (_animation.CurrentFrame >= _animation.FrameCount)
                        {
                            _animation.CurrentFrame = 0;
                        }
                    }
                }
             
                if (Clicked)
                {
                    timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                var mouseRectangle = new Rectangle((int)RainDropsGame.scaledMouse.X, (int)RainDropsGame.scaledMouse.Y, 1, 1);

                _isHovering = false;

                if (mouseRectangle.Intersects(Rectangle))
                {
                    _isHovering = true;
                    if (RainDropsGame.mouseState.LeftButton == ButtonState.Released)
                    {
                        _animation.CurrentFrame = 0;
                    }
                    if (RainDropsGame.prevMouseState.LeftButton == ButtonState.Pressed)
                    {
                        Clicked = true;
                        _animation.CurrentFrame = _animation.FrameCount - 1;
                    }
                }
                if (Clicked && timer > 250f)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                var color = Color.White;

                if (_isHovering)
                {
                    color = Color.LightGray;
                }

                spriteBatch.Draw(_animation.Texture, Position, new Rectangle(_animation.CurrentFrame * _animation.FrameWidth, 0, _animation.FrameWidth, _animation.FrameHeight), color * Opacity, Rotation, Origin, Scale, SpriteEffects.None, Layer);
            }
        }
    }
}

