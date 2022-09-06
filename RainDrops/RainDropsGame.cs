using System;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
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
        private RenderTarget2D renderTarget;

        private Rectangle nativeWindowRectangle;
        private Rectangle windowBoxingRect;
        private float WindowAspect { get { return Window.ClientBounds.Width / (float)Window.ClientBounds.Height; } }
        private float nativeAspect;

        private State _currentState;
        private State? _nextState;

        public static MouseState mouseState, prevMouseState;
        public static Vector2 ClientMouse { get { return new Vector2(mouseState.X, mouseState.Y); } }

        public static Vector2 scaledMouse; // Accurate Mouse X Y when scaling window
        public static LevelManager levelManager;
        public static Random Random = new();

        public static int ScreenWidth = 608; //562
        public static int ScreenHeight = 1080; //1000

        private int ScreenWidthMin = 300;
        private int ScreenHeightMin = 533;

        public RainDropsGame() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.SynchronizeWithVerticalRetrace = true; //vsync
            graphics.HardwareModeSwitch = true;
            Window.AllowUserResizing = true;
            Content.RootDirectory = "Content";
            IsFixedTimeStep = true; //default: 60 fps
            IsMouseVisible = true;
        }
        protected override void Initialize()
        {

            nativeWindowRectangle = new Rectangle(0, 0, ScreenWidth, ScreenHeight);
            nativeAspect = nativeWindowRectangle.Width / (float)nativeWindowRectangle.Height;

            graphics.PreferredBackBufferWidth = nativeWindowRectangle.Width;
            graphics.PreferredBackBufferHeight = nativeWindowRectangle.Height;
            graphics.ApplyChanges();

            PresentationParameters pp = graphics.GraphicsDevice.PresentationParameters;
            renderTarget = new RenderTarget2D(GraphicsDevice, nativeWindowRectangle.Width, nativeWindowRectangle.Height, false, SurfaceFormat.Color, DepthFormat.None, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);
            windowBoxingRect = nativeWindowRectangle;

            Window.ClientSizeChanged += UpdateWindowBoxingRect;

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
            prevMouseState = mouseState;
            UpdateMouseState(GetResolutionMatrix());

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
        private void UpdateMouseState(Matrix transform)
        {
            mouseState = Mouse.GetState();
            scaledMouse = Vector2.Transform(ClientMouse, transform);
        }
        private Matrix GetResolutionMatrix() // Fixes the mouse
        {
            float xRatio = (float)nativeWindowRectangle.Width / Window.ClientBounds.Width;
            float yRatio = (float)nativeWindowRectangle.Height / Window.ClientBounds.Height;
            return Matrix.CreateTranslation(windowBoxingRect.X, windowBoxingRect.Y, 0) *
                   Matrix.CreateScale(xRatio, yRatio, 1f);
        }
        private void UpdateWindowBoxingRect(object sender, EventArgs e) // Updates windowBoxingRect
        {
            int windowWidth, windowHeight;

            if (Window.ClientBounds.Width > ScreenWidth || Window.ClientBounds.Height > ScreenHeight)
            {
                windowWidth = ScreenWidth;
                windowHeight = ScreenHeight;
            }
            else if (Window.ClientBounds.Width < ScreenWidthMin || Window.ClientBounds.Height < ScreenHeightMin)
            {
                windowWidth = ScreenWidthMin;
                windowHeight = ScreenHeightMin;
            }
            else
            {
                windowWidth = Window.ClientBounds.Width; windowHeight = Window.ClientBounds.Height;
            }

            if (WindowAspect <= nativeAspect)
            {
                int presentHeight = (int)((windowWidth / nativeAspect) + 0.5f);
                windowBoxingRect = new Rectangle(0, 0, windowWidth, presentHeight);
            }
            else
            {
                int presentWidth = (int)((windowHeight * nativeAspect) + 0.5f);
                windowBoxingRect = new Rectangle(0, 0, presentWidth, windowHeight);
            }

            graphics.PreferredBackBufferWidth = windowBoxingRect.Width;
            graphics.PreferredBackBufferHeight = windowBoxingRect.Height;
            graphics.ApplyChanges();

        }

        public void ChangeState(State state)
        {
            _nextState = state;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.White);

            _currentState.Draw(gameTime, spriteBatch);

            GraphicsDevice.SetRenderTarget(null); // Sets target to back buffer
            GraphicsDevice.Clear(Color.Black); // Clears for black windowboxing

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, DepthStencilState.Default, RasterizerState.CullNone, null);
            spriteBatch.Draw(renderTarget, windowBoxingRect, Color.White); // Draw the _nativeRenderTarget as a texture
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
