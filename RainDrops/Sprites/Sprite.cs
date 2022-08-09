using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RainDrops.Managers;
using RainDrops.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Sprites
{
    internal abstract class Sprite : Component
    {
        #region Fields
        protected Dictionary<string, Animation> _animations;
        protected AnimationManager _animationManager;
        protected Texture2D _texture;
        protected SpriteFont font;
        protected float rotation = 0f;
        protected float scale = 1f;
        protected float layer = 1f;
        protected Vector2 origin;
        protected Vector2 position;
        #endregion

        #region Properties
        public Color Color { get; set; }
        public float Rotation 
        {
            get { return rotation; }
            set
            {
                rotation = value;
                if (_animationManager != null)
                {
                    _animationManager.Rotation = rotation;
                }
            }
        }
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        public float Layer
        {
            get { return layer; }
            set
            {
                layer = value;
                if (_animationManager != null)
                {
                    _animationManager.Layer = layer;
                }
            }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set
            {
                origin = value;
                if (_animationManager != null)
                {
                    _animationManager.Origin = origin;
                }
            }
        }
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                if (_animationManager != null)
                {
                    _animationManager.position = position;
                }
            }
        }
        public Rectangle Rect {
            get
            {
                if(_texture != null)
                {
                    return new Rectangle((int)(Position.X - (int)Origin.X), (int)(Position.Y - (int)Origin.Y), (int)(_texture.Width * scale), (int)(_texture.Height * scale));
                }
                if(_animationManager != null)
                {
                    var animation = _animations.FirstOrDefault().Value;
                    return new Rectangle((int)Position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, animation.FrameWidth, animation.FrameHeight);
                }
                throw new Exception("Unknown Sprite");
            }
        }
        #endregion

        #region Constructors
        public Sprite(Texture2D texture)
        {
            _texture = texture;
            Color = Color.White;
            Origin = new Vector2((texture.Width*Scale) / 2, (texture.Height*Scale) / 2);
            
        }
        public Sprite(Dictionary<string, Animation> animations)
        {
            _texture = null;
            Color = Color.White;
            _animations = animations;
            var animation = _animations.FirstOrDefault().Value;
            _animationManager = new AnimationManager(animation);
            Origin = new Vector2(animation.FrameWidth / 2, animation.FrameHeight / 2);
        }
        #endregion

        #region Methods
        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_texture != null)
            {
                spriteBatch.Draw(_texture, position, null, Color, Rotation, Origin, Scale, SpriteEffects.None, Layer);
            }
            else if (_animationManager != null)
            {
                _animationManager.Draw(spriteBatch);
            }
            else throw new Exception("insufficient parameters for Draw method");
            
        }
        #endregion
    }
}
