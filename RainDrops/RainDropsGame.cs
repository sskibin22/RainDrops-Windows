using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RainDrops.Managers;
using RainDrops.Sprites;
using RainDrops.States;
using RainDrops.Tools;

namespace RainDrops
{
    internal class RainDropsGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private State _currentState;
        private State? _nextState;

        public static LevelManager levelManager;

        public static Random Random = new();

        public static int ScreenWidth = 562;
        public static int ScreenHeight = 1000;

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

            levelManager = new LevelManager();

            _currentState = new MainMenuState(this, Content);
            _currentState.LoadContent();
            _nextState = null;

            base.LoadContent();
        }

        

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsActive) //pause game if inactive
            {
                if (_nextState != null)
                {
                    _currentState = _nextState;
                    _currentState.LoadContent();

                    _nextState = null;
                }

                _currentState.Update(gameTime);

                _currentState.PostUpdate(gameTime);
                

                base.Update(gameTime);
            }
        }

        public void ChangeState(State state)
        {
            _nextState = state;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSeaGreen);

            _currentState.Draw(gameTime, spriteBatch);

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
