using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Sprites
{
    internal class CloudBar : Sprite
    {
        public CloudBar(Texture2D texture) : base(texture)
        {
            
        }
        public void MoveLeft()
        {
            Velocity.X = Speed;
            Position -= Velocity;
        }
        public void MoveRight()
        {
            Velocity.X = Speed;
            Position += Velocity;
        }
        public void Reset(CloudBar cb, int sign)
        {
            if (sign == 0)
            {
                position.X = cb.Position.X + cb.Rect.Width;
            }
            else if (sign == 1)
            {
                position.X = cb.Position.X - cb.Rect.Width;
            }
            else throw new Exception("invalid parameter input");
        }
    }
}
