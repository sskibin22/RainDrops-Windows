using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Sprites
{
    internal abstract class Sprite : Component
    {
        protected Texture2D texture;
        protected SpriteFont font;
        protected float rotation;
        protected float scale;
        protected float layer;
        public Vector2 Origin;
        public Vector2 Position;
        public bool IsRemoved = false;
        public Color Color { get; set; }
        public Rectangle Rect {
            get
            {
                return new Rectangle((int)(Position.X - ((texture.Width*scale)/2)), (int)(Position.Y - ((texture.Height*scale)/2)), (int)(texture.Width*scale), (int)(texture.Height*scale));
            }
        }
        public Sprite(Texture2D texture, float rotation, float scale, float layer)
        {
            this.texture = texture;
            this.rotation = rotation;
            this.scale = scale;
            this.layer = layer;
            Color = Color.White;
            Origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }
        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color, rotation, Origin, scale, SpriteEffects.None, layer);
        }
    }
}
