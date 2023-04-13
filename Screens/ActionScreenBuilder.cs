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


        public ColliderManager colliderManager;
        public InputManager inputManager;
        public ScreenManager screenManager;
        public AssetManager assetManager;
        public SoundManager soundManager;
        public Player player;


        public int screenWidth;
        public int screenHeight;

        //public List<GameObject> gameObjects = new List<GameObject>();

        public AnimatedGameObject[,] backgroundObjects;
        public GameObject[,] gameObjects;


        List<List<GameObject>> noteTriggerData = new List<List<GameObject>>();
        List<List<GameObject>> movingPlatformAttachmentsData = new List<List<GameObject>>();


        public List<GameObject> gameObjectsAsList = new List<GameObject>();

        public Color color_MovingPlatformPreMultiplyAlpha;
        public Color color_BeamPreMultiplyAlpha;

        public Color color_Add4Y = new Color(255, 255, 156, 255);


        public Color color_NoteTrigger1 = new Color(255, 0, 255, 255);
        public Color color_NoteTrigger2 = new Color(225, 0, 225, 255);
        public Color color_NoteTrigger3 = new Color(200, 0, 225, 255);

        public Color color_MovingPlatformAttached1 = new Color(50, 0, 255, 255);
        public Color color_MovingPlatformAttached2 = new Color(50, 0, 225, 255);
        public Color color_MovingPlatformAttached3 = new Color(50, 0, 200, 255);



        public ActionScreenBuilder(string filename, AssetManager assetManager, ColliderManager colliderManager, InputManager inputManager, ScreenManager screenManager, SoundManager soundManager, Player player, int screenWidth = 40, int screenHeight = 23)
        {
            this.filename = filename;
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;
            this.assetManager = assetManager;
            this.colliderManager = colliderManager;
            this.inputManager = inputManager;
            this.screenManager = screenManager;
            this.player = player;
            this.soundManager = soundManager;



            backgroundObjects = new AnimatedGameObject[screenWidth, screenHeight];
            gameObjects = new GameObject[screenWidth, screenHeight];

            backgroundDictionary.Add(new Color(199, 245, 255, 255), "Tile_air");
            backgroundDictionary.Add(new Color(75, 105, 47, 255), "Tile_grass");
            backgroundDictionary.Add(new Color(102, 57, 49, 255), "Tile_ground");

            gameObjectDictionary.Add(new Color(255, 157, 4, 255), "Door"); // Gold
            gameObjectDictionary.Add(new Color(255, 255, 255, 255), "Spike"); // White
            gameObjectDictionary.Add(new Color(41, 20, 52, 255), "movingPlatform1"); // Very dark purple
            gameObjectDictionary.Add(new Color(72, 37, 91, 255), "movingPlatformHalfLoop"); // Dark purple
            gameObjectDictionary.Add(new Color(104, 104, 104, 255), "LaunchPad"); // Grey
            gameObjectDictionary.Add(new Color(192, 0, 0, 255), "CKeyRound"); // Reds ...
            gameObjectDictionary.Add(new Color(240, 0, 0, 255), "FKeyRound");
            gameObjectDictionary.Add(new Color(155, 173, 183, 255), "Beam"); // Light grey

            //color_MovingPlatformPreMultiplyAlpha = Color.FromNonPremultiplied(color_MovingPlatformAlpha.ToVector4());
            //color_MovingPlatformPreMultiplyAlpha = Color.FromNonPremultiplied(gameObjectDictionary[])
            color_MovingPlatformPreMultiplyAlpha = Color.FromNonPremultiplied(41, 20, 52, 150);
            color_BeamPreMultiplyAlpha = Color.FromNonPremultiplied(155,173,183,150);


            noteTriggerData.Add(new List<GameObject>());
            noteTriggerData.Add(new List<GameObject>());
            noteTriggerData.Add(new List<GameObject>());
            movingPlatformAttachmentsData.Add(new List<GameObject>());
            movingPlatformAttachmentsData.Add(new List<GameObject>());
            movingPlatformAttachmentsData.Add(new List<GameObject>());


        }

        public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            // Load in aseprite file
            asepriteFile = contentManager.Load<AsepriteFile>(filename);

            // From the frame we obtain Cel information
            ReadOnlySpan<AsepriteCel> asepriteCels = asepriteFile.GetFrame(0).Cels;

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
                else if (layer.Name == "AdjustByFour")
                {
                    HandleAdjustByFourLayer(layer, data[layer]);
                }
                else if (layer.Name.Contains("NoteTriggers"))
                {
                    HandleNoteTriggerLayer(layer, data[layer]);
                }
                else if (layer.Name.Contains("PlatformAttachments"))
                {
                    HandleMovingPlatformAttachmentsLayer(layer, data[layer]);
                }
            }

            FormAttachments<Note>(noteTriggerData);
            FormAttachments<MovingPlatform>(movingPlatformAttachmentsData);


            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject != null)
                {
                    gameObjectsAsList.Add(gameObject);
                }
            }


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
                        backgroundObjects[i, j] = new AnimatedGameObject(new Vector2(8 * i, 8 * j), backgroundDictionary[colors[i, j]], assetManager);
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
                        Vector2 position = new Vector2(8 * i, 8 * j);

                        if (gameObjectDictionary[colors[i, j]] == "Door")
                        {
                            int[] ints = ParseDoorString(ref info);
                            gameObjects[i, j] = new Door(position, "Door", ints[0], ints[1], assetManager, colliderManager, inputManager, screenManager, player);
                        }
                        else if (gameObjectDictionary[colors[i, j]] == "Spike")
                        {
                            gameObjects[i, j] = new Spike(position, "Spike", assetManager, colliderManager, player);
                        }
                        else if (gameObjectDictionary[colors[i, j]] == "movingPlatform1")
                        {
                            int[] ints = ParseMovingPlatformString(ref info);
                            Vector2 endPosition = FindEndPointForGameObject(colors, color_MovingPlatformPreMultiplyAlpha, i, j, ints[0]);
                            gameObjects[i, j] = new MovingPlatform(position, "movingPlatform1", endPosition, ints[1], ints[2], assetManager, colliderManager, player, ints[3], null);
                        }
                        else if (gameObjectDictionary[colors[i, j]] == "movingPlatformHalfLoop")
                        {
                            int[] ints = ParseMovingPlatformString(ref info);
                            Vector2 endPosition = FindEndPointForGameObject(colors, color_MovingPlatformPreMultiplyAlpha, i, j, ints[0]);
                            gameObjects[i, j] = new MovingPlatformHalfLoop(position, "movingPlatform1", endPosition, ints[1], ints[2], assetManager, colliderManager, player, ints[3], null);
                        }
                        else if (gameObjectDictionary[colors[i, j]] == "FKeyRound")
                        {
                            gameObjects[i, j] = new Note(position, "FKeyRound", "F", assetManager, colliderManager, inputManager, soundManager, player);
                        }
                        else if (gameObjectDictionary[colors[i, j]] == "CKeyRound")
                        {
                            gameObjects[i, j] = new Note(position, "CKeyRound", "C", assetManager, colliderManager, inputManager, soundManager, player);
                        }
                        else if (gameObjectDictionary[colors[i, j]] == "LaunchPad")
                        {
                            gameObjects[i, j] = new LaunchPad(position, "LaunchPad", assetManager, colliderManager, player);
                        }
                        else if (gameObjectDictionary[colors[i,j]] == "Beam")
                        {
                            int[] ints = ParseBeamString(ref info);
                            Vector2 endPosition = FindEndPointForGameObject(colors, color_BeamPreMultiplyAlpha, i, j, ints[0]);
                            gameObjects[i, j] = new Beam(position, endPosition, assetManager, colliderManager, screenManager);
                        }
                    }

                }

            }

        }


        public void HandleAdjustByFourLayer(AsepriteLayer layer, Color[,] colors)
        {
            for (int i = 0; i < colors.GetLength(0); i++)
            {
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    if (colors[i, j] == color_Add4Y)
                    {
                        if (gameObjects[i, j] is AnimatedGameObject test)
                        {
                            test.position.Y += 4;
                        }
                    }

                }
            }
        }

        // This is a generic method - we feed in a parameter Type T and behaviour can alter depending on the value of T
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
                                gameObject.attachedGameObjects.Add(gameObject1);
                            }
                        }
                    }
                }
            }

        }
        

        public void HandleNoteTriggerLayer(AsepriteLayer layer, Color[,] colors)
        {

            for (int i = 0; i < colors.GetLength(0); i++)
            {
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    if (colors[i, j] == color_NoteTrigger1)
                    {
                        noteTriggerData[0].Add(gameObjects[i, j]);
                    }
                    else if (colors[i, j] == color_NoteTrigger2)
                    {
                        noteTriggerData[1].Add(gameObjects[i, j]);
                    }
                    else if (colors[i, j] == color_NoteTrigger3)
                    {
                        noteTriggerData[2].Add(gameObjects[i, j]);
                    }
                }
            }

           
        }

        public void HandleMovingPlatformAttachmentsLayer(AsepriteLayer layer, Color[,] colors)
        {

            for (int i = 0; i < colors.GetLength(0); i++)
            {
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    if (colors[i, j] == color_MovingPlatformAttached1)
                    {
                        movingPlatformAttachmentsData[0].Add(gameObjects[i, j]);
                    }
                    else if (colors[i, j] == color_MovingPlatformAttached2)
                    {
                        movingPlatformAttachmentsData[1].Add(gameObjects[i, j]);
                    }
                    else if (colors[i, j] == color_MovingPlatformAttached3)
                    {
                        movingPlatformAttachmentsData[2].Add(gameObjects[i, j]);
                    }
                }
            }


        }


        public void ParseUntilNextComma(ref string stringToAdd, ref string str)
        {
            stringToAdd += str[0];

            for (int i = 1; i <= 2; i++)
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
        }

        // We pass in a string by reference as we want the original input to be modified 
        // This is important as when we loop over on to other doors on the screen we want the string to change
        public int[] ParseDoorString(ref string door)
        {

            string screenNumber = "";
            string doorNumber = "";

            ParseUntilNextComma(ref screenNumber, ref door);
            ParseUntilNextComma(ref doorNumber, ref door);
           
            int[] result = new int[2];
            result[0] = int.Parse(screenNumber);
            result[1] = int.Parse(doorNumber);

            return result;




        }

        // Issue: want to return floats for speed NOT ints
        public int[] ParseMovingPlatformString(ref string platform)
        {
            string direction = "";
            string timeStationaryAtEndPoints = "";
            string speed = "";
            string delay = "";

            direction += platform[0];
            platform = platform.Remove(0, 2);

            ParseUntilNextComma(ref timeStationaryAtEndPoints, ref platform);
            ParseUntilNextComma(ref speed, ref platform);
            ParseUntilNextComma(ref delay, ref platform);


            int[] result = new int[4];
            result[0] = int.Parse(direction);
            result[1] = int.Parse(timeStationaryAtEndPoints);
            result[2] = int.Parse(speed);
            result[3] = int.Parse(delay);

            return result;


        }

        public int[] ParseBeamString(ref string beam)
        {
            int[] result = new int[1];
            result[0] = int.Parse(beam[0].ToString());
            return result;
        }

       

        public Vector2 FindEndPointForGameObject(Color[,] colors, Color color, int i, int j, int direction)
        {
            Vector2 result = new Vector2();

            bool foundEndPoint = false;

            if (direction == 0)
            {

                if (j > 0)
                {
                    for (int k = j - 1; k >= 0; k--)
                    {
                        if (colors[i, k] == color)
                        {
                            result.X = 8 * i;
                            result.Y = 8 * k;
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
                            result.X = 8 * i;
                            result.Y = 8 * k;
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
                            result.X = 8 * k;
                            result.Y = 8 * j;
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
                            result.X = 8 * k;
                            result.Y = 8 * j;
                            break;
                        }
                    }
                }



            }

            return result;

        }
    }
}
