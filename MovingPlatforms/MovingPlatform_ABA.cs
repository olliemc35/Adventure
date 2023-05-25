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
        // This type of MovingPlatform will be controlled by the player.
        // It will move from A to B and back to A again.
        // When the player presses a Note...
            // The platform will move from position A to position B back to position A again.
            // Pressing the Note again, mid-movement, does nothing.

        public MovingPlatform_ABA(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, List<int> stationaryTimes, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player) : base(new List<Vector2>() { initialPosition, endPoint }, filename, speed, stationaryTimes, assetManager, colliderManager, screenManager, player)
        {
            movePlatform = false;
        }

        // Adjust UpdateAtStationaryPoints to stop at (the return to) A but not at B
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

            if (!movePlatform)
            {
                movePlatform = true;
            }
        }

    }
}
