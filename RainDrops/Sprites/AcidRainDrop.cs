using Microsoft.Xna.Framework.Graphics;
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
        public AcidRainDrop(Texture2D texture, float rotation, float scale, float layer) : base(texture, rotation, scale, layer)
        {
            //WeightedRandomExecutor wre = new WeightedRandomExecutor(
            //    new WeightedRandomParam(() => { PH = 0; Value = -40f; }, 2.5), // 2.5% chance for PH to be 0
            //    new WeightedRandomParam(() => { PH = 1; Value = -20f; }, 5),   // 5.0% chance for PH to be 1
            //    new WeightedRandomParam(() => { PH = 2; Value = -15f; }, 7.5), // 7.5% chance for PH to be 2
            //    new WeightedRandomParam(() => { PH = 3; Value = -10f; }, 10),  // 10.% chance for PH to be 3
            //    new WeightedRandomParam(() => { PH = 4; Value = -7.5f; }, 15),  // 15.% chance for PH to be 4
            //    new WeightedRandomParam(() => { PH = 5; Value = -5f; }, 20),  // 20.% chance for PH to be 5
            //    new WeightedRandomParam(() => { PH = 6; Value = -2.5f; }, 40)) ; // 40.% chance for PH to be 6
            //wre.Execute();

            WeightedRandomExecutor wre = new WeightedRandomExecutor(
                new WeightedRandomParam(() => { PH = 0; }, 10), // 2.5% chance for PH to be 0
                new WeightedRandomParam(() => { PH = 3; }, 80));   // 5.0% chance for PH to be 1
            wre.Execute();
        }



    }
}
