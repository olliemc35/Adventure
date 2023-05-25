using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;


namespace Adventure
{
    public class MovingPlatform_AB : MovingPlatform
    {
        // This type of MovingPlatform will be controlled by the player.
        // The player will press a corresponding Note and the platform will move from position A to position B
        // When the player presses the Note then ...
            // If platform is stationary at B it will move to A and if platform is stationary at A it will move to B
            // If platform is moving between A and B it will halt for 60 frames and then reverse direction, moving back to the point it came from.
        // The player trigger is the movePlatform bool.
       
        public MovingPlatform_AB(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, List<int> stationaryTimes, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player) : base(new List<Vector2>() { initialPosition, endPoint }, filename, speed, stationaryTimes, assetManager, colliderManager, screenManager, player)
        {
            movePlatform = false;
        }


        // We update the UpdateAtStationaryPoints method to incorporate stopping at each end-point.
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
                    UpdateIndices();
                    direction = Direction.stationary;
                    movePlatform = false;
                }

            }


        }



        public override void HandleNoteTrigger()
        {
            if (movePlatform)
            {
                halt = true;
                ReverseDirection();
            }

            movePlatform = true;
        }

    }
}
