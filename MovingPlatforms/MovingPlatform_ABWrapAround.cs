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
            if (position == positions[indexToMoveTo])
            {
                movePlatform = false;
                //int temp = indexToMoveTo;
                //currentIndex = 0;
                //firstLoop = true;
                position = positions[currentIndex];
            }
            
        }

        public override void ReverseDirection()
        {
            if (position == positions[currentIndex])
            {
                position = positions[indexToMoveTo];
            }

            base.ReverseDirection();

        }


    }
}
