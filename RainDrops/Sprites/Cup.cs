using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RainDrops.Models;
using RainDrops.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Sprites
{
    internal class Cup : Sprite
    {
        public bool splashing = false;
        public bool acid = false;
        public int dropCount;

        private Color green1, green2, green3, green4,
                      purple1, purple2, purple3, purple4,
                      blue;
        public Cup(Dictionary<string, Texture2D> textures, Dictionary<string, Animation> animations) : base(textures, animations)
        {
            blue = new Color(13, 73, 93);
            green1 = new Color(13, 90, 93);
            green2 = new Color(13, 93, 61);
            green3 = new Color(13, 91, 46);
            green4 = new Color(28, 77, 24);
            purple1 = new Color(44, 50, 95);
            purple2 = new Color(54, 34, 89);
            purple3 = new Color(74, 29, 86);
            purple4 = new Color(61, 21, 68);

            dropCount = 0;
            Scale = 0.9f;
            Position = new Vector2(RainDropsGame.ScreenWidth / 2, RainDropsGame.ScreenHeight - (Rect.Height / 2) - GameState.phBarHeight);
        }

        public override void Update(GameTime gameTime)
        {
            Move();
            if (!splashing)
            {
                return;
            }
            Splash(gameTime);
            //if (!acid)
            //{
            //    Splash(gameTime);
            //}
            //else
            //{
            //    Fizz(gameTime);
            //}
            
            
        }
        public void TextureTo(string key)
        {
                _texture = _textures[key];
        }
        public void IncreaseDropCount()
        {
            if(dropCount < 30)
                dropCount++;
        }
        public void DecreaseDropCount()
        {
            if(dropCount > 0)
                dropCount--;
        }
        public void IncreaseFrame()
        {
            if (_animations["defaultWater"].CurrentFrame < _animations["defaultWater"].FrameCount - 1)
            {
                _animations["defaultWater"].CurrentFrame++;
            }
        }
        public void DecreaseFrame()
        {
            if (_animations["defaultWater"].CurrentFrame > 0)
            {
                _animations["defaultWater"].CurrentFrame--;
            }
        }
        public bool EmptyCup(GameTime gameTime)
        {
            if(_animationManager.PlayOnceBackwards(_animations["defaultWater"], gameTime))
            {
                Reset();
                GameState.emptyingCup = false;
                return true;
            }
            return false;
        }
        public void Splash(GameTime gameTime)
        {
            if (dropCount - 1 >= 0 && _animationManager.PlayOnce(_animations[$"splash_{dropCount-1}"], gameTime))
            {
                _animationManager.SetAnimation(_animations["defaultWater"]);
                splashing = false;
            }

        }
        public void Fizz(GameTime gameTime)
        {
            if (dropCount - 1 >= 0 && _animationManager.PlayOnce(_animations[$"acid_30"], gameTime))
            {
                _animationManager.SetAnimation(_animations["defaultWater"]);
                splashing = false;
                acid = false;
            }

        }
        public bool IsFull()
        {
            if(dropCount >= 30)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Reset()
        {
            dropCount = 0;
            _texture = _textures["defaultCup"];
            _animations["defaultWater"].CurrentFrame = 0;
            _animationManager.SetAnimation(_animations["defaultWater"]);
        }
        private void Move()
        {
            X = RainDropsGame.scaledMouse.X;
            X = MathHelper.Clamp(Position.X, Rect.Width / 2, RainDropsGame.ScreenWidth - Rect.Width/2);
        }
    }
}
