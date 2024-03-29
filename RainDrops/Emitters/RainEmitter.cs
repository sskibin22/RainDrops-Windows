﻿using Microsoft.Xna.Framework;
using RainDrops.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Emitters
{
    internal class RainEmitter : Emitter
    {
        public RainEmitter(Sprite sprite) : base(sprite)
        {
            GenerateSpeed = 10f;
            MaxParticles = 150;
            isActive = true;
    }

        public override void RemoveParticles()
        {
            for (int i = 0; i < particles.Count; i++)
            {
                if (particles[i].IsRemoved)
                {
                    particles.RemoveAt(i);
                    i--;
                }
            }
        }

        protected override void AddParticle()
        {
            if (generateTimer > GenerateSpeed)
            {
                generateTimer = 0;

                if (particles.Count < MaxParticles)
                {
                    particles.Add(GenerateParticle());
                }
            }
        }

        protected override Sprite GenerateParticle()
        {
            var rain = particlePrefab.Clone() as Rain;

            var xPosition = RainDropsGame.Random.Next(-300, RainDropsGame.ScreenWidth);
            var ySpeed = RainDropsGame.Random.Next(8, 10); //RainDropsGame.Random.Next(80000, 100000) / 10000f;
           
            rain.Position = new Vector2(xPosition, -rain.Rect.Height);
            rain.Opacity =  (float)RainDropsGame.Random.Next(2, 8) / 10f; //(float)RainDropsGame.Random.NextDouble();
            rain.Rotation = -0.25f;
            rain.Scale = RainDropsGame.Random.Next(6, 8) / 10f;
            rain.Velocity = new Vector2(1.5f, ySpeed);

            return rain;
        }
    }
}
