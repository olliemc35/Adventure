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
    public class ConstantOrbEmitter : GameObject
    {
        //public SeriesOfMovingPlatform_ABWrapAround orbs;
        public AnimatedGameObject orbReceptor;

        public MovingOrbReceptor movingOrbReceptor;

        public List<MovingPlatform_ABWrapAround> orbs = new List<MovingPlatform_ABWrapAround>();
        public int index = 0;
        public int counter = 0;
        public int timeBetweenOrbs;

        public int numberOfPlatforms;

        public ConstantOrbEmitter(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, List<int> stationaryTimes, int numberOfPlatforms, int timeBetweenOrbs, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player) : base()
        {
            //orbs = new SeriesOfMovingPlatform_ABWrapAround(initialPosition, endPoint, "BouncingOrb", speed, stationaryTimes, numberOfPlatforms, spacing, assetManager, colliderManager, screenManager, player);
            orbReceptor = new AnimatedGameObject(initialPosition, "OrbReceptors", assetManager);
            this.numberOfPlatforms = numberOfPlatforms;
            this.timeBetweenOrbs = timeBetweenOrbs;

            for (int i = 0; i < numberOfPlatforms; i++)
            {
                orbs.Add(new MovingPlatform_ABWrapAround(initialPosition, endPoint, "BouncingOrb", speed, stationaryTimes, assetManager, colliderManager, screenManager, player));

            }

        }

        public override void LoadContent()
        {
            foreach (MovingPlatform_ABWrapAround orb in orbs)
            {
                orb.LoadContent();
                orb.detectCollisionsWithTerrain = true;
                orb.idleHitbox.isActive = true;
                orb.movePlayer = false;
                orb.Hazard = true;

            }

            orbs[0].movePlatform = true;


            //orbs.LoadContent();

            //foreach (MovingPlatform_ABWrapAround orb in orbs.platforms)
            //{
            //    orb.detectCollisionsWithTerrain = true;
            //    orb.idleHitbox.isActive = true;
            //    orb.movePlayer = false;
            //    orb.Hazard = true;
            //}

            orbReceptor.LoadContent();
            orbReceptor.CollisionObject = true;
            orbReceptor.idleHitbox.isActive = true;
            //orbReceptor.CollisionObject = true;
            //orbReceptor.idleHitbox.isActive = true;

        }

        public override void Update(GameTime gameTime)
        {


            if (counter < timeBetweenOrbs)
            {
                counter++;
            }
            else
            {
                counter = 0;

                index = (index + 1) % numberOfPlatforms;
                orbs[index].movePlatform = true;

            }

            foreach (MovingPlatform_ABWrapAround orb in orbs)
            {
                orb.Update(gameTime);

                if (orb.flagCollision)
                {
                    orb.flagCollision = false;
                }

                if (!orb.movePlatform)
                {
                    orb.idleHitbox.isActive = false;
                }
                else
                {
                    orb.idleHitbox.isActive = true;
                }


            }

            //orbs.Update(gameTime);
            orbReceptor.Update(gameTime);

            //Debug.WriteLine(orbs.indexOfPlatformClosestToStart);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (MovingPlatform_ABWrapAround orb in orbs)
            {
                orb.Draw(spriteBatch);
            }
            //orbs.Draw(spriteBatch);
            orbReceptor.Draw(spriteBatch);
        }

        public override void HandleNoteTrigger()
        {
            //orb.HandleNoteTrigger();
        }


    }
}
