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
        public AcidRainDrop(GraphicsDevice graphics, Texture2D texture, float rotation, float scale, float layer) : base(graphics, texture, rotation, scale, layer)
        {
            WeightedRandomExecutor wre = new WeightedRandomExecutor(
                new WeightedRandomParam(() => PH = 0, 2.5), // 2.5% chance for PH to be 0
                new WeightedRandomParam(() => PH = 1, 5),   // 5.0% chance for PH to be 1
                new WeightedRandomParam(() => PH = 2, 7.5), // 7.5% chance for PH to be 2
                new WeightedRandomParam(() => PH = 3, 10),  // 10.% chance for PH to be 3
                new WeightedRandomParam(() => PH = 4, 15),  // 15.% chance for PH to be 4
                new WeightedRandomParam(() => PH = 5, 20),  // 20.% chance for PH to be 5
                new WeightedRandomParam(() => PH = 6, 40)); // 40.% chance for PH to be 6
            wre.Execute();
            
        }



    }
}
