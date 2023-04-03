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
    public class HangingRope : Rope
    {


        public HangingRope(Vector2 initialPosition, int NumberOfRopeBits, int LengthBetweenRopeBits) : base(initialPosition)
        {
            this.NumberOfRopeBits = NumberOfRopeBits;
            this.LengthBetweenRopeBits = LengthBetweenRopeBits;
            ropeAnchor = initialPosition;
            RopeLength = (NumberOfRopeBits - 1) * LengthBetweenRopeBits;


            distanceTilEquilibriumY = 1.1f * LengthBetweenRopeBits;
            distanceStretchRequiredToOvercomeFriction = 1.1f * LengthBetweenRopeBits;
            springConstant = DistanceToNearestInteger(rope[0].mass * rope[0].gravityConstant / (float)(distanceTilEquilibriumY - LengthBetweenRopeBits));
            groundFrictionNormalConstant = (float)(distanceStretchRequiredToOvercomeFriction - LengthBetweenRopeBits) / (distanceTilEquilibriumY - LengthBetweenRopeBits);


            for (int i = 0; i < NumberOfRopeBits; i++)
            {
                Vector2 position = new Vector2(initialPosition.X, initialPosition.Y + (NumberOfRopeBits - i - 1) * LengthBetweenRopeBits);
                RopeBit ropeBit = new RopeBit(position, "RedDot");
                rope.Add(ropeBit);

            }

            for (int i = 0; i < 2 * RopeLength; i++)
            {
                RopeBit ropeBit = new RopeBit(spritePosition);
                ropeBit.Enabled = false;
                ropeBit.spriteFilename = "YellowDot";
                ropeBit.idleHitbox.rectangle.Width = 1;
                ropeBit.idleHitbox.rectangle.Height = 1;
                ropeBit.idleHitbox.offsetX = 3;
                ropeBit.idleHitbox.offsetY = 3;
                ropeBitsDrawnOnScreen.Add(ropeBit);

            }

        }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            for (int i = 0; i < NumberOfRopeBits; i++)
            {
                rope[i].LoadContent(contentManager, graphicsDevice);

            }

            for (int i = 0; i < 2 * RopeLength; i++)
            {
                ropeBitsDrawnOnScreen[i].LoadContent(contentManager, graphicsDevice);
            }


        }


        public override void Update(GameTime gametime)
        {
            for (int i = 0; i <= IndexOfFirstEnabledRopeBit; i++)
            {
                if (Math.Abs(rope[i].spriteVelocity.X) < 0.001)
                {
                    rope[i].FirstLoopX = true;
                }
                if (Math.Abs(rope[i].spriteVelocity.Y) < 0.001)
                {
                    rope[i].FirstLoopY = true;
                }
            }

            SimulateRopeHanging();

            foreach (RopeBit ropeBit in rope)
            {
                ropeBit.Update(gametime);
            }

            base.Update(gametime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < NumberOfRopeBits; i++)
            {
                rope[i].animatedSprite.Render(spriteBatch);
            }

            base.Draw(spriteBatch);
        }


        public void SimulateRopeHanging()
        {

            rope[NumberOfRopeBits - 1].spritePosition = ropeAnchor;
            rope[NumberOfRopeBits - 1].idleHitbox.rectangle.X = (int)rope[NumberOfRopeBits - 1].spritePosition.X + rope[NumberOfRopeBits - 1].idleHitbox.offsetX;
            rope[NumberOfRopeBits - 1].idleHitbox.rectangle.Y = (int)rope[NumberOfRopeBits - 1].spritePosition.Y + rope[NumberOfRopeBits - 1].idleHitbox.offsetY;
            rope[NumberOfRopeBits - 1].spriteVelocity.X = 0;
            rope[NumberOfRopeBits - 1].spriteVelocity.Y = 0;
            rope[NumberOfRopeBits - 1].animatedSprite.Position = rope[NumberOfRopeBits - 1].spritePosition;

            for (int i = 0; i < NumberOfRopeBits - 1; i++)
            {

                rope[i].totalForce.X = 0;
                rope[i].totalForce.Y = 0;
                rope[i].previousSpriteVelocity = rope[i].spriteVelocity;
                rope[i].previousSpritePosition = rope[i].spritePosition;

            }



            for (int i = NumberOfRopeBits - 2; i >= 0; i--)
            {
                if (Vector2.Distance(rope[i].spritePosition, rope[i + 1].spritePosition) >= LengthBetweenRopeBits)
                {
                    FindSpringForcesPairWise(rope[i], rope[i + 1], LengthBetweenRopeBits);
                    FindSpringFrictionPairWise(rope[i], rope[i + 1]);
                }
            }

            for (int i = NumberOfRopeBits - 2; i >= 0; i--)
            {

                rope[i].FindGravityForce();
                FindGroundFrictionWithNormal(rope[i]);
                FindAirFrictionEULER(rope[i]);
                rope[i].FindNormalForces();
                FindVelocityAndDisplacementForRopeBitX(i);
                FindVelocityAndDisplacementForRopeBitY(i);

            }

            spriteCollider.AdjustForRopeAgainstListOfSpritesBackward(this, References.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);

        }









    }
}
