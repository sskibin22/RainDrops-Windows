using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RainDrops.Controls;
using RainDrops.Emitters;
using RainDrops.Events;
using RainDrops.Managers;
using RainDrops.Models;
using RainDrops.Sprites;
using RainDrops.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainDrops.States
{
    internal class GameState : State
    {
        #region Global Static Variables
        public static bool hasStarted = false; //start game boolean variable

        public static int phBarHeight = 45;
        public static float cupHeight;
        public static float splashHeightMult = 1.035f;

        public static int dropCount = 0; //Total drops on current GameState screen
        public static int lifeCount;

        public static bool emptyingCup = false;
        
        public static List<float> dropCols;
        public static int numDropCols;

        public static Color bgColor = Color.White;
        public static float bgAlpha = 1f;
        #endregion

        #region Private Variables
        private List<Sprite> sprites; //Contains all sprites on GameState screen
        private List<AnimatedButton> buttons;

        /* Drop attributes calculated during game */
        private float scaledDropFrameWidth;
        private float scaledDropFrameHeight;
        
        private int randCol;
        private float randDropSpeed;
        private float dropScale = 0.9f; //scaling multiplier for drops

        /* Game attributes */
        private int maxLives = 3;
        private int maxDrops = 35;  //Max drop instances allowed on screen at one time
        private int diffCount = 0;  //difficulty counter for level manager
        private float dropSpawnRate; //spawn rate for drops
        private double dropSpeedMin; //minimum drop speed on Y-axis
        private double dropSpeedMax; //maximum drop speed on Y-axis
        private float animationFrameSpeed = 83.33f; //(about 12fps)
        private bool gameOver = false;

        /* Must total 1 (ex: rainDropChance = 0.2, acidDropChance = 0.4, alkDropChance = 0.4) */
        private double rainDropChance; 
        private double acidDropChance;
        private double alkDropChance;

        /* Timers */
        private float timer = 0;
        private float endTimer = 0;
        private float lightningEventTimer = 0;
        #endregion

        #region Sprite Fonts
        private SpriteFont pixelDigiFont; //100% free
        #endregion

        #region Textures
        private Texture2D bgTexture;
        private Texture2D phBarTexture;
        private Texture2D cloudBarTexture;
        private Texture2D phSelectTexture;
        private Texture2D standardCupTexture;
        private Texture2D rainAnimTexture;
        private Texture2D acidAnimTexture;
        private Texture2D alkAnimTexture;
        private Texture2D acidGlowTexture;
        private Texture2D alkGlowTexture;
        private Texture2D playAgainButtonTexture;
        private Texture2D quitButtonTexture;
        #endregion

        #region Animation Dictionaries
        private Dictionary<string, Animation> dropDict;
        private Dictionary<string, Animation> startDict;
        #endregion

        #region Animations
        private Animation rainDropAnimation;
        private Animation acidDropAnimation;
        private Animation alkDropAnimation;
        private Animation acidGlowAnimation;
        private Animation alkGlowAnimation;
        private Animation lifeDropAnimation;
        private Animation acidIndAnimation;
        private Animation alkIndAnimation;
        #endregion

        #region Global Static Sprites
        public static Cup cup;
        public static List<LifeIndicator>? lives;
        #endregion

        #region Private Sprites
        private PHselector? phSelect;
        private Drop currDrop;
        private CloudBar cloud1;
        private CloudBar cloud2;
        private CloudBar cloud3;
        private CloudBar cloud4;
        private CountDown countDown;
        private List<DebuffIndicator> debuffIndicators;
        private GameOverPrompt gameOverPrompt; 
        private GameOverText gameOverText;
        private LightningBall lightningBall;
        private LightningBolt lightningBolt;
        #endregion

        #region Public Static Managers
        public static StatManager statManager;
        #endregion

        #region Private Managers
        private DeBuffManager deBuffManager;
        #endregion

        #region Private Events
        private LightningEvent lightningEvent;
        #endregion

        #region Private Emitters
        private RainEmitter rainEmitter;
        #endregion

        #region UI States
        private KeyboardState currKeyState;
        private KeyboardState previousKeyState;
        #endregion

        #region GameState Contructor
        public GameState(RainDropsGame game, ContentManager content) : base(game, content)
        {
        }
        #endregion

        #region Game Attribute Methods
        /* Updates Game difficulty settings through given level manager variables */
        public void UpdateDifficulty()
        {
            dropSpawnRate = RainDropsGame.levelManager._dropSpawnRate;
            dropSpeedMin = RainDropsGame.levelManager._dropSpeedMin;
            dropSpeedMax = RainDropsGame.levelManager._dropSpeedMax;
            rainDropChance = RainDropsGame.levelManager._rainDropChance;
            acidDropChance = RainDropsGame.levelManager._acidDropChance;
            alkDropChance = RainDropsGame.levelManager._alkDropChance;
        }
        /* Increases difficulty settings by given level manager difficulty modifier variables */
        public void ModifyLevelDifficulty()
        {
            dropSpawnRate += RainDropsGame.levelManager.dropSpawnRateMod;
            dropSpeedMin += RainDropsGame.levelManager.dropSpeedMinMod;
            dropSpeedMax += RainDropsGame.levelManager.dropSpeedMaxMod;
            rainDropChance += RainDropsGame.levelManager.rainDropChanceMod;
            acidDropChance += RainDropsGame.levelManager.acidDropChanceMod;
            alkDropChance += RainDropsGame.levelManager.alkDropChanceMod;
        }
        /* Tells which PH number should be selected by PHSelector given a drop's PH level */
        private void MovePHSelector(Drop drop)
        {
            switch (drop.PH)
            {
                case 14:
                    phSelect.phSelected += 3;
                    if (phSelect.phSelected > 14)
                        phSelect.phSelected = 14;
                    break;
                case 11:
                    if (phSelect.phSelected != 14)
                        phSelect.phSelected += 1;
                    break;
                case 3:
                    if (phSelect.phSelected != 0)
                        phSelect.phSelected -= 1;
                    break;
                case 0:
                    phSelect.phSelected -= 3;
                    if (phSelect.phSelected < 0)
                        phSelect.phSelected = 0;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Game Methods
        public override void LoadContent()
        {
            /* Load GameState SpriteFonts */
            pixelDigiFont = _content.Load<SpriteFont>("Fonts/PixelDigi");

            /* Load GameState Textures */
            bgTexture = _content.Load<Texture2D>("BackGrounds/MountainsBG2");
            phBarTexture = _content.Load<Texture2D>("UI/Panels/phBar");
            cloudBarTexture = _content.Load<Texture2D>("Floaters/cloudBarLong");
            phSelectTexture = _content.Load<Texture2D>("UI/phSelectCircle");
            rainAnimTexture = _content.Load<Texture2D>("Drops/dropBlueSS");
            acidAnimTexture = _content.Load<Texture2D>("Drops/dropGreenSS");
            alkAnimTexture = _content.Load<Texture2D>("Drops/dropPurpleSS");
            acidGlowTexture = _content.Load<Texture2D>("Drops/glowGreenSS");
            alkGlowTexture = _content.Load<Texture2D>("Drops/glowPurpleSS");
            standardCupTexture = _content.Load<Texture2D>("Cups/StandardCup/StandardCupStates/standardCup7");
            playAgainButtonTexture = _content.Load<Texture2D>("UI/Buttons/playAgainButton");
            quitButtonTexture = _content.Load<Texture2D>("UI/Buttons/quitButton");

            /* Instantiate Animations */
            rainDropAnimation = new Animation(rainAnimTexture, 2) { FrameSpeed = animationFrameSpeed }; 
            acidDropAnimation = new Animation(acidAnimTexture, 2) { FrameSpeed = animationFrameSpeed }; 
            alkDropAnimation = new Animation(alkAnimTexture, 2) { FrameSpeed = animationFrameSpeed }; 
            acidGlowAnimation = new Animation(acidGlowTexture, 10) { FrameSpeed = animationFrameSpeed }; 
            alkGlowAnimation = new Animation(alkGlowTexture, 10) { FrameSpeed = animationFrameSpeed }; 
            lifeDropAnimation = new Animation(rainAnimTexture, 2) { FrameSpeed = animationFrameSpeed };
            acidIndAnimation = new Animation(_content.Load<Texture2D>("Floaters/IndicatorSprites/acidDebuff"), 12) { FrameSpeed = animationFrameSpeed };
            alkIndAnimation = new Animation(_content.Load<Texture2D>("Floaters/IndicatorSprites/alkDebuff"), 24) { FrameSpeed = animationFrameSpeed };

            /* Instantiate Animation Dictionaries */
            dropDict = new Dictionary<string, Animation>()
            {
                {"rainDrop", rainDropAnimation },
                {"acidDrop", acidDropAnimation },
                {"alkDrop", alkDropAnimation },
                {"acidGlow", acidGlowAnimation },
                {"alkGlow", alkGlowAnimation }
            };

            startDict = new Dictionary<string, Animation>()
            {
                {"three", new Animation(_content.Load<Texture2D>("TextSprites/THREEtext"), 3){FrameSpeed = animationFrameSpeed } },
                {"two", new Animation(_content.Load<Texture2D>("TextSprites/TWOtext"), 3){FrameSpeed = animationFrameSpeed } },
                {"one", new Animation(_content.Load<Texture2D>("TextSprites/ONEtext"), 3){FrameSpeed = animationFrameSpeed } },
                {"rain", new Animation(_content.Load<Texture2D>("TextSprites/RAINtext"), 3){FrameSpeed = animationFrameSpeed } },
            };

            /* Calculate Drop column positions and add to dropCols List */
            dropCols = new List<float>();

            scaledDropFrameWidth = rainDropAnimation.FrameWidth * dropScale;
            scaledDropFrameHeight = rainDropAnimation.FrameHeight * dropScale;
            numDropCols = (int)(RainDropsGame.ScreenWidth / (scaledDropFrameWidth));
            float preTotal = (scaledDropFrameWidth * (numDropCols - 1)) + (scaledDropFrameWidth/2);
            float difference = (RainDropsGame.ScreenWidth - preTotal) - (scaledDropFrameWidth/2);
            float centerBy = difference / 2f;
            float total = 0;
            for (int i = 0; i < numDropCols; i++)
            {
                if (i == 0)
                {
                    total += ((scaledDropFrameWidth) / 2) + centerBy;

                }
                else
                {
                    total += scaledDropFrameWidth;
                }
                dropCols.Add(total);

            }

            /* Instantiate Necessary Sprites */
            cloud1 = new CloudBar(cloudBarTexture) { Origin = Vector2.Zero, Layer = 0.9f, Speed = 0.8f, Position = new Vector2(0, 0) };
            cloud2 = new CloudBar(cloudBarTexture) { Origin = Vector2.Zero, Layer = 0.9f, Speed = 0.8f, Position = new Vector2(cloudBarTexture.Width, 0) };
            cloud3 = new CloudBar(cloudBarTexture) { Origin = Vector2.Zero, Layer = 0.8f, Speed = 1f, Position = new Vector2(50, 20) };
            cloud4 = new CloudBar(cloudBarTexture) { Origin = Vector2.Zero, Layer = 0.8f, Speed = 1f, Position = new Vector2(-cloudBarTexture.Width + 50, 20) };
            phSelect = new PHselector(phSelectTexture) { Scale = 0.7f, Layer = 0.91f };
            cup = new Cup(
                new Dictionary<string, Texture2D>()
            {
                { "defaultCup", standardCupTexture},
                { "acidState1", _content.Load<Texture2D>("Cups/StandardCup/StandardCupStates/standardCup6") },
                { "acidState2", _content.Load<Texture2D>("Cups/StandardCup/StandardCupStates/standardCup4") },
                { "acidState3", _content.Load<Texture2D>("Cups/StandardCup/StandardCupStates/standardCup2") },
                { "acidState4", _content.Load<Texture2D>("Cups/StandardCup/StandardCupStates/standardCup0") },
                { "alkState1", _content.Load<Texture2D>("Cups/StandardCup/StandardCupStates/standardCup8") },
                { "alkState2", _content.Load<Texture2D>("Cups/StandardCup/StandardCupStates/standardCup10") },
                { "alkState3", _content.Load<Texture2D>("Cups/StandardCup/StandardCupStates/standardCup12") },
                { "alkState4", _content.Load<Texture2D>("Cups/StandardCup/StandardCupStates/standardCup14") }
            }
                , new Dictionary<string, Animation>()
            {
                { "defaultWater", new Animation(_content.Load<Texture2D>("Cups/StandardCup/WaterStates"), 31) {FrameSpeed = 15 } },
                { "splash_0", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_0"), 19) {FrameSpeed = 10 } },
                { "splash_1", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_1"), 19) {FrameSpeed = 10 } },
                { "splash_2", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_2"), 19) {FrameSpeed = 10 } },
                { "splash_3", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_3"), 19) {FrameSpeed = 10 } },
                { "splash_4", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_4"), 19) {FrameSpeed = 10 } },
                { "splash_5", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_5"), 19) {FrameSpeed = 10 } },
                { "splash_6", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_6"), 19) {FrameSpeed = 10 } },
                { "splash_7", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_7"), 19) {FrameSpeed = 10 } },
                { "splash_8", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_8"), 19) {FrameSpeed = 10 } },
                { "splash_9", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_9"), 19) {FrameSpeed = 10 } },
                { "splash_10", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_10"), 19) {FrameSpeed = 10 } },
                { "splash_11", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_11"), 19) {FrameSpeed = 10 } },
                { "splash_12", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_12"), 19) {FrameSpeed = 10 } },
                { "splash_13", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_13"), 19) {FrameSpeed = 10 } },
                { "splash_14", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_14"), 19) {FrameSpeed = 10 } },
                { "splash_15", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_15"), 19) {FrameSpeed = 10 } },
                { "splash_16", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_16"), 19) {FrameSpeed = 10 } },
                { "splash_17", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_17"), 19) {FrameSpeed = 10 } },
                { "splash_18", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_18"), 19) {FrameSpeed = 10 } },
                { "splash_19", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_19"), 19) {FrameSpeed = 10 } },
                { "splash_20", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_20"), 19) {FrameSpeed = 10 } },
                { "splash_21", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_21"), 19) {FrameSpeed = 10 } },
                { "splash_22", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_22"), 19) {FrameSpeed = 10 } },
                { "splash_23", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_23"), 19) {FrameSpeed = 10 } },
                { "splash_24", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_24"), 19) {FrameSpeed = 10 } },
                { "splash_25", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_25"), 19) {FrameSpeed = 10 } },
                { "splash_26", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_26"), 19) {FrameSpeed = 10 } },
                { "splash_27", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_27"), 19) {FrameSpeed = 10 } },
                { "splash_28", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_28"), 19) {FrameSpeed = 10 } },
                { "splash_29", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_29"), 19) {FrameSpeed = 10 } },
                { "splash_30", new Animation(_content.Load<Texture2D> ("Cups/StandardCup/SplashAnimations/SplashAnimation_30"), 19) {FrameSpeed = 10 } }
            });
            /*Calculate cup height*/
            cupHeight = cup.GetHeight();

            lives = new List<LifeIndicator>()
            {
                new LifeIndicator(new Dictionary<string, Animation>(){{"lifeDrop", lifeDropAnimation } }){Position = new Vector2((lifeDropAnimation.FrameWidth*0.5f), (lifeDropAnimation.FrameHeight*0.5f/2)+1), Scale = 0.5f, Layer = 1f},
                new LifeIndicator(new Dictionary<string, Animation>(){{"lifeDrop", lifeDropAnimation } }){Position = new Vector2(((lifeDropAnimation.FrameWidth*0.5f)*2)+10, (lifeDropAnimation.FrameHeight*0.5f/2)+1), Scale = 0.5f, Layer = 1f},
                new LifeIndicator(new Dictionary<string, Animation>(){{"lifeDrop", lifeDropAnimation } }){Position = new Vector2(((lifeDropAnimation.FrameWidth*0.5f)*3)+20, (lifeDropAnimation.FrameHeight*0.5f/2)+1), Scale = 0.5f, Layer = 1f}
            };

            gameOverPrompt = new GameOverPrompt(new Dictionary<string, Animation>() { { "shine", new Animation(_content.Load<Texture2D>("UI/Panels/gameOverBG"), 24) { FrameSpeed = 100f } } });

            gameOverText = new GameOverText(new Dictionary<string, Animation>() { { "default", new Animation(_content.Load<Texture2D>("TextSprites/GAMEOVERtext"), 3) { FrameSpeed = animationFrameSpeed } } });

            float deBuffScale = 0.5f;
            debuffIndicators = new List<DebuffIndicator>()
            {
                new DebuffIndicator(new Dictionary<string, Animation>(){{"default", acidIndAnimation } }){Position = new Vector2(acidIndAnimation.FrameWidth*deBuffScale, RainDropsGame.ScreenHeight-phBarHeight-(acidIndAnimation.FrameHeight*deBuffScale)/2), Scale = deBuffScale, Layer = 1f},
                new DebuffIndicator(new Dictionary<string, Animation>(){{"default", alkIndAnimation } }){Position = new Vector2(alkIndAnimation.FrameWidth*deBuffScale, RainDropsGame.ScreenHeight-phBarHeight-(alkIndAnimation.FrameHeight*deBuffScale)/2), Scale = deBuffScale, Layer = 1f}
            };
            countDown = new CountDown(startDict);

            lightningBall = new LightningBall(new Dictionary<string, Animation>() { { "default", new Animation(_content.Load<Texture2D>("Events/LightningEvent/lightningBall"), 4) { FrameSpeed = animationFrameSpeed } } });
            lightningBolt = new LightningBolt(new Dictionary<string, Animation>() { { "default", new Animation(_content.Load<Texture2D>("Events/LightningEvent/lightningBolt"), 6) { FrameSpeed = 166.67f } } });

            /*Instantiate Managers */
            deBuffManager = new DeBuffManager(cup, debuffIndicators);
            statManager = new StatManager(pixelDigiFont);

            /*Instantiate Events */
            lightningEvent = new LightningEvent(lightningBolt, lightningBall);

            /* Instantiate Emitters */
            rainEmitter = new RainEmitter(new Rain(_content.Load<Texture2D>("EmitterSprites/rain_1")));

            /* Load Initial Game settings */
            UpdateDifficulty(); //set initial difficulty settings
            Restart();
        }
        private void LevelUp()
        {
            RainDropsGame.levelManager.IncreaseLevel();
            diffCount++;
            /* Apply difficulty settings as needed */
            if (diffCount == 4)
            {
                RainDropsGame.levelManager.SetBaseDifficulty();
                UpdateDifficulty();
                diffCount = 0;
            }
            if (diffCount > 0)
            {
                ModifyLevelDifficulty();
            }

            if (lifeCount < maxLives && lifeCount >= 0)
            {
                lives[lifeCount].IsRemoved = false;
                sprites.Add(lives[lifeCount]);
                lifeCount++;
            }

        }
        /* Restart Game back to it's original state */
        private void Restart()
        {
            RainDropsGame.levelManager.ResetLevel();
            /* Apply difficulty settings as needed */
            if (diffCount == 4)
            {
                RainDropsGame.levelManager.SetBaseDifficulty();
                UpdateDifficulty();
                diffCount = 0;
            }
            if(diffCount > 0)
            {
                ModifyLevelDifficulty();
            }
            gameOver = false;
            gameOverText.isActive = false;
            gameOverPrompt.Scale = 0f;
            /* Initialize Game attributes */
            statManager.Reset();
            endTimer = 0f;
            splashHeightMult = 1.035f;
            lifeCount = 3;
            dropCount = 0;
            phSelect.Reset();
            cup.Reset();
            countDown.Reset();
            deBuffManager.Reset();
            lightningEvent.Stop();
            lightningEventTimer = 0f;
            bgAlpha = 1f;
            foreach (LifeIndicator life in lives)
            {
                life.IsRemoved = false;
            }
            buttons = new List<AnimatedButton>()
            {
                new AnimatedButton(new Animation(playAgainButtonTexture, 2){FrameSpeed = animationFrameSpeed})
                {
                    Position = new Vector2((RainDropsGame.ScreenWidth/2)-100, (RainDropsGame.ScreenHeight/2)+170),
                    Click = new EventHandler(PlayAgainButton_Click),
                    Layer = 1f,
                    Opacity = 1f,
                    Scale = 1f,
                    IsActive = false
                },

                new AnimatedButton(new Animation(quitButtonTexture, 2){FrameSpeed = animationFrameSpeed})
                {
                    Position = new Vector2((RainDropsGame.ScreenWidth/2)+100, (RainDropsGame.ScreenHeight/2)+170),
                    Click = new EventHandler(QuitButton_Click),
                    Layer = 1f,
                    Opacity = 1f,
                    Scale = 1f,
                    IsActive = false
                },
            };
            /* Initialize sprites list */
            sprites = new List<Sprite>()
            {
                lives[0],
                lives[1],
                lives[2],
                cup,
                phSelect, 
                cloud1,
                cloud2, 
                cloud3, 
                cloud4,
                debuffIndicators[0],
                debuffIndicators[1],
                gameOverText
            };

            /* set Game started boolean to false */
            hasStarted = false;
        }

        private void PlayAgainButton_Click(object sender, EventArgs args)
        {
            Restart();
        }

        private void QuitButton_Click(object sender, EventArgs args)
        {
            _game.ChangeState(new MainMenuState(_game, _content));
        }
        private void SpawnDrops(GameTime gameTime)
        {
            //increment game timer based on total seconds elapsed
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            //if timer is greater than drop rate, reset timer, spawn a new drop
            if (timer > dropSpawnRate)
            {
                timer = 0f;
                randCol = RainDropsGame.Random.Next(0, numDropCols);
                randDropSpeed = (float)(RainDropsGame.Random.NextDouble() * (dropSpeedMax - dropSpeedMin) + dropSpeedMin);
                /* If random column position has another drop within range of starting spawn position, randomly choose another drop column. 
                 * This is so that drops have less of a chance of spawing on top of eachother.
                 */
                foreach (var sprite in sprites)
                {
                    if (sprite is Drop)
                    {
                        var drop = (Drop)sprite;
                        if (drop.Position.X == dropCols[randCol])
                        {
                            if (drop.Rect.Top < scaledDropFrameHeight)
                            {
                                randCol = RainDropsGame.Random.Next(0, numDropCols);
                            }
                        }
                    }
                }
                /* Randomly spawn euither a rain drop, acid drop, or alkaline drop based on drop chances for each. 
                 * Apply attributes randomly selected above.
                 */
                WeightedRandomExecutor wre = new WeightedRandomExecutor(
                new WeightedRandomParam(() => currDrop = new RainDrop(dropDict)
                {
                    Position = new Vector2(dropCols[randCol], scaledDropFrameHeight / 2),
                    DropSpeed = randDropSpeed,
                    Scale = dropScale
                }, rainDropChance),
                new WeightedRandomParam(() => currDrop = new AcidRainDrop(dropDict)
                {

                    Position = new Vector2(dropCols[randCol], scaledDropFrameHeight / 2),
                    DropSpeed = randDropSpeed,
                    Scale = dropScale
                }, acidDropChance),
                new WeightedRandomParam(() => currDrop = new AlkRainDrop(dropDict)
                {
                    Position = new Vector2(dropCols[randCol], scaledDropFrameHeight / 2),
                    DropSpeed = randDropSpeed,
                    Scale = dropScale
                }, alkDropChance));
                wre.Execute();
                if (dropCount < maxDrops)
                {
                    sprites.Add(currDrop);
                    dropCount++;
                }
            }
        }

        private void CheckCollisions()
        {
            foreach (var sprite in sprites)
            {
                if (sprite is Drop)
                {
                    var drop = (Drop)sprite;
                    if (drop.IsCupCollision(cup) && sprite is RainDrop)
                    {
                        if(deBuffManager.alkDebuff && deBuffManager.AlkDebuffActive())
                        {
                            //TODO: animation for when drop misses
                        }
                        else
                        {
                            cup.splashing = true;
                            cup.IncreaseFrame();
                            cup.IncreaseDropCount();
                            statManager.IncreaseRainDropTotal();
                        }
                        dropCount--;
                        sprite.IsRemoved = true;
                    }
                    if (drop.IsCupCollision(cup) && sprite is AcidRainDrop)
                    {
                        var acidDrop = sprite as AcidRainDrop;
                        statManager.IncreaseAcidDropTotal();
                        dropCount--;
                        sprite.IsRemoved = true;
                        MovePHSelector(acidDrop);
                        deBuffManager.ChangeState(phSelect);
                    }
                    if (drop.IsCupCollision(cup) && sprite is AlkRainDrop)
                    {
                        var alkDrop = sprite as AlkRainDrop;
                        statManager.IncreaseAlkDropTotal();
                        dropCount--;
                        sprite.IsRemoved = true;
                        MovePHSelector(alkDrop);
                        deBuffManager.ChangeState(phSelect);
                    }
                }
            }
        }

        private void UpdateCloudAnimation()
        {
            cloud1.MoveLeft();
            cloud2.MoveLeft();
            cloud3.MoveRight();
            cloud4.MoveRight();
            if (cloud1.Position.X <= -cloud1.Rect.Width)
            {
                cloud1.Reset(cloud2, 0);
            }
            if (cloud2.Position.X <= -cloud2.Rect.Width)
            {
                cloud2.Reset(cloud1, 0);
            }
            if (cloud3.Position.X >= cloud3.Rect.Width)
            {
                cloud3.Reset(cloud4, 1);
            }
            if (cloud4.Position.X >= cloud4.Rect.Width)
            {
                cloud4.Reset(cloud3, 1);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
            /* Check if any sprite.IsRemoved property has been set to true. If so remove from sprites List */
            for (int i = 0; i < sprites.Count; i++)
            {
                var sprite = sprites[i];

                if (sprite.IsRemoved)
                {
                    sprites.RemoveAt(i);
                    i--;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            //Cloud Movement Animations
            UpdateCloudAnimation();

            //Update rain emitter
            rainEmitter.Update(gameTime);

            previousKeyState = currKeyState;
            currKeyState = keyState;
            //if ESC key is pressed exit game
            if (keyState.IsKeyDown(Keys.Escape))
                _game.Exit();

            if (gameOver)
            {
                gameOverPrompt.Update(gameTime);
                if(gameOverPrompt.Scale >= 1f)
                {
                    gameOverText.isActive = true;
                    gameOverText.Update(gameTime);
                    statManager.IsActive = true;
                    foreach (var button in buttons)
                    {
                        button.IsActive = true;
                        button.Update(gameTime);
                    }
                }
                return;
            }
            //if Space key is pressed, start game
            if (previousKeyState.IsKeyDown(Keys.Space) && currKeyState.IsKeyUp(Keys.Space))
            {
                sprites.Add(countDown);
            }

            if (cup.IsFull())
            {
                emptyingCup = true;
                if (!cup.splashing)
                {
                    if (cup.EmptyCup(gameTime))
                    {
                        LevelUp();
                    }
                }
            }
            else
            {
                CheckCollisions();
            }
            if (deBuffManager.acidDebuff && !emptyingCup)
            {
                deBuffManager.AcidDeBuffActive(gameTime);
            }
            //Update cup sprite
            cup.Update(gameTime);

            //if cup is being emptied return and do not update other sprites
            if (emptyingCup)
            {
                return;
            }

            //update all other sprites
            foreach (var sprite in sprites)
            {
                if(sprite is not Cup)
                {
                    sprite.Update(gameTime);
                }
            }

            //if game has not started(space has not been pressed yet) return
            if (!hasStarted)
                return;

            lightningEventTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(lightningEventTimer >= lightningEvent.EventTimer)
            {
                lightningEvent.Start();

                lightningEvent.Update(gameTime, cup);
                if(lightningEvent.EventActive == false)
                {
                    lightningEventTimer = 0;
                    lightningEvent.Stop();
                }
            }

            SpawnDrops(gameTime);
            
            /* If player loses all lives reset difficulty and game. */
            if (lifeCount < 0)
            {
                gameOver = true;
                sprites.Add(gameOverPrompt);
                //RainDropsGame.levelManager.ResetLevel();
                //Restart();
            }

            PostUpdate(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            //Draw background
            spriteBatch.Draw(bgTexture, new Rectangle(0, 0, RainDropsGame.ScreenWidth, RainDropsGame.ScreenHeight), null, bgColor*bgAlpha, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            //Draw ph bar
            spriteBatch.Draw(phBarTexture, new Rectangle(0, RainDropsGame.ScreenHeight - phBarHeight, RainDropsGame.ScreenWidth, phBarHeight), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.9f);
            //Draw rain emitter
            rainEmitter.Draw(gameTime, spriteBatch);
            //Draw all sprites and show their hitboxes if showHitBox is set to true
            foreach (var sprite in sprites)
            {
                sprite.Draw(gameTime, spriteBatch);
            }
            //Draw Components
            foreach (var button in buttons)
            {
                button.Draw(gameTime, spriteBatch);
            }
            //Draw Events
            lightningEvent.Draw(gameTime, spriteBatch);
            //Draw Stats
            statManager.Draw(gameTime, spriteBatch);
            //Draw Level
            spriteBatch.DrawString(pixelDigiFont, $"Level {RainDropsGame.levelManager.Level}", new Vector2((int)(RainDropsGame.ScreenWidth * 0.8)-2, (int)(RainDropsGame.ScreenHeight * 0.012)+2), Color.Black*0.4f, 0f, pixelDigiFont.MeasureString($"{cup.dropCount}") / 2, 1f, SpriteEffects.None, 0.99f);
            spriteBatch.DrawString(pixelDigiFont, $"Level {RainDropsGame.levelManager.Level}", new Vector2((int)(RainDropsGame.ScreenWidth*0.8), (int)(RainDropsGame.ScreenHeight*0.012)), Color.Black, 0f, pixelDigiFont.MeasureString($"{cup.dropCount}") / 2, 1f, SpriteEffects.None, 1f);
            //Draw score
            spriteBatch.DrawString(pixelDigiFont, $"{cup.dropCount}", new Vector2(cup.Position.X-2, cup.Position.Y+2), Color.Black*0.3f, 0f, pixelDigiFont.MeasureString($"{cup.dropCount}")/2, 1f, SpriteEffects.None, 0.99f);
            spriteBatch.DrawString(pixelDigiFont, $"{cup.dropCount}", new Vector2(cup.Position.X, cup.Position.Y), Color.Black, 0f, pixelDigiFont.MeasureString($"{cup.dropCount}") / 2, 1f, SpriteEffects.None, 1f);
            spriteBatch.End();
        }
    }
    #endregion
}
