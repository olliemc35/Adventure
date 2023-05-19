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
        public MovingPlatform_ABWrapAround(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, List<int> stationaryTimes, AssetManager assetManager, ColliderManager colliderManager, Player player) : base(new List<Vector2>() { initialPosition, endPoint }, filename, speed, stationaryTimes, assetManager, colliderManager, player)
        {
            movePlatform = false;
        }

        public override void UpdateAtStationaryPoints()
        {
            //Debug.WriteLine(position.Y);

            if (direction == Direction.stationary)
            {
                if (timeStationaryCounter < stationaryTimes[currentIndex])
                {
                    timeStationaryCounter += 1;
                }
                else
                {
                    timeStationaryCounter = 0;
                    UpdateDirection();
                }
            }
            else
            {
                if (position == positions[indexToMoveTo])
                {
                    position = positions[currentIndex];
                    idleHitbox.rectangle.X = (int)position.X + idleHitbox.offsetX;
                    idleHitbox.rectangle.Y = (int)position.Y + idleHitbox.offsetY;
                    movePlatform = false;
                }


            }
        }

        public override void HandleNoteTrigger()
        {
            movePlatform = true;
        }



        public override void AdjustHorizontally(ref List<int> ints)
        {

            positions[0] = new Vector2(positions[0].X + ints[0], positions[0].Y);
            positions[1] = new Vector2(positions[1].X + ints[1], positions[1].Y);
            position.X += ints[0];


            ints.RemoveRange(0, 2);

        }
        public override void AdjustVertically(ref List<int> ints)
        {

            positions[0] = new Vector2(positions[0].X, positions[0].Y + ints[0]);
            positions[1] = new Vector2(positions[1].X, positions[1].Y + ints[1]);
            position.Y += ints[0];


            ints.RemoveRange(0, 2);

        }


    }
}
