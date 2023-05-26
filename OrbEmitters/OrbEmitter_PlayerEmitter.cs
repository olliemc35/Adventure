using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Diagnostics.Metrics;

namespace Adventure
{
    public class OrbEmitter_PlayerEmitter : SeriesOfMovingPlatform_ABWrapAround_PlayerEmitter
    {

        public OrbEmitter_PlayerEmitter(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, List<int> stationaryTimes, int framesBetweenEmitting, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player, MovingPlatform orbEmitter) : base(initialPosition, endPoint, filename, speed, stationaryTimes, framesBetweenEmitting, assetManager, colliderManager, screenManager, player, orbEmitter)
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
