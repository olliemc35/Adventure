using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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

        public MovingPlatform_ABA(Vector2 initialPosition, Vector2 endPoint, string filename, int timeStationaryAtEndPoints, float speed, int delay, AssetManager assetManager, ColliderManager colliderManager, Player player) : base(new List<Vector2>() { initialPosition, endPoint }, new List<int>() { 0, 1 }, filename, timeStationaryAtEndPoints, speed, delay, assetManager, colliderManager, player)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (movePlatform)
            {
                base.Update(gameTime);
                StopAtStartPoint();              
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
            if (!movePlatform)
            {
                movePlatform = true;
            }
        }

    }
}
