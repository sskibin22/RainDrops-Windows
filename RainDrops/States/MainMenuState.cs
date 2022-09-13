using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RainDrops.Controls;
using RainDrops.Emitters;
using RainDrops.Models;
using RainDrops.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RainDrops.States
{
    internal class MainMenuState : State
    {
        private List<Component> _components;

        private Texture2D backgroundTexture;
        private MenuTitle menuTitle;
        private RainEmitter rainEmitterMenu;
        private GameOverPrompt menuPanel;
        public MainMenuState(RainDropsGame game, ContentManager content) : base(game, content)
        {
        }

        public override void LoadContent()
        {
            var buttonTexture = _content.Load<Texture2D>("UI/simpleButton");
            var buttonFont = _content.Load<SpriteFont>("Fonts/scoreFont");

            backgroundTexture = _content.Load<Texture2D>("BackGrounds/MountainsBG2");

            rainEmitterMenu = new RainEmitter(new Rain(_content.Load<Texture2D>("EmitterSprites/rain_1")));

            menuTitle = new MenuTitle(new Dictionary<string, Animation>() 
            { 
                { "startUp", new Animation(_content.Load<Texture2D>("TextSprites/RainDropsStartPacked"), 60, 8, 8) { FrameSpeed = 83.33f }},
                { "loop", new Animation(_content.Load<Texture2D>("TextSprites/RainDropsTitle"), 3) { FrameSpeed = 83.33f }}
            });
            
            menuPanel = new GameOverPrompt(new Dictionary<string, Animation>() { { "shine", new Animation(_content.Load<Texture2D>("UI/Panels/gameOverBG"), 24) { FrameSpeed = 100f } } });

            _components = new List<Component>()
            {
                new GameOverPrompt(new Dictionary<string, Animation>() { { "shine", new Animation(_content.Load<Texture2D>("UI/Panels/gameOverBG"), 24) { FrameSpeed = 100f } } })
                {
                    Position = new Vector2(RainDropsGame.ScreenWidth*0.50f, RainDropsGame.ScreenHeight+35),
                    Scale = 1f
                },

                new AnimatedButton(new Animation(_content.Load<Texture2D>("UI/Buttons/playButton"), 2){FrameSpeed = 83.33f})
                {
                    Position = new Vector2(RainDropsGame.ScreenWidth*0.50f, RainDropsGame.ScreenHeight - 130),
                    Click = new EventHandler(PlayButton_Click),
                    Layer = 1f,
                    Opacity = 1f,
                    Scale = 1f,
                    IsActive = true
                },
                new AnimatedButton(new Animation(_content.Load<Texture2D>("UI/Buttons/statsButton"), 2){FrameSpeed = 83.33f})
                {
                    Position = new Vector2(RainDropsGame.ScreenWidth*0.275f, RainDropsGame.ScreenHeight - 80),
                    Click = new EventHandler(StatsButton_Click),
                    Layer = 1f,
                    Opacity = 1f,
                    Scale = 1f,
                    IsActive = true
                },
                new AnimatedButton(new Animation(_content.Load<Texture2D>("UI/Buttons/quitGameButton"), 2){FrameSpeed = 83.33f})
                {
                    Position = new Vector2(RainDropsGame.ScreenWidth*0.725f, RainDropsGame.ScreenHeight - 80),
                    Click = new EventHandler(QuitGameButton_Click),
                    Layer = 1f,
                    Opacity = 1f,
                    Scale = 1f,
                    IsActive = true
                },
                new AnimatedButton(new Animation(_content.Load<Texture2D>("UI/Buttons/helpButton"), 2){FrameSpeed = 83.33f})
                {
                    Position = new Vector2(RainDropsGame.ScreenWidth*0.50f, RainDropsGame.ScreenHeight - 40),
                    Click = new EventHandler(HelpButton_Click),
                    Layer = 1f,
                    Opacity = 1f,
                    Scale = 1f,
                    IsActive = true
                }
            };
        }

        private void HelpButton_Click(object? sender, EventArgs e)
        {
            //TODO: help state
        }

        private void StatsButton_Click(object? sender, EventArgs e)
        {
            //TODO: stats state
        }

        private void PlayButton_Click(object sender, EventArgs args)
        {
            _game.ChangeState(new GameState(_game, _content));
        }

        private void QuitGameButton_Click(object sender, EventArgs args)
        {
            _game.Exit();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //TODO: Remove sprites if neccessary
        }

        public override void Update(GameTime gameTime)
        {
            rainEmitterMenu.Update(gameTime);
            rainEmitterMenu.RemoveParticles();
            menuTitle.Update(gameTime);
            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, RainDropsGame.ScreenWidth, RainDropsGame.ScreenHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);

            rainEmitterMenu.Draw(gameTime, spriteBatch);

            menuTitle.Draw(gameTime, spriteBatch);

            foreach (var component in _components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
