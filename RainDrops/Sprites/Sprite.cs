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
    internal abstract class Sprite
    {
        protected Texture2D rectTexture;
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
        public bool ShowRect { get; set; }
        public Sprite(Texture2D texture, float rotation, float scale, float layer)
        {
            this.texture = texture;
            this.rotation = rotation;
            this.scale = scale;
            this.layer = layer;
            Color = Color.White;
            Origin = new Vector2(texture.Width / 2, texture.Height / 2);
            ShowRect = false;
        }

        public Sprite(GraphicsDevice graphics, Texture2D texture, float rotation, float scale, float layer) : this(texture, rotation, scale, layer)
        {
            SetRectTexture(graphics, texture);
            
        }

        private void SetRectTexture(GraphicsDevice graphics, Texture2D texture)
        {
            var colors = new List<Color>();
            for(int y = 0; y < texture.Height; y++)
            {
                for(int x = 0; x < texture.Width; x++)
                {
                    if(y == 0 || x == 0 || y == texture.Height -1 || x == texture.Width - 1)
                    {
                        colors.Add(new Color(255, 255, 255, 255));
                    }
                    else
                    {
                        colors.Add(new Color(0, 0, 0, 0));
                    }
                }
            }
            rectTexture = new Texture2D(graphics, texture.Width, texture.Height);
            rectTexture.SetData<Color>(colors.ToArray());
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color, rotation, Origin, scale, SpriteEffects.None, layer);

            if (ShowRect)
            {
                if(rectTexture != null)
                {
                    spriteBatch.Draw(rectTexture, Position, null, Color.Red, rotation, Origin, scale, SpriteEffects.None, layer);
                }
            }
        }
    }
}
