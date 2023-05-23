using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class MovingPlatform_ABA : MovingPlatform
    {
        // This type of MovingPlatform is similar to MovingPlatformHalfLoop
        // It will have two positions to move between and the starting position must be given first in the list (i.e. at index 0)
        // It is controlled by the player - when the player plays a corresponding Note it will move from position 1 to position 2 back to position 1 again

        public MovingPlatform_ABA(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, List<int> stationaryTimes, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player) : base(new List<Vector2>() { initialPosition, endPoint }, filename, speed, stationaryTimes, assetManager, colliderManager, screenManager, player)
        {
            movePlatform = false;
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
                    UpdateDirection();
                }
            }
            else
            {
                if (position == positions[indexToMoveTo])
                {
                    if (indexToMoveTo == 0)
                    {
                        UpdateIndices();
                        direction = Direction.stationary;
                        movePlatform = false;
                    }
                    else
                    {
                        if (stationaryTimes[indexToMoveTo] == 0)
                        {
                            timeStationaryCounter = 0;
                            UpdateIndices();
                            UpdateDirection();
                        }
                        else
                        {
                            UpdateIndices();
                            direction = Direction.stationary;
                        }
                    }
                }
            }
        }


        public override void HandleNoteTrigger()
        {
            //if (movePlatform)
            //{
            //    ReverseDirection();
            //}

            //movePlatform = true;

            if (!movePlatform)
            {
                movePlatform = true;
            }
        }

    }
}
