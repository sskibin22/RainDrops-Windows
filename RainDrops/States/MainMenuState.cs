using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RainDrops.Controls;
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
        public MainMenuState(RainDropsGame game, ContentManager content) : base(game, content)
        {
        }

        public override void LoadContent()
        {
            var buttonTexture = _content.Load<Texture2D>("UI/simpleButton");
            var buttonFont = _content.Load<SpriteFont>("Fonts/scoreFont");

            backgroundTexture = _content.Load<Texture2D>("BackGrounds/MenuBG");

            //instantiate new game button
            _components = new List<Component>()
            {
                new Button(buttonTexture, buttonFont)
                {
                    Position = new Vector2(RainDropsGame.ScreenWidth/2, (int)RainDropsGame.ScreenHeight*0.75f),
                    Text = "Start Game",
                    Click = new EventHandler(StartGameButton_Click),
                    Layer = 0.1f
                },

                new Button(buttonTexture, buttonFont)
                {
                    Position = new Vector2(RainDropsGame.ScreenWidth/2, (int)(RainDropsGame.ScreenHeight*0.75f)+50),
                    Text = "Quit Game",
                    Click = new EventHandler(QuitGameButton_Click),
                    Layer = 0.1f
                },
            };
        }

        private void StartGameButton_Click(object sender, EventArgs args)
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
            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, RainDropsGame.ScreenWidth, RainDropsGame.ScreenHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            
            foreach (var component in _components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
