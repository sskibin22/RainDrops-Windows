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
        protected float generateTimer;
        protected Sprite particlePrefab;
        public List<Sprite> particles;
        public bool isActive = false;

        public float GenerateSpeed { get; set; }
        public int MaxParticles { get; set; }

        public Emitter(Sprite sprite)
        {
            particlePrefab = sprite;
            particles = new List<Sprite>();
        }

        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                generateTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                AddParticle();
            }

            foreach (var particle in particles)
            {
                particle.Update(gameTime);
            }
        }

        public abstract void RemoveParticles();
        //{
        //    for (int i = 0; i < particles.Count; i++)
        //    {
        //        if (particles[i].IsRemoved)
        //        {
        //            particles.RemoveAt(i);
        //            i--;
        //        }
        //    }
        //}

        protected abstract void AddParticle();

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
