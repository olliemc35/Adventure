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
    public class RopeBit : MovingSprite
    {
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


        public RopeBit(Vector2 initialPosition) : base(initialPosition)
        {
            spriteFilename = "RedDot";
            terminalSpeedY = 30f;
            constantVelocity.X = 0;
            constantVelocity.Y = 0;
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


        public RopeBit(Vector2 initialPosition, string filename) : base(initialPosition, filename)
        {
            terminalSpeedY = 30f;
            constantVelocity.X = 0;
            constantVelocity.Y = 0;
            gravityConstant = 750;
            mass = 0.8f;
        }



        public RopeBit(Vector2 initialPosition, int numberOfPreviousPositions) : base(initialPosition)
        {
            spriteFilename = "RedDot";
            terminalSpeedY = 30f;
            constantVelocity.X = 0;
            constantVelocity.Y = 0;
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

        public RopeBit(Vector2 initialPosition, Vector2 direction) : base(initialPosition)
        {
            spriteFilename = "RedDot";
            terminalSpeedY = 30f;
            //constantVelocity.X = 120;
            //constantVelocity.Y = -128;
            gravityConstant = 750;
            mass = 0.8f;
            //gravityConstant = 256;
            throwDirection = direction;
            constantVelocity.X = 120 * throwDirection.X;
            constantVelocity.Y = 360 * throwDirection.Y;
            //References.activeScreen.screenSprites.Add(this);
        }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            base.LoadContent(contentManager, graphicsDevice);

            idleHitbox.rectangle.Width = (int)(((float)2 / 8) * baseFrame.Width);
            idleHitbox.rectangle.Height = (int)(((float)2 / 8) * baseFrame.Height);
            idleHitbox.offsetX = (int)(((float)3 / 8) * baseFrame.Width);
            idleHitbox.offsetY = (int)(((float)3 / 8) * baseFrame.Height);
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
                foreach (HitboxRectangle hitbox in spriteHitboxes)
                {
                    if (hitbox.isActive)
                    {

                        spriteBatch.Draw(spriteHitboxTexture, hitbox.rectangle, Color.Red);
                    }
                }
            }

            animation_Idle.Draw(spriteBatch, animationPosition);
        }


        public void RemovePivots()
        {

            pivotsBetweenThisRopeBitandOneMinus.Clear();
            pivotsBetweenThisRopeBitandOnePlus.Clear();

        }





    }
}
