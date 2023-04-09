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
    public class BouncingOrb : MovingGameObject
    {
        public float speed;

        // Angle is given between 0 and 2pi where 0 angle is measured as horizontally right
        public float angle;

        public Vector2 initialPosition = new Vector2();
        public float initialAngle;


        public BouncingOrb() : base()
        {

        }

        public BouncingOrb(Vector2 initialPosition, string filename, float speed, float angle, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager) : base(initialPosition, filename, assetManager)
        {
            this.initialPosition = initialPosition;
            this.screenManager = screenManager;
            this.colliderManager = colliderManager;
            this.speed = speed;
            this.angle = angle;
            initialAngle = angle;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            idleHitbox.isActive = true;
        }

        public override void Update(GameTime gameTime)
        {
            displacement.X = speed * (float)Math.Cos(angle);
            displacement.Y = -speed * (float)Math.Sin(angle);

            colliderManager.AdjustForCollisionsMovingSpriteAgainstListOfSprites(this, screenManager.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);

            if (CollidedOnBottom)
            {
                angle -= (float)Math.PI / 2;
            }
            else if (CollidedOnTop)
            {
                angle += (float)Math.PI / 2;
            }

            if (position.X <= -8)
            {
                position = initialPosition;
                angle = initialAngle;
            }


            base.Update(gameTime);
        }







    }
}
