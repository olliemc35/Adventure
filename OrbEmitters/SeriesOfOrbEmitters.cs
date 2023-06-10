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
    public class SeriesOfOrbEmitters : GameObject
    {
        public List<MovingPlatform_ABWrapAround_WithEmitterReceiver> orbEmitters = new List<MovingPlatform_ABWrapAround_WithEmitterReceiver>();
        public int indexOfActiveOrb = 0;
        public List<int> order;

        public SeriesOfOrbEmitters(List<Vector2> positions, List<string> movementDirections, List<int> order, string filename, string emitterFilename, float speed, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player)
        {
            this.assetManager = assetManager;
            this.colliderManager = colliderManager;
            this.screenManager = screenManager;
            this.player = player;
            this.order = order;

            for (int i = 0; i<positions.Count; i++)
            {
                MovingPlatform_ABWrapAround_WithEmitterReceiver test = new MovingPlatform_ABWrapAround_WithEmitterReceiver(positions[i], movementDirections[i], filename, emitterFilename, speed, new List<int>() { 0, 0 }, assetManager, colliderManager, screenManager, player);
                orbEmitters.Add(test);
            }

            orbEmitters[order[0]].movePlatform = true;
        }


        public override void LoadContent()
        {
            foreach (MovingPlatform_ABWrapAround_WithEmitterReceiver orbEmitter in orbEmitters)
            {
                orbEmitter.LoadContent();
                orbEmitter.detectCollisionsWithTerrain = true;
                orbEmitter.idleHitbox.isActive = true;
                orbEmitter.movePlayer = false;
                orbEmitter.Hazard = true;
            }



           


        }

        public override void Update(GameTime gameTime)
        {
            orbEmitters[order[indexOfActiveOrb]].Update(gameTime);

            if (!orbEmitters[order[indexOfActiveOrb]].movePlatform)
            {
                indexOfActiveOrb = (indexOfActiveOrb + 1) % orbEmitters.Count;
                orbEmitters[order[indexOfActiveOrb]].movePlatform = true;
            }


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (MovingPlatform_ABWrapAround_WithEmitterReceiver orbEmitter in orbEmitters)
            {
                orbEmitter.Draw(spriteBatch);
            }
        }

    }
}
