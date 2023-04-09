using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace Adventure
{
    public class Note : GameObject
    {
        public Key key;

        public List<GameObject> attachedGameObjects;

        public string symbolFilename;

        public List<MovingPlatformOneLoop> movingPlatformsOneLoop;
        public List<MovingPlatformNoLoop> movingPlatformsNoLoop;
        public List<MovingPlatformHalfLoop> movingPlatformsHalfLoop;
        public NoteShip noteShip;
        public int displacementScalingForNoteShip;

        public List<Gate> gates;
        public LaunchPad launchPad;

        public float orbSpeed;
        public string orbFilename;


        public List<int> indicesOfAttachedGates = new List<int>();

        public bool ribbonAttached = false;
        public Vector2 ribbonPosition = new Vector2();

        public string noteValue;
        public SoundEffect noteSound;

        public List<SoundEffectInstance> noteInstances = new List<SoundEffectInstance>();

        public float duration;
        public bool playerInteractedWith = false;
        public bool keyPlayInteractedAnimation = false;
        public bool flagPlayerInteractedWith = false;
        public bool movePlatform = false;
        public bool symbolTurnedOn = false;
        int counter = 0;

        public List<int> indicesToRemove = new List<int>();

        int numberOfFramesBetweenPlayerInteractions = 60;

        public Note(Vector2 keyPosition, string keyFilename, string noteValue, AssetManager assetManager, ColliderManager colliderManager, InputManager inputManager, SoundManager soundManager, Player player, List<GameObject> gameObjects = null, string symbolFilename = null, string orbFilename = null, float speedOfOrb = 0, int displacementScalingForNoteShip = 0)
        {
            this.colliderManager = colliderManager;
            this.inputManager = inputManager;
            this.soundManager = soundManager;
            this.assetManager = assetManager;
            this.player = player;
            key = new Key(keyPosition, keyFilename, assetManager);
            this.noteValue = noteValue;
            this.symbolFilename = symbolFilename;
            this.displacementScalingForNoteShip = displacementScalingForNoteShip;
            attachedGameObjects = gameObjects;
            orbSpeed = speedOfOrb;
            this.orbFilename = orbFilename;
        }




        public override void LoadContent()
        {
            key.LoadContent();
            noteSound = soundManager.soundEffects[noteValue];
        }


        public override void Update(GameTime gameTime)
        {
            if (flagPlayerInteractedWith)
            {
                flagPlayerInteractedWith = false;
                CreateAndPlayNote();

            }

            if (!playerInteractedWith)
            {
                if (colliderManager.CheckForCollision(player.idleHitbox, key.idleHitbox) && (inputManager.OnKeyUp(Keys.E) || inputManager.OnKeyUp(Keys.Up)))
                {
                    if (ribbonAttached)
                    {
                        foreach (Ribbon ribbon in player.ribbons)
                        {
                            if (ribbon.listOfNotes.Contains(this))
                            {
                                ribbon.playNotes = true;
                                ribbon.indexOfNoteToPlay = ribbon.listOfNotes.IndexOf(this);

                            }
                        }
                    }
                    else
                    {
                        flagPlayerInteractedWith = true;

                    }
                }

                
            }

            //    // BOMB CODE

            //if (!References.player.bombPlanted)
            //{
            //    if (colliderManager.CheckForCollision(References.player.idleHitbox, key.idleHitbox) && inputManager.OnKeyUp(Keys.B))
            //    {
            //        if (!References.player.bombPlanted)
            //        {
            //            References.player.bombPlanted = true;
            //            References.player.bomb.position = References.player.position;
            //            References.player.bomb.attachedNote = this;
            //        }

            //    }
            //}

            //    // RIBBON CODE

            if (colliderManager.CheckForCollision(player.idleHitbox, key.idleHitbox) && inputManager.OnKeyUp(Keys.R))
            {
                if (!player.ribbonInHand)
                {
                    player.ribbonInHand = true;
                    player.ribbons[player.ribbonIndex].inPlayersHand = true;
                    player.ribbons[player.ribbonIndex].Enabled = true;
                    player.ribbons[player.ribbonIndex].position = player.position;
                    player.ribbons[player.ribbonIndex].FixRibbonToNote(this);
                }
                else
                {
                    player.ribbons[player.ribbonIndex].FixRibbonToNote(this);
                }
            }


          

            if (playerInteractedWith && counter < numberOfFramesBetweenPlayerInteractions)
            {
                counter += 1;
            }
            else if (playerInteractedWith && counter >= numberOfFramesBetweenPlayerInteractions)
            {
                playerInteractedWith = false;
                counter = 0;
            }

            if (noteInstances.Count() > 0)
            {
                for (int i = noteInstances.Count - 1; i >= 0; i--)
                {
                    if (noteInstances[i].State == SoundState.Stopped)
                    {
                        noteInstances.RemoveAt(i);
                    }
                }
            }

            if (noteInstances.Count() == 0)
            {
                keyPlayInteractedAnimation = false;
            }


            ManageAnimations();


            key.Update(gameTime);





        }

        public void CreateAndPlayNote()
        {
            playerInteractedWith = true;
            keyPlayInteractedAnimation = true;
            noteInstances.Add(noteSound.CreateInstance());
            noteInstances.Last().Play();


            if (attachedGameObjects != null)
            {
                foreach (GameObject gameObject in attachedGameObjects)
                {

                    if (gameObject is Gate gate)
                    {
                        if (gate.open)
                        {
                            gate.open = false;
                        }
                        else
                        {
                            gate.open = true;
                        }
                    }
                    else if (gameObject is MovingPlatformOneLoop platform_oneLoop)
                    {
                        if (!platform_oneLoop.movePlatform)
                        {
                            platform_oneLoop.movePlatform = true;
                        }
                    }
                    else if (gameObject is MovingPlatformHalfLoop platform_halfLoop)
                    {
                        if (!platform_halfLoop.movePlatform)
                        {
                            platform_halfLoop.movePlatform = true;
                        }
                    }
                    else if (gameObject is LaunchPad launchPad)
                    {
                        launchPad.launchFlag = true;
                    }
                    else if (gameObject is SeriesOfMovingPlatformNoLoop movingPlatformsNoLoop)
                    {
                        int indexofFirstPlatformNotMoving = -1;

                        for (int i = 0; i < movingPlatformsNoLoop.platforms.Count; i++)
                        {
                            if (movingPlatformsNoLoop.platforms[i].horizontalMovement)
                            {
                                if (movingPlatformsNoLoop.platforms[i].position.X == movingPlatformsNoLoop.platforms[i].startPosition.X)
                                {
                                    indexofFirstPlatformNotMoving = i;
                                    break;
                                }
                            }
                            else
                            {
                                if (movingPlatformsNoLoop.platforms[i].position.Y == movingPlatformsNoLoop.platforms[i].startPosition.Y)
                                {
                                    indexofFirstPlatformNotMoving = i;
                                    break;
                                }
                            }
                        }

                        if (indexofFirstPlatformNotMoving == -1)
                        {
                            // do nothing - I need to supply more platforms so this doesn't happen
                            Debug.WriteLine("SUPPLY MORE PLATFORMS");
                        }
                        else
                        {
                            movingPlatformsNoLoop.platforms[indexofFirstPlatformNotMoving].movePlatform = true;
                        }
                    }
                    else if (gameObject is NoteShip noteShip)
                    {
                        noteShip.displacementScaling = displacementScalingForNoteShip;
                        noteShip.moveVertically = true;
                    }


                }
            }



        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            key.Draw(spriteBatch);
        }


        public void ManageAnimations()
        {

            if (keyPlayInteractedAnimation)
            {
                key.UpdatePlayingAnimation(key.animation_Interacted);
            }
            else
            {
                key.UpdatePlayingAnimation(key.animation_Idle);
            }


        }


    }
}
