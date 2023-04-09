using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Adventure
{
    public class ActionScreen : GameScreen
    {
        public Player player;
        public ColliderManager colliderManager = new ColliderManager();
        // It is important, for the draw order, that we add the tiles FIRST - so that these are drawn in the background
        public Tileset tileset;
        public AnimatedGameObject[,] arrayofTiles;

        public AnimatedGameObject[,] backgroundObjects;

        ActionScreenBuilder screenBuilder;


        public KeyboardState keyboardState;
        public KeyboardState oldKeyboardState;



        public ActionScreen(SpriteBatch spriteBatch, SpriteFont spriteFont, Player player, List<GameObject> gameObjects, Tileset tileset, KeyboardState keyboard, KeyboardState oldKeyboard) : base(spriteBatch)
        {
            this.player = player;
            this.screenGameObjectsToLoadIn = gameObjects;
            this.tileset = tileset;
            this.keyboardState = keyboard;
            this.oldKeyboardState = oldKeyboard;
            arrayofTiles = new AnimatedGameObject[tileset.rows, tileset.columns];
            ScreenWidth = 8 * tileset.columns;
            ScreenHeight = 8 * tileset.rows;
        }
        public ActionScreen(SpriteBatch spriteBatch, SpriteFont spriteFont, Player player, List<GameObject> gameObjects, KeyboardState keyboard, KeyboardState oldKeyboard) : base(spriteBatch)
        {
            this.player = player;
            this.screenGameObjectsToLoadIn = gameObjects;
            this.keyboardState = keyboard;
            this.oldKeyboardState = oldKeyboard;
            ScreenWidth = 8 * 40;
            ScreenHeight = 8 * 23;
        }

        public ActionScreen(SpriteBatch spriteBatch, SpriteFont spriteFont, Player player, List<GameObject> gameObjects, AnimatedGameObject[,] backgroundObjects, KeyboardState keyboard, KeyboardState oldKeyboard) : base(spriteBatch)
        {
            this.player = player;
            this.screenGameObjectsToLoadIn = gameObjects;
            this.backgroundObjects = backgroundObjects;
            this.keyboardState = keyboard;
            this.oldKeyboardState = oldKeyboard;
            ScreenWidth = 8 * 40;
            ScreenHeight = 8 * 23;
        }

        //public ActionScreen(SpriteBatch spriteBatch, SpriteFont spriteFont, Player player, string screenBuilderFilename, KeyboardState keyboard, KeyboardState oldKeyboard) : base(spriteBatch)
        //{
        //    this.player = player;

        //    screenBuilder = new ActionScreenBuilder(screenBuilderFilename, colliderManager, inputManager, this, player);
        //    level1.LoadContent(References.content, References.graphicsDevice);

        //    this.keyboardState = keyboard;
        //    this.oldKeyboardState = oldKeyboard;
        //    ScreenWidth = 8 * 40;
        //    ScreenHeight = 8 * 23;
        //}



        public override void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            //Debug.WriteLine("here");

            // Build tile sprites and add these to screenSprites FIRST so they are drawn first i.e. in the background
            //BuildTileSet();
            //BuildHitboxSet();

            BuildBackground();
            BuildBackgroundHitboxSet();


            if (screenGameObjectsToLoadIn != null)
            {
                foreach (GameObject gameObject in screenGameObjectsToLoadIn)
                {
                    gameObject.LoadContent(References.content, References.graphicsDevice);
                    screenGameObjects.Add(gameObject);

                    if (gameObject is AnimatedGameObject sprite)
                    {
                        if (sprite.climable)
                        {
                            screenClimables.Add(sprite);
                        }

                        if (sprite.CollisionObject)
                        {
                            hitboxesToCheckCollisionsWith.Add(sprite.idleHitbox);
                        }
                    }

                    if (gameObject is Spike spike)
                    {
                        screenSpikes.Add(spike);
                        screenHazards.Add(spike);
                    }

                    if (gameObject is Door door)
                    {
                        screenDoors.Add(door);
                    }




                    //if (gameObject is FlashingBeam beam)
                    //{
                    //    screenBeams.Add(beam);
                    //    screenHazards.Add(beam);
                    //    hitboxesToCheckCollisionsWith.Add(beam.startHitbox);
                    //    hitboxesToCheckCollisionsWith.Add(beam.endHitbox);
                    //}

                    if (gameObject is Beam beam)
                    {
                        screenBeams.Add(beam);
                        screenHazards.Add(beam);
                        hitboxesToCheckCollisionsWith.Add(beam.startHitbox);
                        hitboxesToCheckCollisionsWith.Add(beam.endHitbox);
                    }

                    if (gameObject is BouncingOrb orb)
                    {
                        screenBouncingOrbs.Add(orb);
                        screenHazards.Add(orb);

                    }

                    if (gameObject is MovingPlatform platform)
                    {

                        screenMovingPlatforms.Add(platform);
                        hitboxesForAimLine.Add(platform.idleHitbox);

                    }

                    if (gameObject is HookPoint hookPoint)
                    {
                        screenHookPoints.Add(hookPoint);
                    }

                    if (gameObject is Note note)
                    {
                        screenNotes.Add(note);
                    }

                    if (gameObject is Gate gate)
                    {
                        hitboxesForAimLine.Add(gate.idleHitbox);
                        screenGates.Add(gate);
                    }

                    if (gameObject is NoteShip noteShip)
                    {
                        screenNoteShip = noteShip;
                    }

                    if (gameObject is NoteAndGateAndOrbPuzzle noteAndGateAndOrbPuzzle)
                    {
                        noteAndGateAndOrbPuzzles.Add(noteAndGateAndOrbPuzzle);

                    }

                    if (gameObject is DiagonalOrbsPattern1 diagonalOrbs)
                    {
                        foreach (MusicalMovingPlatformNoLoop diagonalOrb in diagonalOrbs.orbs)
                        {
                            screenHazards.Add(diagonalOrb);
                        }
                    }

                    if (gameObject is DiagonalOrbsPattern2 diagonalOrbs2)
                    {
                        foreach (MusicalMovingPlatformNoLoop diagonalOrb in diagonalOrbs2.orbs)
                        {
                            screenHazards.Add(diagonalOrb);
                        }
                    }

                    if (gameObject is NoteAndGatePuzzle noteAndGatePuzzle)
                    {
                        noteAndGatePuzzles.Add(noteAndGatePuzzle);
                    }

                    if (gameObject is HangingRopeWithWeightAttached rope)
                    {

                        foreach (List<HitboxRectangle> hitboxes in rope.weight.hitboxesForGunlineForEachFrame)
                        {
                            foreach (HitboxRectangle hitbox in hitboxes)
                            {
                                hitboxesForAimLine.Add(hitbox);

                            }
                        }
                    }




                }

            }

            if (screenGameObjectsToLoadIn != null)
            {
                screenGameObjectsToLoadIn.Clear();
            }

            // We add the player to screenGameObjects LAST
            screenGameObjects.Add(player);
            // We add this list of screen game objects in the references class (i.e. the "global" list)




        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }






        public void BuildTileSet()
        {
            for (int i = 0; i < tileset.arrayofTileSetSpriteFilenames.GetLength(0); i++)
            {

                for (int j = 0; j < tileset.arrayofTileSetSpriteFilenames.GetLength(1); j++)
                {

                    AnimatedGameObject tile = new AnimatedGameObject(new Vector2(8 * j, 8 * i), tileset.arrayofTileSetSpriteFilenames[i, j]);

                    tile.LoadContent(References.content, References.graphicsDevice);

                    if (tile.filename == "Tile_air")
                    {
                        tile.idleHitbox.isActive = false;
                    }
                    else
                    {
                        tile.idleHitbox.isActive = true;
                    }
                    arrayofTiles[i, j] = tile;
                    gameObjectsDrawOnly.Add(tile);
                }
            }
        }



        public void BuildHitboxSet()
        {

            for (int i = 0; i < arrayofTiles.GetLength(1); i++)
            {
                for (int j = 0; j < arrayofTiles.GetLength(0); j++)
                {

                    bool GotHeight = false;

                    if (arrayofTiles[j, i].idleHitbox.isActive)
                    {
                        HitboxRectangle newHitbox = new HitboxRectangle(0, 0, 0, 0);
                        newHitbox.rectangle.X = arrayofTiles[j, i].idleHitbox.rectangle.X;
                        newHitbox.rectangle.Y = arrayofTiles[j, i].idleHitbox.rectangle.Y;


                        // Get correct width
                        if (i < arrayofTiles.GetLength(1) - 1)
                        {
                            for (int k = i + 1; k < arrayofTiles.GetLength(1); k++)
                            {
                                if (!arrayofTiles[j, k].idleHitbox.isActive)
                                {
                                    newHitbox.rectangle.Width = (k - i) * 8;
                                    break;
                                }

                                if (k == arrayofTiles.GetLength(1) - 1 && arrayofTiles[j, k].idleHitbox.isActive)
                                {
                                    newHitbox.rectangle.Width = (k - i + 1) * 8;
                                    break;
                                }

                            }
                        }
                        else
                        {
                            newHitbox.rectangle.Width = 1;
                        }


                        // Get correct height
                        if (j < arrayofTiles.GetLength(0) - 1)
                        {
                            for (int h = j + 1; h < arrayofTiles.GetLength(0); h++)
                            {
                                for (int l = i; l < i + (newHitbox.rectangle.Width) / 8; l++)
                                {
                                    if (!arrayofTiles[h, l].idleHitbox.isActive)
                                    {
                                        newHitbox.rectangle.Height = (h - j) * 8;
                                        GotHeight = true;
                                        break;
                                    }
                                }

                                if (GotHeight)
                                {
                                    break;
                                }

                                // I only reach this point if everything is fine
                                if (h == arrayofTiles.GetLength(0) - 1)
                                {
                                    newHitbox.rectangle.Height = (h - j + 1) * 8;
                                }

                            }

                        }
                        else
                        {
                            newHitbox.rectangle.Height = 1;
                        }


                        for (int j2 = j; j2 < j + newHitbox.rectangle.Height / 8; j2++)
                        {
                            for (int i2 = i; i2 < i + newHitbox.rectangle.Width / 8; i2++)
                            {
                                arrayofTiles[j2, i2].idleHitbox.isActive = false;
                            }
                        }


                        newHitbox.isActive = true;
                        hitboxesToCheckCollisionsWith.Add(newHitbox);
                        hitboxesForAimLine.Add(newHitbox);

                    }
                }


            }
        }

        public void BuildBackground()
        {
            for (int i = 0; i < backgroundObjects.GetLength(0); i++)
            {

                for (int j = 0; j < backgroundObjects.GetLength(1); j++)
                {

                    backgroundObjects[i, j].LoadContent(References.content, References.graphicsDevice);

                    if (backgroundObjects[i, j].filename == "Tile_air")
                    {
                        backgroundObjects[i, j].idleHitbox.isActive = false;
                    }
                    else
                    {
                        backgroundObjects[i, j].idleHitbox.isActive = true;
                    }

                    gameObjectsDrawOnly.Add(backgroundObjects[i, j]);
                }
            }
        }

        public void BuildBackgroundHitboxSet()
        {

            for (int i = 0; i < backgroundObjects.GetLength(0); i++)
            {
                for (int j = 0; j < backgroundObjects.GetLength(1); j++)
                {

                    bool GotHeight = false;

                    if (backgroundObjects[i, j].idleHitbox.isActive)
                    {
                        HitboxRectangle newHitbox = new HitboxRectangle(0, 0, 0, 0);
                        newHitbox.rectangle.X = backgroundObjects[i, j].idleHitbox.rectangle.X;
                        newHitbox.rectangle.Y = backgroundObjects[i, j].idleHitbox.rectangle.Y;


                        // Get correct width
                        if (i < backgroundObjects.GetLength(0) - 1)
                        {
                            for (int k = i + 1; k < backgroundObjects.GetLength(0); k++)
                            {
                                if (!backgroundObjects[k, j].idleHitbox.isActive)
                                {
                                    newHitbox.rectangle.Width = (k - i) * 8;
                                    break;
                                }

                                if (k == backgroundObjects.GetLength(1) - 1 && backgroundObjects[k, j].idleHitbox.isActive)
                                {
                                    newHitbox.rectangle.Width = (k - i + 1) * 8;
                                    break;
                                }

                            }
                        }
                        else
                        {
                            newHitbox.rectangle.Width = 1;
                        }


                        // Get correct height
                        if (j < backgroundObjects.GetLength(1) - 1)
                        {
                            for (int h = j + 1; h < backgroundObjects.GetLength(1); h++)
                            {
                                for (int l = i; l < i + newHitbox.rectangle.Width / 8; l++)
                                {
                                    if (!backgroundObjects[l, h].idleHitbox.isActive)
                                    {
                                        newHitbox.rectangle.Height = (h - j) * 8;
                                        GotHeight = true;
                                        break;
                                    }
                                }

                                if (GotHeight)
                                {
                                    break;
                                }

                                // I only reach this point if everything is fine
                                if (h == backgroundObjects.GetLength(1) - 1)
                                {
                                    newHitbox.rectangle.Height = (h - j + 1) * 8;
                                }

                            }

                        }
                        else
                        {
                            newHitbox.rectangle.Height = 1;
                        }

                        for (int i2 = i; i2 < i + newHitbox.rectangle.Width / 8; i2++)
                        {
                            for (int j2 = j; j2 < j + newHitbox.rectangle.Height / 8; j2++)
                            {
                                backgroundObjects[i2, j2].idleHitbox.isActive = false;
                            }
                        }




                        newHitbox.isActive = true;
                        hitboxesToCheckCollisionsWith.Add(newHitbox);
                        hitboxesForAimLine.Add(newHitbox);

                    }
                }


            }
        }

       


    }
}
