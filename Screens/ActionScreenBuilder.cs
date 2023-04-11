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

namespace Adventure
{
    public class ActionScreenBuilder
    {
        public string filename;
        public AsepriteFile asepriteFile;
        public Sprite sprite;
        public AsepriteLayer backgroundLayer;
        public AsepriteLayer gameObjectLayer;
        public Color[,] backgroundColorArray;
        public Color[,] gameObjectColorArray;



        public ColliderManager colliderManager;
        public InputManager inputManager;
        public ScreenManager screenManager;
        public AssetManager assetManager;
        public Player player;


        public int screenWidth;
        public int screenHeight;

        public List<GameObject> gameObjects = new List<GameObject>();

        public AnimatedGameObject[,] backgroundObjects;

        public Color color_MovingPlatform = new Color(41, 20, 52, 255);
        public Color color_MovingPlatformAlpha = new Color(41, 20, 52, 150);
        public Color color_MovingPlatformPreMultiplyAlpha;


        public ActionScreenBuilder(string filename, AssetManager assetManager, ColliderManager colliderManager, InputManager inputManager, ScreenManager screenManager, Player player, int screenWidth = 40, int screenHeight = 23)
        {
            this.filename = filename;
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;
            this.assetManager = assetManager;
            this.colliderManager = colliderManager;
            this.inputManager = inputManager;
            this.screenManager = screenManager;
            this.player = player;

            backgroundColorArray = new Color[screenWidth, screenHeight];
            gameObjectColorArray = new Color[screenWidth, screenHeight];

            backgroundObjects = new AnimatedGameObject[screenWidth, screenHeight];


            //color_MovingPlatformPreMultiplyAlpha = Color.FromNonPremultiplied(color_MovingPlatformAlpha.ToVector4());
            color_MovingPlatformPreMultiplyAlpha = Color.FromNonPremultiplied(color_MovingPlatform.R, color_MovingPlatform.G, color_MovingPlatform.B, 150);
        }

        public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            // Load in aseprite file and get Layer information
            asepriteFile = contentManager.Load<AsepriteFile>(filename);
            backgroundLayer = asepriteFile.GetLayer(0);
            gameObjectLayer = asepriteFile.GetLayer(1);
            ReadOnlySpan<AsepriteLayer> layers = asepriteFile.Layers;

            foreach (AsepriteLayer layer in layers )
            {
                Debug.WriteLine(layer.Name);
            }
            //Debug.WriteLine(layers);

            //// From the frame we obtain Cel information, which in turn contains pixel data
            ReadOnlySpan<AsepriteCel> asepriteCels = asepriteFile.GetFrame(0).Cels;
            ReadOnlySpan<Color> backgroundColors = null;
            ReadOnlySpan<Color> gameObjectColors = null;


            // Get pixel data from Cel data
            foreach (AsepriteCel cel in asepriteCels)
            {
                if (cel.Layer == backgroundLayer)
                {
                    AsepriteImageCel imageCel = cel as AsepriteImageCel;
                    backgroundColors = imageCel.Pixels;
                }
                else if (cel.Layer == gameObjectLayer)
                {
                    AsepriteImageCel imageCel = cel as AsepriteImageCel;
                    gameObjectColors = imageCel.Pixels;

                }
            }

            //ReadOnlySpan<AsepriteCel> asepriteCels1 = asepriteFile.GetFrame(0).Cels;
            //ReadOnlySpan<AsepriteCel> asepriteCels2 = asepriteFile.GetFrame(1).Cels;
            //foreach (AsepriteCel cel in asepriteCels1)
            //{
            //    AsepriteImageCel imageCel = cel as AsepriteImageCel;
            //    backgroundColors = imageCel.Pixels;

            //}
            //foreach (AsepriteCel cel in asepriteCels2)
            //{
            //    AsepriteImageCel imageCel = cel as AsepriteImageCel;
            //    gameObjectColors = imageCel.Pixels;
            //}


            //Debug.WriteLine(gameObjectColors[100]);

            // Pixel data is presented as a list, read from right to left then top to bottom from the aseprite file
            // We convert this into a 2D array which is easier to use
            // The i,j component of the array correponds to the pixel data in the (x,y) = (8 * i, 8 * j) co-ordinate on the screen
            for (int i = 0; i < backgroundColors.Length; i++)
            {
                backgroundColorArray[i % screenWidth, (i - (i % screenWidth)) / screenWidth] = backgroundColors[i];
            }

            for (int i = 0; i < gameObjectColors.Length; i++)
            {
                gameObjectColorArray[i % screenWidth, (i - (i % screenWidth)) / screenWidth] = gameObjectColors[i];
            }


            //Debug.WriteLine(gameObjectColors.Length);

            for (int i = 0; i < backgroundColorArray.GetLength(0); i++)
            {
                for (int j = 0; j < backgroundColorArray.GetLength(1); j++)
                {
                   // backgroundColorArray[i, j].R = (byte)Math.Ceiling(backgroundColorArray[i, j].R * (float)255 / backgroundColorArray[i, j].A);
                    //backgroundColorArray[i, j].B = (byte)Math.Ceiling(backgroundColorArray[i, j].B * (float)255 / backgroundColorArray[i, j].A);
                    //backgroundColorArray[i, j].G = (byte)Math.Ceiling(backgroundColorArray[i, j].G * (float)255 / backgroundColorArray[i, j].A);

                    //Debug.WriteLine(backgroundColorArray[i, j]);

                }
            }


            for (int i = 0; i < gameObjectColorArray.GetLength(0); i++)
            {
                for (int j = 0; j < gameObjectColorArray.GetLength(1); j++)
                {
                    //gameObjectColorArray[i, j].R = (byte)Math.Ceiling(gameObjectColorArray[i, j].R * (float)255 / gameObjectColorArray[i, j].A);
                    //gameObjectColorArray[i, j].B = (byte)Math.Ceiling(gameObjectColorArray[i, j].B * (float)255 / gameObjectColorArray[i, j].A);
                   // gameObjectColorArray[i, j].G = (byte)Math.Ceiling(gameObjectColorArray[i, j].G * (float)255 / gameObjectColorArray[i, j].A);

                    //Debug.WriteLine(gameObjectColorArray[i, j]);

                }
            }

            //Debug.WriteLine(gameObjectColorArray[33, 3]);

            for (int i = 0; i < backgroundColorArray.GetLength(0); i++)
            {
                for (int j = 0; j < backgroundColorArray.GetLength(1); j++)
                {

                    Vector2 position = new Vector2(8 * i, 8 * j);

                    if (backgroundColorArray[i, j].R == 199 && backgroundColorArray[i, j].G == 245 && backgroundColorArray[i, j].B == 255)
                    {
                        backgroundObjects[i, j] = new AnimatedGameObject(position, "Tile_air", assetManager);
                    }
                    else if (backgroundColorArray[i, j].R == 75 && backgroundColorArray[i, j].G == 105 && backgroundColorArray[i, j].B == 47)
                    {
                        backgroundObjects[i, j] = new AnimatedGameObject(position, "Tile_grass", assetManager);
                    }
                    else if (backgroundColorArray[i, j].R == 102 && backgroundColorArray[i, j].G == 57 && backgroundColorArray[i, j].B == 49)
                    {
                        backgroundObjects[i, j] = new AnimatedGameObject(position, "Tile_ground", assetManager);

                    }
                }

            }

            for (int i = 0; i < gameObjectColorArray.GetLength(0); i++)
            {
                for (int j = 0; j < gameObjectColorArray.GetLength(1); j++)
                {

                    Vector2 position = new Vector2(8 * i, 8 * j);

                    if (gameObjectColorArray[i, j].A == 255)
                    {
                        if (gameObjectColorArray[i, j].R == 255 && gameObjectColorArray[i, j].G == 157 && gameObjectColorArray[i, j].B == 4)
                        {
                            gameObjects.Add(new Door(position, "Door", 255 - gameObjectColorArray[i, j + 1].A, 255 - gameObjectColorArray[i + 1, j + 1].A, assetManager, colliderManager, inputManager, screenManager, player));
                        }
                        else if (gameObjectColorArray[i, j].R == 255 && gameObjectColorArray[i, j].G == 255 && gameObjectColorArray[i, j].B == 255)
                        {
                            gameObjects.Add(new Spike(position, "Spike", assetManager, colliderManager, player));
                        }
                        else if (gameObjectColorArray[i, j].R == 41 && gameObjectColorArray[i, j].G == 20 && gameObjectColorArray[i, j].B == 52)
                        {
                            //MovingPlatform platform = new MovingPlatform(position, "movingPlatform1", assetManager);
                            //platform.player = player;
                            //platform.colliderManager = colliderManager;
                            Vector2 endPosition = new Vector2();
                            bool foundEndPoint = false;

                            if (j > 0)
                            {
                                for (int k = j - 1; k >= 0; k--)
                                {
                                    Debug.WriteLine(gameObjectColorArray[i, k]);
                                    if (gameObjectColorArray[i, k] == color_MovingPlatformPreMultiplyAlpha)
                                    {
                                        Debug.WriteLine("here");
                                        endPosition.X = 8 * i;
                                        endPosition.Y = 8 * k;
                                        foundEndPoint = true;
                                        break;
                                    }
                                }
                            }

                            if (!foundEndPoint)
                            {
                                for (int k = j + 1; k < gameObjectColorArray.GetLength(1); k++)
                                {
                                    if (gameObjectColorArray[i, k].R == 41 && gameObjectColorArray[i, k].G == 20 && gameObjectColorArray[i, k].B == 52 && gameObjectColorArray[i, k].A == 150)
                                    {
                                        endPosition.X = 8 * i;
                                        endPosition.Y = 8 * k;
                                        foundEndPoint = true;
                                        break;
                                    }
                                }
                            }

                            //Debug.WriteLine(platform.endPosition);

                            gameObjects.Add(new MovingPlatform(position, "movingPlatform1", endPosition, 254 - gameObjectColorArray[i + 1, j].A, 255 - gameObjectColorArray[i + 2, j].A, assetManager, colliderManager, player, 254 - gameObjectColorArray[i + 3, j].A, null));


                           

                        }
                    }
                }

            }


        }




    }
}
