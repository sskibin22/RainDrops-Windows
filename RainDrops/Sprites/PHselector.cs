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
        private List<int> numPositions;
        public int phSelected;

        private float timer;
        private bool switchScale;
        private float maxScale = 7.5f;
        public PHselector(Texture2D texture) : base(texture)
        {
            numPositions = new List<int>();
            int sectionWidth = RainDropsGame.ScreenWidth / 16;
            int startPoint = (RainDropsGame.ScreenWidth / 2) - (sectionWidth * 7);
            for (int i = 0; i < 15; i++)
            {
                numPositions.Add(startPoint);
                startPoint += sectionWidth;
            }
            phSelected = 7;
            Position = new Vector2(RainDropsGame.ScreenWidth / 2, RainDropsGame.ScreenHeight - (States.GameState.phBarHeight / 2));
        }

        public override void Update(GameTime gameTime)
        {
            position.X = numPositions[phSelected];

            if (phSelected == 5 || phSelected == 9)
            {
                maxScale = 0.75f;
            }
            else if (phSelected == 4 || phSelected == 10)
            {
                maxScale = 0.8f;
            }
            else if (phSelected == 3 || phSelected == 11)
            {
                maxScale = 0.85f;
            }
            else if (phSelected == 2 || phSelected == 12)
            {
                maxScale = 0.9f;
            }
            else if (phSelected == 1 || phSelected == 13)
            {
                maxScale = 0.95f;
            }
            else if (phSelected == 0 || phSelected == 14)
            {
                maxScale = 1f;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (phSelected <= 5 || phSelected >= 9)
            {
                if (timer > 50f)
                {
                    timer = 0f;
                    if (Scale <= 0.7f)
                    {
                        switchScale = true;
                    }
                    if (Scale >= maxScale)
                    {
                        switchScale = false;
                    }

                    if (switchScale)
                    {
                        Scale += .02f;
                    }
                    else
                    {
                        Scale -= .02f;
                    }
                }
            }
        }

        public void Reset()
        {
            scale = 0.7f;
            phSelected = 7;
            position.X = numPositions[phSelected];
        }
    }
}
