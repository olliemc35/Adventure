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

        public MovingPlatformHalfLoop(Vector2 initialPosition, string filename, Vector2 endPoint, int timeStationaryAtEndPoints, float speed, AssetManager assetManager, Player player, float delay = 0, List<GameObject> attachedGameObjects = null) : base(initialPosition, filename, endPoint, timeStationaryAtEndPoints, speed, assetManager, player, delay, attachedGameObjects)
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
                        if (position.X == endPosition.X)
                        {
                            movePlatform = false;
                            TravellingFromStartPoint = false;
                        }
                    }
                    else
                    {
                        if (position.X == startPosition.X)
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
                        if (position.Y == endPosition.Y)
                        {
                            movePlatform = false;
                            TravellingFromStartPoint = false;
                        }
                    }
                    else
                    {
                        if (position.Y == startPosition.Y)
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
