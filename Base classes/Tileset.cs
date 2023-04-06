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
    public class Tileset
    {
        public AnimatedGameObject spriteMapOfTiles;
        public Color[,] colorMapOfSpriteSheet;

        public AnimatedGameObject spriteMapOfMarkers;
        public Color[,] colorMapOfSpriteSheetMarkers;

        public List<GameObject> screenGameObjects = new List<GameObject>();
        public List<Note> screenNotes = new List<Note>();
        public List<GameObject> screenTriggerables = new List<GameObject>();

        // THESE WILL HAVE SAME LENGTH
        public string tilesetString;
        public List<int> tilesetCounter;


        public int counter = 0;
        public string[,] arrayofTileSetSpriteFilenames;
        public int rows;
        public int columns;

        // This will correspond to the tile allocated to that 8x8 space on the screen

        // Each screen will consist of 40 x 23 tile sprites of size 8 x 8 (technically 40 x 22.5)
        // We will encode as a string list according to the following code:
        // A = Tile_air 
        // B = Tile_grass 
        // C = Tile_ground


        // Default is to create an array of 23 rows and 40 columns
        public Tileset(string str, List<int> ints)
        {
            rows = 23;
            columns = 40;
            arrayofTileSetSpriteFilenames = new string[23, 40];
            tilesetString = str;
            tilesetCounter = ints;
            BuildArrayOfTileSpriteFilenames();
        }

        public Tileset(string str, List<int> ints, int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            arrayofTileSetSpriteFilenames = new string[rows, columns];
            tilesetString = str;
            tilesetCounter = ints;
            BuildArrayOfTileSpriteFilenames();
        }


        public Tileset(string levelString)
        {
            spriteMapOfTiles = new AnimatedGameObject(levelString);
            spriteMapOfTiles.LoadContent(References.content, References.graphicsDevice);
            BuildArrayOfTileSpriteFilenamesFromAsepriteFile();

        }

        public Tileset(string levelString, string markerString)
        {
            spriteMapOfTiles = new AnimatedGameObject(levelString);
            spriteMapOfTiles.LoadContent(References.content, References.graphicsDevice);
            spriteMapOfMarkers = new AnimatedGameObject(markerString);
            spriteMapOfMarkers.LoadContent(References.content, References.graphicsDevice);
            BuildArrayOfTileSpriteFilenamesFromAsepriteFile();
            BuildArrayOfGameObjectFilenamesFromAsepriteFile();

        }


        public void BuildArrayOfTileSpriteFilenames()
        {

            for (int i = 0; i < tilesetString.Length; i++)
            {
                for (int j = counter; j < counter + tilesetCounter[i]; j++)
                {
                    if (tilesetString[i] == 'A')
                    {
                        arrayofTileSetSpriteFilenames[(j - j % columns) / columns, j % columns] = "Tile_air";
                    }
                    else if (tilesetString[i] == 'B')
                    {
                        arrayofTileSetSpriteFilenames[(j - j % columns) / columns, j % columns] = "Tile_grass";
                    }
                    else if (tilesetString[i] == 'C')
                    {
                        arrayofTileSetSpriteFilenames[(j - j % columns) / columns, j % columns] = "Tile_ground";
                    }

                }

                counter += tilesetCounter[i];
            }

        }


        public void BuildArrayOfTileSpriteFilenamesFromAsepriteFile()
        {
            colorMapOfSpriteSheet = TextureTo2DArrayOfColors(spriteMapOfTiles.texture);
            arrayofTileSetSpriteFilenames = new string[colorMapOfSpriteSheet.GetLength(0), colorMapOfSpriteSheet.GetLength(1)];
            rows = colorMapOfSpriteSheet.GetLength(0);
            columns = colorMapOfSpriteSheet.GetLength(1);

            //Debug.WriteLine(arrayofTileSetSpriteFilenames.GetLength(1));


            for (int i = 0; i < colorMapOfSpriteSheet.GetLength(0); i++)
            {
                for (int j = 0; j < colorMapOfSpriteSheet.GetLength(1); j++)
                {
                    Color color = colorMapOfSpriteSheet[i, j];


                    if (color.R == 199 && color.G == 245 && color.B == 255)
                    {
                        arrayofTileSetSpriteFilenames[i, j] = "Tile_air";
                    }
                    else if (color.R == 75 && color.G == 105 && color.B == 47)
                    {
                        arrayofTileSetSpriteFilenames[i, j] = "Tile_grass";
                    }
                    else if (color.R == 102 && color.G == 57 && color.B == 49)
                    {
                        arrayofTileSetSpriteFilenames[i, j] = "Tile_ground";
                    }


                }
            }



        }


        // The following code takes as input a Texture2D object 
        // and returns a 2-dim array of pixel colours

        public Color[,] TextureTo2DArrayOfColors(Texture2D texture)
        {
            // First we create 1-dim array which can be obtained via the standard GetData method of Texture2D
            Color[] colorsOne = new Color[texture.Width * texture.Height];
            texture.GetData(colorsOne);
            // Now we convert this into an easier 2-dim array
            Color[,] colorsTwo = new Color[texture.Height, texture.Width];

            for (int i = 0; i < texture.Height; i++)
            {
                for (int j = 0; j < texture.Width; j++)
                {
                    colorsTwo[i, j] = colorsOne[j + i * texture.Width];
                }
            }

            return colorsTwo;

        }




        public void BuildArrayOfGameObjectFilenamesFromAsepriteFile()
        {
            colorMapOfSpriteSheetMarkers = TextureTo2DArrayOfColors(spriteMapOfMarkers.texture);

            for (int i = 0; i < colorMapOfSpriteSheetMarkers.GetLength(0); i++)
            {
                for (int j = 0; j < colorMapOfSpriteSheetMarkers.GetLength(1); j++)
                {
                    Color color = colorMapOfSpriteSheetMarkers[i, j];

                    // Check if doors
                    if (color.R == 255 && color.G == 157 && color.B == 4 && color.A == 255)
                    {

                        if (colorMapOfSpriteSheet[i, j + 1].A == 202)
                        {
                            screenGameObjects.Add(new Door(new Vector2(i, j), "Door", 1, 1));
                        }
                        else if (colorMapOfSpriteSheet[i, j + 1].A == 118)
                        {
                            screenGameObjects.Add(new Door(new Vector2(i, j), "Door", 1, 2));
                        }

                        continue;

                    }

                    // Check if spikes
                    if (color.R == 255 && color.G == 255 && color.B == 255 && color.A == 255)
                    {
                        screenGameObjects.Add(new Spike(new Vector2(8 * j, 8 * i), "Spike"));
                        continue;

                    }


                    // Check if notes
                    if (color.R == 160 && color.G == 0 && color.B == 0 && color.A == 255)
                    {
                        screenNotes.Add(new Note(new Vector2(i, j), "AKeyRound", "A", null, "rune_A"));
                        continue;
                    }

                    if (color.R == 41 && color.G == 20 && color.B == 52 && color.A == 255)
                    {
                        Color test = colorMapOfSpriteSheet[i + 1, j];

                        if (test.R == 41 && test.G == 20 && test.B == 52)
                        {
                            MovingPlatformOneLoop plat = new MovingPlatformOneLoop(new Vector2(i, j), "movingPlatform1", new Vector2(i, j - test.A), 0, 1);
                            screenTriggerables.Add(plat);
                        }

                    }


                }
            }


            //foreach (GameObject gameObject in screenTriggerables)
            //{
            //    if (gameObject is MovingPlatformOneLoop plat)
            //    {
            //        Color color = colorMapOfSpriteSheetMarkers[(int)plat.spritePosition.X, (int)plat.spritePosition.Y + 1];

            //        if (color.R == 160)
            //        {
            //            foreach (Note note in screenNotes)
            //            {
            //                if (note.noteValue == "A")
            //                {
            //                    note.movingPlatformsOneLoop.Add(plat);
            //                }
            //            }
            //        }

            //    }
            //}

            screenGameObjects.AddRange(screenTriggerables);
            screenGameObjects.AddRange(screenNotes);

        }








    }
}
