using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class MovingPlatformNoLoop : MovingPlatform
    {
        public MovingPlatformNoLoop(Vector2 initialPosition) : base(initialPosition)
        {

        }

        public MovingPlatformNoLoop(Vector2 initialPosition, string filename, Vector2 endPoint, int timeStationaryAtEndPoints, float speed) : base(initialPosition, filename, endPoint, timeStationaryAtEndPoints, speed)
        {

        }


        public override void Update(GameTime gameTime)
        {
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



        }



    }
}
