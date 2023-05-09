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

        public MovingPlatform_ABA(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, List<int> stationaryTimes, AssetManager assetManager, ColliderManager colliderManager, Player player) : base(new List<Vector2>() { initialPosition, endPoint }, filename, speed, stationaryTimes, assetManager, colliderManager, player)
        {
        }

        public override void Update(GameTime gameTime)
        {

            if (movePlatform)
            {
                base.Update(gameTime);

                if (direction != Direction.stationary) // Necessary in order to actually move - otherwise will always be stopped at position[0].
                {
                    StopAtStartPoint();

                }

            }

        }

        public void StopAtStartPoint()
        {
            if (position == positions[0])
            {
                movePlatform = false;
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
