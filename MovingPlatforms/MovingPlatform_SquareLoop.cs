using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class MovingPlatform_SquareLoop : MovingPlatform
    {

        // This type of MovingPlatform will move in a square loop indefinitely
        public MovingPlatform_SquareLoop(Vector2 firstPosition, Vector2 secondPosition, Vector2 thirdPosition, Vector2 fourthPosition, string filename, float speed, List<int> stationaryTimes, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player) : base(new List<Vector2>() { firstPosition, secondPosition, thirdPosition, fourthPosition }, filename, speed, stationaryTimes, assetManager, colliderManager, screenManager, player)
        {
        }


        

    }
}
