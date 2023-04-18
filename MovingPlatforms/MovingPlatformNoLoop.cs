using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class MovingPlatformNoLoop : MovingPlatformLooping
    {
        public MovingPlatformNoLoop(Vector2 initialPosition) : base(initialPosition)
        {

        }

        public MovingPlatformNoLoop(Vector2 initialPosition, string filename, Vector2 endPoint, int timeStationaryAtEndPoints, float speed, AssetManager assetManager, ColliderManager colliderManager, Player player, float delay = 0, List<GameObject> attachedGameObjects = null) : base(initialPosition, filename, endPoint, timeStationaryAtEndPoints, speed, assetManager, colliderManager, player, delay, attachedGameObjects)
        {

        }


        public override void Update(GameTime gameTime)
        {
            //if (playerControlled)
            //{

                if (movePlatform)
                {
                    if (delayCounter < delay)
                    {
                        delayCounter += 1;
                    }
                    else
                    {
                        base.Update(gameTime);

                        if (horizontalMovement)
                        {
                            if (position.X == endPosition.X)
                            {
                                movePlatform = false;
                                timeStationaryCounter = 0;
                                firstLoop = true;
                                position.X = startPosition.X;
                            }
                        }
                        else if (verticalMovement)
                        {
                            if (position.Y == endPosition.Y)
                            {
                                movePlatform = false;
                                timeStationaryCounter = 0;
                                firstLoop = true;
                                position.Y = startPosition.Y;
                            }
                        }
                    }


                }
            //}
            //else
            //{
            //        if (delayCounter < delay)
            //        {
            //            delayCounter += 1;
            //        }
            //        else
            //        {
            //            base.Update(gameTime);

            //            if (horizontalMovement)
            //            {
            //                if (position.X == endPosition.X)
            //                {
            //                    timeStationaryCounter = 0;
            //                    firstLoop = true;
            //                    position.X = startPosition.X;
            //                }
            //            }
            //            else if (verticalMovement)
            //            {
            //                if (position.Y == endPosition.Y)
            //                {
            //                    timeStationaryCounter = 0;
            //                    firstLoop = true;
            //                    position.Y = startPosition.Y;
            //                }
            //            }
            //        }


                
            //}



        }



    }
}
