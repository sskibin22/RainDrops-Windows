using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RainDrops
{
    internal class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D bg;
        //private KeyboardState previousState;
        public Game1() : base()
        {

            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.SynchronizeWithVerticalRetrace = true; //vsync
            this.Content.RootDirectory = "Content";
            this.IsFixedTimeStep = true; //default: 60 fps
            this.IsMouseVisible = true;
        }
        protected override void Initialize()
        {
            this.graphics.PreferredBackBufferWidth = 562;
            this.graphics.PreferredBackBufferHeight = 1000;
            this.graphics.ApplyChanges();
            

            base.Initialize();
            //this.previousState = Keyboard.GetState();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.bg = this.Content.Load<Texture2D>("BackGrounds/MountainsBG");
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            bg.Dispose();
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsActive) //pause game if inactive
            {
                KeyboardState keyState = Keyboard.GetState();
                MouseState mouseState = Mouse.GetState();
                if (keyState.IsKeyDown(Keys.Escape))
                    Exit();

                base.Update(gameTime);
                //previousState = keyState; //required for single press keydown functionality
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.LightSeaGreen);
            Viewport viewport = this.GraphicsDevice.Viewport;
            //var fps = 1 / gameTime.ElapsedGameTime.TotalSeconds; //calculate fps
            //this.Window.Title = fps.ToString(); //display fps to window title
            this.spriteBatch.Begin();
            spriteBatch.Draw(this.bg, new Rectangle(0, 0, 562, 1000), Color.White); //draw background
            this.spriteBatch.End();
            base.Draw(gameTime);
        }

        protected override void OnActivated(object sender, EventArgs args)
        {
            this.Window.Title = "RainDrops";
            base.OnActivated(sender, args);
        }
        protected override void OnDeactivated(object sender, EventArgs args)
        {
            this.Window.Title = "RainDrops";
            base.OnDeactivated(sender, args);
        }
    }
}
