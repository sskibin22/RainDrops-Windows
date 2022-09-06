using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RainDrops.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Managers
{
    internal class StatManager
    {
        private SpriteFont _spriteFont;
        private Vector2 topPosition;
        public bool IsActive { get; set; }
        public int RainDropTotal { get; private set; }
        public int AcidDropTotal { get; private set; }
        public int AlkDropTotal { get; private set; }
        public int MissedDropTotal { get; private set; }
        public StatManager(SpriteFont spriteFont)
        {
            IsActive = false;
            _spriteFont = spriteFont;
            topPosition = new Vector2((RainDropsGame.ScreenWidth / 2), (RainDropsGame.ScreenHeight / 2) - 120);
            RainDropTotal = 0;
            AcidDropTotal = 0;
            AlkDropTotal = 0;
            MissedDropTotal = 0;
        }
        public void IncreaseRainDropTotal()
        {
            RainDropTotal++;
        }
        public void IncreaseAcidDropTotal()
        {
            AcidDropTotal++;
        }
        public void IncreaseAlkDropTotal()
        {
            AlkDropTotal++;
        }
        public void IncreaseMissedDropTotal()
        {
            MissedDropTotal++;
        }
        public void Reset()
        {
            IsActive = false;
            RainDropTotal = 0;
            AcidDropTotal = 0;
            AlkDropTotal = 0;
            MissedDropTotal = 0;
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                spriteBatch.DrawString(_spriteFont, "Stats", topPosition, Color.Black, 0f, _spriteFont.MeasureString("Stats") / 2, 1f, SpriteEffects.None, 1f);
                spriteBatch.DrawString(_spriteFont, "Stats", new Vector2(topPosition.X-2, topPosition.Y+2), Color.Black * 0.4f, 0f, _spriteFont.MeasureString("Stats") / 2, 1f, SpriteEffects.None, 0.99f);
                
                spriteBatch.DrawString(_spriteFont, "_________________________", new Vector2(topPosition.X, topPosition.Y), Color.Black, 0f, _spriteFont.MeasureString("_________________________") / 2, 1f, SpriteEffects.None, 1f);
                spriteBatch.DrawString(_spriteFont, "_________________________", new Vector2(topPosition.X - 2, topPosition.Y + 2), Color.Black * 0.4f, 0f, _spriteFont.MeasureString("_________________________") / 2, 1f, SpriteEffects.None, 0.99f);

                spriteBatch.DrawString(_spriteFont, $"Rain Drops: {RainDropTotal}", new Vector2(topPosition.X - 180, topPosition.Y + 30), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                spriteBatch.DrawString(_spriteFont, $"Rain Drops: {RainDropTotal}", new Vector2(topPosition.X - 182, topPosition.Y + 32), Color.Black*0.4f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.99f);

                spriteBatch.DrawString(_spriteFont, $"Acid Drops: {AcidDropTotal}", new Vector2(topPosition.X - 180, topPosition.Y + 60), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                spriteBatch.DrawString(_spriteFont, $"Acid Drops: {AcidDropTotal}", new Vector2(topPosition.X - 182, topPosition.Y + 62), Color.Black * 0.4f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.99f);

                spriteBatch.DrawString(_spriteFont, $"Alkaline Drops: {AlkDropTotal}", new Vector2(topPosition.X - 180, topPosition.Y + 90), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                spriteBatch.DrawString(_spriteFont, $"Alkaline Drops: {AlkDropTotal}", new Vector2(topPosition.X - 182, topPosition.Y + 92), Color.Black * 0.4f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.99f);

                spriteBatch.DrawString(_spriteFont, $"Total Lives Lost: {MissedDropTotal}", new Vector2(topPosition.X - 180, topPosition.Y + 120), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                spriteBatch.DrawString(_spriteFont, $"Total Lives Lost: {MissedDropTotal}", new Vector2(topPosition.X - 182, topPosition.Y + 122), Color.Black * 0.4f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.99f);
            }
            
        }
    }
}
