using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class MovingPlatform_ABWrapAround_WithEmitterReceiver : MovingPlatform_ABWrapAround
    {
        public MovingPlatform platformEmitter;
        public MovingPlatform platformReceiver;


        public MovingPlatform_ABWrapAround_WithEmitterReceiver(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, List<int> stationaryTimes, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player, bool receptorBehaviour = false, string noteValue = null, SoundManager soundManager = null) : base(initialPosition, endPoint, filename, speed, stationaryTimes, assetManager, colliderManager, screenManager, player, receptorBehaviour, noteValue, soundManager)
        {

        }

        public MovingPlatform_ABWrapAround_WithEmitterReceiver(Vector2 initialPosition, string movementDirection, string platformFilename, string emitterFilename, float speed, List<int> stationaryTimes, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player, bool receptorBehaviour = false, string noteValue = null, SoundManager soundManager = null) : base(initialPosition, movementDirection, platformFilename, speed, stationaryTimes, assetManager, colliderManager, screenManager, player, receptorBehaviour, noteValue, soundManager)
        {
            platformEmitter = new MovingPlatform(new List<Vector2>() { initialPosition, initialPosition }, emitterFilename, 0, new List<int>() { 0, 0 }, assetManager, colliderManager, screenManager, player);

        }


        public override void LoadContent()
        {
            base.LoadContent();
            platformEmitter?.LoadContent();
            platformReceiver?.LoadContent();

            if (platformEmitter != null)
            {
                platformEmitter.CollisionObject = true;
                platformEmitter.idleHitbox.isActive = true;
            }
            if (platformReceiver != null)
            {
                platformReceiver.CollisionObject = true;
                platformReceiver.idleHitbox.isActive = true;
            }

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            platformEmitter?.Update(gameTime);
            platformReceiver?.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            platformEmitter?.Draw(spriteBatch);
            platformReceiver?.Draw(spriteBatch);
        }


    }
}
