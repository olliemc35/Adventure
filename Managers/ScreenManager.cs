using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Aseprite;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;


namespace Adventure
{
    public class ScreenManager
    {
        SpriteBatch spriteBatch;
        SpriteFont menuFont;

        public List<List<AnimatedGameObject>> sprites = new List<List<AnimatedGameObject>>();
        public List<List<GameObject>> gameObjects = new List<List<GameObject>>();

        public ActionScreen screen1;
        public Tileset tileSetScreen1;

        public ActionScreen screen2;
        public Tileset tileSetScreen2;

        public ActionScreen screen3;
        public Tileset tileSetScreen3;

        public ActionScreen screen4;
        public Tileset tileSetScreen4;

        public ActionScreen screen5;
        public Tileset tileSetScreen5;

        public ActionScreen screen6;
        public Tileset tileSetScreen6;

        public ActionScreen screen7;
        public Tileset tileSetScreen7;

        public ActionScreen screen8;
        public Tileset tileSetScreen8;

        public ActionScreen screen9;
        public Tileset tileSetScreen9;

        public ActionScreen screen10;
        public Tileset tileSetScreen10;

        public ActionScreen screen11;
        public Tileset tileSetScreen11;

        public ActionScreen screen12;
        public Tileset tileSetScreen12;

        public ActionScreen screen13;
        public Tileset tileSetScreen13;

        public ActionScreen screen14;
        public Tileset tileSetScreen14;

        public ActionScreen screen15;
        public Tileset tileSetScreen15;

        public ActionScreen screen16;
        public Tileset tileSetScreen16;

        public ActionScreen screen17;
        public Tileset tileSetScreen17;

        public ActionScreen screen18;
        public Tileset tileSetScreen18;

        public ActionScreen screen19;
        public Tileset tileSetScreen19;

        public ActionScreen screen20;
        public Tileset tileSetScreen20;

        public ActionScreen screen21;
        public Tileset tileSetScreen21;

        public ActionScreen screen22;
        public Tileset tileSetScreen22;

        public ActionScreen screen23;
        public Tileset tileSetScreen23;

        public ActionScreen screen24;
        public Tileset tileSetScreen24;

        public ActionScreen screen25;
        public Tileset tileSetScreen25;

        public ActionScreen screen26;
        public Tileset tileSetScreen26;

        public ActionScreen screen27;
        public Tileset tileSetScreen27;

        public ActionScreen screen28;
        public Tileset tileSetScreen28;

        public ActionScreen screen29;
        public Tileset tileSetScreen29;

        public ActionScreen screen30;
        public Tileset tileSetScreen30;

        public ActionScreen screen31;
        public Tileset tileSetScreen31;

        public ActionScreen screen32;
        public Tileset tileSetScreen32;

        public ActionScreen screen33;
        public Tileset tileSetScreen33;

        public ActionScreen screen34;
        public Tileset tileSetScreen34;

        public ActionScreen screen35;
        public Tileset tileSetScreen35;

        public ActionScreen screen36;
        public Tileset tileSetScreen36;

        public ActionScreen screen37;
        public Tileset tileSetScreen37;

        public List<GameScreen> screens;

        public GameScreen activeScreen;
        public GameScreen previousActiveScreen;

        public ColliderManager colliderManager;
        public InputManager inputManager;
        public AssetManager assetManager;
        public SoundManager soundManager;

        public KeyboardState keyboardState = new KeyboardState();
        public KeyboardState oldKeyboardState = new KeyboardState();

        public Player player;


        public int ScreenNumber = 0;
        public int PreviousScreenNumber = 0;
        public int DoorNumberToMoveTo = 0;


     

        public ScreenManager(SpriteBatch spriteBatch, AssetManager assetManager, ColliderManager colliderManager, InputManager inputManager, SoundManager soundManager)
        {
            this.spriteBatch = spriteBatch;
            this.colliderManager = colliderManager;
            this.inputManager = inputManager;
            this.assetManager = assetManager;

            player = new Player(new Vector2(8, 0), "hoodedoldmanv2", assetManager, colliderManager, inputManager, this);

            this.inputManager = inputManager;
            this.soundManager = soundManager;
        }


        public void LoadScreens(ContentManager content, GraphicsDevice graphicsDevice)
        {
            menuFont = content.Load<SpriteFont>("menufont");


            CreateScreens(content, graphicsDevice);

            foreach (GameScreen screen in screens)
            {
                if (screen != null)
                {
                    //Debug.WriteLine("here");
                    screen.LoadContent();
                    screen.Hide();
                }
            }

            activeScreen = screens[5];
            activeScreen.Show();

            player.LoadContent();
            player.bomb.LoadContent();

            foreach (Ribbon ribbon in player.ribbons)
            {
                ribbon.LoadContent();
            }

            player.position = activeScreen.respawnPoint;




        }

        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            activeScreen.Update(gameTime);

            if (activeScreen.ChangeScreenFlag)
            {
                activeScreen.ChangeScreenFlag = false;
                activeScreen = screens[ScreenNumber - 1];
                DisableShowScreenTransition(screens[PreviousScreenNumber - 1], screens[ScreenNumber - 1]);
                player.position.X = activeScreen.screenDoors[DoorNumberToMoveTo - 1].position.X;
                player.position.Y = activeScreen.screenDoors[DoorNumberToMoveTo - 1].position.Y;
            }

            //soundManager.Update(gameTime);

            oldKeyboardState = keyboardState;

        }

        public void Draw(GameTime gameTime)
        {

            spriteBatch.Begin(transformMatrix: activeScreen.camera.Transform);
            activeScreen.Draw(gameTime);
            player.Draw(spriteBatch);
            spriteBatch.End();



        }



        public void HideShowScreenTransition(GameScreen gameScreen1, GameScreen gameScreen2)
        {
            activeScreen.Hide();
            previousActiveScreen = gameScreen1;
            activeScreen = gameScreen2;
            gameScreen1.DisableScreenGameObjects();
            activeScreen.EnableScreenGameObjects();
            activeScreen.Show();
        }

        public void DisableShowScreenTransition(GameScreen gameScreen1, GameScreen gameScreen2)
        {
            gameScreen1.Enabled = false;
            gameScreen1.DisableScreenGameObjects();
            previousActiveScreen = activeScreen;
            activeScreen = gameScreen2;
            activeScreen.EnableScreenGameObjects();
            activeScreen.Show();
        }

        private bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) && oldKeyboardState.IsKeyDown(theKey);
        }

        public void CreateScreens(ContentManager content, GraphicsDevice graphicsDevice)
        {
            
            screens = new List<GameScreen>();
            //ActionScreenBuilder level = new ActionScreenBuilder("levelTest2", assetManager, colliderManager, inputManager, this, player, 2, 2);
            //level.LoadContent(content, graphicsDevice);




            for (int i = 1; i <= 6; i++)
            {
                //ActionScreenBuilder level = new ActionScreenBuilder("Level1_TEST", assetManager, colliderManager, inputManager, this, soundManager, player);

                ActionScreenBuilder level = new ActionScreenBuilder("Level" + i.ToString(), assetManager, colliderManager, inputManager, this, soundManager, player);
                level.LoadContent(content, graphicsDevice);

                screens.Add(
                    new ActionScreen(spriteBatch, menuFont, player, level.gameObjectsAsList, level.backgroundObjects)
                    {
                        respawnPoint = new Vector2(8 * 8, 8 * 8),
                        screenNumber = i,
                        cameraBehaviourType1 = true
                    });

            }



            //ActionScreenBuilder level1 = new ActionScreenBuilder("Level1", colliderManager, inputManager, this, player);
            //level1.LoadContent(content, graphicsDevice);
            //screen1 = new ActionScreen(spriteBatch, menuFont, player, level1.gameObjects, level1.backgroundObjects, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 19, 8 * 8),
            //    screenNumber = 1,
            //    cameraBehaviourType1 = true
            //};

            //ActionScreenBuilder level2 = new ActionScreenBuilder("Level2", colliderManager, inputManager, this, player);
            //level2.LoadContent(content, graphicsDevice);
            //screen2 = new ActionScreen(spriteBatch, menuFont, player, level2.gameObjects, level2.backgroundObjects, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 19, 8 * 8),
            //    screenNumber = 2,
            //    cameraBehaviourType1 = true
            //};


            //// Create screen 1
            //string tileSetStringScreen1 = "ABABCACACACACABACBC";
            //List<int> tileSetCounterScreen1 = new List<int>() { 600, 2, 36, 2, 2, 36, 4, 36, 4, 36, 4, 36, 4, 19, 3, 14, 4, 38, 40 };
            //tileSetScreen1 = new Tileset(tileSetStringScreen1, tileSetCounterScreen1);

            //List<GameObject> screen1GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 21 + 4, 8 * 18), "Door", 2, 1, colliderManager, inputManager)
            //};

            //screen1 = new ActionScreen(spriteBatch, menuFont, player, screen1GameObjects, tileSetScreen1, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8, 0),
            //    screenNumber = 1,
            //    cameraBehaviourType1 = true
            //};



            //// Create screen 2
            //string tileSetStringScreen2 = "AABAABAABABACACC";
            //List<int> tileSetCounterScreen2 = new List<int>() { 680, 20, 6, 14, 14, 3, 23, 8, 3, 29, 5, 35, 5, 35, 5, 35 };
            //tileSetScreen2 = new Tileset(tileSetStringScreen2, tileSetCounterScreen2);

            //List<GameObject> screen2GameObjects = new List<GameObject>()
            //{
            //    new MovingPlatform(new Vector2(8 * 28, 8 * 17), "movingPlatform1", new Vector2(8 * 28, 8 * 7), 0, 1),
            //    new MovingPlatform(new Vector2(8 * 12, 8 * 7), "movingPlatform1", new Vector2(8 * 22, 8 * 7), 0, 1),
            //    new Door(new Vector2(8 * 1, 8 * 18), "Door", 1, 1, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 22, 8 * 15), "Door", 3, 1, colliderManager, inputManager)
            //};

            //for (int i = 5; i < 40; i++)
            //{
            //    screen2GameObjects.Add(new Spike(new Vector2(8 * i, 21 * 8), "Spike", colliderManager));
            //}


            //screen2 = new ActionScreen(spriteBatch, menuFont, player, screen2GameObjects, tileSetScreen2, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 1, 8 * 14),
            //    screenNumber = 2,
            //    cameraBehaviourType1 = true
            //};



            //// Create screen 3
            //string tileSetStringScreen3 = "AABACACACACACACBCCC";
            //List<int> tileSetCounterScreen3 = new List<int>() { 520, 30, 10, 30, 10, 30, 10, 30, 10, 30, 10, 30, 10, 30, 10, 30, 10, 40, 40 };
            //tileSetScreen3 = new Tileset(tileSetStringScreen3, tileSetCounterScreen3);

            //List<GameObject> screen3GameObjects = new List<GameObject>()
            //{
            //    new AnimatedGameObject(new Vector2(8 * 30, 8 * 12), "Post"),
            //    new Door(new Vector2(8 * 1, 8 * 18), "Door", 2, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 36, 8 * 11), "Door", 4, 1, colliderManager, inputManager)
            //};



            //screen3 = new ActionScreen(spriteBatch, menuFont, player, screen3GameObjects, tileSetScreen3, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 1, 8 * 14),
            //    screenNumber = 3,
            //    cameraBehaviourType1 = true
            //};


            //// Create screen 4
            //string tileSetStringScreen4 = "CCACACCACCACCACCACCACCACCBACCACCACCACCACCACCABCCACCACCACCACCBCC";
            //List<int> tileSetCounterScreen4 = new List<int>() { 120, 3, 15, 3, 16, 3, 3, 34, 3, 3, 34, 3, 3, 34, 3, 3, 34, 3, 3, 34, 3, 3, 34, 3, 3, 8, 26, 3, 11, 26, 3, 11, 26, 3, 11, 26, 3, 11, 26, 3, 11, 26, 3, 11, 16, 10, 3, 11, 16, 13, 11, 16, 13, 11, 16, 13, 11, 16, 13, 11, 16, 13, 40 };
            //tileSetScreen4 = new Tileset(tileSetStringScreen4, tileSetCounterScreen4);

            //List<GameObject> screen4GameObjects = new List<GameObject>()
            //{
            //    new HookPoint(new Vector2(8 * 19, 8 *4), "PostDown"),
            //    new Ladder(new Vector2(8 * 11, 8 * 10), 11, colliderManager),
            //    new Door(new Vector2(8 * 5, 8 * 8), "Door", 3, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 33, 8 * 14), "Door", 5, 1, colliderManager, inputManager)

            //};

            //screen4 = new ActionScreen(spriteBatch, menuFont, player, screen4GameObjects, tileSetScreen4, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 5, 8 * 8),
            //    screenNumber = 4,
            //    cameraBehaviourType1 = true
            //};

            //// Create screen 5
            //string tileSetStringScreen5 = "ABCC";
            //List<int> tileSetCounterScreen5 = new List<int>() { 800, 40, 40, 40 };
            //tileSetScreen5 = new Tileset(tileSetStringScreen5, tileSetCounterScreen5);

            //List<GameObject> screen5GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 3, 8 * 18), "Door", 4, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 25, 8 * 18), "Door", 6, 1, colliderManager, inputManager),
            //};

            //screen5 = new ActionScreen(spriteBatch, menuFont, player, screen5GameObjects, tileSetScreen5, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 5, 8 * 8),
            //    screenNumber = 5,
            //    cameraBehaviourType1 = true
            //};

            //// Create screen 6
            //string tileSetStringScreen6 = "ABCC";
            //List<int> tileSetCounterScreen6 = new List<int>() { 800, 40, 40, 40 };
            //tileSetScreen6 = new Tileset(tileSetStringScreen6, tileSetCounterScreen6);

            //List<GameObject> screen6GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 3, 8 * 18), "Door", 5, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 35, 8 * 18), "Door", 7, 1, colliderManager, inputManager)
            //};


            //List<Note> screen6Notes = new List<Note>()
            //{
            //    new Note(new Vector2(8 * 22, 8 * 16), "TuningForkC", "C", colliderManager, inputManager, null, "rune_C"),
            //    new Note(new Vector2(8 * 18, 8 * 17), "TuningForkE", "E", colliderManager, inputManager, null, "rune_E"),
            //    new Note(new Vector2(8 * 14, 8 * 16), "TuningForkG", "G", colliderManager, inputManager, null, "rune_G")
            //};

            //Gate screen6Gate = new Gate(new Vector2(8 * 30, 8 * 14), "AncientDoor", new Vector2(8 * 30, 8 * 10));


            //NoteAndGatePuzzle screen6NoteAndGatePuzzle = new NoteAndGatePuzzle(new Vector2(8 * 17 - 4, 8 * 11 - 4), "symbolPlate", screen6Notes, screen6Gate);

            //screen6GameObjects.Add(screen6NoteAndGatePuzzle);
            //screen6GameObjects.AddRange(screen6Notes);
            //screen6GameObjects.Add(screen6Gate);


            //screen6 = new ActionScreen(spriteBatch, menuFont, player, screen6GameObjects, tileSetScreen6, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 5, 8 * 8),
            //    screenNumber = 6,
            //    cameraBehaviourType1 = true
            //};


            //// Create screen 7
            //string tileSetStringScreen7 = "ABCC";
            //List<int> tileSetCounterScreen7 = new List<int>() { 800, 40, 40, 40 };
            //tileSetScreen7 = new Tileset(tileSetStringScreen7, tileSetCounterScreen7);

            //List<GameObject> screen7GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 3, 8 * 18), "Door", 6, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 35, 8 * 18), "Door", 8, 1, colliderManager, inputManager)
            //};


            //OrbVessel screen7OrbVessel = new OrbVessel(32, 72, 0, 312);

            //List<Note> screen7Notes = new List<Note>()
            //{
            //    new Note(new Vector2(8 * 22, 8 * 16), "TuningForkC", "C",colliderManager, inputManager,  null, "rune_C", "Orb", 3f),
            //    new Note(new Vector2(8 * 18, 8 * 17), "TuningForkE", "E", colliderManager, inputManager, null,  "rune_E", "Orb", 3f),
            //    new Note(new Vector2(8 * 14, 8 * 16), "TuningForkG", "G", colliderManager, inputManager, null, "rune_G", "Orb", 3f)
            //};

            //Gate screen7Gate = new Gate(new Vector2(8 * 30, 8 * 14), "AncientDoor", new Vector2(8 * 30, 8 * 10));

            //NoteAndGateAndOrbPuzzle screen7NoteAndGateAndOrbPuzzle = new NoteAndGateAndOrbPuzzle(new Vector2(8 * 17 - 4, 8 * 11 - 4), "symbolPlate", screen7Notes, screen7Gate, screen7OrbVessel);

            //screen7GameObjects.Add(screen7NoteAndGateAndOrbPuzzle);
            //screen7GameObjects.AddRange(screen7Notes);
            //screen7GameObjects.Add(screen7Gate);

            //screen7 = new ActionScreen(spriteBatch, menuFont, player, screen7GameObjects, tileSetScreen7, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 5, 8 * 8),
            //    screenNumber = 7,
            //    cameraBehaviourType1 = true
            //};


            //// Create screen 8
            //string tileSetStringScreen8 = "CCACACACACACCACCACCACCACCACCACCACCACCACCACCACACACACACBCC";
            //List<int> tileSetCounterScreen8 = new List<int>() { 40, 30, 10, 30, 10, 30, 10, 30, 10, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 35, 5, 35, 5, 35, 5, 35, 5, 35, 5, 40 };
            //tileSetScreen8 = new Tileset(tileSetStringScreen8, tileSetCounterScreen8);

            //List<GameObject> screen8GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 3, 8 * 19), "Door", 7, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 37, 8 * 3), "Door", 9, 1, colliderManager, inputManager)
            //};


            //screen8 = new ActionScreen(spriteBatch, menuFont, player, screen8GameObjects, tileSetScreen8, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 3, 8 * 19),
            //    screenNumber = 8,
            //    cameraBehaviourType1 = true
            //};


            //// Create screen 9
            //string tileSetStringScreen9 = "ABCC";
            //List<int> tileSetCounterScreen9 = new List<int>() { 800, 40, 40, 40 };
            //tileSetScreen9 = new Tileset(tileSetStringScreen9, tileSetCounterScreen9);

            //List<GameObject> screen9GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 3, 8 * 18), "Door", 8, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 35, 8 * 18), "Door", 10, 1, colliderManager, inputManager),
            //    new LocalTeleport(new Vector2(8 * 19, 8 *12), "RopeRing2")
            //};


            //screen9 = new ActionScreen(spriteBatch, menuFont, player, screen9GameObjects, tileSetScreen9, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 5, 8 * 8),
            //    screenNumber = 9,
            //    cameraBehaviourType1 = true
            //};



            //// Create screen 10
            //string tileSetStringScreen10 = "ABCC";
            //List<int> tileSetCounterScreen10 = new List<int>() { 800, 40, 40, 40 };
            //tileSetScreen10 = new Tileset(tileSetStringScreen10, tileSetCounterScreen10);

            //List<GameObject> screen10GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 1, 8 * 18), "Door", 9, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 37, 8 * 18), "Door", 11, 1, colliderManager, inputManager),
            //    new GlobalTeleport(new Vector2(8 * 19, 8 *12), "RopeRing2"),
            //};



            //OrbVessel screen10OrbVessel = new OrbVessel(32, 72, 0, 312);

            //List<Note> screen10Notes = new List<Note>()
            //{
            //    new Note(new Vector2(8 * 30, 8 * 16), "TuningForkC", "C",colliderManager, inputManager,  null, "rune_C", "Orb", 4f),
            //    new Note(new Vector2(8 * 6, 8 * 16), "TuningForkG", "G", colliderManager, inputManager, null, "rune_G", "Orb", 4f)
            //};

            //Gate screen10Gate = new Gate(new Vector2(8 * 35, 8 * 14), "AncientDoor", new Vector2(8 * 35, 8 * 10));

            //NoteAndGateAndOrbPuzzle screen10NoteAndGateAndOrbPuzzle = new NoteAndGateAndOrbPuzzle(new Vector2(8 * 17 - 4, 8 * 5 - 4), "symbolPlate", screen10Notes, screen10Gate, screen10OrbVessel);

            //screen10GameObjects.Add(screen10NoteAndGateAndOrbPuzzle);
            //screen10GameObjects.AddRange(screen10Notes);
            //screen10GameObjects.Add(screen10Gate);

            //screen10 = new ActionScreen(spriteBatch, menuFont, player, screen10GameObjects, tileSetScreen10, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 5, 8 * 8),
            //    screenNumber = 10,
            //    cameraBehaviourType1 = true
            //};


            //// Create screen 11
            //string tileSetStringScreen11 = "ABABCACCACCACCACCACCACC";
            //List<int> tileSetCounterScreen11 = new List<int>() { 600, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 40 };
            //tileSetScreen11 = new Tileset(tileSetStringScreen11, tileSetCounterScreen11);

            //List<GameObject> screen11GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 1, 8 * 13), "Door", 10, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 37, 8 * 13), "Door", 12, 1, colliderManager, inputManager),
            //    new MovingPlatform(new Vector2(8 * 6, 8 * 15), "movingPlatform1", new Vector2(8 * 30, 8 * 15), 30, 2)
            //};


            //for (int i = 5; i < 35; i++)
            //{
            //    screen11GameObjects.Add(new Spike(new Vector2(8 * i, 21 * 8), "Spike"));
            //}



            //List<Note> screen11Notes = new List<Note>()
            //{
            //    new Note(new Vector2(8 * 26, 8 * 13), "FKeyRound", "F", colliderManager, inputManager, null, "rune_F"),
            //    new Note(new Vector2(8 * 10, 8 * 13), "AKeyRound", "A", colliderManager, inputManager, null, "rune_A"),
            //    new Note(new Vector2(8 * 18, 8 * 13), "CKeyRound", "C", colliderManager, inputManager, null, "rune_C")
            //};

            //Gate screen11Gate = new Gate(new Vector2(8 * 35, 8 * 9), "AncientDoor", new Vector2(8 * 35, 8 * 4));

            //NoteAndGatePuzzle screen11NoteAndGatePuzzle = new NoteAndGatePuzzle(new Vector2(8 * 17 - 4, 8 * 5 - 4), "symbolPlate", screen11Notes, screen11Gate);

            //screen11GameObjects.Add(screen11NoteAndGatePuzzle);
            //screen11GameObjects.AddRange(screen11Notes);
            //screen11GameObjects.Add(screen11Gate);

            //screen11 = new ActionScreen(spriteBatch, menuFont, player, screen11GameObjects, tileSetScreen11, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 3, 8 * 8),
            //    screenNumber = 11,
            //    cameraBehaviourType1 = true
            //};





            //// Create screen 12
            //string tileSetStringScreen12 = "ABABCACCACCACCACCACCACC";
            //List<int> tileSetCounterScreen12 = new List<int>() { 600, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 40 };
            //tileSetScreen12 = new Tileset(tileSetStringScreen12, tileSetCounterScreen12);

            //List<GameObject> screen12GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 1, 8 * 13), "Door", 11, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 37, 8 * 13), "Door", 13, 1, colliderManager, inputManager),
            //    new GlobalTeleport(new Vector2(8 * 19, 8 *5), "RopeRing2"),
            //    new MovingPlatform(new Vector2(8 * 9, 8 * 15), "movingPlatform1", new Vector2(8 * 26-4, 8 * 15), 12, 2, 60)
            //};

            //for (int i = 5; i < 35; i++)
            //{
            //    screen12GameObjects.Add(new Spike(new Vector2(8 * i, 21 * 8), "Spike"));
            //}


            //OrbVessel screen12OrbVessel = new OrbVessel(32, 72, 0, 312);

            //List<Note> screen12Notes = new List<Note>()
            //{
            //    new Note(new Vector2(8 * 26, 8 * 13), "FKeyRound", "F",colliderManager, inputManager,  null, "rune_F", "Orb", 4f),
            //    new Note(new Vector2(8 * 10, 8 * 13), "AKeyRound", "A", colliderManager, inputManager, null, "rune_A", "Orb", 4f),
            //    new Note(new Vector2(8 * 18, 8 * 13), "CKeyRound", "C",colliderManager, inputManager,  null, "rune_C", "Orb", 4f)
            //};

            //Gate screen12Gate = new Gate(new Vector2(8 * 35, 8 * 9), "AncientDoor", new Vector2(8 * 35, 8 * 4));

            //NoteAndGateAndOrbPuzzle screen12NoteAndGateAndOrbPuzzle = new NoteAndGateAndOrbPuzzle(new Vector2(8 * 17 - 4, 8 * 3 - 4), "symbolPlate", screen12Notes, screen12Gate, screen12OrbVessel);

            //screen12GameObjects.Add(screen12NoteAndGateAndOrbPuzzle);
            //screen12GameObjects.AddRange(screen12Notes);
            //screen12GameObjects.Add(screen12Gate);

            //screen12 = new ActionScreen(spriteBatch, menuFont, player, screen12GameObjects, tileSetScreen12, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 3, 8 * 8),
            //    screenNumber = 12,
            //    cameraBehaviourType1 = true
            //};


            //// Create screen 13
            //string tileSetStringScreen13 = "AABACACACACACACACACACBCCC";
            //List<int> tileSetCounterScreen13 = new List<int>() { 400, 35, 5, 35, 5, 35, 5, 35, 5, 35, 5, 35, 5, 35, 5, 35, 5, 35, 5, 35, 5, 35, 5, 40, 40 };
            //tileSetScreen13 = new Tileset(tileSetStringScreen13, tileSetCounterScreen13);

            //List<GameObject> screen13GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 1, 8 * 18), "Door", 12, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 36, 8 * 8), "Door", 14, 1, colliderManager, inputManager),
            //};


            //List<GameObject> screen13GameObjectsForNotes = new List<GameObject>()
            //{
            //    new MovingPlatformOneLoop(new Vector2(8 * 20, 8 * 19), "movingPlatform1", new Vector2(8 * 20, 8 * 16), 0, 1),
            //    new MovingPlatformOneLoop(new Vector2(8 * 25, 8 * 16), "movingPlatform1", new Vector2(8 * 25, 8 * 13), 0, 1),
            //    new MovingPlatformOneLoop(new Vector2(8 * 30, 8 * 13), "movingPlatform1", new Vector2(8 * 30, 8 * 10), 0, 1)
            //};


            //List<Note> screen13Notes = new List<Note>()
            //{
            //    new Note(new Vector2(8 * 16, 8 * 16), "TuningForkC", "C", colliderManager, inputManager, new List<GameObject>(){ screen13GameObjectsForNotes[0]}),
            //    new Note(new Vector2(8 * 12, 8 * 17), "TuningForkE", "E", colliderManager, inputManager,  new List<GameObject>(){ screen13GameObjectsForNotes[1]}),
            //    new Note(new Vector2(8 * 8, 8 * 16), "TuningForkG", "G", colliderManager, inputManager,  new List<GameObject>(){ screen13GameObjectsForNotes[2]})
            //};

            //screen13GameObjects.AddRange(screen13GameObjectsForNotes);
            //screen13GameObjects.AddRange(screen13Notes);



            //screen13 = new ActionScreen(spriteBatch, menuFont, player, screen13GameObjects, tileSetScreen13, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 1, 8 * 14),
            //    screenNumber = 13,
            //    cameraBehaviourType1 = true
            //};



            //// Create screen 14
            //string tileSetStringScreen14 = "ABCC";
            //List<int> tileSetCounterScreen14 = new List<int>() { 800, 40, 40, 40 };
            //tileSetScreen14 = new Tileset(tileSetStringScreen14, tileSetCounterScreen14);

            //List<GameObject> screen14GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 1, 8 * 18), "Door", 13, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 37, 8 * 18), "Door", 15, 1, colliderManager, inputManager),
            //};

            //List<Gate> screen14Gates = new List<Gate>()
            //{
            //    new Gate(new Vector2( 8 * 25, 8 * 16), "WoodenGate", new Vector2( 8 * 25, 8 * 13)),
            //    new Gate(new Vector2(8 * 35, 8 * 14), "AncientDoor", new Vector2(8 * 35, 8 * 10))
            //};

            //List<Note> screen14Notes = new List<Note>()
            //{
            //    new Note(new Vector2(8 * 8, 8 * 17), "FKeyRound", "F", colliderManager, inputManager, new List<GameObject>(){screen14Gates[0]}, "rune_F"),
            //    new Note(new Vector2(8 * 18, 8 * 17), "AKeyRound", "A",colliderManager, inputManager,  new List<GameObject>(){screen14Gates[0]}, "rune_A"),
            //    new Note(new Vector2(8 * 30, 8 * 17), "CKeyRound", "C", colliderManager, inputManager, null, "rune_C")
            //};

            //NoteAndGatePuzzle screen14NoteAndGatePuzzle = new NoteAndGatePuzzle(new Vector2(8 * 17 - 4, 8 * 3 - 4), "symbolPlate", screen14Notes, screen14Gates[1]);

            //screen14GameObjects.Add(screen14NoteAndGatePuzzle);
            //screen14GameObjects.AddRange(screen14Gates);
            //screen14GameObjects.AddRange(screen14Notes);


            //screen14 = new ActionScreen(spriteBatch, menuFont, player, screen14GameObjects, tileSetScreen14, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 5, 8 * 8),
            //    screenNumber = 14,
            //    cameraBehaviourType1 = true
            //};

            //// Create screen 15
            //string tileSetStringScreen15 = "ABCC";
            //List<int> tileSetCounterScreen15 = new List<int>() { 800, 40, 40, 40 };
            //tileSetScreen15 = new Tileset(tileSetStringScreen15, tileSetCounterScreen15);

            //List<GameObject> screen15GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 9, 8 * 18), "Door", 14, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 37, 8 * 18), "Door", 16, 1, colliderManager, inputManager)
            //};

            //List<Gate> screen15Gates = new List<Gate>()
            //{
            //    new Gate(new Vector2( 8 * 5, 8 * 16), "WoodenGate", new Vector2( 8 * 5, 8 * 13)),
            //    new Gate(new Vector2( 8 * 19, 8 * 16), "WoodenGate", new Vector2( 8 * 19, 8 * 13)),
            //    new Gate(new Vector2( 8 * 26, 8 * 16), "WoodenGate", new Vector2( 8 * 26, 8 * 13)),
            //    new Gate(new Vector2(8 * 35, 8 * 14), "AncientDoor", new Vector2(8 * 35, 8 * 10))
            //};


            //List<Note> screen15Notes = new List<Note>()
            //{

            //    new Note(new Vector2(8 * 2, 8 * 17), "CKeyRound", "C", colliderManager, inputManager, new List<GameObject>(){screen15Gates[0],screen15Gates[2]}, "rune_C"),
            //    new Note(new Vector2(8 * 22, 8 * 17), "AKeyRound", "A", colliderManager, inputManager, new List<GameObject>(){screen15Gates[0],screen15Gates[1]},  "rune_A"),
            //    new Note(new Vector2(8 * 13, 8 * 17), "FKeyRound", "F", colliderManager, inputManager, new List<GameObject>(){screen15Gates[1]}, "rune_F"),
            //    new Note(new Vector2(8 * 30, 8 * 17), "TuningForkE", "E", colliderManager, inputManager, null, "rune_E"),

            //};

            //NoteAndGatePuzzle screen15NoteAndGatePuzzle = new NoteAndGatePuzzle(new Vector2(8 * 17 - 4, 8 * 5 - 4), "symbolPlateLong", screen15Notes, screen15Gates[3]);

            //screen15GameObjects.Add(screen15NoteAndGatePuzzle);
            //screen15GameObjects.AddRange(screen15Gates);
            //screen15GameObjects.AddRange(screen15Notes);

            //screen15 = new ActionScreen(spriteBatch, menuFont, player, screen15GameObjects, tileSetScreen15, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 9, 8 * 8),
            //    screenNumber = 15,
            //    cameraBehaviourType1 = true
            //};


            //// Create screen 16
            //string tileSetStringScreen16 = "ABABAAAAAAC";
            //List<int> tileSetCounterScreen16 = new List<int>() { 600, 7, 24, 9, 40, 40, 40, 40, 40, 40, 40 };
            //tileSetScreen16 = new Tileset(tileSetStringScreen16, tileSetCounterScreen16);

            //List<GameObject> screen16GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 1, 8 * 13), "Door", 15, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 37, 8 * 13), "Door", 17, 1, colliderManager, inputManager),
            //};


            //for (int i = 0; i < 40; i++)
            //{
            //    screen16GameObjects.Add(new Spike(new Vector2(8 * i, 21 * 8), "Spike"));
            //}

            //List<GameObject> screen16MovingPlatformsNoLoop = new List<GameObject>()
            //{
            //    new MovingPlatformNoLoop(new Vector2(8 * 40, 8 * 17), "movingPlatform1", new Vector2(8 * -4, 8 * 17), 0, 2),
            //    new MovingPlatformNoLoop(new Vector2(8 * 40, 8 * 17), "movingPlatform1", new Vector2(8 * -4, 8 * 17), 0, 2),
            //    new MovingPlatformNoLoop(new Vector2(8 * 40, 8 * 17), "movingPlatform1", new Vector2(8 * -4, 8 * 17), 0, 2),

            //};

            //screen16GameObjects.Add(new Note(new Vector2(8 * 4, 8 * 12), "AKeyRound", "A", colliderManager, inputManager, screen16MovingPlatformsNoLoop, "rune_A"));
            //screen16GameObjects.AddRange(screen16MovingPlatformsNoLoop);

            //screen16 = new ActionScreen(spriteBatch, menuFont, player, screen16GameObjects, tileSetScreen16, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 3, 8 * 8),
            //    screenNumber = 16,
            //    cameraBehaviourType1 = true
            //};


            //// Create screen 17
            //string tileSetStringScreen17 = "ABABAAAAAAC";
            //List<int> tileSetCounterScreen17 = new List<int>() { 600, 7, 24, 9, 40, 40, 40, 40, 40, 40, 40 };
            //tileSetScreen17 = new Tileset(tileSetStringScreen17, tileSetCounterScreen17);

            //List<GameObject> screen17GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 1, 8 * 13), "Door", 16, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 37, 8 * 13), "Door", 18, 1, colliderManager, inputManager),
            //};


            //for (int i = 0; i < 40; i++)
            //{
            //    screen17GameObjects.Add(new Spike(new Vector2(8 * i, 21 * 8), "Spike"));
            //}


            //List<GameObject> screen17MovingPlatformsNoLoop = new List<GameObject>()
            //{
            //    new MovingPlatformNoLoop(new Vector2(8 * 40, 8 * 17), "movingPlatform1", new Vector2(8 * -4, 8 * 17), 0, 2),
            //    new MovingPlatformNoLoop(new Vector2(8 * 40, 8 * 17), "movingPlatform1", new Vector2(8 * -4, 8 * 17), 0, 2),
            //    new MovingPlatformNoLoop(new Vector2(8 * 40, 8 * 17), "movingPlatform1", new Vector2(8 * -4, 8 * 17), 0, 2),

            //};

            //List<Note> screen17Notes = new List<Note>()
            //{
            //    new Note(new Vector2(8 * 4, 8 * 12), "AKeyRound", "A", colliderManager, inputManager, screen17MovingPlatformsNoLoop, "rune_A"),
            //    new Note(new Vector2(8 * 18, 8 * 14), "FKeyRound", "F", colliderManager, inputManager, null, "rune_F"),
            //    new Note(new Vector2(8 * 32, 8 * 12), "CKeyRound", "C", colliderManager, inputManager, null,  "rune_C")
            //};

            //Gate screen17Gate = new Gate(new Vector2(8 * 35, 8 * 9), "AncientDoor", new Vector2(8 * 35, 8 * 4));

            //NoteAndGatePuzzle screen17NoteAndGatePuzzle = new NoteAndGatePuzzle(new Vector2(8 * 17 - 4, 8 * 5 - 4), "symbolPlate", screen17Notes, screen17Gate);

            //screen17GameObjects.Add(screen17NoteAndGatePuzzle);
            //screen17GameObjects.AddRange(screen17MovingPlatformsNoLoop);
            //screen17GameObjects.AddRange(screen17Notes);

            //screen17 = new ActionScreen(spriteBatch, menuFont, player, screen17GameObjects, tileSetScreen17, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 3, 8 * 8),
            //    screenNumber = 17,
            //    cameraBehaviourType1 = true
            //};



            //// Create screen 18
            //string tileSetStringScreen18 = "AABABAABC";
            //List<int> tileSetCounterScreen18 = new List<int>() { 440, 8, 6, 12, 6, 8, 360, 40, 40 };
            //tileSetScreen18 = new Tileset(tileSetStringScreen18, tileSetCounterScreen18);

            //List<GameObject> screen18GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 1, 8 * 19), "Door", 17, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 37, 8 * 19), "Door", 19, 1, colliderManager, inputManager),
            //    new GlobalTeleport(new Vector2(8 * 19, 8 *5), "RopeRing2")
            //};


            //OrbVessel screen18OrbVessel = new OrbVessel(32, 72, 0, 312);

            //List<Note> screen18Notes = new List<Note>()
            //{
            //    new Note(new Vector2(8 * 10, 8 * 8), "FKeyRound", "F", colliderManager, inputManager,  null, "rune_F","OrangeOrb", 5f ),
            //    new Note(new Vector2(8 * 28, 8 * 18), "TuningForkE", "E", colliderManager, inputManager, null, "rune_E","OrangeOrb", 5f),
            //    new Note(new Vector2(8 * 28, 8 * 8), "AKeyRound", "A", colliderManager, inputManager, null, "rune_A","OrangeOrb", 5f),
            //    new Note(new Vector2(8 * 10, 8 * 18), "CKeyRound", "C", colliderManager, inputManager, null, "rune_C","OrangeOrb", 5f)
            //};

            //Gate screen18Gate = new Gate(new Vector2(8 * 35, 8 * 15), "AncientDoor", new Vector2(8 * 35, 8 * 4));

            //NoteAndGateAndOrbPuzzle screen18NoteAndGateAndOrbPuzzle = new NoteAndGateAndOrbPuzzle(new Vector2(8 * 17 - 4, 8 * 3 - 4), "symbolPlateLong", screen18Notes, screen18Gate, screen18OrbVessel);

            //screen18GameObjects.Add(screen18NoteAndGateAndOrbPuzzle);
            //screen18GameObjects.AddRange(screen18Notes);
            //screen18GameObjects.Add(screen18Gate);

            //screen18 = new ActionScreen(spriteBatch, menuFont, player, screen18GameObjects, tileSetScreen18, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 3, 8 * 8),
            //    screenNumber = 18,
            //    cameraBehaviourType1 = true
            //};



            //// Create screen 19
            //string tileSetStringScreen19 = "ABABCACCACCACCACCACCACCACCACCACCACCACCACACACACCACCACCACC";
            //List<int> tileSetCounterScreen19 = new List<int>() { 120, 10, 6, 24, 10, 6, 24, 10, 6, 24, 10, 6, 24, 10, 6, 24, 10, 6, 24, 10, 6, 24, 10, 6, 24, 10, 6, 24, 10, 6, 24, 10, 6, 24, 10, 6, 24, 10, 30, 10, 30, 10, 30, 10, 6, 24, 10, 6, 24, 10, 6, 24, 10, 6, 24, 40 };
            //tileSetScreen19 = new Tileset(tileSetStringScreen19, tileSetCounterScreen19);

            //List<GameObject> screen19GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 1, 8 * 1), "Door", 18, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 37, 8 * 16), "Door", 20, 1, colliderManager, inputManager),

            //};

            //List<Gate> screen19Gates = new List<Gate>()
            //{
            //    new Gate(new Vector2( 8 * 10, 8 * 5), "WoodenGateHorizontal", new Vector2( 8 * 4, 8 * 5)),
            //    new Gate(new Vector2( 8 * 10, 8 * 18), "WoodenGateHorizontal", new Vector2( 8 * 4, 8 * 18)),
            //};

            //screen19GameObjects.Add(new Note(new Vector2(8 * 5, 8 * 1), "CKeyRound", "C", colliderManager, inputManager, new List<GameObject>() { screen19Gates[0], screen19Gates[1] }, "Rune_C"));
            //screen19GameObjects.AddRange(screen19Gates);

            //for (int i = 10; i < 16; i++)
            //{
            //    screen19GameObjects.Add(new Spike(new Vector2(8 * i, 21 * 8), "Spike"));
            //}



            //screen19 = new ActionScreen(spriteBatch, menuFont, player, screen19GameObjects, tileSetScreen19, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 1, 8 * 1),
            //    screenNumber = 19,
            //    cameraBehaviourType1 = true
            //};



            //// Create screen 20
            //string tileSetStringScreen20 = "CABC";
            //List<int> tileSetCounterScreen20 = new List<int>() { 200, 480, 40, 200 };
            //tileSetScreen20 = new Tileset(tileSetStringScreen20, tileSetCounterScreen20);

            //List<GameObject> screen20GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 2, 8 * 15), "Door", 19, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 36, 8 * 15), "Door", 21, 1, colliderManager, inputManager),
            //    new FlashingBeam(new Vector2( 8 * 15, 8 * 5), 120, 60, 0, new Vector2( 8 * 15, 8 * 16))
            //};

            //screen20 = new ActionScreen(spriteBatch, menuFont, player, screen20GameObjects, tileSetScreen20, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 3, 8 * 8),
            //    screenNumber = 20,
            //    cameraBehaviourType1 = true
            //};



            //// Create screen 21
            //string tileSetStringScreen21 = "CABC";
            //List<int> tileSetCounterScreen21 = new List<int>() { 320, 360, 40, 200 };
            //tileSetScreen21 = new Tileset(tileSetStringScreen21, tileSetCounterScreen21);

            //List<GameObject> screen21GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 2, 8 * 15), "Door", 20, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 37, 8 * 15), "Door", 22, 1, colliderManager, inputManager),
            //    new FlashingBeam(new Vector2( 8 * 10, 8 * 8), 51, 85, 0, new Vector2( 8 * 10, 8 * 16)),
            //    new FlashingBeam(new Vector2( 8 * 20, 8 * 8), 51, 85, 51, new Vector2( 8 * 20, 8 * 16)),
            //    new FlashingBeam(new Vector2( 8 * 30, 8 * 8), 34, 102, 102, new Vector2( 8 * 30, 8 * 16)),
            //    new BouncingOrb(new Vector2(8 * 35, 8 * 10), "BouncingOrb", 2, 5 * (float)Math.PI / 4)

            //};

            //List<Note> screen21Notes = new List<Note>()
            //{
            //    new Note(new Vector2(8 * 25, 8 * 14), "AKeyRound", "A", colliderManager, inputManager, null, "rune_A"),
            //    new Note(new Vector2(8 * 15, 8 * 14), "FKeyRound", "F",colliderManager, inputManager,  null, "rune_F"),
            //};

            //Gate screen21Gate = new Gate(new Vector2(8 * 35, 8 * 11), "AncientDoor", new Vector2(8 * 35, 8 * 4));

            //NoteAndGatePuzzle screen21NoteAndGatePuzzle = new NoteAndGatePuzzle(new Vector2(8 * 17 - 4, 8 * 5 - 4), "symbolPlate", screen21Notes, screen21Gate);

            //screen21GameObjects.Add(screen21NoteAndGatePuzzle);
            //screen21GameObjects.AddRange(screen21Notes);
            //screen21GameObjects.Add(screen21Gate);


            //screen21 = new ActionScreen(spriteBatch, menuFont, player, screen21GameObjects, tileSetScreen21, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 2, 8 * 15),
            //    screenNumber = 21,
            //    cameraBehaviourType1 = true
            //};

            //// Create screen 22
            //string tileSetStringScreen22 = "ABABCACCACCACCACCACCACCACCAC";
            //List<int> tileSetCounterScreen22 = new List<int>() { 560, 10, 20, 10, 10, 20, 10, 10, 20, 10, 10, 20, 10, 10, 20, 10, 10, 20, 10, 10, 20, 10, 10, 20, 10, 10, 20, 10 };
            //tileSetScreen22 = new Tileset(tileSetStringScreen22, tileSetCounterScreen22);

            //List<GameObject> screen22GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 1, 8 * 12), "Door", 21, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 37, 8 * 12), "Door", 23, 1, colliderManager, inputManager)
            //};

            //AnimatedGameObject screen22Platform = new AnimatedGameObject(new Vector2(8 * 17, 8 * 15), "ClimablePlatform")
            //{
            //    climable = true,
            //    CollisionObject = true
            //};


            //screen22GameObjects.Add(screen22Platform);

            //List<Note> screen22Notes = new List<Note>()
            //{
            //    new Note(new Vector2(8 * 19, 8 * 19), "FKeyRound", "F", colliderManager, inputManager, null,  "rune_F")
            //};

            //Gate screen22Gate = new Gate(new Vector2(8 * 35, 8 * 8), "AncientDoor", new Vector2(8 * 35, 8 * 4));

            //NoteAndGatePuzzle screen22NoteAndGatePuzzle = new NoteAndGatePuzzle(new Vector2(8 * 17 - 4, 8 * 3 - 4), "symbolPlate", screen22Notes, screen22Gate);

            //screen22GameObjects.Add(screen22NoteAndGatePuzzle);
            //screen22GameObjects.AddRange(screen22Notes);
            //screen22GameObjects.Add(screen22Gate);

            //screen22 = new ActionScreen(spriteBatch, menuFont, player, screen22GameObjects, tileSetScreen22, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 1, 8 * 12),
            //    screenNumber = 22,
            //    cameraBehaviourType1 = true
            //};


            //// Create screen 23
            //string tileSetStringScreen23 = "CABABCACCACCACCACCACCACCACCAC";
            //List<int> tileSetCounterScreen23 = new List<int>() { 120, 440, 10, 20, 10, 10, 20, 10, 10, 20, 10, 10, 20, 10, 10, 20, 10, 10, 20, 10, 10, 20, 10, 10, 20, 10, 10, 20, 10 };
            //tileSetScreen23 = new Tileset(tileSetStringScreen23, tileSetCounterScreen23);

            //List<GameObject> screen23GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 1, 8 * 12), "Door", 22, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 37, 8 * 12), "Door", 24, 1, colliderManager, inputManager),
            //    new FlashingBeam(new Vector2( 8 * 19, 8 * 3), 600, 10, 0, new Vector2( 8 * 19, 8 * 21)),
            //    new FlashingBeam(new Vector2( 8 * 20, 8 * 3), 600, 10, 0, new Vector2( 8 * 20, 8 * 21)),
            //};

            //AnimatedGameObject screen23Platform = new AnimatedGameObject(new Vector2(8 * 17, 8 * 15), "ClimablePlatform")
            //{
            //    climable = true,
            //    CollisionObject = true
            //};


            //screen23GameObjects.Add(screen23Platform);


            //screen23 = new ActionScreen(spriteBatch, menuFont, player, screen23GameObjects, tileSetScreen23, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 1, 8 * 12),
            //    screenNumber = 23,
            //    cameraBehaviourType1 = true
            //};


            //// Create screen 24
            //string tileSetStringScreen24 = "CABABCACCACCACCACCACCACCACCAC";
            //List<int> tileSetCounterScreen24 = new List<int>() { 120, 440, 10, 20, 10, 10, 20, 10, 10, 20, 10, 10, 20, 10, 10, 20, 10, 10, 20, 10, 10, 20, 10, 10, 20, 10, 10, 20, 10 };
            //tileSetScreen24 = new Tileset(tileSetStringScreen24, tileSetCounterScreen24);

            //List<GameObject> screen24GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 1, 8 * 12), "Door", 23, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 37, 8 * 12), "Door", 25, 1, colliderManager, inputManager),
            //    new FlashingBeam(new Vector2( 8 * 19, 8 * 3), 600, 10, 0, new Vector2( 8 * 19, 8 * 21)),
            //    new FlashingBeam(new Vector2( 8 * 20, 8 * 3), 600, 10, 0, new Vector2( 8 * 20, 8 * 21)),
            //};

            //MovingPlatform screen24Platform = new MovingPlatform(new Vector2(8 * 12, 8 * 15), "MovingClimablePlatform", new Vector2(8 * 24, 8 * 15), 30, 1f)
            //{
            //    climable = true,
            //    CollisionObject = true
            //};


            //screen24GameObjects.Add(screen24Platform);


            //screen24 = new ActionScreen(spriteBatch, menuFont, player, screen24GameObjects, tileSetScreen24, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 1, 8 * 12),
            //    screenNumber = 24,
            //    cameraBehaviourType1 = true
            //};



            //// Create screen 25
            //string tileSetStringScreen25 = "AABACACACACACACBCCC";
            //List<int> tileSetCounterScreen25 = new List<int>() { 520, 30, 10, 30, 10, 30, 10, 30, 10, 30, 10, 30, 10, 30, 10, 30, 10, 40, 40 };
            //tileSetScreen25 = new Tileset(tileSetStringScreen25, tileSetCounterScreen25);

            //List<GameObject> screen25GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 1, 8 * 18), "Door", 24, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 36, 8 * 11), "Door", 26, 1, colliderManager, inputManager)
            //};

            //LaunchPad screen25LaunchPad1 = new LaunchPad(new Vector2(8 * 10, 8 * 18 + 4), "LaunchPad");
            //Note screen25Note1 = new Note(new Vector2(8 * 10, 8 * 16), "FKeyRound", "F", colliderManager, inputManager, new List<GameObject>() { screen25LaunchPad1 });


            //LaunchPad screen25LaunchPad2 = new LaunchPad(new Vector2(8 * 26, 8 * 18 + 4), "LaunchPad");
            //Note screen25Note2 = new Note(new Vector2(8 * 18, 8 * 6), "CKeyRound", "C", colliderManager, inputManager, new List<GameObject>() { screen25LaunchPad2 });

            //screen25GameObjects.Add(screen25LaunchPad1);
            //screen25GameObjects.Add(screen25Note1);
            //screen25GameObjects.Add(screen25LaunchPad2);
            //screen25GameObjects.Add(screen25Note2);


            //screen25 = new ActionScreen(spriteBatch, menuFont, player, screen25GameObjects, tileSetScreen25, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 1, 8 * 14),
            //    screenNumber = 25,
            //    cameraBehaviourType1 = true
            //};


            //// Create screen 26
            //string tileSetStringScreen26 = "CCACCACCACCABACCACACCACACCACCACCACCACCBCC";
            //List<int> tileSetCounterScreen26 = new List<int>() { 240, 14, 12, 14, 14, 12, 14, 14, 12, 14, 14, 4, 4, 4, 14, 14, 4, 4, 4, 14, 14, 4, 4, 4, 14, 14, 12, 14, 8, 24, 8, 8, 24, 8, 8, 24, 8, 8, 24, 8, 240 };
            //tileSetScreen26 = new Tileset(tileSetStringScreen26, tileSetCounterScreen26);

            //List<GameObject> screen26GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 10, 8 * 14), "Door", 25, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 28, 8 * 14), "Door", 27, 1, colliderManager, inputManager)
            //};

            //OrbVessel screen26OrbVessel = new OrbVessel(32, 72, 0, 312);

            //List<Note> screen26Notes = new List<Note>()
            //{
            //    new Note(new Vector2(8 * 15, 8 * 13), "CKeyRound", "C", colliderManager, inputManager, null, "rune_C", "Orb", 3f),
            //    new Note(new Vector2(8 * 23, 8 * 13), "FKeyRound", "FHigh",colliderManager, inputManager, null, "rune_F", "Orb", 3f),
            //    new Note(new Vector2(8 * 15, 8 * 7), "GKeyRound", "GLow",colliderManager, inputManager,  null, "rune_G", "Orb", 3f),
            //    new Note(new Vector2(8 * 23, 8 * 7), "EKeyRound", "E",colliderManager, inputManager,  null, "rune_E", "Orb", 3f)
            //};

            //Gate screen26Gate = new Gate(new Vector2(8 * 26, 8 * 10), "AncientDoor", new Vector2(8 * 26, 8 * 5));

            //NoteAndGateAndOrbPuzzle screen26NoteAndGateAndOrbPuzzle = new NoteAndGateAndOrbPuzzle(new Vector2(8 * 17 - 4, 8 * 4 - 4), "symbolPlateLong", screen26Notes, screen26Gate, screen26OrbVessel);

            //screen26GameObjects.Add(screen26NoteAndGateAndOrbPuzzle);
            //screen26GameObjects.AddRange(screen26Notes);
            //screen26GameObjects.Add(screen26Gate);


            //screen26 = new ActionScreen(spriteBatch, menuFont, player, screen26GameObjects, tileSetScreen26, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 10, 8 * 14),
            //    screenNumber = 26,
            //    cameraBehaviourType1 = true
            //};


            //// Create screen 27
            //string tileSetStringScreen27 = "CACAACAACABACABCACACCACACCACACCACACCACACCACACCACACCACACCACACCACACCACACCACCACCACCACCACCACCAC";
            //List<int> tileSetCounterScreen27 = new List<int>() { 40, 18, 4, 18, 18, 4, 18, 18, 4, 18, 12, 6, 4, 6, 12, 12, 6, 4, 6, 12, 12, 6, 4, 6, 12, 12, 6, 4, 6, 12, 12, 6, 4, 6, 12, 12, 6, 4, 6, 12, 12, 6, 4, 6, 12, 12, 6, 4, 6, 12, 12, 6, 4, 6, 12, 12, 6, 4, 6, 12, 12, 6, 4, 6, 12, 12, 6, 4, 6, 12, 12, 16, 12, 12, 16, 12, 12, 16, 12, 12, 16, 12, 12, 16, 12, 12, 16, 12, 12, 16, 12 };
            //tileSetScreen27 = new Tileset(tileSetStringScreen27, tileSetCounterScreen27);

            //List<GameObject> screen27GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 1, 8 * 2), "Door", 26, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 38, 8 * 2), "Door", 28, 1, colliderManager, inputManager),
            //    new LocalTeleport(new Vector2(8 * 19, 8 *18), "RopeRing2")
            //};

            //for (int i = 12; i < 28; i++)
            //{
            //    screen27GameObjects.Add(new Spike(new Vector2(8 * i, 8 * 22), "Spike"));
            //}



            //screen27 = new ActionScreen(spriteBatch, menuFont, player, screen27GameObjects, tileSetScreen27, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 1, 8 * 2),
            //    screenNumber = 27,
            //    cameraBehaviourType1 = true
            //};

            //// Create screen 28
            //string tileSetStringScreen28 = "CACAACAACABACABACAACAACAACAACAACAACAACAACAACAACAACAA";
            //List<int> tileSetCounterScreen28 = new List<int>() { 40, 18, 4, 18, 18, 4, 18, 18, 4, 18, 12, 6, 4, 6, 12, 18, 4, 18, 18, 4, 18, 18, 4, 18, 18, 4, 18, 18, 4, 18, 18, 4, 18, 18, 4, 18, 18, 4, 18, 18, 4, 18, 18, 4, 18, 18, 4, 18, 18, 4, 18, 240 };
            //tileSetScreen28 = new Tileset(tileSetStringScreen28, tileSetCounterScreen28);

            //List<GameObject> screen28GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 1, 8 * 2), "Door", 27, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 38, 8 * 2), "Door", 29, 1, colliderManager, inputManager),
            //    new LocalTeleport(new Vector2(8 * 19, 8 *18), "RopeRing2"),
            //    new LocalTeleport(new Vector2(8 * 30, 8 *15), "RopeRing2"),
            //    new LocalTeleport(new Vector2(8 * 25, 8 *8), "RopeRing2")

            //};

            //for (int i = 0; i < 40; i++)
            //{
            //    screen28GameObjects.Add(new Spike(new Vector2(8 * i, 8 * 22), "Spike"));
            //}



            //screen28 = new ActionScreen(spriteBatch, menuFont, player, screen28GameObjects, tileSetScreen28, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 1, 8 * 2),
            //    screenNumber = 28,
            //    cameraBehaviourType1 = true
            //};


            //// Create screen 29
            //string tileSetStringScreen29 = "ABC";
            //List<int> tileSetCounterScreen29 = new List<int>() { 1040, 80, 720 };
            //tileSetScreen29 = new Tileset(tileSetStringScreen29, tileSetCounterScreen29, 23, 80);

            //List<GameObject> screen29GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 1, 8 * 11), "Door", 28, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 70, 8 * 11), "Door", 30, 1, colliderManager, inputManager)

            //};



            //screen29 = new ActionScreen(spriteBatch, menuFont, player, screen29GameObjects, tileSetScreen29, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 1, 8 * 11),
            //    screenNumber = 29,
            //    cameraBehaviourType2 = true
            //};


            //// Create screen 30
            //string tileSetStringScreen30 = "ABABCACCACCACCACCACCACC";
            //List<int> tileSetCounterScreen30 = new List<int>() { 600, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 40 };
            //tileSetScreen30 = new Tileset(tileSetStringScreen30, tileSetCounterScreen30);

            //List<GameObject> screen30GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 1, 8 * 13), "Door", 29, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 37, 8 * 13), "Door", 31, 1, colliderManager, inputManager)
            //};

            //Note screen30Note = new Note(new Vector2(8 * 10, 8 * 11), "TuningForkC", "C", colliderManager, inputManager, null, "rune_C");

            //List<GameObject> screen30MovingPlatforms = new List<GameObject>()
            //{
            //    new MovingPlatformHalfLoop(new Vector2(8 * 9, 8 * 15), "movingPlatform1", new Vector2(8 * 26-4, 8 * 15), 0, 2){spritesOnPlatform = new List<GameObject>() {screen30Note } }
            //};

            //screen30Note.attachedGameObjects = screen30MovingPlatforms;

            ////Note screen30Note = new Note(new Vector2(8 * 10, 8 * 11), "TuningForkC", "C", screen30MovingPlatforms);

            ////(MovingPlatformHalfLoop)screen30MovingPlatforms[0].spritesOnPlatform = new List<AnimatedGameObject>() { screen30Note };


            ////Note screen30Note = new Note(new Vector2(8 * 10, 8 * 11), "TuningForkC", "C", screen30MovingPlatforms);

            ////List<GameObject> screen30MovingPlatforms = new List<GameObject>()
            ////{
            ////    new MovingPlatformHalfLoop(new Vector2(8 * 9, 8 * 15), "movingPlatform1", new Vector2(8 * 26-4, 8 * 15), 0, 2)
            ////};

            ////Note screen30Note = new Note(new Vector2(8 * 10, 8 * 11), "TuningForkC", "C", screen30MovingPlatforms);

            ////(MovingPlatformHalfLoop)screen30MovingPlatforms[0].spritesOnPlatform = new List<AnimatedGameObject>() { screen30Note };

            //screen30GameObjects.AddRange(screen30MovingPlatforms);
            //screen30GameObjects.Add(screen30Note);



            //for (int i = 5; i < 35; i++)
            //{
            //    screen30GameObjects.Add(new Spike(new Vector2(8 * i, 21 * 8), "Spike"));
            //}

            //screen30 = new ActionScreen(spriteBatch, menuFont, player, screen30GameObjects, tileSetScreen30, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 3, 8 * 8),
            //    screenNumber = 30,
            //    cameraBehaviourType1 = true
            //};


            //// Create screen 31
            //string tileSetStringScreen31 = "ABABA";
            //List<int> tileSetCounterScreen31 = new List<int>() { 1320, 8, 104, 8, 1320 };
            //tileSetScreen31 = new Tileset(tileSetStringScreen31, tileSetCounterScreen31, 23, 120);

            //List<GameObject> screen31GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 1, 8 * 9), "Door", 30, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 118, 8 * 9), "Door", 32, 1, colliderManager, inputManager),
            //    new BouncingOrb(new Vector2(8 * 40, 8 * 9), "BouncingOrb", 0, 0),
            //    new FlashingBeam(new Vector2( 8 * 60, 8 * 4), 600, 10, 0, new Vector2( 8 * 80, 8 *4 ))
            //};

            //for (int i = 0; i < 120; i++)
            //{
            //    screen31GameObjects.Add(new AnimatedGameObject(new Vector2(8 * i, 8 * 5), "NoteLine"));
            //    screen31GameObjects.Add(new AnimatedGameObject(new Vector2(8 * i, 8 * 17), "NoteLine"));

            //    if (i >= 8 && i <= 112)
            //    {
            //        screen31GameObjects.Add(new AnimatedGameObject(new Vector2(8 * i, 8 * 11), "NoteLine"));

            //    }

            //}

            //NoteShip screen31NoteShip = new NoteShip(new Vector2(8 * 9, 8 * 11), "movingPlatform1Long", new Vector2(8 * 102, 8 * 11), 0, 2, 8 * 6);

            //List<GameObject> screen31NoteShipAsList = new List<GameObject>() { screen31NoteShip };

            //List<GameObject> screen31Notes = new List<GameObject>()
            //{
            //    new Note (new Vector2(8 * 9, 8* 9), "FKeyRound", "F", colliderManager, inputManager, screen31NoteShipAsList, null, null, 0, 1),
            //    new Note (new Vector2(8 * 13, 8* 9), "AKeyRound", "A", colliderManager, inputManager, screen31NoteShipAsList, null, null, 0, 0),
            //    new Note (new Vector2(8 * 17, 8* 9), "CKeyRound", "C", colliderManager, inputManager, screen31NoteShipAsList, null, null, 0, -1),

            //};

            //screen31NoteShip.spritesOnPlatform = screen31Notes;

            //screen31GameObjects.Add(screen31NoteShip);
            //screen31GameObjects.AddRange(screen31Notes);







            //screen31 = new ActionScreen(spriteBatch, menuFont, player, screen31GameObjects, tileSetScreen31, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 1, 8 * 9),
            //    screenNumber = 31,
            //    cameraBehaviourType5 = true
            //};



            //// Create screen 32
            //string tileSetStringScreen32 = "CCACACACAABABABCACCACCACCACCACCACC";
            //List<int> tileSetCounterScreen32 = new List<int>() { 160, 15, 25, 15, 25, 15, 25, 15, 25, 25, 15, 240, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 5, 30, 5, 40 };
            //tileSetScreen32 = new Tileset(tileSetStringScreen32, tileSetCounterScreen32);

            //List<GameObject> screen32GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 1, 8 * 13), "Door", 31, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 37, 8 * 6), "Door", 33, 1, colliderManager, inputManager)
            //};

            //LaunchPad screen32LaunchPad = new LaunchPad(new Vector2(8 * 19, 8 * 20 + 4), "LaunchPad");
            //Note screen32Note = new Note(new Vector2(8 * 30, 8 * 19), "FKeyRound", "TimpaniC", colliderManager, inputManager, new List<GameObject>() { screen32LaunchPad });
            //screen32GameObjects.Add(screen32LaunchPad);
            //screen32GameObjects.Add(screen32Note);

            //DiagonalOrbsPattern2 screen32DiagonalOrbs = new DiagonalOrbsPattern2(new Vector2(8 * 35, 8 * 20), new Vector2(8 * 4, 8 * 20), 6, 2, 1.75f);
            //screen32GameObjects.Add(screen32DiagonalOrbs);

            //screen32 = new ActionScreen(spriteBatch, menuFont, player, screen32GameObjects, tileSetScreen32, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 1, 8 * 13),
            //    screenNumber = 32,
            //    cameraBehaviourType1 = true
            //};


            //// Create screen 33
            //string tileSetStringScreen33 = "CCACACACABCACCACCACCACCACCACCACCACCACCACCACCACACACACBCC";
            //List<int> tileSetCounterScreen33 = new List<int>() { 80, 34, 6, 34, 6, 34, 6, 3, 34, 3, 3, 34, 3, 3, 34, 3, 3, 34, 3, 3, 34, 3, 3, 34, 3, 3, 34, 3, 3, 34, 3, 3, 34, 3, 3, 34, 3, 3, 34, 3, 3, 34, 3, 3, 34, 3, 6, 34, 6, 34, 6, 34, 6, 34, 40 };
            //tileSetScreen33 = new Tileset(tileSetStringScreen33, tileSetCounterScreen33);

            //List<GameObject> screen33GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 0, 8 * 19), "Door", 32, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 37, 8 * 3), "Door", 34, 1, colliderManager, inputManager),
            //    new Beam(new Vector2(8*32, 8*17), new Vector2(8*7, 8*17)),
            //    new Beam(new Vector2(8*32, 8*5), new Vector2(8*7, 8*5))
            //};

            //List<GameObject> screen33Platforms = new List<GameObject>()
            //{
            //    new MovingPlatformOneLoop(new Vector2(8 * 23, 8 * 5), "ClimableBox", new Vector2(8 * 23, 8 * 16), 60, 1){ climable = true },
            //    new MovingPlatformOneLoop(new Vector2(8 * 13, 8 * 16), "ClimableBox", new Vector2(8 * 13, 8 * 5), 60, 1){ climable = true }
            //};

            //Note screen33Note = new Note(new Vector2(8 * 3, 8 * 19), "FKeyRound", "F", colliderManager, inputManager, screen33Platforms);
            //screen33GameObjects.AddRange(screen33Platforms);
            //screen33GameObjects.Add(screen33Note);



            //screen33 = new ActionScreen(spriteBatch, menuFont, player, screen33GameObjects, tileSetScreen33, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 1, 8 * 19),
            //    screenNumber = 33,
            //    cameraBehaviourType1 = true
            //};

            //// Create screen 34
            //string tileSetStringScreen34 = "CCACACACABCACCACCACCACCACCACCACCACCACCACCACCACACACACBCC";
            //List<int> tileSetCounterScreen34 = new List<int>() { 80, 34, 6, 34, 6, 34, 6, 34, 3, 3, 34, 3, 3, 34, 3, 3, 3, 34, 3, 3, 34, 3, 3, 34, 3, 3, 34, 3, 3, 34, 3, 3, 34, 3, 3, 34, 3, 3, 34, 3, 3, 3, 34, 3, 3, 34, 6, 34, 6, 34, 6, 34, 6, 34, 40 };
            //tileSetScreen34 = new Tileset(tileSetStringScreen34, tileSetCounterScreen34);

            //List<GameObject> screen34GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 0, 8 * 19), "Door", 33, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 37, 8 * 3), "Door", 35, 1, colliderManager, inputManager),
            //    new Beam(new Vector2(8*32, 8*15), new Vector2(8*7, 8*15)),
            //    new Beam(new Vector2(8*32, 8*8), new Vector2(8*7, 8*8))
            //};

            //List<GameObject> screen34Platforms = new List<GameObject>()
            //{
            //    new MovingPlatformHalfLoop(new Vector2(8 * 19, 8 * 8), "ClimableBox", new Vector2(8 * 19, 8 * 14), 0, 1){ climable = true },
            //    new MovingPlatformHalfLoop(new Vector2(8 * 13, 8 * 14), "ClimableBox", new Vector2(8 * 13, 8 * 8), 0, 1){ climable = true },
            //    new MovingPlatformHalfLoop(new Vector2(8 * 24, 8 * 14), "ClimableBox", new Vector2(8 * 24, 8 * 8), 0, 1){ climable = true }

            //};

            //Note screen34Note = new Note(new Vector2(8 * 3, 8 * 19), "FKeyRound", "F", colliderManager, inputManager, screen34Platforms);
            //screen34GameObjects.AddRange(screen34Platforms);
            //screen34GameObjects.Add(screen34Note);



            //screen34 = new ActionScreen(spriteBatch, menuFont, player, screen34GameObjects, tileSetScreen34, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 1, 8 * 19),
            //    screenNumber = 34,
            //    cameraBehaviourType1 = true
            //};

            //// Create screen 35
            //string tileSetStringScreen35 = "CACACACCACCACCACCACCACCACCACCACCACCACCACCACCACCACCBCC";
            //List<int> tileSetCounterScreen35 = new List<int>() { 120, 36, 4, 36, 4, 36, 4, 4, 32, 4, 4, 32, 4, 4, 32, 4, 4, 32, 4, 4, 32, 4, 4, 32, 4, 4, 32, 4, 4, 32, 4, 4, 32, 4, 4, 32, 4, 4, 32, 4, 4, 32, 4, 4, 32, 4, 4, 32, 4, 4, 32, 4, 80 };
            //tileSetScreen35 = new Tileset(tileSetStringScreen35, tileSetCounterScreen35);

            //List<GameObject> screen35GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 0, 8 * 4), "Door", 34, 2, colliderManager, inputManager),
            //    new Door(new Vector2(8 * 32, 8 * 18), "Door", 36, 1, colliderManager, inputManager),
            //    new Beam(new Vector2(8*21, 8*3), new Vector2(8*21, 8*19)),
            //    new Beam(new Vector2(8*35, 8*13), new Vector2(8*4, 8*13))
            //};



            //for (int i = 4; i < 21; i++)
            //{
            //    screen35GameObjects.Add(new Spike(new Vector2(8 * i, 19 * 8), "Spike"));
            //}

            //screen35GameObjects.Add(new MovingPlatformInSquare(new Vector2(8 * 30, 8 * 8), "ClimableBox", new Vector2(8 * 8, 8 * 8), new Vector2(8 * 8, 8 * 15), new Vector2(8 * 30, 8 * 15), 0, 1) { climable = true });



            //screen35 = new ActionScreen(spriteBatch, menuFont, player, screen35GameObjects, tileSetScreen35, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 0, 8 * 4),
            //    screenNumber = 35,
            //    cameraBehaviourType1 = true
            //};


            //// Create screen 36            
            //tileSetScreen36 = new Tileset("Level36");

            //List<GameObject> screen36GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 1, 8 * 15), "Door", 35, 2),
            //    new Door(new Vector2(8 * 34, 8 * 15), "Door", 37, 1),
            //};

            //screen36 = new ActionScreen(spriteBatch, menuFont, player, screen36GameObjects, tileSetScreen36, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 1, 8 * 15),
            //    screenNumber = 36,
            //    cameraBehaviourType1 = true
            //};

            //// Create screen 37            
            //tileSetScreen37 = new Tileset("Level37", "Level37Markers");

            //List<GameObject> screen37GameObjects = new List<GameObject>()
            //{
            //    new Door(new Vector2(8 * 1, 8 * 19), "Door", 36, 2),
            //    new Door(new Vector2(8 * 34, 8 * 14), "Door", 1, 1),
            //};

            //screen37 = new ActionScreen(spriteBatch, menuFont, player, tileSetScreen37.screenGameObjects, tileSetScreen37, keyboardState, oldKeyboardState)
            //{
            //    respawnPoint = new Vector2(8 * 1, 8 * 15),
            //    screenNumber = 37,
            //    cameraBehaviourType1 = true
            //};


            //References.soundManager.flagTest = true;

            //activeScreen = new GameScreen(spriteBatch);
            //previousActiveScreen = activeScreen;

            //screens.Add(screen1);
            //screens.Add(screen2);
            //screens.Add(screen3);
            //screens.Add(screen4);
            //screens.Add(screen5);
            //screens.Add(screen6);
            //screens.Add(screen7);
            //screens.Add(screen8);
            //screens.Add(screen9);
            //screens.Add(screen10);
            //screens.Add(screen11);
            //screens.Add(screen12);
            //screens.Add(screen13);
            //screens.Add(screen14);
            //screens.Add(screen15);
            //screens.Add(screen16);
            //screens.Add(screen17);
            //screens.Add(screen18);
            //screens.Add(screen19);
            //screens.Add(screen20);
            //screens.Add(screen21);
            //screens.Add(screen22);
            //screens.Add(screen23);
            //screens.Add(screen24);
            //screens.Add(screen25);
            //screens.Add(screen26);
            //screens.Add(screen27);
            //screens.Add(screen28);
            //screens.Add(screen29);
            //screens.Add(screen30);
            //screens.Add(screen31);
            //screens.Add(screen32);
            //screens.Add(screen33);
            //screens.Add(screen34);
            //screens.Add(screen35);
            //screens.Add(screen36);
            //screens.Add(screen37);

            //screens.Add(previousActiveScreen);
            //screens.Add(activeScreen);


        }



    }
}
