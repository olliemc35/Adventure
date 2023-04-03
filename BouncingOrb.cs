﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class BouncingOrb : MovingSprite
    {
        public float speed;

        // Angle is given between 0 and 2pi where 0 angle is measured as horizontally right
        public float angle;

        public Vector2 initialPosition = new Vector2();
        public float initialAngle;

        public ColliderManager colliderManager = new ColliderManager();

        public BouncingOrb() : base()
        {

        }

        public BouncingOrb(Vector2 initialPosition, string filename, float speed, float angle) : base(initialPosition, filename)
        {
            this.initialPosition = initialPosition;
            this.speed = speed;
            this.angle = angle;
            this.initialAngle = angle;
        }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            base.LoadContent(contentManager, graphicsDevice);
            idleHitbox.isActive = true;
        }

        public override void Update(GameTime gameTime)
        {
            spriteDisplacement.X = speed * (float)Math.Cos(angle);
            spriteDisplacement.Y = -speed * (float)Math.Sin(angle);

            spriteCollider.AdjustForCollisionsMovingSpriteAgainstListOfSprites(this, References.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);

            if (SpriteCollidedOnBottom)
            {
                angle -= (float)Math.PI / 2;
            }
            else if (SpriteCollidedOnTop)
            {
                angle += (float)Math.PI / 2;
            }

            if (spritePosition.X <= -8)
            {
                spritePosition = initialPosition;
                angle = initialAngle;
            }


            base.Update(gameTime);
        }







    }
}
