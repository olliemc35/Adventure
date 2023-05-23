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
    public class ConstantOrbEmitter2 : GameObject
    {
        //public SeriesOfMovingPlatform_ABWrapAround orbs;
        public AnimatedGameObject orbReceptor;

        public MovingOrbReceptor movingOrbReceptor;

        public SeriesOfMovingPlatform_ABWrapAround2 orbs;
        public int index = 0;
        public int counter = 0;
        public int timeBetweenOrbs;

        public int numberOfPlatforms;

        public ConstantOrbEmitter2(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, List<int> stationaryTimes, int numberOfPlatforms, int timeBetweenOrbs, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player) : base()
        {
            //orbs = new SeriesOfMovingPlatform_ABWrapAround(initialPosition, endPoint, "BouncingOrb", speed, stationaryTimes, numberOfPlatforms, spacing, assetManager, colliderManager, screenManager, player);
            orbReceptor = new AnimatedGameObject(initialPosition, "OrbReceptors", assetManager);
            this.numberOfPlatforms = numberOfPlatforms;
            this.timeBetweenOrbs = timeBetweenOrbs;

            orbs = new SeriesOfMovingPlatform_ABWrapAround2(initialPosition, endPoint, "BouncingOrb", speed, stationaryTimes, numberOfPlatforms, 16, assetManager, colliderManager, screenManager, player);
       

        }

        public override void LoadContent()
        {
            orbReceptor.LoadContent();
            orbs.LoadContent();

            foreach (MovingPlatform_ABWrapAround orb in orbs.platforms)
            {
                orb.detectCollisionsWithTerrain = true;
                orb.idleHitbox.isActive = true;
                orb.movePlayer = false;
                orb.Hazard = true;
            }

        }

        public override void Update(GameTime gameTime)
        {
            orbs.Update(gameTime);

            foreach (MovingPlatform_ABWrapAround orb in orbs.platforms)
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

            orbReceptor.Update(gameTime);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            orbs.Draw(spriteBatch);
            orbReceptor.Draw(spriteBatch);
        }


        public override void HandleNoteTrigger()
        {
            orbs.HandleNoteTrigger();
        }


    }
}
