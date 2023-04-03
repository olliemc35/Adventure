using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class MovingPlatformInSquare : MovingPlatform
    {
        public List<Vector2> positions = new List<Vector2>();
        public int indexOfCurrentStartPosition = 0;


        public MovingPlatformInSquare(Vector2 firstPosition, string filename, Vector2 secondPosition, Vector2 thirdPosition, Vector2 fourthPosition, int timeStationaryAtEndPoints, float speed) : base(firstPosition, filename, secondPosition, timeStationaryAtEndPoints, speed)
        {

            positions.Add(firstPosition);
            positions.Add(secondPosition);
            positions.Add(thirdPosition);
            positions.Add(fourthPosition);

            startPosition = firstPosition;
            endPosition = secondPosition;

        }



        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);


            if (horizontalMovement)
            {
                if (spritePosition.X == endPosition.X)
                {
                    startPosition = positions[(indexOfCurrentStartPosition + 1) % 4];
                    endPosition = positions[(indexOfCurrentStartPosition + 2) % 4];
                    horizontalMovement = false;
                    spriteVelocity.X = 0;
                    spriteDisplacement.X = 0;
                    movingRight = false;
                    movingLeft = false;
                    firstLoop = true;

                    if (startPosition.Y > endPosition.Y)
                    {
                        movingUp = true;
                        movingDown = false;
                    }
                    else
                    {
                        movingUp = false;
                        movingDown = true;
                    }

                    indexOfCurrentStartPosition = (indexOfCurrentStartPosition + 1) % 4;


                }
            }
            else
            {
                if (spritePosition.Y == endPosition.Y)
                {
                    startPosition = positions[(indexOfCurrentStartPosition + 1) % 4];
                    endPosition = positions[(indexOfCurrentStartPosition + 2) % 4];
                    horizontalMovement = true;
                    spriteVelocity.Y = 0;
                    spriteDisplacement.Y = 0;
                    movingUp = false;
                    movingDown = false;
                    firstLoop = true;

                    if (startPosition.X < endPosition.X)
                    {
                        movingLeft = false;
                        movingRight = true;
                    }
                    else
                    {
                        movingLeft = true;
                        movingRight = false;
                    }

                    indexOfCurrentStartPosition = (indexOfCurrentStartPosition + 1) % 4;
                }
            }




        }

    }
}
