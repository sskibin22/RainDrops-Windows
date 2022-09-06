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
    internal abstract class Sprite : Component, ICloneable
    {
        #region Fields
        protected Dictionary<string, Animation> _animations;
        protected AnimationManager _animationManager;
        protected Texture2D _texture;
        protected Dictionary<string, Texture2D> _textures;
        protected float rotation = 0f;
        protected float scale = 1f;
        protected float layer = 1f;
        protected float opacity = 1f;
        protected Color color = Color.White;
        protected Vector2 origin;
        protected Vector2 position;
        #endregion

        #region Properties
        public Color Color 
        {
            get { return color; }
            set
            {
                color = value;
                if (_animationManager != null)
                {
                    _animationManager.Color = color;
                }
            }
        }
        public float Opacity
        {
            get { return opacity; }
            set
            {
                opacity = value;
                if (_animationManager != null)
                {
                    _animationManager.Opacity = opacity;
                }
            }
        }
        public float Speed;
        public Vector2 Velocity;
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
            set 
            { 
                scale = value; 
                if( _animationManager != null)
                {
                    _animationManager.Scale = scale;
                }
            }
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
                    _animationManager.Position = position;
                }
            }
        }
        public float X
        {
            get { return Position.X; }
            set
            {
                Position = new Vector2(value, Position.Y);
            }
        }

        public float Y
        {
            get { return Position.Y; }
            set
            {
                Position = new Vector2(Position.X, value);
            }
        }
        public Rectangle Rect {
            get
            {
                if((_texture != null && _animationManager != null) || (_texture != null && _animationManager == null))
                {
                    return new Rectangle((int)(Position.X - (int)Origin.X), (int)(Position.Y - (int)Origin.Y), (int)(_texture.Width * Scale), (int)(_texture.Height * Scale));
                }
                else if(_texture == null && _animationManager != null)
                {
                    var animation = _animations.FirstOrDefault().Value;
                    return new Rectangle((int)Position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, (int)(animation.FrameWidth*Scale), (int)(animation.FrameHeight*Scale));
                }
                throw new Exception("Unknown Sprite");
            }
        }
        #endregion

        #region Constructors
        public Sprite(Texture2D texture)
        {
            _texture = texture;
            //Color = Color.White;
            Opacity = 1f;
            Origin = new Vector2((texture.Width*Scale) / 2, (texture.Height*Scale) / 2);
            
        }
        public Sprite(Dictionary<string, Animation> animations)
        {
            _texture = null;
            //Color = Color.White;
            //Opacity = 1f;
            _animations = animations;
            var animation = _animations.FirstOrDefault().Value;
            _animationManager = new AnimationManager(animation);
            _animationManager.Color = Color;
            Origin = new Vector2(animation.FrameWidth / 2, animation.FrameHeight / 2);
        }
        public Sprite(Dictionary<string, Texture2D> textures, Dictionary<string, Animation> animations)
        {
            _textures = textures;
            _texture = _textures.FirstOrDefault().Value;
            Opacity = 1f;
            _animations = animations;
            var animation = _animations.FirstOrDefault().Value;
            _animationManager = new AnimationManager(animation);
            _animationManager.Color = Color;
            Origin = new Vector2(_texture.Width*Scale / 2, _texture.Height*Scale / 2);
        }
        #endregion

        #region Methods
        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_texture != null && _animationManager == null)
            {
                spriteBatch.Draw(_texture, position, null, Color*Opacity, Rotation, Origin, Scale, SpriteEffects.None, Layer);
            }
            else if (_texture == null && _animationManager != null)
            {
                _animationManager.Draw(spriteBatch); 
            }
            else if(_texture != null && _animationManager != null)
            {
                _animationManager.Draw(spriteBatch);
                spriteBatch.Draw(_texture, position, null, Color.White * Opacity, Rotation, Origin, Scale, SpriteEffects.None, Layer + 0.01f);
            }
            else throw new Exception("insufficient parameters for Draw method");
            
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
