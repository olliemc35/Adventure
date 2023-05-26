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

        public string symbolFilename;

        public int displacementScalingForNoteShip;

        public LaunchPad launchPad;

        public float orbSpeed;
        public string orbFilename;



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

        public Note(Vector2 keyPosition, string keyFilename, string noteValue, string symbolFilename, AssetManager assetManager, ColliderManager colliderManager, InputManager inputManager, SoundManager soundManager, Player player, string orbFilename = null, float speedOfOrb = 0, int displacementScalingForNoteShip = 0)
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
            orbSpeed = speedOfOrb;
            this.orbFilename = orbFilename;

            attachedGameObjects = new List<GameObject>();
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
                if (colliderManager.CheckForCollision(player.idleHitbox, key.idleHitbox) && (inputManager.OnKeyDown(Keys.E) || inputManager.OnKeyDown(Keys.Up)))
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
                    if (gameObject is NoteShip noteShip)
                    {
                        noteShip.displacementScaling = displacementScalingForNoteShip;
                    }

                    if (gameObject.noteTriggerData == null)
                    {
                        gameObject.HandleNoteTrigger();
                    }
                    else
                    {
                        gameObject.HandleNoteTrigger(noteTriggerData);

                    }


                }
            }



        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            key.Draw(spriteBatch);
        }


        public override void MoveManually(Vector2 moveVector)
        {
            key.MoveManually(moveVector);
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
