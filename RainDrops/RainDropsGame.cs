using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RainDrops.Sprites;

namespace RainDrops
{
    internal class RainDropsGame : Game
    {

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public static Random Random = new();

        public static int ScreenWidth = 562;
        public static int ScreenHeight = 1000;

        private SpriteFont font;

        private Texture2D bg;
        private Texture2D cupTexture;
        private Texture2D rainDropTexture;
        private Texture2D acidDropTexture;

        private Cup cup;
        private List<Sprite> sprites;

        private float timer = 0;
        private float timer2 = 0;
        private bool hasStarted = false;
        private float rainDropRate = 0.30f;
        private float acidDropRate = 0.35f;
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

            font = Content.Load<SpriteFont>("Fonts/scoreFont");

            cupTexture = Content.Load<Texture2D>("Cups/emptyCup");
            rainDropTexture = Content.Load<Texture2D>("Drops/rainDropL");
            acidDropTexture = Content.Load<Texture2D>("Drops/acidDropL");

            Restart();

            base.LoadContent();
        }

        private void Restart()
        {
            cup = new Cup(graphics.GraphicsDevice, cupTexture, 0f, 1f, 1f); 
            sprites = new List<Sprite>()
            {
                cup,
            };
            hasStarted = false;
        }

        protected override void UnloadContent()
        {
            bg.Dispose();
            cupTexture.Dispose();
            rainDropTexture.Dispose();
            acidDropTexture.Dispose();
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
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                timer2 += (float)gameTime.ElapsedGameTime.TotalSeconds;
                //Update each sprite in sprites List
                foreach (var sprite in sprites)
                {  
                    sprite.Update(gameTime, sprites); 
                }
                //if timer is greater than rain drop rate, reset timer, spawn a new rain drop
                if(timer > rainDropRate)
                {
                    timer = 0f;
                    sprites.Add(new RainDrop(graphics.GraphicsDevice, rainDropTexture, 0f, 0.75f, 0f));
                }
                if (timer2 > acidDropRate)
                {
                    timer2 = 0f;
                    sprites.Add(new AcidRainDrop(graphics.GraphicsDevice, acidDropTexture, 0f, 0.75f, 0f));
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
            
            spriteBatch.Begin();
            //Draw background
            spriteBatch.Draw(bg, new Rectangle(0, 0, 562, 1000), Color.White);
            //Draw all sprites and show their hitboxes if showHitBox is set to true
            foreach(var sprite in sprites)
            {
                sprite.ShowRect = showHitBox;
                sprite.Draw(spriteBatch);
            }

            spriteBatch.DrawString(font, $"Drops: {cup.dropsCaught}", new Vector2(cup.Position.X - 30, cup.Position.Y - 20), Color.Black);
            spriteBatch.DrawString(font, $"PH: {cup.phScore}", new Vector2(cup.Position.X - 20, cup.Position.Y), Color.Black);
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
