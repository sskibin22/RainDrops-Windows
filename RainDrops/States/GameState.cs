using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RainDrops.Models;
using RainDrops.Sprites;
using RainDrops.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.States
{
    internal class GameState : State
    {
        #region Global Static Variables
        public static int phBarHeight = 40;
        public static float splashHeightMult = 1.035f;

        public static int dropCount = 0; //Total drops on current GameState screen
        public static int lifeCount;
        #endregion

        #region Private Variables
        private List<Sprite> sprites; //Contains all sprites on GameState screen

        /* Drop attributes calculated during game */
        private List<float> dropCols;
        private float scaledDropFrameWidth;
        private float scaledDropFrameHeight;
        private int numDropCols;
        private int randCol;
        private float randDropSpeed;
        private float dropScale = 0.8f; //scaling multiplier for drops

        /* Caught drop counters */
        private int dropsCaught;    //number of rain drops caught
        private int acidDropsCaught;
        private int alkDropsCaught;

        /* Game attributes */
        private int maxDrops = 25;  //Max drop instances allowed on screen at one time
        private int diffCount = 0;  //difficulty counter for level manager
        private int catchCount; //How many drops need to be caught to win the level
        private float dropSpawnRate; //spawn rate for drops
        private double dropSpeedMin; //minimum drop speed on Y-axis
        private double dropSpeedMax; //maximum drop speed on Y-axis
        private float animationFrameSpeed = 83.33f; //(about 12fps)

        /* Must total 1 (ex: rainDropChance = 0.2, acidDropChance = 0.4, alkDropChance = 0.4) */
        private double rainDropChance; 
        private double acidDropChance;
        private double alkDropChance;

        /* Timers */
        private float timer = 0;
        private float endTimer = 0;
        
        private bool hasStarted = false; //start game boolean variable
        #endregion

        #region Sprite Fonts
        private SpriteFont font;
        #endregion

        #region Textures
        private Texture2D bgTexture;
        private Texture2D phBarTexture;
        private Texture2D cloudBarTexture;
        private Texture2D phSelectTexture;
        private Texture2D standardCupTexture;
        private Texture2D rainAnimTexture;
        private Texture2D acidAnimTexture;
        private Texture2D alkAnimTexture;
        private Texture2D acidGlowTexture;
        private Texture2D alkGlowTexture;
        #endregion

        #region Animation Dictionaries
        private Dictionary<string, Animation> splashDict;
        private Dictionary<string, Animation> dropDict;
        #endregion

        #region Animations
        private Animation rainDropAnimation;
        private Animation acidDropAnimation;
        private Animation alkDropAnimation;
        private Animation acidGlowAnimation;
        private Animation alkGlowAnimation;
        private Animation lifeDropAnimation;
        #endregion

        #region Global Static Sprites
        public static Cup cup;
        public static List<LifeIndicator>? lives;
        #endregion

        #region Private Sprites
        private PHselector? phSelect;
        private Drop currDrop;
        private CloudBar cloud1;
        private CloudBar cloud2;
        private CloudBar cloud3;
        private CloudBar cloud4;
        #endregion

        #region UI States
        private KeyboardState currKeyState;
        private KeyboardState previousKeyState;
        #endregion

        #region GameState Contructor
        public GameState(RainDropsGame game, ContentManager content) : base(game, content)
        {
        }
        #endregion

        #region Game Attribute Methods
        /* Updates Game difficulty settings through given level manager variables */
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
        /* Increases difficulty settings by given level manager difficulty modifier variables */
        public void ModifyLevelDifficulty()
        {
            dropSpawnRate += RainDropsGame.levelManager.dropSpawnRateMod;
            dropSpeedMin += RainDropsGame.levelManager.dropSpeedMinMod;
            dropSpeedMax += RainDropsGame.levelManager.dropSpeedMaxMod;
            rainDropChance += RainDropsGame.levelManager.rainDropChanceMod;
            acidDropChance += RainDropsGame.levelManager.acidDropChanceMod;
            alkDropChance += RainDropsGame.levelManager.alkDropChanceMod;
        }
        /* Tells which PH number should be selected by PHSelector given a drop's PH level */
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
        #endregion

        #region Game Methods
        public override void LoadContent()
        {
            /* Load GameState SpriteFonts */
            font = _content.Load<SpriteFont>("Fonts/scoreFont");

            /* Load GameState Textures */
            bgTexture = _content.Load<Texture2D>("BackGrounds/MountainsBG2");
            phBarTexture = _content.Load<Texture2D>("UI/phBar");
            cloudBarTexture = _content.Load<Texture2D>("Floaters/cloudBar2");
            phSelectTexture = _content.Load<Texture2D>("UI/phSelectCircle");
            rainAnimTexture = _content.Load<Texture2D>("Drops/dropBlueSS");
            acidAnimTexture = _content.Load<Texture2D>("Drops/dropGreenSS");
            alkAnimTexture = _content.Load<Texture2D>("Drops/dropPurpleSS");
            acidGlowTexture = _content.Load<Texture2D>("Drops/glowGreenSS");
            alkGlowTexture = _content.Load<Texture2D>("Drops/glowPurpleSS");
            standardCupTexture = _content.Load<Texture2D>("Cups/standardCupSS");

            /* Instantiate Animations */
            rainDropAnimation = new Animation(rainAnimTexture, 2) { FrameSpeed = animationFrameSpeed }; 
            acidDropAnimation = new Animation(acidAnimTexture, 2) { FrameSpeed = animationFrameSpeed }; 
            alkDropAnimation = new Animation(alkAnimTexture, 2) { FrameSpeed = animationFrameSpeed }; 
            acidGlowAnimation = new Animation(acidGlowTexture, 10) { FrameSpeed = animationFrameSpeed }; 
            alkGlowAnimation = new Animation(alkGlowTexture, 10) { FrameSpeed = animationFrameSpeed }; 
            lifeDropAnimation = new Animation(rainAnimTexture, 2) { FrameSpeed = animationFrameSpeed }; 

            /* Instantiate Animation Dictionaries */
            splashDict = new Dictionary<string, Animation>()
            {
                {"rainSplash", new Animation(_content.Load<Texture2D>("Animations/Splash/rainSplashSS"), 5){FrameSpeed = animationFrameSpeed } }, 
                {"acidSplash", new Animation(_content.Load<Texture2D>("Animations/Splash/acidSplashSS"), 5){FrameSpeed = animationFrameSpeed } }, 
                {"alkSplash", new Animation(_content.Load<Texture2D>("Animations/Splash/alkSplashSS"), 5){FrameSpeed = animationFrameSpeed } } 
            };

            dropDict = new Dictionary<string, Animation>()
            {
                {"rainDrop", rainDropAnimation },
                {"acidDrop", acidDropAnimation },
                {"alkDrop", alkDropAnimation },
                {"acidGlow", acidGlowAnimation },
                {"alkGlow", alkGlowAnimation }
            };

            /* Instantiate Necessary Sprites */
            cloud1 = new CloudBar(cloudBarTexture) { Origin = Vector2.Zero, Layer = 0.9f, Speed = 0.8f, Position = new Vector2(0, 0) };
            cloud2 = new CloudBar(cloudBarTexture) { Origin = Vector2.Zero, Layer = 0.9f, Speed = 0.8f, Position = new Vector2(cloudBarTexture.Width, 0) };
            cloud3 = new CloudBar(cloudBarTexture) { Origin = Vector2.Zero, Layer = 0.8f, Speed = 1f, Position = new Vector2(50, 20) };
            cloud4 = new CloudBar(cloudBarTexture) { Origin = Vector2.Zero, Layer = 0.8f, Speed = 1f, Position = new Vector2(-cloudBarTexture.Width + 50, 20) };
            phSelect = new PHselector(phSelectTexture) { Scale = 0.7f, Layer = 0.91f };
            cup = new Cup(new Dictionary<string, Animation>() { { "standardCups", new Animation(standardCupTexture, 31) {FrameSpeed = animationFrameSpeed } } }) { Layer = 0.9f }; 
            lives = new List<LifeIndicator>()
            {
                new LifeIndicator(new Dictionary<string, Animation>(){{"lifeDrop", new Animation(rainAnimTexture, 2) { FrameSpeed = animationFrameSpeed }} }){Position = new Vector2((lifeDropAnimation.FrameWidth*0.5f), (lifeDropAnimation.FrameHeight*0.5f/2)+1), Scale = 0.5f, Layer = 1f},
                new LifeIndicator(new Dictionary<string, Animation>(){{"lifeDrop", new Animation(rainAnimTexture, 2) { FrameSpeed = animationFrameSpeed }} }){Position = new Vector2(((lifeDropAnimation.FrameWidth*0.5f)*2)+10, (lifeDropAnimation.FrameHeight*0.5f/2)+1), Scale = 0.5f, Layer = 1f},
                new LifeIndicator(new Dictionary<string, Animation>(){{"lifeDrop", new Animation(rainAnimTexture, 2) { FrameSpeed = animationFrameSpeed }} }){Position = new Vector2(((lifeDropAnimation.FrameWidth*0.5f)*3)+20, (lifeDropAnimation.FrameHeight*0.5f/2)+1), Scale = 0.5f, Layer = 1f}
            };

            /* Calculate Drop column positions and add to dropCols List */
            dropCols = new List<float>();

            scaledDropFrameWidth = rainDropAnimation.FrameWidth * dropScale;
            scaledDropFrameHeight = rainDropAnimation.FrameHeight * dropScale;
            numDropCols = (int)(RainDropsGame.ScreenWidth / (scaledDropFrameWidth));
            float preTotal = (scaledDropFrameWidth * (numDropCols - 1)) + (scaledDropFrameWidth/2);
            float difference = (RainDropsGame.ScreenWidth - preTotal) - (scaledDropFrameWidth/2);
            float centerBy = difference / 2f;
            float total = 0;
            for (int i = 0; i < numDropCols; i++)
            {
                if (i == 0)
                {
                    total += ((scaledDropFrameWidth) / 2) + centerBy;

                }
                else
                {
                    total += scaledDropFrameWidth;
                }
                dropCols.Add(total);

            }

            /* Load Initial Game settings */
            UpdateDifficulty(); //set initial difficulty settings
            Restart();
        }
        /* Restart Game back to it's original state */
        private void Restart()
        {
            /* Apply difficulty settings as needed */
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

            /* Initialize Game attributes */
            endTimer = 0f;
            splashHeightMult = 1.035f;
            dropsCaught = 0;
            acidDropsCaught = 0;
            alkDropsCaught = 0;
            lifeCount = 3;
            dropCount = 0;
            phSelect.Reset();
            cup.Reset();
            
            foreach(LifeIndicator life in lives)
            {
                life.IsRemoved = false;
            }

            /* Initialize sprites list */
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

            /* set Game started boolean to false */
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
            //Cloud Movement Animations
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
                /* If random column position has another drop within range of starting spawn position, randomly choose another drop column. 
                 * This is so that drops have less of a chance of spawing on top of eachother.
                 */
                foreach (var sprite in sprites)
                {
                    if (sprite is Drop)
                    {
                        var drop = (Drop)sprite;
                        if (drop.Position.X == dropCols[randCol])
                        {
                            if (drop.Rect.Top < scaledDropFrameHeight)
                            {
                                randCol = RainDropsGame.Random.Next(0, numDropCols);
                            }
                        }
                    }
                }
                /* Randomly spawn eaither a rain drop, acid drop, or alkaline drop based on drop chances for each. 
                 * Apply attributes randomly selected above.
                 */
                WeightedRandomExecutor wre = new WeightedRandomExecutor(
                new WeightedRandomParam(() => currDrop = new RainDrop(dropDict)
                {
                    Position = new Vector2(dropCols[randCol], scaledDropFrameHeight / 2),
                    DropSpeed = randDropSpeed,
                    Scale = dropScale
                }, rainDropChance),
                new WeightedRandomParam(() => currDrop = new AcidRainDrop(dropDict)
                {

                    Position = new Vector2(dropCols[randCol], scaledDropFrameHeight / 2),
                    DropSpeed = randDropSpeed,
                    Scale = dropScale
                }, acidDropChance),
                new WeightedRandomParam(() => currDrop = new AlkRainDrop(dropDict)
                {
                    Position = new Vector2(dropCols[randCol], scaledDropFrameHeight / 2),
                    DropSpeed = randDropSpeed,
                    Scale = dropScale
                }, alkDropChance)); 
                wre.Execute();
                if(dropCount < maxDrops)
                {
                    sprites.Add(currDrop);
                    dropCount++;
                }
            }
            /* Checks for collisions between cup and drops. */
            foreach(var sprite in sprites)
            {
                if(sprite is Drop)
                {
                    var drop = (Drop)sprite;
                    if (drop.IsCupCollision(cup) && sprite is RainDrop)
                    {
                        cup.IncreaseFrame();
                        var rainDrop = sprite as RainDrop;
                        dropsCaught++;
                        dropCount--;
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
                        dropCount--;
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
                        dropCount--;
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

            /* Check if any sprite.IsRemoved property has been set to true. If so remove from sprites List */
            for (int i = 0; i < sprites.Count; i++)
            {
                var sprite = sprites[i];

                if (sprite.IsRemoved)
                {
                    sprites.RemoveAt(i);
                    i--;
                }
                
            }
            /* If player catches the required number of rain drops increase level difficulty and reset game. */
            if (dropsCaught >= catchCount)
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
            /* If player loses all live, reset difficulty and game. */
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
            spriteBatch.Draw(bgTexture, new Rectangle(0, 0, RainDropsGame.ScreenWidth, RainDropsGame.ScreenHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            //Draw ph bar
            spriteBatch.Draw(phBarTexture, new Rectangle(0, RainDropsGame.ScreenHeight - phBarHeight, RainDropsGame.ScreenWidth, phBarHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.9f);
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
    #endregion
}
