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
    internal class DeBuffManager
    {
        private Random Random = new Random();
        public bool acidDebuff;
        public bool alkDebuff;

        private float acidTimer;
        private float spillRate;
        
        private float missChance;
        private Cup _cup;
        private List<DebuffIndicator> _indicators;

        public DeBuffManager(Cup cup, List<DebuffIndicator> indicators)
        {
            _cup = cup;
            _indicators = indicators;
            acidTimer = 0;
            spillRate = 2000;
            missChance = 50;
            acidDebuff = false;
            alkDebuff = false;
        }
        public void ChangeState(PHselector phSelect)
        {
            if (phSelect.phSelected > 7 && phSelect.phSelected < 10)
            {
                StopAcidDebuff();
                alkDebuff = true;
                missChance = 50;
                _indicators[0].isActive = false;
                _indicators[1].isActive = true;
                _cup.TextureTo("alkState1");
            }
            else if (phSelect.phSelected > 9 && phSelect.phSelected < 12)
            {
                StopAcidDebuff();
                alkDebuff = true;
                missChance = 65;
                _indicators[0].isActive = false;
                _indicators[1].isActive = true;
                _cup.TextureTo("alkState2");
            }
            else if (phSelect.phSelected > 11 && phSelect.phSelected < 14)
            {
                StopAcidDebuff();
                alkDebuff = true;
                missChance = 80;
                _indicators[0].isActive = false;
                _indicators[1].isActive = true;
                _cup.TextureTo("alkState3");
            }
            else if (phSelect.phSelected == 14)
            {
                StopAcidDebuff();
                alkDebuff = true;
                missChance = 95;
                _indicators[0].isActive = false;
                _indicators[1].isActive = true;
                _cup.TextureTo("alkState4");
            }
            else if (phSelect.phSelected < 7 && phSelect.phSelected > 4)
            {
                StopAlkDeBuff();
                acidDebuff = true;
                spillRate = 2000;
                _indicators[1].isActive = false;
                _indicators[0].isActive = true;
                _cup.TextureTo("acidState1");
            }
            else if (phSelect.phSelected < 5 && phSelect.phSelected > 2)
            {
                StopAlkDeBuff();
                acidDebuff = true;
                spillRate = 1500;
                _indicators[1].isActive = false;
                _indicators[0].isActive = true;
                _cup.TextureTo("acidState2");
            }
            else if (phSelect.phSelected < 3 && phSelect.phSelected > 0)
            {
                StopAlkDeBuff();
                acidDebuff = true;
                spillRate = 1000;
                _indicators[1].isActive = false;
                _indicators[0].isActive = true;
                _cup.TextureTo("acidState3");
            }
            else if (phSelect.phSelected == 0)
            {
                StopAlkDeBuff();
                acidDebuff = true;
                spillRate = 500;
                _indicators[1].isActive = false;
                _indicators[0].isActive = true;
                _cup.TextureTo("acidState4");
            }
            else
            {
                acidTimer = 0;
                acidDebuff = false;
                alkDebuff = false;
                missChance = 50;
                spillRate = 2000;
                _indicators[0].isActive = false;
                _indicators[1].isActive = false;
                _cup.TextureTo("defaultCup");
            }
        }
        public void ActivateLightningShieldBuff()
        {
            _indicators[2].isActive = true;
        }
        public void DeactivateLightningShieldBuff()
        {
            _indicators[2].isActive = false;
        }
        public void AcidDeBuffActive(GameTime gameTime)
        {
            acidTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (acidTimer > spillRate)
            {
                acidTimer = 0;
                _cup.DecreaseDropCount();
                _cup.DecreaseFrame();
            }
        }
        public bool AlkDebuffActive()
        {
            if(Random.Next(101) <= missChance)
            {
                return true;
            }
            return false;
        }
        private void StopAcidDebuff()
        {
            acidDebuff = false;
            acidTimer = 0;
        }

        private void StopAlkDeBuff()
        {
            alkDebuff = false;
            missChance = 50;
        }

        public void Reset()
        {
            acidTimer = 0;
            acidDebuff = false;
            alkDebuff = false;
            missChance = 50;
            spillRate = 2000;
            _indicators[0].isActive = false;
            _indicators[1].isActive = false;
            _indicators[2].isActive = false;
        }




    }
}
