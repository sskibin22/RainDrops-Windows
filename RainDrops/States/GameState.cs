using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RainDrops.Sprites;
using RainDrops.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.States
{
    internal class GameState : State
    {
        public static int phBarHeight = 40;
        public static int cloudBarHeight = 50;

        private SpriteFont font;

        private Texture2D bg;
        private Texture2D phBar;
        private Texture2D cloudBar;
        private Texture2D phSelectTexture;
        private Texture2D cupTexture;
        private Texture2D rainDropTexture;
        private Texture2D acidDropTexture;
        private Texture2D alkDropTexture;
        public static List<Texture2D>? rainCupTextures;

        public static float dropScale = 0.75f;

        public static int sectionWidth;
        public static List<int> numPositions = new List<int>();
        public static PHselector? phSelect;
        public static List<LifeIndicator>? lives;
        public static int lifeCount;
        private Cup cup;
        private Drop currDrop;

        public static List<Sprite> sprites;

        private List<float> dropCols;
        int numDropCols;
        int randCol;
        float randDropSpeed;

        public static int dropCount = 0;

        private float timer = 0;

        private bool hasStarted = false;

        private float dropLayerDepth = 0.1f;

        private float dropSpawnRate = 200f;

        private double dropSpeedMin = 0.001;
        private double dropSpeedMax = 0.005;

        private double rainDropChance = 7.5;
        private double acidDropChance = 46.25;
        private double alkDropChance = 46.25;

        private KeyboardState currKeyState;
        private KeyboardState previousKeyState;
        public GameState(RainDropsGame game, ContentManager content) : base(game, content)
        {
        }

        public override void LoadContent()
        {
            dropCols = new List<float>();

            bg = _content.Load<Texture2D>("BackGrounds/MountainsBG");
            phBar = _content.Load<Texture2D>("UI/phBar");
            cloudBar = _content.Load<Texture2D>("UI/rainCloudBar");
            phSelectTexture = _content.Load<Texture2D>("UI/phSelectCircle");

            font = _content.Load<SpriteFont>("Fonts/scoreFont");

            cupTexture = _content.Load<Texture2D>("Cups/emptyCup");


            rainCupTextures = new List<Texture2D>();
            for (int i = 0; i < 31; i++)
            {
                if (i < 10)
                {
                    rainCupTextures.Add(_content.Load<Texture2D>($"Cups/RainCupFrames/cup0{i}"));
                }
                else
                {
                    rainCupTextures.Add(_content.Load<Texture2D>($"Cups/RainCupFrames/cup{i}"));
                }

            }
            rainDropTexture = _content.Load<Texture2D>("Drops/rainDrop");
            acidDropTexture = _content.Load<Texture2D>("Drops/acidDrop");
            alkDropTexture = _content.Load<Texture2D>("Drops/alkDrop");

            numDropCols = (int)(RainDropsGame.ScreenWidth / (rainDropTexture.Width * dropScale));
            float total = 0;
            for (int i = 0; i < numDropCols; i++)
            {
                if (i == 0)
                {
                    total += ((rainDropTexture.Width * dropScale) / 2) + 10;

                }
                else
                {
                    total += rainDropTexture.Width * dropScale;
                }
                dropCols.Add(total);

            }

            sectionWidth = RainDropsGame.ScreenWidth / 16;
            int startPoint = (RainDropsGame.ScreenWidth / 2) - (sectionWidth * 7);
            for (int i = 0; i < 15; i++)
            {
                numPositions.Add(startPoint);
                startPoint += sectionWidth;
            }

            Restart();
        }

        private void Restart()
        {
            lifeCount = 3;
            dropCount = 0;
            phSelect = new PHselector(phSelectTexture, 0f, 0.7f, 0.4f);
            cup = new Cup(rainCupTextures[0], 0f, 1f, 0.9f); //cupTexture

            lives = new List<LifeIndicator>() {
                new LifeIndicator(rainDropTexture,0f, 0.5f, 0.3f){Position = new Vector2((rainDropTexture.Width*0.5f), (rainDropTexture.Height*0.5f/2)+1)},
                new LifeIndicator(rainDropTexture,0f, 0.5f, 0.3f){Position = new Vector2(((rainDropTexture.Width*0.5f)*2)+10, (rainDropTexture.Height*0.5f/2)+1)},
                new LifeIndicator(rainDropTexture,0f, 0.5f, 0.3f){Position = new Vector2(((rainDropTexture.Width*0.5f)*3)+20, (rainDropTexture.Height*0.5f/2)+1)}};

            sprites = new List<Sprite>()
            {
                lives[0],
                lives[1],
                lives[2],
                cup,
                phSelect
            };
            hasStarted = false;
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //TODO
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            previousKeyState = currKeyState;
            currKeyState = keyState;
            //if ESC key is pressed exit game
            if (keyState.IsKeyDown(Keys.Escape))
                _game.Exit();
            //if Space key is pressed, start game
            if (previousKeyState.IsKeyDown(Keys.Space) && currKeyState.IsKeyUp(Keys.Space))
                hasStarted = true;
            //if game has not started(space has not been pressed yet) return
            if (!hasStarted)
                return;
            //increment game timer based on total seconds elapsed
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            //Update each sprite in sprites List
            foreach (var sprite in sprites)
            {
                sprite.Update(gameTime);
            }
            //if timer is greater than drop rate, reset timer, spawn a new drop
            if (timer > dropSpawnRate)
            {
                timer = 0f;
                randCol = RainDropsGame.Random.Next(0, numDropCols);
                randDropSpeed = (float)(RainDropsGame.Random.NextDouble() * (dropSpeedMax - dropSpeedMin) + dropSpeedMin);

                foreach (var sprite in sprites)
                {
                    if (sprite is Drop)
                    {
                        var drop = (Drop)sprite;
                        if (drop.Position.X == dropCols[randCol])
                        {
                            if (drop.Position.Y - drop.Rect.Height / 2 < rainDropTexture.Height * dropScale)
                            {
                                randCol = RainDropsGame.Random.Next(0, numDropCols);
                            }
                        }
                    }
                }
                //System.Diagnostics.Debug.WriteLine(randDropSpeed);
                WeightedRandomExecutor wre = new WeightedRandomExecutor(
                new WeightedRandomParam(() => currDrop = new RainDrop(rainDropTexture, 0f, dropScale, dropLayerDepth)
                {
                    Position = new Vector2(dropCols[randCol], -rainDropTexture.Height * dropScale / 2),
                    DropSpeed = randDropSpeed,
                }, rainDropChance),
                new WeightedRandomParam(() => currDrop = new AcidRainDrop(acidDropTexture, 0f, dropScale, dropLayerDepth)
                {
                    Position = new Vector2(dropCols[randCol], -acidDropTexture.Height * dropScale / 2),
                    DropSpeed = randDropSpeed,
                }, acidDropChance),
                new WeightedRandomParam(() => currDrop = new AlkRainDrop(alkDropTexture, 0f, dropScale, dropLayerDepth)
                {
                    Position = new Vector2(dropCols[randCol], -alkDropTexture.Height * dropScale / 2),
                    DropSpeed = randDropSpeed,
                }, alkDropChance));
                wre.Execute();

                sprites.Add(currDrop);
                dropCount++;
            }


            //check if any sprite.IsRemoved property has been set to true. If so remove from sprites List
            for (int i = 0; i < sprites.Count; i++)
            {
                var sprite = sprites[i];

                if (sprite.IsRemoved)
                {
                    sprites.RemoveAt(i);
                    i--;
                }
                //if Cup has caught 30 RainDrops, restart the game
                if (sprite is Cup)
                {
                    if (sprite is Cup cupInstance && cupInstance.dropsCaught == 30)
                    {
                        Restart();
                    }
                }
            }
            if (lifeCount < 0)
            {
                Restart();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            //Draw background
            spriteBatch.Draw(bg, new Rectangle(0, 0, RainDropsGame.ScreenWidth, RainDropsGame.ScreenHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            //Draw ph bar
            spriteBatch.Draw(phBar, new Rectangle(0, RainDropsGame.ScreenHeight - phBarHeight, RainDropsGame.ScreenWidth, phBarHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.2f);
            //Draw cloud bar
            spriteBatch.Draw(cloudBar, new Rectangle(0, 0, RainDropsGame.ScreenWidth, cloudBarHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.2f);

            //spriteBatch.Draw(rainDropTexture, new Rectangle(10, 1, 22, 25), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.3f);
            //spriteBatch.Draw(rainDropTexture, new Rectangle(42, 1, 22, 25), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.3f);
            //spriteBatch.Draw(rainDropTexture, new Rectangle(74, 1, 22, 25), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.3f);

            //Draw all sprites and show their hitboxes if showHitBox is set to true
            foreach (var sprite in sprites)
            {
                sprite.Draw(gameTime, spriteBatch);
                //if(sprite is Drop)
                //{
                //    var drop = sprite as Drop;
                //    spriteBatch.DrawString(font, $"{drop.PH}", new Vector2(drop.Position.X-5, drop.Position.Y-5), Color.Black, 0f, Vector2.Zero, 1.2f, SpriteEffects.None, 0.11f);
                //}
            }
            //Draw score
            spriteBatch.DrawString(font, $"Drops: {cup.dropsCaught}", new Vector2(cup.Position.X - 30, cup.Position.Y - 20), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            spriteBatch.End();
        }
    }
}
