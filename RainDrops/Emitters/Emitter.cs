using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RainDrops.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Emitters
{
    internal abstract class Emitter : Component
    {
        private float generateTimer;
        protected Sprite particlePrefab;
        protected List<Sprite> particles;

        public float GenerateSpeed = 10f;
        public int MaxParticles = 150;

        public Emitter(Sprite sprite)
        {
            particlePrefab = sprite;
            particles = new List<Sprite>();
        }

        public override void Update(GameTime gameTime)
        {
            generateTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            AddParticle();

            foreach (var particle in particles)
            {
                particle.Update(gameTime);
            }

            RemoveParticles();
        }

        private void RemoveParticles()
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

        private void AddParticle()
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

        protected abstract Sprite GenerateParticle();


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var particle in particles)
            {
                particle.Draw(gameTime, spriteBatch);
            }
        }
    }
}
