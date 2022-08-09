using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RainDrops.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Sprites
{
    internal class Cup : Sprite
    {
        public List<Texture2D> CupTextures { get; set; }
        private int frameCount = 0;
        public Cup(Texture2D texture) : base(texture)
        {
            Position = new Vector2(RainDropsGame.ScreenWidth / 2, RainDropsGame.ScreenHeight - (texture.Height / 2) - States.GameState.phBarHeight);
        }

        public override void Update(GameTime gameTime)
        {
            Move();
        }
        public void UpdateFrame()
        {
            frameCount++;
            _texture = CupTextures[frameCount];
        }
        private void Move()
        {
            position.X = Mouse.GetState().Position.X;
            position.X = MathHelper.Clamp(Position.X, Rect.Width / 2, RainDropsGame.ScreenWidth - Rect.Width/2);
        }
    }
}
