﻿using Microsoft.Xna.Framework.Graphics;
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
        public AlkRainDrop(GraphicsDevice graphics, Texture2D texture, float rotation, float scale, float layer) : base(graphics, texture, rotation, scale, layer)
        {
            WeightedRandomExecutor wre = new WeightedRandomExecutor(
                new WeightedRandomParam(() => PH = 14, 2.5), // 2.5% chance for PH to be 14
                new WeightedRandomParam(() => PH = 13, 5),   // 5.0% chance for PH to be 13
                new WeightedRandomParam(() => PH = 12, 7.5), // 7.5% chance for PH to be 12
                new WeightedRandomParam(() => PH = 11, 10),  // 10.% chance for PH to be 11
                new WeightedRandomParam(() => PH = 10, 15),  // 15.% chance for PH to be 10
                new WeightedRandomParam(() => PH = 9, 20),  // 20.% chance for PH to be 9
                new WeightedRandomParam(() => PH = 8, 40)); // 40.% chance for PH to be 8
            wre.Execute();

        }



    }
}
