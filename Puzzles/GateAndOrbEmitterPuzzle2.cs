using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Adventure
{
    public class GateAndOrbEmitterPuzzle2 : GameObject
    {
        public int index = 0;

        public List<SingleOrbEmitter_PlayerEmitter> orbs = new List<SingleOrbEmitter_PlayerEmitter>();

        public MovingPlatform_ABWrapAround activeOrb; // reference

        public Gate gate;

        public bool puzzleActive = false;
        public bool complete = false;
        public bool WaitToReset = false;


        public GateAndOrbEmitterPuzzle2(ColliderManager colliderManager)
        {
            attachedGameObjects = new List<GameObject>();
            this.colliderManager = colliderManager;
        }

        public override void LoadContent()
        {
            if (attachedGameObjects != null)
            {
                foreach (GameObject gameObject in attachedGameObjects)
                {
                    if (gameObject is SingleOrbEmitter_PlayerEmitter emitter)
                    {
                        orbs.Add(emitter);
                    }
                    else if (gameObject is Gate gate)
                    {
                        this.gate = gate;
                    }
                }
            }
        }


        public override void Update(GameTime gametime)
        {
            if (!complete)
            {
                PuzzleLogic();
            }
            else
            {
                gate.open = true;
            }
        }

        public void PuzzleLogic()
        {
            if (!puzzleActive)
            {
                if (WaitToReset)
                {
                    bool test = false;

                    foreach (SingleOrbEmitter_PlayerEmitter emitter in orbs)
                    {
                        if (emitter.platforms[0].movePlatform)
                        {
                            test = true;
                        }
                    }

                    if (test)
                    {
                        return;
                    }
                    else
                    {
                        WaitToReset = false;
                    }
                }


                foreach (SingleOrbEmitter_PlayerEmitter emitter in orbs)
                {
                    if (emitter.platforms[0].movePlatform)
                    {
                        Debug.WriteLine("here");
                        puzzleActive = true;
                        emitter.platformEmitter.UpdatePlayingAnimation(emitter.platformEmitter.animation_Active);
                        activeOrb = emitter.platforms[0];
                        index += 1;
                        return;
                    }
                }
            }

            if (puzzleActive)
            {
                bool collided = false;

                foreach (SingleOrbEmitter_PlayerEmitter emitter in orbs)
                {

                    if (emitter.platforms[0].movePlatform && emitter.platforms[0] != activeOrb && colliderManager.CheckForCollision(activeOrb.idleHitbox, emitter.platforms[0].idleHitbox))
                    {
                        collided = true;
                        emitter.platformEmitter.UpdatePlayingAnimation(emitter.platformEmitter.animation_Active);

                        activeOrb.position = activeOrb.positions[activeOrb.currentIndex];
                        activeOrb.idleHitbox.rectangle.X = (int)activeOrb.position.X + activeOrb.idleHitbox.offsetX;
                        activeOrb.idleHitbox.rectangle.Y = (int)activeOrb.position.Y + activeOrb.idleHitbox.offsetY;
                        activeOrb.movePlatform = false;



                        activeOrb = emitter.platforms[0];
                        index += 1;

                        if (index == orbs.Count)
                        {
                            complete = true;

                            activeOrb.position = activeOrb.positions[activeOrb.currentIndex];
                            activeOrb.idleHitbox.rectangle.X = (int)activeOrb.position.X + activeOrb.idleHitbox.offsetX;
                            activeOrb.idleHitbox.rectangle.Y = (int)activeOrb.position.Y + activeOrb.idleHitbox.offsetY;
                            activeOrb.movePlatform = false;

                            foreach (SingleOrbEmitter_PlayerEmitter emitter2 in orbs)
                            {
                                emitter2.platformEmitter.UpdatePlayingAnimation(emitter2.platformEmitter.animation_Success);
                            }

                            return;
                        }

                        break;
                    }


                }


                if (!collided && !activeOrb.movePlatform)
                {
                    puzzleActive = false;
                    WaitToReset = true;
                    index = 0;

                    foreach (SingleOrbEmitter_PlayerEmitter emitter in orbs)
                    {
                        emitter.platformEmitter.UpdatePlayingAnimation(emitter.platformEmitter.animation_Idle);
                    }
                }




            }


        }





    }
}
