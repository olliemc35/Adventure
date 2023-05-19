using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class OrbEmitter : GameObject
    {

        public MovingPlatform_ABWrapAround orb;
        public List<AnimatedGameObject> orbReceptors = new List<AnimatedGameObject>();

        public OrbEmitter(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, List<int> stationaryTimes, AssetManager assetManager, ColliderManager colliderManager, Player player) : base()
        {
            orb = new MovingPlatform_ABWrapAround(initialPosition, endPoint , filename, speed, stationaryTimes, assetManager, colliderManager, player);
            orb.movePlayer = false;
            orbReceptors.Add(new AnimatedGameObject(initialPosition, "OrbReceptors", assetManager));
            orbReceptors.Add(new AnimatedGameObject(endPoint, "OrbReceptors", assetManager));

        }

        public override void LoadContent()
        {
            orb.LoadContent();
            //orb.CollisionObject = true;
            orb.idleHitbox.isActive = true;
            orb.Hazard = true;
            orbReceptors[0].LoadContent();
            orbReceptors[0].CollisionObject = true;
            orbReceptors[0].idleHitbox.isActive = true;
            orbReceptors[1].LoadContent();
            orbReceptors[1].CollisionObject = true;
            orbReceptors[1].idleHitbox.isActive = true;
        }

        public override void Update(GameTime gameTime)
        {
            orb.Update(gameTime);
            orbReceptors[0].Update(gameTime);
            orbReceptors[1].Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            orb.Draw(spriteBatch);
            orbReceptors[0].Draw(spriteBatch);
            orbReceptors[1].Draw(spriteBatch);
        }

        public override void HandleNoteTrigger()
        {
            orb.HandleNoteTrigger();
        }




    }
}
