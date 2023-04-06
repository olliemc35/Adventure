using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class MovingPlatformOneLoop : MovingPlatform
    {
        public MovingPlatformOneLoop(Vector2 initialPosition) : base(initialPosition)
        {

        }

        public MovingPlatformOneLoop(Vector2 initialPosition, string filename, Vector2 endPoint, int timeStationaryAtEndPoints, float speed) : base(initialPosition, filename, endPoint, timeStationaryAtEndPoints, speed)
        {
            timeStationaryCounter = timeStationaryAtEndPoints;
        }

        public override void Update(GameTime gameTime)
        {


            if (movePlatform)
            {
                base.Update(gameTime);


                if (horizontalMovement)
                {
                    if (position.X == startPosition.X)
                    {
                        movePlatform = false;
                        timeStationaryCounter = timeStationaryAtEndPoints;
                    }
                }
                else if (verticalMovement)
                {
                    if (position.Y == startPosition.Y)
                    {
                        movePlatform = false;
                        timeStationaryCounter = timeStationaryAtEndPoints;
                    }
                }
            }



        }




    }
}
