using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite;
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Aseprite.Content.Processors;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using System.Reflection.Emit;

namespace Adventure
{
    public class ActionScreenBuilder
    {
        public string filename;
        public AsepriteFile asepriteFile;

        //public IDictionary<Color[,], string> backgroundLayerAndData = new Dictionary<Color[,], string>();

        public IDictionary<AsepriteLayer, Color[,]> data = new Dictionary<AsepriteLayer, Color[,]>();


        //public Color[,] gameObjectColorArray;
        public IDictionary<Color, string> backgroundDictionary = new Dictionary<Color, string>();
        public IDictionary<Color, string> gameObjectDictionary = new Dictionary<Color, string>();
        public IDictionary<string, List<List<GameObject>>> attachmentsDictionary = new Dictionary<string, List<List<GameObject>>>();
        public IDictionary<Type, List<List<GameObject>>> attachmentsDictionary2 = new Dictionary<Type, List<List<GameObject>>>();


        public ColliderManager colliderManager;
        public InputManager inputManager;
        public ScreenManager screenManager;
        public AssetManager assetManager;
        public SoundManager soundManager;
        public Player player;

        public Vector2 respawnPoint;
        public int cameraTypeIndex = 0;
        public int renderTargetIndex;
        public int screenWidth;
        public int screenHeight;

        public int tileSize = 16;

        //public List<GameObject> gameObjects = new List<GameObject>();

        public AnimatedGameObject[,] backgroundObjects;
        public GameObject[,] gameObjects;



        public List<GameObject> gameObjectsAsList = new List<GameObject>();


        public Color color_MovingPlatformPreMultiplyAlpha_1;
        public Color color_MovingPlatformPreMultiplyAlpha_2;
        public Color color_MovingPlatformPreMultiplyAlpha_3;

        public Color color_BeamPreMultiplyAlpha;

        public Color color_AdjustPosition = new Color(255, 255, 156, 255); // Pale yellow
        public Color color_Climable = new Color(85, 255, 0, 255); // Bright green
        public Color color_Boosters = new Color(203, 203, 203, 255); // Silver

        public Color color_Spawn = new Color(0, 241, 255, 255); // Bright cyan



        public List<Color> attachmentColors = new List<Color>();



        public ActionScreenBuilder(string filename, AssetManager assetManager, ColliderManager colliderManager, InputManager inputManager, ScreenManager screenManager, SoundManager soundManager, Player player)
        {
            this.filename = filename;

            this.assetManager = assetManager;
            this.colliderManager = colliderManager;
            this.inputManager = inputManager;
            this.screenManager = screenManager;
            this.player = player;
            this.soundManager = soundManager;

            attachmentColors.Add(new Color(255, 0, 255, 255));
            attachmentColors.Add(new Color(225, 0, 225, 255));
            attachmentColors.Add(new Color(200, 0, 225, 255));
            attachmentColors.Add(new Color(175, 0, 225, 255));
            attachmentColors.Add(new Color(150, 0, 225, 255));
            attachmentColors.Add(new Color(125, 0, 225, 255));
            attachmentColors.Add(new Color(100, 0, 225, 255));

            backgroundDictionary.Add(new Color(199, 245, 255, 255), "Tile_air");
            backgroundDictionary.Add(new Color(75, 105, 47, 255), "Tile_grass");
            backgroundDictionary.Add(new Color(102, 57, 49, 255), "Tile_ground");

            gameObjectDictionary.Add(new Color(255, 157, 4, 255), "Door"); // Gold
            gameObjectDictionary.Add(new Color(255, 255, 255, 255), "Spike"); // White
            gameObjectDictionary.Add(new Color(121, 49, 160, 255), "SeriesOfMovingPlatform_ABWrapAround_PlayerEmitter"); // Light purple
            gameObjectDictionary.Add(new Color(41, 20, 52, 255), "movingPlatform_ABLoop"); // Very dark purple
            gameObjectDictionary.Add(new Color(72, 37, 91, 255), "movingPlatform_AB"); // Dark purple
            gameObjectDictionary.Add(new Color(97, 52, 121, 255), "SeriesOfMovingPlatform_ABWrapAround_Reversing"); // Purple
            gameObjectDictionary.Add(new Color(104, 104, 104, 255), "LaunchPad"); // Grey
            gameObjectDictionary.Add(new Color(240, 0, 0, 255), "Note_KeyRound"); // Bright red
            gameObjectDictionary.Add(new Color(155, 173, 183, 255), "Beam"); // Light grey
            gameObjectDictionary.Add(new Color(65, 25, 18, 255), "Gate"); // Dark brown
            gameObjectDictionary.Add(new Color(255, 120, 0, 255), "NoteAndGatePuzzle"); // Bright orange
            gameObjectDictionary.Add(new Color(255, 81, 0, 255), "GateAndOrbEmitterPuzzle"); // Bright orange-red
            gameObjectDictionary.Add(new Color(207, 66, 0, 255), "GateAndOrbEmitterPuzzle2"); // orange-red
            gameObjectDictionary.Add(new Color(56, 37, 37, 255), "OrganStop"); // Dark brown
            gameObjectDictionary.Add(new Color(50, 60, 57, 255), "Ivy"); // Dark green
            gameObjectDictionary.Add(new Color(151, 151, 151, 255), "BreakingPlatform"); // Light grey
            gameObjectDictionary.Add(new Color(17, 0, 86, 255), "OrganPipe"); // Dark blue
            gameObjectDictionary.Add(new Color(101, 225, 39, 255), "OrbEmitter_Stationary"); // Very light green
            gameObjectDictionary.Add(new Color(94, 233, 24, 255), "OrbEmitter_PlayerEmitter_Stationary"); // Very very light green
            gameObjectDictionary.Add(new Color(76, 220, 4, 255), "SingleOrbEmitter_PlayerEmitter"); // Very very very light green
            gameObjectDictionary.Add(new Color(179, 255, 141, 255), "SeriesOfOrbEmitters"); // Pale green

            gameObjectDictionary.Add(new Color(188, 206, 216, 255), "ActionScreenTransitionWall"); // Very light grey
            gameObjectDictionary.Add(new Color(104, 86, 86, 255), "BoostPad"); // Dark grey / brown
            gameObjectDictionary.Add(new Color(68, 165, 186, 255), "AnimatedGameObject"); // Turqoise
            gameObjectDictionary.Add(new Color(115, 232, 255, 255), "ScreenWrap"); // Light blue


            //color_MovingPlatformPreMultiplyAlpha = Color.FromNonPremultiplied(color_MovingPlatformAlpha.ToVector4());
            //color_MovingPlatformPreMultiplyAlpha = Color.FromNonPremultiplied(gameObjectDictionary[])
            color_MovingPlatformPreMultiplyAlpha_1 = Color.FromNonPremultiplied(41, 20, 52, 150);
            color_MovingPlatformPreMultiplyAlpha_2 = Color.FromNonPremultiplied(41, 20, 52, 125);
            color_MovingPlatformPreMultiplyAlpha_3 = Color.FromNonPremultiplied(41, 20, 52, 100);


            color_BeamPreMultiplyAlpha = Color.FromNonPremultiplied(155, 173, 183, 150);



            // Initialise attachmentsDictionary
            attachmentsDictionary.Add("Note", new List<List<GameObject>>());
            attachmentsDictionary.Add("MovingPlatform", new List<List<GameObject>>());
            attachmentsDictionary.Add("NoteAndGatePuzzle", new List<List<GameObject>>());
            attachmentsDictionary.Add("GateAndOrbEmitterPuzzle", new List<List<GameObject>>());
            attachmentsDictionary.Add("GateAndOrbEmitterPuzzle2", new List<List<GameObject>>());
            attachmentsDictionary.Add("OrganStop", new List<List<GameObject>>());


            foreach (string typeName in attachmentsDictionary.Keys)
            {
                for (int i = 0; i < attachmentColors.Count; i++)
                {
                    attachmentsDictionary[typeName].Add(new List<GameObject>());
                }
            }



        }

        public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            // Load in aseprite file
            asepriteFile = contentManager.Load<AsepriteFile>(filename);

            // From the frame we obtain Cel information
            ReadOnlySpan<AsepriteCel> asepriteCels = asepriteFile.GetFrame(0).Cels;

            foreach (AsepriteCel cel in asepriteCels)
            {
                if (cel.Layer.Name == "Data")
                {
                    string info = cel.Layer.UserData.Text;
                    List<int> ints = ParseStringToInts(ref info, 4);
                    screenWidth = ints[0];
                    screenHeight = ints[1];
                    renderTargetIndex = ints[2];
                    cameraTypeIndex = ints[3];
                }
            }

            backgroundObjects = new AnimatedGameObject[screenWidth, screenHeight];
            gameObjects = new GameObject[screenWidth, screenHeight];





            // From Cel data we obtain pixel data
            foreach (AsepriteCel cel in asepriteCels)
            {
                AsepriteImageCel imageCel2 = cel as AsepriteImageCel;
                data.Add(cel.Layer, CreatePixelArray(imageCel2));
            }



            foreach (AsepriteLayer layer in data.Keys)
            {
                if (layer.Name == "Background")
                {
                    HandleBackgroundLayer(layer, data[layer]);
                }
                else if (layer.Name.Contains("GameObjects"))
                {
                    HandleGameObjectLayer(layer, data[layer]);
                }
                else if (layer.Name == "AdjustHorizontal")
                {
                    HandleAdjustHorizontalLayer(layer, data[layer]);
                }
                else if (layer.Name == "AdjustVertical")
                {
                    HandleAdjustVerticalLayer(layer, data[layer]);
                }
                else if (layer.Name == "Climables")
                {
                    HandleClimableLayer(layer, data[layer]);
                }
                else if (layer.Name.Contains("Attachments"))
                {
                    string info = layer.UserData.Text;
                    string typeName = ParseUntilNextComma(ref info);
                    HandleAttachmentsLayer(attachmentsDictionary[typeName], data[layer], ref info, typeName);
                }
                else if (layer.Name.Contains("Boosters"))
                {
                    HandleBoostersLayer(layer, data[layer]);
                }
                else if (layer.Name.Contains("Data"))
                {
                    HandleDataLayer(layer, data[layer]);
                }

            }
            // AGH. Must be a better way to do this ... 
            FormAttachments<Note>(attachmentsDictionary["Note"]);
            FormAttachments<MovingPlatform>(attachmentsDictionary["MovingPlatform"]);
            FormAttachments<NoteAndGatePuzzle>(attachmentsDictionary["NoteAndGatePuzzle"]);
            FormAttachments<GateAndOrbEmitterPuzzle>(attachmentsDictionary["GateAndOrbEmitterPuzzle"]);
            FormAttachments<GateAndOrbEmitterPuzzle2>(attachmentsDictionary["GateAndOrbEmitterPuzzle2"]);
            FormAttachments<OrganStop>(attachmentsDictionary["OrganStop"]);

            List<GameObject> gameObjects_LoadLast = new List<GameObject>();
            List<GameObject> gameObjects_LoadFirst = new List<GameObject>();

            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject != null)
                {
                    if (gameObject.LoadLast)
                    {
                        gameObjects_LoadLast.Add(gameObject);
                    }
                    else if (gameObject.LoadFirst)
                    {
                        gameObjects_LoadFirst.Add(gameObject);
                    }
                    else
                    {
                        gameObjectsAsList.Add(gameObject);
                    }
                }

            }



            gameObjectsAsList.InsertRange(0, gameObjects_LoadFirst);
            gameObjectsAsList.AddRange(gameObjects_LoadLast);



        }

        // Pixel data is presented as a list, read from right to left then top to bottom from the aseprite file
        // We convert this into a 2D array which is easier to use
        // The i,j component of the array correponds to the pixel data in the (x,y) = (8 * i, 8 * j) co-ordinate on the screen
        public Color[,] CreatePixelArray(AsepriteImageCel imageCel)
        {
            Color[,] array = new Color[screenWidth, screenHeight];
            for (int i = 0; i < imageCel.Pixels.Length; i++)
            {
                array[i % screenWidth, (i - (i % screenWidth)) / screenWidth] = imageCel.Pixels[i];
            }
            return array;
        }

        public void HandleBackgroundLayer(AsepriteLayer layer, Color[,] colors)
        {
            for (int i = 0; i < colors.GetLength(0); i++)
            {
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    if (backgroundDictionary.ContainsKey(colors[i, j]))
                    {
                        backgroundObjects[i, j] = new AnimatedGameObject(new Vector2(tileSize * i, tileSize * j), backgroundDictionary[colors[i, j]], assetManager);
                    }
                }

            }
        }

        public void HandleGameObjectLayer(AsepriteLayer layer, Color[,] colors)
        {
            string info = layer.UserData.Text;

            for (int i = 0; i < colors.GetLength(0); i++)
            {
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    if (gameObjectDictionary.ContainsKey(colors[i, j]))
                    {
                        Vector2 position = new Vector2(tileSize * i, tileSize * j);

                        if (gameObjectDictionary[colors[i, j]] == "AnimatedGameObject")
                        {
                            string filename = ParseUntilNextComma(ref info);
                            List<float> ints = ParseString(ref info);

                            bool receptor = false;

                            if (filename.Contains("Receptor"))
                            {
                                receptor = true;
                            }

                            gameObjects[i, j] = new AnimatedGameObject(position, filename, assetManager) { receptorBehaviour = receptor };
                            SetRectangularColorRegionToZero(ref colors, i, j, (int)ints[0], (int)ints[1]);

                        }
                        else if (gameObjectDictionary[colors[i, j]] == "Door")
                        {
                            string filename = ParseUntilNextComma(ref info);
                            List<float> ints = ParseString(ref info, 3);
                            gameObjects[i, j] = new Door(position, filename, (int)ints[0], (int)ints[1], (int)ints[2], assetManager, colliderManager, inputManager, screenManager, player);
                            SetRectangularColorRegionToZero(ref colors, i, j, 2, 2);
                        }
                        else if (gameObjectDictionary[colors[i, j]] == "Spike")
                        {
                            gameObjects[i, j] = new Spike(position, "Spike", assetManager, colliderManager, player);
                        }
                        else if (gameObjectDictionary[colors[i, j]] == "movingPlatform_ABLoop")
                        {
                            string filename = ParseUntilNextComma(ref info);
                            string direction = ParseUntilNextComma(ref info);
                            List<float> ints = ParseString(ref info);

                            bool receptor = false;

                            if (filename.Contains("Receptor"))
                            {
                                receptor = true;
                            }

                            Vector2 endPosition = FindEndPointForGameObject(colors, color_MovingPlatformPreMultiplyAlpha_1, i, j, direction);
                            gameObjects[i, j] = new MovingPlatform_ABLoop(position, endPosition, filename, ints[0], new List<int>() { (int)ints[1], (int)ints[2] }, assetManager, colliderManager, screenManager, player, receptor);
                            SetRectangularColorRegionToZero(ref colors, i, j, (int)ints[3], (int)ints[4]);

                        }
                        else if (gameObjectDictionary[colors[i, j]] == "movingPlatform_AB")
                        {
                            string filename = ParseUntilNextComma(ref info);
                            string direction = ParseUntilNextComma(ref info);
                            List<float> ints = ParseString(ref info);

                            bool receptor = false;

                            if (filename.Contains("Receptor"))
                            {
                                receptor = true;
                            }

                            Vector2 endPosition = FindEndPointForGameObject(colors, color_MovingPlatformPreMultiplyAlpha_1, i, j, direction);
                            gameObjects[i, j] = new MovingPlatform_AB(position, endPosition, filename, ints[0], new List<int>() { (int)ints[1], (int)ints[2] }, assetManager, colliderManager, screenManager, player, receptor);
                            SetRectangularColorRegionToZero(ref colors, i, j, (int)ints[3], (int)ints[4]);

                        }
                        else if (gameObjectDictionary[colors[i, j]] == "SeriesOfMovingPlatform_ABWrapAround_PlayerEmitter")
                        {
                            string filename = ParseUntilNextComma(ref info);
                            string direction = ParseUntilNextComma(ref info);
                            List<float> ints = ParseString(ref info);

                            Vector2 endPosition = FindEndPointForGameObject(colors, color_MovingPlatformPreMultiplyAlpha_1, i, j, direction);
                            gameObjects[i, j] = new SeriesOfMovingPlatform_ABWrapAround_PlayerEmitter(position, endPosition, filename, ints[1], new List<int>() { (int)ints[2], (int)ints[3] }, (int)ints[4], assetManager, colliderManager, screenManager, player);
                            SetRectangularColorRegionToZero(ref colors, i, j, (int)ints[5], (int)ints[6]);
                        }
                        else if (gameObjectDictionary[colors[i, j]] == "SeriesOfMovingPlatform_ABWrapAround_Reversing")
                        {
                            string filename = ParseUntilNextComma(ref info);
                            string direction = ParseUntilNextComma(ref info);
                            List<float> ints = ParseString(ref info);

                            Vector2 endPosition = FindEndPointForGameObject(colors, color_MovingPlatformPreMultiplyAlpha_1, i, j, direction);
                            gameObjects[i, j] = new SeriesOfMovingPlatform_ABWrapAround_Reversing(position, endPosition, filename, ints[1], new List<int>() { (int)ints[2], (int)ints[3] }, (int)ints[4], assetManager, colliderManager, screenManager, player);
                            SetRectangularColorRegionToZero(ref colors, i, j, (int)ints[5], (int)ints[6]);

                        }
                        else if (gameObjectDictionary[colors[i, j]] == "OrganStop")
                        {
                            string direction = ParseUntilNextComma(ref info);
                            List<float> ints = ParseString(ref info, 6);
                            Vector2 endPosition = FindEndPointForGameObject(colors, color_MovingPlatformPreMultiplyAlpha_1, i, j, direction);
                            gameObjects[i, j] = new OrganStop(position, endPosition, ints[1], new List<int>() { (int)ints[2], (int)ints[3] }, assetManager, colliderManager, screenManager, player, tileSize * (int)ints[4], (int)ints[5]);
                            if (position.X == endPosition.X)
                            {
                                SetRectangularColorRegionToZero(ref colors, i, j, 5, 2);
                            }
                            else
                            {
                                SetRectangularColorRegionToZero(ref colors, i, j, 2, 5);
                            }
                        }
                        else if (gameObjectDictionary[colors[i, j]] == "Note_KeyRound")
                        {
                            string noteValue = ParseUntilNextComma(ref info);
                            gameObjects[i, j] = new Note(position, noteValue + "KeyRound", noteValue, "rune_" + noteValue, assetManager, colliderManager, inputManager, soundManager, player);
                            SetRectangularColorRegionToZero(ref colors, i, j, 2, 2);
                        }
                        else if (gameObjectDictionary[colors[i, j]] == "LaunchPad")
                        {
                            gameObjects[i, j] = new LaunchPad(position, "LaunchPad", assetManager, colliderManager, player);
                            SetRectangularColorRegionToZero(ref colors, i, j, 2, 2);

                        }
                        else if (gameObjectDictionary[colors[i, j]] == "Beam")
                        {
                            string direction = ParseUntilNextComma(ref info);
                            string onOrOff = ParseUntilNextComma(ref info);

                            List<float> ints = ParseString(ref info);
                            Vector2 endPosition = FindEndPointForGameObject(colors, color_BeamPreMultiplyAlpha, i, j, direction);
                            gameObjects[i, j] = new Beam(position, endPosition, onOrOff, assetManager, colliderManager, screenManager, player);
                        }
                        else if (gameObjectDictionary[colors[i, j]] == "Gate")
                        {
                            string filename = ParseUntilNextComma(ref info);
                            List<int> ints = ParseStringToInts(ref info);

                            Vector2 endPoint = new Vector2(position.X + tileSize * ints[0], position.Y + tileSize * ints[1]);
                            gameObjects[i, j] = new Gate(position, endPoint, filename, assetManager, colliderManager, screenManager, player);
                            SetRectangularColorRegionToZero(ref colors, i, j, ints[2], ints[3]);
                        }
                        else if (gameObjectDictionary[colors[i, j]] == "NoteAndGatePuzzle")
                        {
                            List<int> ints = ParseStringToInts(ref info);
                            gameObjects[i, j] = new NoteAndGatePuzzle(position, "symbolPlate", ints, assetManager);
                            SetRectangularColorRegionToZero(ref colors, i, j, 6, 1);
                        }
                        else if (gameObjectDictionary[colors[i, j]] == "GateAndOrbEmitterPuzzle")
                        {
                            gameObjects[i, j] = new GateAndOrbEmitterPuzzle();
                            SetRectangularColorRegionToZero(ref colors, i, j, 1, 1);
                        }
                        else if (gameObjectDictionary[colors[i, j]] == "GateAndOrbEmitterPuzzle2")
                        {
                            gameObjects[i, j] = new GateAndOrbEmitterPuzzle2(colliderManager);
                            SetRectangularColorRegionToZero(ref colors, i, j, 1, 1);
                        }
                        else if (gameObjectDictionary[colors[i, j]] == "Ivy")
                        {
                            List<float> ints = ParseString(ref info);
                            gameObjects[i, j] = new Ivy(position, "Ivy", assetManager, colliderManager, player, (int)ints[0]);
                            SetRectangularColorRegionToZero(ref colors, i, j, 1, (int)ints[0]);
                        }
                        else if (gameObjectDictionary[colors[i, j]] == "BreakingPlatform")
                        {
                            gameObjects[i, j] = new BreakingPlatform(position, "platform_breaking", assetManager, colliderManager, screenManager, player);
                            SetRectangularColorRegionToZero(ref colors, i, j, 2, 1);
                        }
                        else if (gameObjectDictionary[colors[i, j]] == "OrganPipe")
                        {
                            string direction = ParseUntilNextComma(ref info);
                            List<float> ints = ParseString(ref info);

                            Vector2 endPosition = FindEndPointForGameObject(colors, color_MovingPlatformPreMultiplyAlpha_1, i, j, direction);
                            gameObjects[i, j] = new OrganPipe(position, endPosition, ints[1], new List<int>() { (int)ints[2], (int)ints[3] }, assetManager, colliderManager, screenManager, player, tileSize * (int)ints[4], (int)ints[5]);
                            SetRectangularColorRegionToZero(ref colors, i, j, 2, 1);

                        }
                        else if (gameObjectDictionary[colors[i, j]] == "ActionScreenTransitionWall")
                        {
                            string direction = ParseUntilNextComma(ref info);
                            List<int> ints = ParseStringToInts(ref info);
                            gameObjects[i, j] = new ActionScreenTransitionWall(position, direction, tileSize * ints[0], tileSize * ints[1], ints[2], ints[3], ints[4], colliderManager, inputManager, screenManager, player);
                            SetRectangularColorRegionToZero(ref colors, i, j, ints[0], ints[1]);
                        }
                        else if (gameObjectDictionary[colors[i, j]] == "OrbEmitter_Stationary")
                        {
                            string emitterFilename = ParseUntilNextComma(ref info);
                            string orbFilename = ParseUntilNextComma(ref info);
                            string orbDirection = ParseUntilNextComma(ref info);
                            List<float> ints = ParseString(ref info, 4);

                            if (orbDirection == "h" || orbDirection == "v")
                            {
                                MovingPlatform orbEmitter = new MovingPlatform(new List<Vector2>() { position, position }, emitterFilename, 0, new List<int>() { 0, 0 }, assetManager, colliderManager, screenManager, player);
                                Vector2 endPosition = FindEndPointForGameObject(colors, color_MovingPlatformPreMultiplyAlpha_1, i, j, orbDirection);
                                gameObjects[i, j] = new OrbEmitter(position, endPosition, orbFilename, ints[0], new List<int>() { (int)ints[1], (int)ints[2] }, (int)ints[3], assetManager, colliderManager, screenManager, player, orbEmitter);
                                SetRectangularColorRegionToZero(ref colors, i, j, 1, 1);
                            }
                            else
                            {
                                MovingPlatform orbEmitter = new MovingPlatform(new List<Vector2>() { position, position }, emitterFilename, 0, new List<int>() { 0, 0 }, assetManager, colliderManager, screenManager, player);
                                gameObjects[i, j] = new OrbEmitter(position, orbDirection, orbFilename, ints[0], new List<int>() { (int)ints[1], (int)ints[2] }, (int)ints[3], assetManager, colliderManager, screenManager, player, orbEmitter);
                                SetRectangularColorRegionToZero(ref colors, i, j, 1, 1);
                            }

                        }
                        else if (gameObjectDictionary[colors[i, j]] == "OrbEmitter_PlayerEmitter_Stationary")
                        {
                            string emitterFilename = ParseUntilNextComma(ref info);
                            string orbFilename = ParseUntilNextComma(ref info);
                            string orbDirection = ParseUntilNextComma(ref info);
                            List<float> ints = ParseString(ref info);
                            if (orbDirection == "h" || orbDirection == "v")
                            {
                                MovingPlatform orbEmitter = new MovingPlatform(new List<Vector2>() { position, position }, emitterFilename, 0, new List<int>() { 0, 0 }, assetManager, colliderManager, screenManager, player);
                                Vector2 endPosition = FindEndPointForGameObject(colors, color_MovingPlatformPreMultiplyAlpha_1, i, j, orbDirection);
                                gameObjects[i, j] = new OrbEmitter_PlayerEmitter(position, endPosition, orbFilename, ints[0], new List<int>() { (int)ints[1], (int)ints[2] }, (int)ints[3], assetManager, colliderManager, screenManager, player, orbEmitter);
                                SetRectangularColorRegionToZero(ref colors, i, j, 1, 1);
                            }
                            else
                            {
                                MovingPlatform orbEmitter = new MovingPlatform(new List<Vector2>() { position, position }, emitterFilename, 0, new List<int>() { 0, 0 }, assetManager, colliderManager, screenManager, player);
                                gameObjects[i, j] = new OrbEmitter_PlayerEmitter(position, orbDirection, orbFilename, ints[0], new List<int>() { (int)ints[1], (int)ints[2] }, (int)ints[3], assetManager, colliderManager, screenManager, player, orbEmitter);
                                SetRectangularColorRegionToZero(ref colors, i, j, 1, 1);
                            }


                        }
                        else if (gameObjectDictionary[colors[i, j]] == "SingleOrbEmitter_PlayerEmitter")
                        {
                            //Debug.WriteLine("here");

                            string emitterFilename = ParseUntilNextComma(ref info);
                            string filename = ParseUntilNextComma(ref info);
                            string orbDirection = ParseUntilNextComma(ref info);
                            string behaviour = ParseUntilNextComma(ref info);

                            if (behaviour == "Stationary")
                            {
                                List<float> ints = ParseString(ref info);
                                MovingPlatform orbEmitter = new MovingPlatform(new List<Vector2>() { position, position }, emitterFilename, 0, new List<int>() { 0, 0 }, assetManager, colliderManager, screenManager, player);
                                Vector2 endPosition = FindEndPointForGameObject(colors, color_MovingPlatformPreMultiplyAlpha_1, i, j, orbDirection);
                                gameObjects[i, j] = new SingleOrbEmitter_PlayerEmitter(position, endPosition, filename, ints[0], new List<int>() { (int)ints[1], (int)ints[2] }, assetManager, colliderManager, screenManager, player, orbEmitter);
                                SetRectangularColorRegionToZero(ref colors, i, j, 1, 1);
                            }
                            else if (behaviour == "ABLoop")
                            {
                                string emitterDirection = ParseUntilNextComma(ref info);
                                List<float> ints = ParseString(ref info);
                                Vector2 endPositionOrb = FindEndPointForGameObject(colors, color_MovingPlatformPreMultiplyAlpha_1, i, j, orbDirection);
                                Vector2 endPositionEmitter = FindEndPointForGameObject(colors, color_MovingPlatformPreMultiplyAlpha_2, i, j, emitterDirection);
                                MovingPlatform_ABLoop orbEmitter = new MovingPlatform_ABLoop(position, endPositionEmitter, emitterFilename, ints[3], new List<int>() { (int)ints[4], (int)ints[5] }, assetManager, colliderManager, screenManager, player);
                                gameObjects[i, j] = new SingleOrbEmitter_PlayerEmitter(position, endPositionOrb, filename, ints[0], new List<int>() { (int)ints[1], (int)ints[2] }, assetManager, colliderManager, screenManager, player, orbEmitter);
                                SetRectangularColorRegionToZero(ref colors, i, j, 1, 1);
                            }

                        }
                        else if (gameObjectDictionary[colors[i, j]] == "SeriesOfOrbEmitters")
                        {
                            //Debug.WriteLine("here");
                            List<int> numberOfEmitters = ParseStringToInts(ref info, 1);
                            List<Vector2> positions = new List<Vector2>();
                            positions.Add(position);

                            if (j < colors.GetLength(1) - 1)
                            {
                                for (int j1 = j + 1; j1 < colors.GetLength(1); j1++)
                                {
                                    Debug.WriteLine(j1);

                                    if (gameObjectDictionary.ContainsKey(colors[i, j1]))
                                    {
                                        if (gameObjectDictionary[colors[i, j1]] == "SeriesOfOrbEmitters")
                                        {
                                            positions.Add(new Vector2(i * tileSize, j1 * tileSize));
                                            SetRectangularColorRegionToZero(ref colors, i, j1, 1, 1);
                                        }
                                    }
                                }
                            }

                            if (i < colors.GetLength(0) - 1)
                            {
                                for (int i1 = i + 1; i1 < colors.GetLength(0); i1++)
                                {
                                    for (int j1 = 0; j1 < colors.GetLength(1); j1++)
                                    {
                                        if (gameObjectDictionary.ContainsKey(colors[i1, j1]))
                                        {
                                            if (gameObjectDictionary[colors[i1, j1]] == "SeriesOfOrbEmitters")
                                            {
                                                positions.Add(new Vector2(i1 * tileSize, j1 * tileSize));
                                                SetRectangularColorRegionToZero(ref colors, i1, j1, 1, 1);
                                            }
                                        }
                                    }
                                }
                            }

                    

                            List<string> movementDirections = new List<string>();
                            
                            for (int k = 0; k < numberOfEmitters[0]; k++)
                            {
                                movementDirections.Add(ParseUntilNextComma(ref info));
                            }

                            List<int> order = ParseStringToInts(ref info, numberOfEmitters[0]);


                            string emitterFilename = ParseUntilNextComma(ref info);
                            string orbFilename = ParseUntilNextComma(ref info);
                            List<float> ints = ParseString(ref info);


                            gameObjects[i, j] = new SeriesOfOrbEmitters(positions, movementDirections, order, orbFilename, emitterFilename, ints[0], assetManager, colliderManager, screenManager, player);
                            SetRectangularColorRegionToZero(ref colors, i, j, 1,1);



                        }
                        else if (gameObjectDictionary[colors[i, j]] == "BoostPad")
                        {
                            List<float> ints = ParseString(ref info);
                            gameObjects[i, j] = new BoostPad(position, "BoostPad", ints[0], assetManager, colliderManager, player);
                            SetRectangularColorRegionToZero(ref colors, i, j, 2, 1);

                        }
                        else if (gameObjectDictionary[colors[i, j]] == "ScreenWrap")
                        {
                            string location = ParseUntilNextComma(ref info);
                            List<int> ints = ParseStringToInts(ref info);
                            gameObjects[i, j] = new ScreenWrap(position, "ScreenWrap", location, tileSize * ints[0], tileSize * ints[1], assetManager, screenManager, player);
                            SetRectangularColorRegionToZero(ref colors, i, j, ints[0], ints[1]);

                        }
                    }

                }

            }

        }


        // In these methods it is necessary to pass the gameObject the list by reference.
        // This is because different gameObjects will require different numbers of (consecutive) elements from the list - we cannot just feed in ints[0] each time, for example.
        // And so we let the gameObjects handle these by themselves.
        public void HandleAdjustHorizontalLayer(AsepriteLayer layer, Color[,] colors)
        {
            string info = layer.UserData.Text;
            List<int> ints = ParseStringToInts(ref info);

            for (int i = 0; i < colors.GetLength(0); i++)
            {
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    if (colors[i, j] == color_AdjustPosition)
                    {
                        gameObjects[i, j].AdjustHorizontally(ref ints);
                    }

                }
            }
        }

        public void HandleAdjustVerticalLayer(AsepriteLayer layer, Color[,] colors)
        {
            string info = layer.UserData.Text;
            List<int> ints = ParseStringToInts(ref info);

            for (int i = 0; i < colors.GetLength(0); i++)
            {
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    if (colors[i, j] == color_AdjustPosition)
                    {

                        gameObjects[i, j].AdjustVertically(ref ints);


                    }

                }
            }
        }



        public void HandleClimableLayer(AsepriteLayer layer, Color[,] colors)
        {

            for (int i = 0; i < colors.GetLength(0); i++)
            {
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    if (colors[i, j] == color_Climable)
                    {
                        gameObjects[i, j].SetClimable();
                    }

                }
            }
        }
        public void HandleDataLayer(AsepriteLayer layer, Color[,] colors)
        {
            string info = layer.UserData.Text;
            List<int> ints = ParseStringToInts(ref info);


            for (int i = 0; i < colors.GetLength(0); i++)
            {
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    if (colors[i, j] == color_Spawn)
                    {
                        respawnPoint = new Vector2(tileSize * i, tileSize * j);
                    }

                }
            }
        }
        public void HandleBoostersLayer(AsepriteLayer layer, Color[,] colors)
        {

            for (int i = 0; i < colors.GetLength(0); i++)
            {
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    if (colors[i, j] == color_Boosters)
                    {
                        gameObjects[i, j].SetBoosters();
                    }

                }
            }
        }
        // This is a generic method - we feed in a parameter Type T and behaviour can alter depending on the value of T
        // We give as input a list of lists of GameObjects, and consider each list in turn
        // For each list, we find an object of type T, and then let this objects attachedGameObjects consist of every other object in the list not of type T 
        public void FormAttachments<T>(List<List<GameObject>> list)
        {

            for (int i = 0; i < list.Count; i++)
            {
                foreach (GameObject gameObject in list[i])
                {

                    if (gameObject is T)
                    {

                        foreach (GameObject gameObject1 in list[i])
                        {
                            if (gameObject1 is not T)
                            {
                                gameObject.AddAttachedGameObject(gameObject1);
                            }
                        }
                    }
                }
            }

        }

        // Some GameObjects e.g. Notes and MovingPlatforms may have a list of attached GameObjects. How we deal with this is as follows:
        // These GameObjects require their own "attachment" layer in the Aseprite file
        // Say a Note is attached to a Gate. We will colour both in one of the "attachmentColors" which are shades of PINK
        // This code scans that layer and will put the Note and Gate in a new list of GameObjects
        // We allow for a single layer to detail multiple attachments - e.g. say there is another Note attached to another Gate. Then on the Aseprite file
        // we colour these another "attachmentColor" (i.e. different shade of PINK) and these will then be put in ANOTHER list.
        // With these lists we then form the attachments, described by the code above.
        public void HandleAttachmentsLayer(List<List<GameObject>> gameObjectsAttachmentsData, Color[,] colors, ref string info, string typeName)
        {
            for (int i = 0; i < colors.GetLength(0); i++)
            {
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    if (attachmentColors.Contains(colors[i, j]))
                    {
                        //if (gameObjects[i, j].GetType().ToString() == "Adventure." + typeName)
                        //{
                        //    if (info.Count() > 0)
                        //    {
                        //        gameObjects[i, j].noteTriggerData = ParseUntilNextComma(ref info);
                        //    }
                        //}

                        gameObjectsAttachmentsData[attachmentColors.IndexOf(colors[i, j])].Add(gameObjects[i, j]);
                    }
                }
            }
        }



        // This code takes a string and creates a new string (stringToAdd) until the next comma
        public string ParseUntilNextComma(ref string str)
        {
            string stringToAdd = "";
            //stringToAdd += str[0];

            for (int i = 0; i < 100; i++)
            {
                if (str[i] != ',')
                {
                    stringToAdd += str[i];
                }
                else
                {
                    str = str.Remove(0, i + 1);
                    break;
                }
            }

            return stringToAdd;
        }

        // We parse strings according to the following rules. The order we scan the screen for GameObjects is top-to-bottom then left-to-right.
        // We end every bit of important information with a comma
        // For direction we always take 0 = vertical and 1 = horizontal. 
        // Doors will go: doorNumber, screenNumberToMoveTo, doorNumberToMoveTo
        // Note_KeyRound: note value (e.g. C or F etc)
        // Puzzle: just parse the NoteOrder
        // MovingPlatform: direction, speed, stationaryTimes (in order of positions)
        // SeriesOfMovingPlatform: direction, speed, stationaryTimes (in order of positions), numberOfPlatforms, spacing
        // Beam: direction
        // OrganStop / OrganPipe: direction, speed, stationaryTimes (in order of positions), distanceFromBase, behaviour (0 = AB, 1 = ABA)
        // Ivy: number of ivy tiles
        // Attachment layer: contains the name of the object Type we are attaching to - e.g. Note or MovingPlatform

        public List<float> ParseString(ref string str, int stop = 100)
        {
            List<float> result = new List<float>();

            for (int k = 0; k < 100; k++)
            {
                if (str.Count() > 0)
                {
                    // This last argument ensures that we look for decimal points as the default looks for commas (e.g. 2.5 vs 2,5)
                    result.Add(float.Parse(ParseUntilNextComma(ref str), CultureInfo.InvariantCulture));

                    if (result.Count == stop)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            return result;

        }

        public List<int> ParseStringToInts(ref string str, int stop = 100)
        {
            List<int> result = new List<int>();

            for (int k = 0; k < 100; k++)
            {
                if (str.Count() > 0)
                {
                    result.Add(int.Parse(ParseUntilNextComma(ref str)));

                    if (result.Count == stop)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            return result;

        }

        public Vector2 FindEndPointForGameObject(Color[,] colors, Color color, int i, int j, string direction)
        {
            Vector2 result = new Vector2();

            bool foundEndPoint = false;

            if (direction == "v")
            {

                if (j > 0)
                {
                    for (int k = j - 1; k >= 0; k--)
                    {
                        if (colors[i, k] == color)
                        {
                            result.X = tileSize * i;
                            result.Y = tileSize * k;
                            foundEndPoint = true;
                            break;
                        }
                    }
                }

                if (!foundEndPoint)
                {
                    for (int k = j + 1; k < colors.GetLength(1); k++)
                    {
                        if (colors[i, k] == color)
                        {
                            result.X = tileSize * i;
                            result.Y = tileSize * k;
                            foundEndPoint = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                if (i > 0)
                {
                    for (int k = i - 1; k >= 0; k--)
                    {
                        if (colors[k, j] == color)
                        {
                            result.X = tileSize * k;
                            result.Y = tileSize * j;
                            foundEndPoint = true;
                            break;
                        }
                    }
                }

                if (!foundEndPoint)
                {
                    for (int k = i + 1; k < colors.GetLength(0); k++)
                    {
                        if (colors[k, j] == color)
                        {
                            result.X = tileSize * k;
                            result.Y = tileSize * j;
                            break;
                        }
                    }
                }



            }

            return result;

        }




        //public Vector2 FindEndPointForGameObject(Color[,] colors, Color color, int i, int j, int direction)
        //{
        //    Vector2 result = new Vector2();

        //    bool foundEndPoint = false;

        //    if (direction == 0)
        //    {

        //        if (j > 0)
        //        {
        //            for (int k = j - 1; k >= 0; k--)
        //            {
        //                if (colors[i, k] == color)
        //                {
        //                    result.X = tileSize * i;
        //                    result.Y = tileSize * k;
        //                    foundEndPoint = true;
        //                    break;
        //                }
        //            }
        //        }

        //        if (!foundEndPoint)
        //        {
        //            for (int k = j + 1; k < colors.GetLength(1); k++)
        //            {
        //                if (colors[i, k] == color)
        //                {
        //                    result.X = tileSize * i;
        //                    result.Y = tileSize * k;
        //                    foundEndPoint = true;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (i > 0)
        //        {
        //            for (int k = i - 1; k >= 0; k--)
        //            {
        //                if (colors[k, j] == color)
        //                {
        //                    result.X = tileSize * k;
        //                    result.Y = tileSize * j;
        //                    foundEndPoint = true;
        //                    break;
        //                }
        //            }
        //        }

        //        if (!foundEndPoint)
        //        {
        //            for (int k = i + 1; k < colors.GetLength(0); k++)
        //            {
        //                if (colors[k, j] == color)
        //                {
        //                    result.X = tileSize * k;
        //                    result.Y = tileSize * j;
        //                    break;
        //                }
        //            }
        //        }



        //    }

        //    return result;

        //}

        public void SetColorToZero(ref Color color)
        {
            color.R = 0;
            color.G = 0;
            color.B = 0;
            color.A = 0;
        }

        public void SetRectangularColorRegionToZero(ref Color[,] colors, int i, int j, int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    SetColorToZero(ref colors[i + x, j + y]);
                }
            }
        }


    }
}
