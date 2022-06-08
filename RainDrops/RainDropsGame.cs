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

        private SpriteFont font;

        private Texture2D bg;
        private Texture2D phBar;
        private Texture2D phSelectTexture;
        private Texture2D cupTexture;
        private Texture2D rainDropTexture;
        private Texture2D acidDropTexture;
        private Texture2D alkDropTexture;

        public static float dropScale = 0.75f;

        public static PHselector phSelect;
        private Cup cup;
        private Drop currDrop;
        private List<Sprite> sprites;

        private float[,] xRegions = new float[2,4];
        int reg = 0;

        public static int dropCount = 0;

        private float timer = 0;
        private float regTimer = 0;

        private bool hasStarted = false;

        private float dropLayerDepth = 0.1f;

        private float dropSpawnRate = 200f;
        private float regionShiftRate = 400f;

        private int dropSpeedMin = 100;
        private int dropSpeedMax = 250;

        private double rainDropChance = 20;
        private double acidDropChance = 40;
        private double alkDropChance = 40;

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            bg = Content.Load<Texture2D>("BackGrounds/MountainsBG");
            phBar = Content.Load<Texture2D>("UI/phBar");
            phSelectTexture = Content.Load<Texture2D>("UI/phSelectCircle");

            font = Content.Load<SpriteFont>("Fonts/scoreFont");

            cupTexture = Content.Load<Texture2D>("Cups/emptyCup");
            rainDropTexture = Content.Load<Texture2D>("Drops/rainDrop");
            acidDropTexture = Content.Load<Texture2D>("Drops/acidDrop");
            alkDropTexture = Content.Load<Texture2D>("Drops/alkDrop");

            float dropBounds = ScreenWidth - ((rainDropTexture.Width * dropScale / 2) * 2);
            float regionLength = dropBounds / 4f;
            xRegions[0, 0] = rainDropTexture.Width * dropScale / 2;
            xRegions[1, 0] = regionLength;
            xRegions[0, 1] = regionLength;
            xRegions[1, 1] = regionLength * 2;
            xRegions[0, 2] = regionLength * 2;
            xRegions[1, 2] = regionLength * 3;
            xRegions[0, 3] = regionLength * 3;
            xRegions[1, 3] = ScreenWidth - (rainDropTexture.Width * dropScale / 2);

            Restart();

            base.LoadContent();
        }

        private void Restart()
        {
            dropCount = 0;
            phSelect = new PHselector(phSelectTexture, 0f, 0.7f, 0.4f);
            cup = new Cup(graphics.GraphicsDevice, cupTexture, 0f, 1f, 0.9f); 

            sprites = new List<Sprite>()
            {
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
                regTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                //Update each sprite in sprites List
                foreach (var sprite in sprites)
                {  
                    sprite.Update(gameTime, sprites); 
                }
                //if timer is greater than drop rate, reset timer, spawn a new drop
                if (timer > dropSpawnRate)
                {
                    timer = 0f;
                    WeightedRandomExecutor wre = new WeightedRandomExecutor(
                    new WeightedRandomParam(() => currDrop = new RainDrop(graphics.GraphicsDevice, rainDropTexture, 0f, dropScale, dropLayerDepth)
                    {
                        //Position = new Vector2(Random.Next((int)(rainDropTexture.Width * dropScale / 2), (int)(ScreenWidth - rainDropTexture.Width * dropScale / 2)), -rainDropTexture.Height * dropScale / 2),
                        Position = new Vector2(Random.Next((int)xRegions[0,reg], (int)xRegions[1,reg]), -rainDropTexture.Height * dropScale / 2),
                        DropSpeed = Random.Next(dropSpeedMin, dropSpeedMax),
                    }, rainDropChance),
                    new WeightedRandomParam(() => currDrop = new AcidRainDrop(graphics.GraphicsDevice, acidDropTexture, 0f, dropScale, dropLayerDepth)
                    {
                        //Position = new Vector2(Random.Next((int)(acidDropTexture.Width * dropScale / 2), (int)(ScreenWidth - acidDropTexture.Width * dropScale / 2)), -acidDropTexture.Height * dropScale / 2),
                        Position = new Vector2(Random.Next((int)xRegions[0, reg], (int)xRegions[1, reg]), -acidDropTexture.Height * dropScale / 2),
                        DropSpeed = Random.Next(dropSpeedMin, dropSpeedMax),
                    }, acidDropChance),   
                    new WeightedRandomParam(() => currDrop = new AlkRainDrop(graphics.GraphicsDevice, alkDropTexture, 0f, dropScale, dropLayerDepth)
                    {
                        //Position = new Vector2(Random.Next((int)(alkDropTexture.Width * dropScale / 2), (int)(ScreenWidth - alkDropTexture.Width * dropScale / 2)), -alkDropTexture.Height * dropScale / 2),
                        Position = new Vector2(Random.Next((int)xRegions[0, reg], (int)xRegions[1, reg]), -alkDropTexture.Height * dropScale / 2),
                        DropSpeed = Random.Next(dropSpeedMin, dropSpeedMax),
                    }, alkDropChance)); 
                    wre.Execute();
                    sprites.Add(currDrop);
                    dropCount++;                   
                }
                if (regTimer > regionShiftRate)
                {
                    regTimer = 0f;
                    reg = Random.Next(0,4);
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
            spriteBatch.Draw(phBar, new Rectangle(0, 0, ScreenWidth, phBarHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.2f);

            //Draw all sprites and show their hitboxes if showHitBox is set to true
            foreach (var sprite in sprites)
            {
                sprite.ShowRect = showHitBox;
                sprite.Draw(spriteBatch);
                if(sprite is Drop)
                {
                    var drop = sprite as Drop;
                    spriteBatch.DrawString(font, $"{drop.PH}", new Vector2(drop.Position.X-5, drop.Position.Y-5), Color.Black, 0f, Vector2.Zero, 1.2f, SpriteEffects.None, 0.11f);
                }
            }
            //Draw score
            spriteBatch.DrawString(font, $"Drops: {cup.dropsCaught}", new Vector2(cup.Position.X - 30, cup.Position.Y - 20), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, $"PH: {cup.phScore}", new Vector2(cup.Position.X - 20, cup.Position.Y), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
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
