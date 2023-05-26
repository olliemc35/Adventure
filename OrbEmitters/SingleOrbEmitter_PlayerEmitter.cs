using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Adventure
{
    public class SingleOrbEmitter_PlayerEmitter : SeriesOfMovingPlatform_ABWrapAround_PlayerEmitter
    {
        // This is the same as ConstantOrbEmitter_PlayerEmitter except now we restrict to a single orb 

        public SingleOrbEmitter_PlayerEmitter(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, List<int> stationaryTimes, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player, MovingPlatform orbEmitter) : base(initialPosition, endPoint, filename, speed, stationaryTimes, 0, assetManager, colliderManager, screenManager, player, orbEmitter, null, 1)
        {

        }

        public override void LoadContent()
        {
            base.LoadContent();

            foreach (MovingPlatform_ABWrapAround orb in platforms)
            {
                orb.detectCollisionsWithTerrain = true;
                orb.idleHitbox.isActive = true;
                orb.movePlayer = false;
                orb.Hazard = true;

            }


        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //Debug.WriteLine(platforms[0].positions[1].Y);

            Debug.WriteLine(platforms[0].position.Y);
            //Debug.WriteLine(platforms[0].positions[1].Y - platforms[0].positions[0].Y);
            foreach (MovingPlatform_ABWrapAround orb in platforms)
            {

                if (!orb.movePlatform)
                {
                    orb.idleHitbox.isActive = false;
                }
                else
                {
                    orb.idleHitbox.isActive = true;
                }


            }
        }


      





    }
}
