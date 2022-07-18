using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RainDrops.Sprites;
using RainDrops.Tools;

namespace RainDrops
{
    internal class RainDropsGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public static Random Random = new();

        public static int ScreenWidth = 562;
        public static int ScreenHeight = 1000;
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
        private List<Sprite> sprites;

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

        private bool showHitBox = false;
        public RainDropsGame() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.SynchronizeWithVerticalRetrace = true; //vsync
            Content.RootDirectory = "Content";
            IsFixedTimeStep = true; //default: 60 fps
            IsMouseVisible = true;
        }
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.ApplyChanges();

            dropCols = new List<float>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            bg = Content.Load<Texture2D>("BackGrounds/MountainsBG");
            phBar = Content.Load<Texture2D>("UI/phBar");
            cloudBar = Content.Load<Texture2D>("UI/rainCloudBar");
            phSelectTexture = Content.Load<Texture2D>("UI/phSelectCircle");

            font = Content.Load<SpriteFont>("Fonts/scoreFont");

            cupTexture = Content.Load<Texture2D>("Cups/emptyCup");


            rainCupTextures = new List<Texture2D>();
            for(int i = 0; i < 31; i++)
            {
                if(i < 10)
                {
                    rainCupTextures.Add(Content.Load<Texture2D>($"Cups/RainCupFrames/cup0{i}"));
                }
                else
                {
                    rainCupTextures.Add(Content.Load<Texture2D>($"Cups/RainCupFrames/cup{i}"));
                }
                
            }
            rainDropTexture = Content.Load<Texture2D>("Drops/rainDrop");
            acidDropTexture = Content.Load<Texture2D>("Drops/acidDrop");
            alkDropTexture = Content.Load<Texture2D>("Drops/alkDrop");

            numDropCols = (int)(ScreenWidth / (rainDropTexture.Width*dropScale));
            float total = 0;
            for (int i = 0; i < numDropCols; i++)
            {
                if(i == 0) 
                {
                    total += ((rainDropTexture.Width*dropScale) / 2) + 10;
                     
                }
                else
                {
                    total += rainDropTexture.Width*dropScale;
                }
                dropCols.Add(total);
                
            }

            sectionWidth = ScreenWidth/16;
            int startPoint = (ScreenWidth / 2) - (sectionWidth * 7);
            for(int i = 0; i < 15; i++)
            {
                numPositions.Add(startPoint);
                startPoint += sectionWidth;
            }
                   
            Restart();

            base.LoadContent();
        }

        private void Restart()
        {
            lifeCount = 3;
            dropCount = 0;
            phSelect = new PHselector(phSelectTexture, 0f, 0.7f, 0.4f);
            cup = new Cup(graphics.GraphicsDevice, rainCupTextures[0], 0f, 1f, 0.9f); //cupTexture

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

        protected override void UnloadContent()
        {
            bg.Dispose();
            phBar.Dispose();
            phSelectTexture.Dispose();
            cupTexture.Dispose();
            rainDropTexture.Dispose();
            acidDropTexture.Dispose();
            alkDropTexture.Dispose();
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsActive) //pause game if inactive
            {
                KeyboardState keyState = Keyboard.GetState();
                MouseState mouseState = Mouse.GetState();

                previousKeyState = currKeyState;
                currKeyState = keyState;
                //if ESC key is pressed exit game
                if (keyState.IsKeyDown(Keys.Escape))
                    Exit();
                //if R key is pressed, show hitbox borders
                if (previousKeyState.IsKeyDown(Keys.R) && currKeyState.IsKeyUp(Keys.R))
                    showHitBox = !showHitBox;
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
                    sprite.Update(gameTime, sprites); 
                }
                //if timer is greater than drop rate, reset timer, spawn a new drop
                if (timer > dropSpawnRate)
                {                   
                    timer = 0f;
                    randCol = Random.Next(0, numDropCols);
                    randDropSpeed = (float)(Random.NextDouble() * (dropSpeedMax - dropSpeedMin) + dropSpeedMin);
                   
                    foreach (var sprite in sprites)
                    {
                        if(sprite is Drop)
                        {
                            var drop = (Drop)sprite;
                            if (drop.Position.X == dropCols[randCol])
                            {
                                if (drop.Position.Y - drop.Rect.Height/2 < rainDropTexture.Height * dropScale)
                                {
                                    randCol = Random.Next(0, numDropCols);
                                }                             
                            }                          
                        }
                    }
                    //System.Diagnostics.Debug.WriteLine(randDropSpeed);
                    WeightedRandomExecutor wre = new WeightedRandomExecutor(
                    new WeightedRandomParam(() => currDrop = new RainDrop(graphics.GraphicsDevice, rainDropTexture, 0f, dropScale, dropLayerDepth)
                    {                       
                        Position = new Vector2(dropCols[randCol], -rainDropTexture.Height * dropScale / 2),
                        DropSpeed = randDropSpeed,
                    }, rainDropChance),
                    new WeightedRandomParam(() => currDrop = new AcidRainDrop(graphics.GraphicsDevice, acidDropTexture, 0f, dropScale, dropLayerDepth)
                    {                       
                        Position = new Vector2(dropCols[randCol], -acidDropTexture.Height * dropScale / 2),
                        DropSpeed = randDropSpeed,
                    }, acidDropChance),
                    new WeightedRandomParam(() => currDrop = new AlkRainDrop(graphics.GraphicsDevice, alkDropTexture, 0f, dropScale, dropLayerDepth)
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
                    if(sprite is Cup)
                    {
                        if (sprite is Cup cupInstance && cupInstance.dropsCaught == 30)
                        {
                            Restart();
                        }
                    }
                }
                if(lifeCount < 0)
                {
                    Restart();
                }

                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSeaGreen);
            
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            //Draw background
            spriteBatch.Draw(bg, new Rectangle(0, 0, ScreenWidth, ScreenHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            //Draw ph bar
            spriteBatch.Draw(phBar, new Rectangle(0, ScreenHeight - phBarHeight, ScreenWidth, phBarHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.2f);
            //Draw cloud bar
            spriteBatch.Draw(cloudBar, new Rectangle(0, 0, ScreenWidth, cloudBarHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.2f);

            //spriteBatch.Draw(rainDropTexture, new Rectangle(10, 1, 22, 25), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.3f);
            //spriteBatch.Draw(rainDropTexture, new Rectangle(42, 1, 22, 25), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.3f);
            //spriteBatch.Draw(rainDropTexture, new Rectangle(74, 1, 22, 25), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.3f);

            //Draw all sprites and show their hitboxes if showHitBox is set to true
            foreach (var sprite in sprites)
            {
                sprite.ShowRect = showHitBox;
                sprite.Draw(spriteBatch);
                //if(sprite is Drop)
                //{
                //    var drop = sprite as Drop;
                //    spriteBatch.DrawString(font, $"{drop.PH}", new Vector2(drop.Position.X-5, drop.Position.Y-5), Color.Black, 0f, Vector2.Zero, 1.2f, SpriteEffects.None, 0.11f);
                //}
            }
            //Draw score
            spriteBatch.DrawString(font, $"Drops: {cup.dropsCaught}", new Vector2(cup.Position.X - 30, cup.Position.Y - 20), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void OnActivated(object sender, EventArgs args)
        {
            Window.Title = "RainDrops(Active)";
            base.OnActivated(sender, args);
        }
        protected override void OnDeactivated(object sender, EventArgs args)
        {
            Window.Title = "RainDrops(Paused)";
            base.OnDeactivated(sender, args);
        }
    }
}
