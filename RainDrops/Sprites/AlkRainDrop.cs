using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RainDrops.Models;
using RainDrops.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Sprites
{
    internal class AlkRainDrop : Drop
    {
        public AlkRainDrop(Dictionary<string, Animation> animations) : base(animations)
        {
            animationNum = 2;
            WeightedRandomExecutor wre = new WeightedRandomExecutor(
                new WeightedRandomParam(() => { PH = 14; }, 20), // 20% chance for PH to be 14
                new WeightedRandomParam(() => { PH = 11; }, 80));   // 80% chance for PH to be 11
            wre.Execute();
        }

    }
}
