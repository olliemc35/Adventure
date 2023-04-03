using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class MovingPlatformHalfLoop : MovingPlatform
    {
        public bool TravellingFromStartPoint = true;

        public MovingPlatformHalfLoop(Vector2 initialPosition) : base(initialPosition)
        {

        }

        public MovingPlatformHalfLoop(Vector2 initialPosition, string filename, Vector2 endPoint, int timeStationaryAtEndPoints, float speed) : base(initialPosition, filename, endPoint, timeStationaryAtEndPoints, speed)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (movePlatform)
            {
                base.Update(gameTime);

                if (horizontalMovement)
                {
                    if (TravellingFromStartPoint)
                    {
                        if (spritePosition.X == endPosition.X)
                        {
                            movePlatform = false;
                            TravellingFromStartPoint = false;
                        }
                    }
                    else
                    {
                        if (spritePosition.X == startPosition.X)
                        {
                            movePlatform = false;
                            TravellingFromStartPoint = true;
                        }
                    }
                }
                else if (verticalMovement)
                {
                    if (TravellingFromStartPoint)
                    {
                        if (spritePosition.Y == endPosition.Y)
                        {
                            movePlatform = false;
                            TravellingFromStartPoint = false;
                        }
                    }
                    else
                    {
                        if (spritePosition.Y == startPosition.Y)
                        {
                            movePlatform = false;
                            TravellingFromStartPoint = true;
                        }
                    }

                }
            }



        }
    }
}
