using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class Bullet : MovingSprite
    {

        public List<Rectangle> bulletPath = new List<Rectangle>();
        public bool remove = false;
        public Vector2 collisionPoint = new Vector2();

        public Vector2 direction = new Vector2();
        public float speed = 300;

        public Bullet(Vector2 initialPosition, string str, List<Rectangle> bulletPath) : base(initialPosition, str)
        {
            this.bulletPath = bulletPath.ToList();
        }

        public Bullet(Vector2 initialPosition, string str, Vector2 endPoint) : base(initialPosition, str)
        {
            direction = endPoint - initialPosition;
            direction.Normalize();
            deltaTime = 1f / 60;
        }

        public override void Update(GameTime gameTime)
        {
            spriteVelocity = speed * direction;
            spriteDisplacement = spriteVelocity * deltaTime;
            spriteCollider.AdjustForCollisionsMovingSpriteAgainstListOfSprites(this, References.activeScreen.hitboxesForAimLine, 1, 10);

            if (SpriteCollidedOnRight)
            {
                collisionPoint.X = DistanceToNearestInteger(spritePosition.X + 1);
                collisionPoint.Y = DistanceToNearestInteger(spritePosition.Y);
                remove = true;
                UpdateEnemyColorMap(collisionPoint);
            }
            else if (SpriteCollidedOnLeft)
            {
                collisionPoint.X = DistanceToNearestInteger(spritePosition.X - 1);
                collisionPoint.Y = DistanceToNearestInteger(spritePosition.Y);
                remove = true;
                UpdateEnemyColorMap(collisionPoint);
            }
            else if (SpriteCollidedOnBottom)
            {
                collisionPoint.X = DistanceToNearestInteger(spritePosition.X);
                collisionPoint.Y = DistanceToNearestInteger(spritePosition.Y + 1);
                remove = true;
                UpdateEnemyColorMap(collisionPoint);
            }
            else if (SpriteCollidedOnTop)
            {
                collisionPoint.X = DistanceToNearestInteger(spritePosition.X);
                collisionPoint.Y = DistanceToNearestInteger(spritePosition.Y - 1);
                remove = true;
                UpdateEnemyColorMap(collisionPoint);
            }



            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }



        // This codes is now incorrect - have to think about the right co-ords on the color map
        public void UpdateEnemyColorMap(Vector2 collisionPoint)
        {
            if (References.activeScreen.screenNotes.Count > 0)
            {

                foreach (Note note in References.activeScreen.screenNotes)
                {
                    bool targetThisWeight = false;


                    //if (note.key is HangingRopeWithWeightAttached bell)
                    //{
                    //    foreach (List<HitboxRectangle> hitboxes in bell.weight.hitboxesForGunlineForEachFrame)
                    //    {
                    //        foreach (HitboxRectangle hitbox in hitboxes)
                    //        {
                    //            if (hitbox.rectangle.Contains(collisionPoint))
                    //            {
                    //                targetThisWeight = true;
                    //                break;
                    //            }
                    //        }
                    //        if (targetThisWeight)
                    //        {
                    //            break;
                    //        }
                    //    }

                    //    if (targetThisWeight)
                    //    {
                    //        bell.giveImpulse = true;
                    //        Vector2 unitVec = new Vector2(0, -1);
                    //        bell.impulseAngle = (float)Math.Acos(Vector2.Dot(direction, unitVec));

                    //        if (collisionPoint.X > bell.weight.spritePosition.X + 0.5f * bell.weight.idleHitbox.rectangle.Width)
                    //        {
                    //            bell.impulseAngle *= -1;
                    //        }

                    //        if (!note.playerInteractedWith)
                    //        {
                    //            note.playerInteractedWith = true;
                    //            note.flagPlayerInteractedWith = true;
                    //            note.keyPlayInteractedAnimation = true;
                    //        }

                    //    }
                    //}

                }

            }

            //foreach (HangingRopeWithWeightAttached rope in References.activeScreen.screenRopesWithWeights)
            //{
            //    bool targetThisWeight = false;

            //    foreach (List<HitboxRectangle> hitboxes in rope.weight.hitboxesForGunlineForEachFrame)
            //    {
            //        foreach (HitboxRectangle hitbox in hitboxes)
            //        {
            //            if (hitbox.rectangle.Contains(collisionPoint))
            //            {
            //                targetThisWeight = true;
            //                break;
            //            }
            //        }
            //        if (targetThisWeight)
            //        {
            //            break;
            //        }
            //    }

            //    if (targetThisWeight)
            //    {
            //        rope.giveImpulse = true;

            //        //Vector2 unitVectorPointingToRopeHanging = new Vector2((float)Math.Sin(rope.weight.swingAngle), (float)Math.Cos(rope.weight.swingAngle));
            //        Vector2 unitVec = new Vector2(0, -1);
            //        rope.impulseAngle = (float)Math.Acos(Vector2.Dot(direction, unitVec));

            //        //rope.impulseAngle = (float)Math.Acos(Vector2.Dot(direction, unitVectorPointingToRopeHanging));

            //        if (collisionPoint.X > rope.weight.spritePosition.X + 0.5f * rope.weight.idleHitbox.rectangle.Width)
            //        {
            //            rope.impulseAngle *= -1;
            //        }

            //        //if (collisionPoint.X <= rope.weight.spritePosition.X + rope.weight.idleHitbox.offsetX + 0.5f * rope.weight.idleHitbox.rectangle.Width)
            //        //{
            //        //    rope.impulseDirection = 1;
            //        //}
            //        //else
            //        //{
            //        //    rope.impulseDirection = -1;
            //        //}

            //    }

            //}

            //foreach (Enemy enemy in References.activeScreen.screenEnemies)
            //{
            //    bool targetThisEnemy = false;

            //    foreach (List<HitboxRectangle> hitboxes in enemy.hitboxesForGunlineForEachFrame)
            //    {
            //        foreach (HitboxRectangle hitbox in hitboxes)
            //        {
            //            if (!enemy.Dead && hitbox.rectangle.Contains(collisionPoint))
            //            {
            //                targetThisEnemy = true;
            //                break;
            //            }
            //        }
            //        if (targetThisEnemy)
            //        {
            //            break;
            //        }
            //    }

            //    if (targetThisEnemy)
            //    {
            //        enemy.HandleGunshot(collisionPoint);
            //    }

            //}
        }
    }
}
