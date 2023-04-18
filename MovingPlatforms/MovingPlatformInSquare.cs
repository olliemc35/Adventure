using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class MovingPlatformInSquare : MovingPlatformLooping
    {
        public List<Vector2> positions = new List<Vector2>();
        public int indexOfCurrentStartPosition = 0;


        public MovingPlatformInSquare(Vector2 firstPosition, string filename, Vector2 secondPosition, Vector2 thirdPosition, Vector2 fourthPosition, int timeStationaryAtEndPoints, float speed, AssetManager assetManager, ColliderManager colliderManager, Player player, float delay = 0, List<GameObject> attachedGameObjects = null) : base(firstPosition, filename, secondPosition, timeStationaryAtEndPoints, speed, assetManager, colliderManager, player, delay, attachedGameObjects)
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
                if (position.X == endPosition.X)
                {
                    startPosition = positions[(indexOfCurrentStartPosition + 1) % 4];
                    endPosition = positions[(indexOfCurrentStartPosition + 2) % 4];
                    horizontalMovement = false;
                    velocity.X = 0;
                    displacement.X = 0;
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
                if (position.Y == endPosition.Y)
                {
                    startPosition = positions[(indexOfCurrentStartPosition + 1) % 4];
                    endPosition = positions[(indexOfCurrentStartPosition + 2) % 4];
                    horizontalMovement = true;
                    velocity.Y = 0;
                    displacement.Y = 0;
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
