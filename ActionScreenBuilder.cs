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
        public Player player;

        public Color[] colors;

        public int screenWidth;
        public int screenHeight;

        public List<GameObject> gameObjects = new List<GameObject>();

        public AnimatedGameObject[,] backgroundObjects;




        public ActionScreenBuilder(string filename, ColliderManager colliderManager, InputManager inputManager, ScreenManager screenManager, Player player, int screenWidth = 40, int screenHeight = 23)
        {
            this.filename = filename;
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;
            this.colliderManager = colliderManager;
            this.inputManager = inputManager;
            this.screenManager = screenManager;
            this.player = player;

            backgroundColorArray = new Color[screenWidth, screenHeight];
            gameObjectColorArray = new Color[screenWidth, screenHeight];

            backgroundObjects = new AnimatedGameObject[screenWidth, screenHeight]; // It's rows then columns
        }

        public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            // Load in aseprite file and get Layer information
            asepriteFile = contentManager.Load<AsepriteFile>(filename);
            backgroundLayer = asepriteFile.GetLayer(0);
            gameObjectLayer = asepriteFile.GetLayer(1);

            // From the frame we obtain Cel information, which in turn contains pixel data
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


            for (int i = 0; i < backgroundColorArray.GetLength(0); i++)
            {
                for (int j = 0; j < backgroundColorArray.GetLength(1); j++)
                {

                    Vector2 position = new Vector2(8 * i, 8 * j);

                    if (backgroundColorArray[i, j].R == 199 && backgroundColorArray[i, j].G == 245 && backgroundColorArray[i, j].B == 255)
                    {
                        backgroundObjects[i, j] = new AnimatedGameObject(position, "Tile_air");
                    }
                    else if (backgroundColorArray[i, j].R == 75 && backgroundColorArray[i, j].G == 105 && backgroundColorArray[i, j].B == 47)
                    {
                        backgroundObjects[i, j] = new AnimatedGameObject(position, "Tile_grass");
                    }
                    else if (backgroundColorArray[i, j].R == 102 && backgroundColorArray[i, j].G == 57 && backgroundColorArray[i, j].B == 49)
                    {
                        backgroundObjects[i, j] = new AnimatedGameObject(position, "Tile_ground");

                    }
                }

            }

            for (int i = 0; i < gameObjectColorArray.GetLength(0); i++)
            {
                for (int j = 0; j < gameObjectColorArray.GetLength(1); j++)
                {

                    Vector2 position = new Vector2(8 * i, 8 * j);

                    if (gameObjectColorArray[i,j].A == 255)
                    {                       
                        if (gameObjectColorArray[i, j].R == 255 && gameObjectColorArray[i, j].G == 157 && gameObjectColorArray[i, j].B == 4)
                        {
                            gameObjects.Add(new Door(position, "Door", 255 - gameObjectColorArray[i, j+1].A, 255 - gameObjectColorArray[i+1, j+1].A, colliderManager, inputManager, screenManager, player));
                        }
                        else if (gameObjectColorArray[i, j].R == 255 && gameObjectColorArray[i, j].G == 255 && gameObjectColorArray[i, j].B == 255)
                        {
                            gameObjects.Add(new Spike(position, "Spike", colliderManager, player));
                        }
                    }
                }

            }


        }




    }
}
