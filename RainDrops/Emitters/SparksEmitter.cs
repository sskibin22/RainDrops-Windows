using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RainDrops.Sprites;
using RainDrops.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Emitters
{
    internal class SparksEmitter : Emitter
    {
        public SparksEmitter(Sprite sprite) : base(sprite)
        {
            GenerateSpeed = 10f;
            MaxParticles = 200;
            isActive = false;
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
            var spark = particlePrefab.Clone() as Spark;

            var xPosition = GameState.lightningBolt.X;
            var yPosition = GameState.cup.Y - (GameState.cup.Rect.Height / 2) - 50;

            var ySpeed = (float)RainDropsGame.Random.Next(2, 5);
            var sign = RainDropsGame.Random.Next(2);
            int xSpeed;
            if (sign == 0)
            {
                xSpeed = RainDropsGame.Random.Next(1, 3);
            }
            else
            {
                xSpeed = RainDropsGame.Random.Next(-3, 0);
            }
            var colorIdx = RainDropsGame.Random.Next(GameState.sparkColors.Count);
            spark.Color = GameState.sparkColors[colorIdx];
            spark.lifespan = RainDropsGame.Random.Next(800, 1201);
            spark.Position = new Vector2(xPosition, yPosition);
            spark.Velocity = new Vector2(xSpeed, ySpeed);

            return spark;
        }
    }
}
