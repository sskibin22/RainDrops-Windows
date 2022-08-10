using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RainDrops.Models;
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

        private Dictionary<string, Animation> splashDict;

        private List<Texture2D>? rainCupTextures;
        private SplashAnimation splashAnimation;

        public static float dropScale = 0.75f;

        public static float splashHeightMult = 1.035f;
        public static int sectionWidth;
        public static List<int> numPositions = new List<int>();
        public static PHselector? phSelect;
        public static List<LifeIndicator>? lives;
        public static int lifeCount;
        public static Cup cup;
        private Drop currDrop;
        private CloudBar cloud1;
        private CloudBar cloud2;
        private CloudBar cloud3;
        private CloudBar cloud4;

        public int dropsCaught;
        public int acidDropsCaught;
        public int alkDropsCaught;

        public static List<Sprite> sprites;

        private List<float> dropCols;
        int numDropCols;
        int randCol;
        float randDropSpeed;

        public static int dropCount = 0;

        private float timer = 0;
        private float endTimer = 0;

        private bool hasStarted = false;

        private float dropLayerDepth = 0.85f;

        //private float dropSpawnRate = 200f;

        //private double dropSpeedMin = 0.001;
        //private double dropSpeedMax = 0.005;

        //private double rainDropChance = 7.5;
        //private double acidDropChance = 46.25;
        //private double alkDropChance = 46.25;

        private int diffCount = 0;

        private int catchCount;
        private float dropSpawnRate;

        private double dropSpeedMin;
        private double dropSpeedMax;

        private double rainDropChance;
        private double acidDropChance;
        private double alkDropChance;

        private KeyboardState currKeyState;
        private KeyboardState previousKeyState;
        public GameState(RainDropsGame game, ContentManager content) : base(game, content)
        {
        }

        public void UpdateDifficulty()
        {
            catchCount = RainDropsGame.levelManager._catchCount;
            dropSpawnRate = RainDropsGame.levelManager._dropSpawnRate;
            dropSpeedMin = RainDropsGame.levelManager._dropSpeedMin;
            dropSpeedMax = RainDropsGame.levelManager._dropSpeedMax;
            rainDropChance = RainDropsGame.levelManager._rainDropChance;
            acidDropChance = RainDropsGame.levelManager._acidDropChance;
            alkDropChance = RainDropsGame.levelManager._alkDropChance;
        }
        public void ModifyLevelDifficulty()
        {
            dropSpawnRate += RainDropsGame.levelManager.dropSpawnRateMod;
            dropSpeedMin += RainDropsGame.levelManager.dropSpeedMinMod;
            dropSpeedMax += RainDropsGame.levelManager.dropSpeedMaxMod;
            rainDropChance += RainDropsGame.levelManager.rainDropChanceMod;
            acidDropChance += RainDropsGame.levelManager.acidDropChanceMod;
            alkDropChance += RainDropsGame.levelManager.alkDropChanceMod;
        }
        public override void LoadContent()
        {
            UpdateDifficulty();
            dropCols = new List<float>();

            bg = _content.Load<Texture2D>("BackGrounds/MountainsBG2");
            phBar = _content.Load<Texture2D>("UI/phBar");
            cloudBar = _content.Load<Texture2D>("Floaters/cloudBar2");
            phSelectTexture = _content.Load<Texture2D>("UI/phSelectCircle");
            cloud1 = new CloudBar(cloudBar) { Origin = Vector2.Zero, Layer = 0.9f, Speed = 0.8f, Position = new Vector2(0, 0) };
            cloud2 = new CloudBar(cloudBar) { Origin = Vector2.Zero, Layer = 0.9f, Speed = 0.8f, Position = new Vector2(cloudBar.Width, 0) };
            cloud3 = new CloudBar(cloudBar) { Origin = Vector2.Zero, Layer = 0.8f, Speed = 1f, Position = new Vector2(50, 20) };
            cloud4 = new CloudBar(cloudBar) { Origin = Vector2.Zero, Layer = 0.8f, Speed = 1f, Position = new Vector2(-cloudBar.Width + 50, 20) };

            splashDict = new Dictionary<string, Animation>()
            {
                {"rainSplash", new Animation(_content.Load<Texture2D>("Animations/Splash/rainSplashSS"), 5){Scale = 1f, FrameSpeed = 60f } },
                {"acidSplash", new Animation(_content.Load<Texture2D>("Animations/Splash/acidSplashSS"), 5){Scale = 1f, FrameSpeed = 60f } },
                {"alkSplash", new Animation(_content.Load<Texture2D>("Animations/Splash/alkSplashSS"), 5) {Scale = 1f, FrameSpeed = 60f } }

            };

            font = _content.Load<SpriteFont>("Fonts/scoreFont");

            cupTexture = _content.Load<Texture2D>("Cups/emptyCup");

            splashAnimation = new SplashAnimation(splashDict)
            {
                Position = new Vector2(300, 300),
                Layer = 1f
            };


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
            if(diffCount == 4)
            {
                RainDropsGame.levelManager.SetBaseDifficulty();
                UpdateDifficulty();
                diffCount = 0;
            }
            if(diffCount > 0)
            {
                ModifyLevelDifficulty();
            }
            endTimer = 0f;
            splashHeightMult = 1.035f;
            dropsCaught = 0;
            acidDropsCaught = 0;
            alkDropsCaught = 0;
            lifeCount = 3;
            dropCount = 0;
            phSelect = new PHselector(phSelectTexture) { Scale = 0.7f, Layer = 0.91f};
            cup = new Cup(rainCupTextures[0]) { CupTextures = rainCupTextures, Scale = 1f, Layer = 0.9f}; //cupTexture

            lives = new List<LifeIndicator>() {
                new LifeIndicator(rainDropTexture){Position = new Vector2((rainDropTexture.Width*0.5f), (rainDropTexture.Height*0.5f/2)+1), Scale = 0.5f, Layer = 1f},
                new LifeIndicator(rainDropTexture){Position = new Vector2(((rainDropTexture.Width*0.5f)*2)+10, (rainDropTexture.Height*0.5f/2)+1), Scale = 0.5f, Layer = 1f},
                new LifeIndicator(rainDropTexture){Position = new Vector2(((rainDropTexture.Width*0.5f)*3)+20, (rainDropTexture.Height*0.5f/2)+1), Scale = 0.5f, Layer = 1f}};

            sprites = new List<Sprite>()
            {
                lives[0],
                lives[1],
                lives[2],
                cup,
                phSelect, 
                cloud1,
                cloud2, 
                cloud3, 
                cloud4
            };
            hasStarted = false;
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //TODO
        }

        private void MovePHSelector(Drop drop)
        {
            switch (drop.PH)
            {
                case 14:
                    phSelect.phSelected += 3;
                    if (phSelect.phSelected > 14)
                        phSelect.phSelected = 14;
                    break;
                case 11:
                    if (phSelect.phSelected != 14)
                        phSelect.phSelected += 1;
                    break;
                case 3:
                    if (phSelect.phSelected != 0)
                        phSelect.phSelected -= 1;
                    break;
                case 0:
                    phSelect.phSelected -= 3;
                    if (phSelect.phSelected < 0)
                        phSelect.phSelected = 0;
                    break;
                default:
                    break;
            }
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
            cloud1.MoveLeft();
            cloud2.MoveLeft();
            cloud3.MoveRight();
            cloud4.MoveRight();
            if (cloud1.Position.X <= -cloud1.Rect.Width)
            {
                cloud1.Reset(cloud2, 0);
            }
            if (cloud2.Position.X <= -cloud2.Rect.Width)
            {
                cloud2.Reset(cloud1, 0);
            }
            if (cloud3.Position.X >= cloud3.Rect.Width)
            {
                cloud3.Reset(cloud4, 1);
            }
            if (cloud4.Position.X >= cloud4.Rect.Width)
            {
                cloud4.Reset(cloud3, 1);
            }
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
                new WeightedRandomParam(() => currDrop = new RainDrop(rainDropTexture)
                {
                    Position = new Vector2(dropCols[randCol], -rainDropTexture.Height * dropScale / 2),
                    DropSpeed = randDropSpeed,
                    Scale = dropScale,
                    Layer = dropLayerDepth
                }, rainDropChance),
                new WeightedRandomParam(() => currDrop = new AcidRainDrop(acidDropTexture)
                {

                    Position = new Vector2(dropCols[randCol], -acidDropTexture.Height * dropScale / 2),
                    DropSpeed = randDropSpeed,
                    Scale = dropScale,
                    Layer = dropLayerDepth
                }, acidDropChance),
                new WeightedRandomParam(() => currDrop = new AlkRainDrop(alkDropTexture)
                {
                    Position = new Vector2(dropCols[randCol], -alkDropTexture.Height * dropScale / 2),
                    DropSpeed = randDropSpeed,
                    Scale = dropScale,
                    Layer = dropLayerDepth
                }, alkDropChance)); 
                wre.Execute();

                sprites.Add(currDrop);
                dropCount++;
            }

            foreach(var sprite in sprites)
            {
                if(sprite is Drop)
                {
                    var drop = (Drop)sprite;
                    if (drop.IsCupCollision(cup) && sprite is RainDrop)
                    {
                        cup.UpdateFrame();
                        var rainDrop = sprite as RainDrop;
                        dropsCaught++;
                        sprites.Add(new SplashAnimation(splashDict)
                        {
                            Position = new Vector2(cup.Position.X, cup.Position.Y * splashHeightMult),
                            Layer = 0.91f,
                            Type = "rainSplash"
                        });
                        splashHeightMult -= 0.0042f;
                        sprite.IsRemoved = true;

                        break;
                    }
                    if (drop.IsCupCollision(cup) && sprite is AcidRainDrop)
                    {
                        var acidDrop = sprite as AcidRainDrop;
                        //dropsCaught++;
                        acidDropsCaught++;
                        sprites.Add(new SplashAnimation(splashDict)
                        {
                            Position = new Vector2(cup.Position.X, cup.Position.Y * splashHeightMult),
                            Layer = 0.91f,
                            Type = "acidSplash"
                        });
                        sprite.IsRemoved = true;
                        MovePHSelector(acidDrop);
                        break;
                    }
                    if (drop.IsCupCollision(cup) && sprite is AlkRainDrop)
                    {
                        var alkDrop = sprite as AlkRainDrop;
                        //dropsCaught++;
                        alkDropsCaught++;
                        sprites.Add(new SplashAnimation(splashDict)
                        {
                            Position = new Vector2(cup.Position.X, cup.Position.Y * splashHeightMult),
                            Layer = 0.91f,
                            Type = "alkSplash"
                        });
                        sprite.IsRemoved = true;
                        MovePHSelector(alkDrop);
                        break;
                    }
                }   
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
                
            }
            if (dropsCaught == catchCount)
            {
                //wait 3 seconds before restarting
                endTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (endTimer > 300f)
                {
                    RainDropsGame.levelManager.IncreaseLevel();
                    diffCount++;
                    Restart();
                }
                    
            }
            if (lifeCount < 0)
            {
                //wait 3 seconds before restarting
                endTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (endTimer > 300f)
                {
                    RainDropsGame.levelManager.ResetLevel();
                    Restart();
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            //Draw background
            spriteBatch.Draw(bg, new Rectangle(0, 0, RainDropsGame.ScreenWidth, RainDropsGame.ScreenHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            //Draw ph bar
            spriteBatch.Draw(phBar, new Rectangle(0, RainDropsGame.ScreenHeight - phBarHeight, RainDropsGame.ScreenWidth, phBarHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.9f);
            //Draw cloud bar
            spriteBatch.Draw(cloudBar, new Rectangle(0, 0, RainDropsGame.ScreenWidth, cloudBarHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.2f);
            //Draw all sprites and show their hitboxes if showHitBox is set to true
            foreach (var sprite in sprites)
            {
                sprite.Draw(gameTime, spriteBatch);
            }
            //Draw Level
            spriteBatch.DrawString(font, $"Level {RainDropsGame.levelManager.Level}", new Vector2((int)(RainDropsGame.ScreenWidth*0.9), (int)(RainDropsGame.ScreenHeight*0.005)), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            //Draw score
            spriteBatch.DrawString(font, $"Drops: {dropsCaught}", new Vector2(cup.Position.X - 30, cup.Position.Y - 20), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            spriteBatch.End();
        }
    }
}
