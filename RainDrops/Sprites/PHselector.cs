using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Sprites
{
    internal class PHselector : Sprite
    {
        public int phSelected;
        public PHselector(Texture2D texture, float rotation, float scale, float layer) : base(texture, rotation, scale, layer)
        {
            phSelected = 7;
            Position = new Vector2(RainDropsGame.ScreenWidth / 2, RainDropsGame.ScreenHeight - (States.GameState.phBarHeight / 2));
        }

        public override void Update(GameTime gameTime)
        {
            Position.X = States.GameState.numPositions[phSelected];
        }
    }
}
