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
    internal class AcidRainDrop : Drop
    {
        public AcidRainDrop(Dictionary<string, Animation> animations) : base(animations)
        {
            animationNum = 1;
            WeightedRandomExecutor wre = new WeightedRandomExecutor(
                new WeightedRandomParam(() => { PH = 0; }, 20), // 20% chance for PH to be 0
                new WeightedRandomParam(() => { PH = 3; }, 80));   // 80% chance for PH to be 3
            wre.Execute();
        }
    }
}
