using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class MovingPlatform_ABWrapAround : MovingPlatform
    {

        // This type of MovingPlatform will go from one side of the screen to the other and loop back again
        // It may or may not be controlled by the player 
        public MovingPlatform_ABWrapAround(Vector2 initialPosition, Vector2 endPoint, string filename, int timeStationaryAtEndPoints, float speed, int delay, AssetManager assetManager, ColliderManager colliderManager, Player player) : base(new List<Vector2>(){ initialPosition, endPoint }, new List<int>(){ 0, 1 }, filename, timeStationaryAtEndPoints, speed, delay, assetManager, colliderManager, player)
        {

        }


        public override void Update(GameTime gameTime)
        {
            if (movePlatform)
            {
                base.Update(gameTime);
                ReturnToBeginningAtStationaryPoints();
            }        

        }

        public void ReturnToBeginningAtStationaryPoints()
        {
            if (position == positions[indexes[1]])
            {
                movePlatform = false;
                currentIndex = indexes[0];
                firstLoop = true;
                position = positions[indexes[0]];
            }
            
        }

        public override void ReverseDirection()
        {
            currentIndex = indexes[1];

            // If I am stationary off-screen then put me at the opposite off-screen location
            if (position == positions[indexes[0]])
            {
                position = positions[indexes[1]];
            }

            switch (direction)
            {
                case Direction.moveRight:
                    {
                        direction = Direction.moveLeft;
                        break;
                    }
                case Direction.moveLeft:
                    {
                        direction = Direction.moveRight;

                        break;
                    }
                case Direction.moveUp:
                    {
                        direction = Direction.moveDown;

                        break;
                    }
                case Direction.moveDown:
                    {
                        direction = Direction.moveUp;
                        break;
                    }
                case Direction.stationary:
                    {
                        break;
                    }

            }

            indexes.Reverse();

        }


    }
}
