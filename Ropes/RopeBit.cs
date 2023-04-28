using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Aseprite;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Linq;


namespace Adventure
{
    public class RopeBit : MovingGameObject
    {
        public Vector2 totalForce = new Vector2(0, 0);

        public bool FirstLoopX = true;
        public bool FirstLoopY = true;

        public Vector2 throwDirection;
        public int RopeBitNumber;
        public bool ThisIsTheRopeAnchor = false;

        public List<Vector2> smoothingPositions = new List<Vector2>();
        public int smoothingParam = 10;

        public List<Vector2> previousPositions = new List<Vector2>();
        public int numberOfPreviousPositions;


        public bool activeAfterRest = false;
        public bool attached = false;


        public bool moveRopeBitOnMovingPlatform = false;

        public float DistancefromRopeAnchorWhenAtRest = 0;
        public float DistancefromNextRopeBitWhenAtRest = 0;
        public float DistancefromLasttRopeBitWhenAtRest = 0;
        public float MaxDistanceFromRopeAnchor = 0;
        public bool HaveIDeterminedTheDistanceYet = false;


        public Pivot pivotBetweenThisRopeBitandOneMinus = new Pivot();
        public Pivot pivotBetweenThisRopeBitandOnePlus = new Pivot();




        public List<Pivot> pivotsBetweenThisRopeBitandOneMinus = new List<Pivot>();
        public List<Pivot> pivotsBetweenThisRopeBitandOnePlus = new List<Pivot>();
        public bool pivotsBetweenThisRopeBitandOneMinusActive = false;
        public bool pivotsBetweenThisRopeBitandOnePlusActive = false;


        public bool isFixed = false;


        public RopeBit(Vector2 initialPosition, AssetManager assetManager) : base(initialPosition)
        {
            this.assetManager = assetManager;
            filename = "RedDot";
            gravityConstant = 750;
            mass = 0.8f;

            for (int k = 0; k < smoothingParam; k++)
            {
                smoothingPositions.Add(new Vector2(0, 0));
            }

            for (int k = 0; k < numberOfPreviousPositions; k++)
            {
                previousPositions.Add(new Vector2(0, 0));
            }
        }


        public RopeBit(Vector2 initialPosition, string filename, AssetManager assetManager) : base(initialPosition, filename, assetManager)
        {
            gravityConstant = 750;
            mass = 0.8f;
        }



        public RopeBit(Vector2 initialPosition, int numberOfPreviousPositions, AssetManager assetManager) : base(initialPosition)
        {
            this.assetManager = assetManager;
            filename = "RedDot";
            gravityConstant = 750;
            mass = 0.8f;
            this.numberOfPreviousPositions = numberOfPreviousPositions;

            for (int k = 0; k < smoothingParam; k++)
            {
                smoothingPositions.Add(new Vector2(0, 0));
            }

            for (int k = 0; k < numberOfPreviousPositions; k++)
            {
                previousPositions.Add(new Vector2(0, 0));
            }
        }

        public RopeBit(Vector2 initialPosition, Vector2 direction, AssetManager assetManager) : base(initialPosition)
        {
            this.assetManager = assetManager;
            filename = "RedDot";
            //constantVelocity.X = 120;
            //constantVelocity.Y = -128;
            gravityConstant = 750;
            mass = 0.8f;
            //gravityConstant = 256;
            throwDirection = direction;
            //References.activeScreen.screenSprites.Add(this);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            idleHitbox.rectangle.Width = (int)(((float)2 / 8) * animation_Idle.Width);
            idleHitbox.rectangle.Height = (int)(((float)2 / 8) * animation_Idle.Height);
            idleHitbox.offsetX = (int)(((float)3 / 8) * animation_Idle.Width);
            idleHitbox.offsetY = (int)(((float)3 / 8) * animation_Idle.Height);
            idleHitbox.rectangle.X += idleHitbox.offsetX;
            idleHitbox.rectangle.Y += idleHitbox.offsetY;
            idleHitbox.isActive = true;
            //animatedSprite_Idle.Play("Idle");
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (drawHitboxes)
            {
                foreach (HitboxRectangle hitbox in hitboxes)
                {
                    if (hitbox.isActive)
                    {

                        spriteBatch.Draw(idleHitbox.texture, hitbox.rectangle, Color.Red);
                    }
                }
            }

            animation_Idle.Draw(spriteBatch, drawPosition);
        }


        public void RemovePivots()
        {

            pivotsBetweenThisRopeBitandOneMinus.Clear();
            pivotsBetweenThisRopeBitandOnePlus.Clear();

        }



        public void FindGravityForce()
        {
            totalForce.Y += mass * gravityConstant;
        }

        public void FindNormalForces()
        {
            // These checks act like normal forces 

            if (CollidedOnBottom && totalForce.Y >= 0)
            {
                totalForce.Y = 0;
            }
            if (CollidedOnTop && totalForce.Y <= 0)
            {
                totalForce.Y = 0;
            }
            if (CollidedOnRight && totalForce.X >= 0)
            {
                totalForce.X = 0;
            }
            if (CollidedOnLeft && totalForce.X <= 0)
            {
                //totalForce.Y -= totalForce.X;
                totalForce.X = 0;
            }

        }

    }
}
