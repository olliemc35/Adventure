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


        public override void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            //Debug.WriteLine("here");

            // Build tile sprites and add these to screenSprites FIRST so they are drawn first i.e. in the background
            BuildTileSet();
            BuildHitboxSet();

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

                    if (gameObject is Ladder ladder)
                    {
                        screenLadders.Add(ladder);
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

                    if (gameObject is Teleport teleport)
                    {
                        screenTeleports.Add(teleport);
                    }


                }

            }

            screenGameObjectsToLoadIn.Clear();

            // We add the player to screenGameObjects LAST
            screenGameObjects.Add(player);
            // We add this list of screen game objects in the references class (i.e. the "global" list)




        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            keyboardState = Keyboard.GetState();

            if (screenDoors.Count > 0)
            {
                foreach (Door door in screenDoors)
                {

                    if (colliderManager.CheckForCollision(player.idleHitbox, door.idleHitbox) && CheckKey(Keys.Up))
                    {
                        ChangeScreen = true;
                        References.PreviousScreenNumber = screenNumber;
                        References.ScreenNumber = door.ScreenNumberToMoveTo;
                        References.DoorNumberToMoveTo = door.DoorNumberToMoveTo;
                    }

                }
            }


            // Important that this comes BEFORE the creation of the bomb below
            // as otherwise the bomb will be planted and destroyed on the same frame

            if (screenBombs.Count > 0)
            {
                if (oldKeyboardState.IsKeyUp(Keys.B) && keyboardState.IsKeyDown(Keys.B))
                {
                    screenBombs[0].detonate = true;
                    screenBombs[0].attachedNote.flagPlayerInteractedWith = true;

                }

                if (screenBombs[0].readyToRemove)
                {
                    player.bombPlanted = false;
                    screenGameObjectsToRemove.Add(screenBombs[0]);
                    screenBombsToRemove.Add(screenBombs[0]);

                }
            }

            if (screenNotes.Count > 0)
            {
                foreach (Note note in screenNotes)
                {
                    if (note.key != null)
                    {

                        // What to do if I press UP (or E) next to a note
                        if (colliderManager.CheckForCollision(player.idleHitbox, note.key.idleHitbox) && (oldKeyboardState.IsKeyUp(Keys.E) && keyboardState.IsKeyDown(Keys.E) || oldKeyboardState.IsKeyUp(Keys.Up) && keyboardState.IsKeyDown(Keys.Up)) && !note.playerInteractedWith)
                        {
                            if (note.ribbonAttached)
                            {
                                if (screenRibbons.Count > 0)
                                {
                                    foreach (Ribbon ribbon in screenRibbons)
                                    {
                                        if (ribbon.listOfNotes.Contains(note))
                                        {
                                            ribbon.playNotes = true;
                                            ribbon.indexOfNoteToPlay = ribbon.listOfNotes.IndexOf(note);

                                        }
                                    }
                                }
                            }
                            else
                            {
                                note.flagPlayerInteractedWith = true;

                            }
                        }

                        // THIS SHOULD REALLY BE player.RibbonButtonFlag or something
                        // What to do if I press R next to a note (i.e. ribbon)
                        if (colliderManager.CheckForCollision(player.idleHitbox, note.key.idleHitbox) && (oldKeyboardState.IsKeyUp(Keys.R) && keyboardState.IsKeyDown(Keys.R)))
                        {
                            if (!player.ribbonInHand)
                            {
                                Ribbon ribbon = new Ribbon(player, player.position);
                                ribbon.FixRibbonToNote(note);
                                ribbon.LoadContent(References.content, References.graphicsDevice);
                                screenGameObjects.Add(ribbon);
                                screenRibbons.Add(ribbon);
                            }
                            else
                            {
                                screenRibbons[screenRibbons.Count - 1].FixRibbonToNote(note);
                            }

                        }

                        // What to do if I press B next to a note (i.e. bomb)
                        if (colliderManager.CheckForCollision(player.idleHitbox, note.key.idleHitbox) && (oldKeyboardState.IsKeyUp(Keys.B) && keyboardState.IsKeyDown(Keys.B)))
                        {
                            if (!player.bombPlanted)
                            {
                                player.bombPlanted = true;
                                Bomb bomb = new Bomb(player.position, "NoteBomb", note);
                                bomb.LoadContent(References.content, References.graphicsDevice);
                                screenGameObjects.Add(bomb);
                                screenBombs.Add(bomb);
                            }

                        }
                    }

                }
            }



            if (screenRibbons.Count > 0)
            {
                foreach (Ribbon ribbon in screenRibbons)
                {
                    // If I'm holding the ribbon and press R when not at a key, ribbon is removed

                    if (ribbon.inPlayersHand)
                    {
                        bool test = true;

                        if (screenNotes.Count > 0)
                        {
                            foreach (Note note in screenNotes)
                            {

                                if (colliderManager.CheckForCollision(player.idleHitbox, note.key.idleHitbox))
                                {
                                    test = false;
                                    break;
                                }

                            }

                            if (test)
                            {
                                if (oldKeyboardState.IsKeyUp(Keys.R) && keyboardState.IsKeyDown(Keys.R))
                                {
                                    if (ribbon.listOfNotes.Count == 1)
                                    {
                                        ribbon.inPlayersHand = false;
                                        ribbon.ClearNoteBools();
                                        screenGameObjectsToRemove.Add(ribbon);
                                        screenRibbonsToRemove.Add(ribbon);
                                    }
                                    else
                                    {
                                        ribbon.FixRibbonAtLastNote();
                                    }
                                }
                            }


                        }

                    }
                }
            }


            //Debug.WriteLine(screenRibbons.Count);

            oldKeyboardState = keyboardState;

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
                    screenGameObjects.Add(tile);
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


        private bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) && oldKeyboardState.IsKeyDown(theKey);
        }


    }
}
