using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;


namespace Adventure
{
    public class MovingPlatform_AB : MovingPlatform
    {
        // This type of MovingPlatform will be controlled by the player.
        // The player will press a corresponding Note and the platform will move from position A to position B
        // When the player presses the Note again AND the platform is at position B, it will move back to position A etc.
        // As always the player trigger is the movePlatform bool

        

        public bool delayed = false;

        public MovingPlatform_AB(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, List<int> stationaryTimes, AssetManager assetManager, ColliderManager colliderManager, Player player) : base(new List<Vector2>() { initialPosition, endPoint }, filename, speed, stationaryTimes, assetManager, colliderManager, player)
        {
        }


        public override void Update(GameTime gameTime)
        {
            //Debug.WriteLine("currentIndex " + currentIndex);
            //Debug.WriteLine("indexToMoveTo " + indexToMoveTo);

            if (halt)
            {
                if (haltCounter == numberOfFramesHalted)
                {
                    haltCounter = 0;
                    halt = false;
                }
                else
                {
                    haltCounter += 1;
                    return;
                }
            }

            if (movePlatform)
            {
                UpdateAtStationaryPoints();
                UpdateVelocityAndDisplacement();
                position.X += displacement.X;
                position.Y += displacement.Y;

                if (colliderManager.CheckForEdgesMeeting(idleHitbox, player.idleHitbox))
                {
                    player.MoveManually(displacement);
                }
                else
                {
                    foreach (GameObject gameObject in attachedGameObjects)
                    {
                        if (gameObject is AnimatedGameObject sprite)
                        {
                            if (sprite.Climable && player.playerStateManager.climbingState.Active && player.playerStateManager.climbingState.platform == sprite)
                            {
                                player.MoveManually(displacement);
                                break;
                            }

                        }
                    }
                }

                MoveAttachedGameObjects();


                idleHitbox.rectangle.X = FindNearestInteger(position.X) + idleHitbox.offsetX;
                idleHitbox.rectangle.Y = FindNearestInteger(position.Y) + idleHitbox.offsetY;


                //StopAtStationaryPoints();

            }


            ManageAnimations();
            BaseUpdate(gameTime);


        }


        public void StopAtStationaryPoints()
        {


            if (position == positions[indexToMoveTo])
            {
                if (stationaryTimes[indexToMoveTo] != 0 && timeStationaryCounter < stationaryTimes[indexToMoveTo])
                {
                    timeStationaryCounter += 1;
                    direction = Direction.stationary;
                }
                else if (stationaryTimes[indexToMoveTo] == 0 || (stationaryTimes[indexToMoveTo] != 0 && timeStationaryCounter == stationaryTimes[indexToMoveTo]))
                {
                    timeStationaryCounter = 0;
                    currentIndex = indexToMoveTo;
                    indexToMoveTo = (indexToMoveTo + positions.Count + sign) % positions.Count; // We add positions.Count here is the way C# handles modular arithmetic is a bit odd if the integer is negative (which is can be if sign = -1 here).
                    UpdateDirection();
                }

            }



        }
        public override void UpdateAtStationaryPoints()
        {
            if (direction == Direction.stationary)
            {
                if (timeStationaryCounter < stationaryTimes[currentIndex])
                {
                    timeStationaryCounter += 1;
                }
                else
                {
                    timeStationaryCounter = 0;
                    //currentIndex = indexToMoveTo;
                    //indexToMoveTo = (indexToMoveTo + positions.Count + sign) % positions.Count; // We add positions.Count here is the way C# handles modular arithmetic is a bit odd if the integer is negative (which is can be if sign = -1 here).
                    UpdateDirection();
                }
            }
            else
            {
                if (position == positions[indexToMoveTo])
                {
                    direction = Direction.stationary;
                    movePlatform = false;
                    currentIndex = indexToMoveTo;
                    indexToMoveTo = (indexToMoveTo + positions.Count + sign) % positions.Count;
                }

            }


            //if (position == positions[indexToMoveTo])
            //{
            //    if (stationaryTimes[indexToMoveTo] != 0 && timeStationaryCounter < stationaryTimes[indexToMoveTo])
            //    {
            //        timeStationaryCounter += 1;
            //        direction = Direction.stationary;
            //    }
            //    else if (stationaryTimes[indexToMoveTo] == 0 || (stationaryTimes[indexToMoveTo] != 0 && timeStationaryCounter == stationaryTimes[indexToMoveTo]))
            //    {
            //        timeStationaryCounter = 0;
            //        currentIndex = indexToMoveTo;
            //        indexToMoveTo = (indexToMoveTo + positions.Count + sign) % positions.Count; // We add positions.Count here is the way C# handles modular arithmetic is a bit odd if the integer is negative (which is can be if sign = -1 here).
            //        UpdateDirection();
            //    }

            //}
        }
        //public override void UpdateAtStationaryPoints()
        //{
        //    if (position == positions[indexToMoveTo])
        //    {
        //        movePlatform = false;
        //        timeStationaryCounter = 0;
        //        currentIndex = indexToMoveTo;

        //        direction = Direction.stationary;
        //    }
        //}


        public override void HandleNoteTrigger()
        {
            if (movePlatform)
            {
                halt = true;
                ReverseDirection();
            }

            movePlatform = true;
        }

    }
}
