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
        public InputManager inputManager;
        // It is important, for the draw order, that we add the tiles FIRST - so that these are drawn in the background
        public Tileset tileset;
        public AnimatedGameObject[,] arrayofTiles;

        public AnimatedGameObject[,] backgroundObjects;



        public ActionScreen(SpriteBatch spriteBatch, SpriteFont spriteFont, Player player, InputManager inputManager, List<GameObject> gameObjects, AnimatedGameObject[,] backgroundObjects) : base(spriteBatch)
        {
            this.player = player;
            this.screenGameObjectsToLoadIn = gameObjects;
            this.backgroundObjects = backgroundObjects;
            this.inputManager = inputManager;
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



        public override void LoadContent()
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
                    gameObject.LoadContent();
                    screenGameObjects.Add(gameObject);

                    if (gameObject is AnimatedGameObject sprite)
                    {

                                             

                        
                        if (sprite.CollisionObject)
                        {
                          
                            hitboxesToCheckCollisionsWith.Add(sprite.idleHitbox);
                        }

                        if (sprite.Hazard)
                        {
                            hazardHitboxes.Add(sprite.idleHitbox);
                        }
                    }

                    if (gameObject is MovingPlatform movingPlatform)
                    {

                         terrainHitboxes.Add(movingPlatform.idleHitbox);
                        
                    }

                    if (gameObject is Door door)
                    {
                        screenDoors.Add(door);
                    }



                    if (gameObject is OrganStop organStop)
                    {
                        hitboxesToCheckCollisionsWith.Add(organStop.platform.idleHitbox);
                        hitboxesToCheckCollisionsWith.Add(organStop.tubeHitbox);
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
                        
                        hitboxesToCheckCollisionsWith.Add(beam.startHitbox);
                        hitboxesToCheckCollisionsWith.Add(beam.endHitbox);

                        terrainHitboxes.Add(beam.startHitbox);
                        terrainHitboxes.Add(beam.endHitbox);
                    }

                    if (gameObject is SeriesOfMovingPlatform_ABWrapAround series)
                    {
                        foreach (MovingPlatform_ABWrapAround platformNoLoop in series.platforms)
                        {
                            terrainHitboxes.Add(platformNoLoop.idleHitbox);
                            hitboxesToCheckCollisionsWith.Add(platformNoLoop.idleHitbox);
                        }
                    }
                  

                    if (gameObject is MovingPlatform_ABLoop platform)
                    {

                        hitboxesForAimLine.Add(platform.idleHitbox);

                    }

                   

                    if (gameObject is Note note)
                    {
                        screenNotes.Add(note);
                    }

                    if (gameObject is Gate gate)
                    {
                        hitboxesForAimLine.Add(gate.idleHitbox);
                    }

                    if (gameObject is NoteShip noteShip)
                    {
                        screenNoteShip = noteShip;
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
            camera.UpdateTransform(this, player);

          
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            //foreach (HitboxRectangle hitbox in hitboxesToCheckCollisionsWith)
            //{
            //    spriteBatch.Draw(player.idleHitbox.texture, hitbox.rectangle, Color.Red);
            //}
        }

        public override void HandleInput()
        {
            inputManager.UpdatePlayerInput(player);
        }

        public void BuildBackground()
        {
            for (int i = 0; i < backgroundObjects.GetLength(0); i++)
            {

                for (int j = 0; j < backgroundObjects.GetLength(1); j++)
                {

                    backgroundObjects[i, j].LoadContent();

                    if (backgroundObjects[i, j].filename == "Tile_air")
                    {
                        backgroundObjects[i, j].idleHitbox.isActive = false;
                    }
                    else
                    {
                        backgroundObjects[i, j].idleHitbox.isActive = true;
                    }

                    gameObjectsDrawOnly.Add(backgroundObjects[i, j]);

                    //hitboxesToCheckCollisionsWith.Add(backgroundObjects[i, j].idleHitbox);
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

                                if (k == backgroundObjects.GetLength(0) - 1 && backgroundObjects[k, j].idleHitbox.isActive)
                                {
                                    newHitbox.rectangle.Width = (k - i + 1) * 8;
                                    break;
                                }

                            }
                        }
                        else
                        {
                            newHitbox.rectangle.Width = 8;
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
                            newHitbox.rectangle.Height = 8;
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
                        terrainHitboxes.Add(newHitbox);
                        hitboxesForAimLine.Add(newHitbox);

                    }
                }


            }
        }



       


    }
}
