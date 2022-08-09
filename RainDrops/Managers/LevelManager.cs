using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.Managers
{
    internal class LevelManager
    {
        public int Level { get; private set; }

        public int _catchCount;
        public float _dropSpawnRate;
        public double _dropSpeedMin;
        public double _dropSpeedMax;
        public double _rainDropChance;
        public double _acidDropChance;
        public double _alkDropChance;

        public float dropSpawnRateMod;
        public double dropSpeedMinMod;
        public double dropSpeedMaxMod;
        public double rainDropChanceMod;
        public double acidDropChanceMod;
        public double alkDropChanceMod;

        public LevelManager()
        {
            Level = 1;
            SetBaseDifficulty();
        }

        public void IncreaseLevel()
        {
            Level++;
            
        }
        public void SetBaseDifficulty()
        {

            if (Level >= 1 && Level <= 5) 
            {
                //_catchCount = 10;
                //_dropSpawnRate = 80f;
                //_dropSpeedMin = 0.003;
                //_dropSpeedMax = 0.008;
                //_rainDropChance = 3;
                //_acidDropChance = 48.5;
                //_alkDropChance = 48.5;

                //dropSpawnRateMod = -10f;
                //dropSpeedMinMod = 0.0002;
                //dropSpeedMaxMod = 0.0002;
                //rainDropChanceMod = -1;
                //acidDropChanceMod = (-1) * rainDropChanceMod/2;
                //alkDropChanceMod = (-1) * rainDropChanceMod/2;

                _catchCount = 10;
                _dropSpawnRate = 350f;
                _dropSpeedMin = 0.001;
                _dropSpeedMax = 0.003;
                _rainDropChance = 20;
                _acidDropChance = 40;
                _alkDropChance = 40;

                dropSpawnRateMod = -10f;
                dropSpeedMinMod = 0.0002;
                dropSpeedMaxMod = 0.0002;
                rainDropChanceMod = -1;
                acidDropChanceMod = (-1) * rainDropChanceMod / 2;
                alkDropChanceMod = (-1) * rainDropChanceMod / 2;
            }
            else if (Level >= 6 && Level <= 10) 
            {
                _catchCount = 10;
                _dropSpawnRate = 300f;
                _dropSpeedMin = 0.001;
                _dropSpeedMax = 0.004;
                _rainDropChance = 15;
                _acidDropChance = 42.5;
                _alkDropChance = 42.5;

                dropSpawnRateMod = -10f;
                dropSpeedMinMod = 0.0002;
                dropSpeedMaxMod = 0.0002;
                rainDropChanceMod = -1;
                acidDropChanceMod = (-1) * rainDropChanceMod / 2;
                alkDropChanceMod = (-1) * rainDropChanceMod / 2;
            }
            else if (Level >= 11 && Level <= 15) 
            {
                _catchCount = 15;
                _dropSpawnRate = 250f;
                _dropSpeedMin = 0.001;
                _dropSpeedMax = 0.005;
                _rainDropChance = 12;
                _acidDropChance = 44;
                _alkDropChance = 44;

                dropSpawnRateMod = -10f;
                dropSpeedMinMod = 0.0002;
                dropSpeedMaxMod = 0.0002;
                rainDropChanceMod = -1;
                acidDropChanceMod = (-1) * rainDropChanceMod / 2;
                alkDropChanceMod = (-1) * rainDropChanceMod / 2;
            }
            else if (Level >= 16 && Level <= 20) 
            {
                _catchCount = 15;
                _dropSpawnRate = 200f;
                _dropSpeedMin = 0.001;
                _dropSpeedMax = 0.006;
                _rainDropChance = 10;
                _acidDropChance = 45;
                _alkDropChance = 45;

                dropSpawnRateMod = -10f;
                dropSpeedMinMod = 0.0002;
                dropSpeedMaxMod = 0.0002;
                rainDropChanceMod = -0.5;
                acidDropChanceMod = (-1) * rainDropChanceMod / 2;
                alkDropChanceMod = (-1) * rainDropChanceMod / 2;
            }

        }

        public void ResetLevel()
        {
            Level = 1;
            SetBaseDifficulty();
        }
    }
}
