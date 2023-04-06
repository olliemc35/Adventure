using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Aseprite;
using MonoGame.Aseprite.Sprites;
using MonoGame.Aseprite.Content.Processors;

namespace Adventure
{
    public class ColliderManager
    {
        public ColliderManager() { }

        //CheckForCollision will take two sprites and return true iff their hitboxes overlap or meet in any edges



        public bool CheckForCollision(HitboxRectangle sprite1hitbox, HitboxRectangle sprite2hitbox)
        {
            Rectangle rect1 = sprite1hitbox.rectangle;
            Rectangle rect2 = sprite2hitbox.rectangle;

            // Check for overlap
            if (rect1.Intersects(rect2)) { return true; }


            return CheckForEdgesMeeting(sprite1hitbox, sprite2hitbox);


        }

        public bool CheckForOverlap(HitboxRectangle sprite1hitbox, HitboxRectangle sprite2hitbox)
        {
            Rectangle rect1 = sprite1hitbox.rectangle;
            Rectangle rect2 = sprite2hitbox.rectangle;

            // Check for overlap
            return rect1.Intersects(rect2);


        }

        // Double check this
        public bool CheckForEdgesMeeting(HitboxRectangle sprite1hitbox, HitboxRectangle sprite2hitbox)
        {
            Rectangle rect1 = sprite1hitbox.rectangle;
            Rectangle rect2 = sprite2hitbox.rectangle;

            if (Rectangle.Intersect(rect1, rect2) != Rectangle.Empty)
            {
                return false;
            }
            else
            {
                rect1.Y += 1;
                if (Rectangle.Intersect(rect1, rect2) != Rectangle.Empty)
                {
                    return true;
                }
                rect1.Y -= 2;
                if (Rectangle.Intersect(rect1, rect2) != Rectangle.Empty)
                {
                    return true;
                }
                rect1.Y += 1;
                rect1.X += 1;
                if (Rectangle.Intersect(rect1, rect2) != Rectangle.Empty)
                {
                    return true;
                }
                rect1.X -= 2;
                if (Rectangle.Intersect(rect1, rect2) != Rectangle.Empty)
                {
                    return true;
                }

                return false;
            }


        }


        // Assume there is a collision and return the rectangle of intersection
        public Rectangle IntersectingRectangle(Rectangle rect1, Rectangle rect2)
        {
            int leftX = Math.Max(rect1.X, rect2.X);
            int rightX = Math.Min(rect1.X + rect1.Width, rect2.X + rect2.Width);
            int topY = Math.Max(rect1.Y, rect2.Y);
            int bottomY = Math.Min(rect1.Y + rect1.Height, rect2.Y + rect2.Height);
            Rectangle intersectRect = new Rectangle(leftX, topY, rightX - leftX, bottomY - topY);
            return intersectRect;

        }


        public bool CheckIfHitboxIsContainedInAListOfHitboxes(HitboxRectangle hitbox, List<HitboxRectangle> listOfHitboxes)
        {
            bool contains = false;

            foreach (HitboxRectangle hitbox2 in listOfHitboxes)
            {
                contains = hitbox2.rectangle.Contains(hitbox.rectangle);

                if (contains)
                {
                    return true;
                }
            }

            return false;
        }




        public void UpdateSpriteColliderBools(MovingGameObject movingSprite)
        {
            foreach (HitboxRectangle hitbox in movingSprite.hitboxes)
            {
                if (hitbox.CollidedOnBottom)
                {
                    movingSprite.CollidedOnBottom = true;
                }
                if (hitbox.CollidedOnTop)
                {
                    movingSprite.CollidedOnTop = true;
                }
                if (hitbox.CollidedOnRight)
                {
                    movingSprite.CollidedOnRight = true;
                }
                if (hitbox.CollidedOnLeft)
                {
                    movingSprite.CollidedOnLeft = true;
                }

            }

        }


        public void ResetColliderBoolsForSprite(MovingGameObject sprite)
        {
            sprite.CollidedOnRight = false;
            sprite.CollidedOnLeft = false;
            sprite.CollidedOnTop = false;
            sprite.CollidedOnBottom = false;
        }


        public void ResetColliderBoolsForHitbox(HitboxRectangle hitbox)
        {
            hitbox.CollidedOnRight = false;
            hitbox.CollidedOnLeft = false;
            hitbox.CollidedOnTop = false;
            hitbox.CollidedOnBottom = false;
        }

        public void ResetGlobalColliderBoolsForHitbox(HitboxRectangle hitbox)
        {
            hitbox.GlobalCollidedOnRight = false;
            hitbox.GlobalCollidedOnLeft = false;
            hitbox.GlobalCollidedOnTop = false;
            hitbox.GlobalCollidedOnBottom = false;
        }


        public int DistanceToNearestInteger(float x)
        {
            if (Math.Ceiling(x) - x <= 0.5)
            {
                return (int)Math.Ceiling(x);
            }
            else
            {
                return (int)Math.Floor(x);
            }

        }



        public void UpdateHitboxRectangleColliderBools2(HitboxRectangle sprite1hitbox, HitboxRectangle sprite2hitbox)
        {
            Rectangle rect1 = sprite1hitbox.rectangle;
            Rectangle rect2 = sprite2hitbox.rectangle;


            rect1.Y += 1;
            if (Rectangle.Intersect(rect1, rect2) != Rectangle.Empty)
            {
                sprite1hitbox.CollidedOnBottom = true;
            }
            rect1.Y -= 2;
            if (Rectangle.Intersect(rect1, rect2) != Rectangle.Empty)
            {
                sprite1hitbox.CollidedOnTop = true;
            }
            rect1.Y += 1;
            rect1.X += 1;
            if (Rectangle.Intersect(rect1, rect2) != Rectangle.Empty)
            {
                sprite1hitbox.CollidedOnRight = true;
            }
            rect1.X -= 2;
            if (Rectangle.Intersect(rect1, rect2) != Rectangle.Empty)
            {
                sprite1hitbox.CollidedOnLeft = true;
            }






        }


        public void UpdateHitboxRectangleColliderBools(HitboxRectangle hitbox, HitboxRectangle secondHitbox)
        {
            Rectangle rect1 = hitbox.rectangle;
            Rectangle rect2 = secondHitbox.rectangle;
            //Rectangle intersectingRect = Rectangle.Intersect(rect1, rect2);
            Rectangle intersectingRect = IntersectingRectangle(rect1, rect2);


            if (rect1.Center.X < intersectingRect.Center.X && intersectingRect.Height > 0)
            {
                hitbox.CollidedOnRight = true;
            }

            if (rect1.Center.X > intersectingRect.Center.X && intersectingRect.Height > 0)
            {
                hitbox.CollidedOnLeft = true;
            }

            if (rect1.Center.Y < intersectingRect.Center.Y && intersectingRect.Width > 0)
            {
                hitbox.CollidedOnBottom = true;
            }

            if (rect1.Center.Y > intersectingRect.Center.Y && intersectingRect.Width > 0)
            {
                hitbox.CollidedOnTop = true;
            }
        }



        public void AdjustForCollisionsMovingSpriteAgainstListOfSprites(MovingGameObject movingSprite, List<HitboxRectangle> hitboxes, int seeFurtherAhead, int acc)
        {
            // We first reset everything
            ResetColliderBoolsForSprite(movingSprite);
            ResetColliderBoolsForHitbox(movingSprite.idleHitbox);


            // Based on input find where the sprite would be on the next frame if there was no collision
            Vector2 positionOnNextFrameIfNoCollision = new Vector2
            {
                X = movingSprite.position.X + movingSprite.displacement.X,
                Y = movingSprite.position.Y + movingSprite.displacement.Y
                //X = DistanceToNearestInteger(movingSprite.spritePosition.X + movingSprite.spriteDisplacement.X),
                //Y = DistanceToNearestInteger(movingSprite.spritePosition.Y + movingSprite.spriteDisplacement.Y)
            };


            // Move idle hitbox to the current position

            movingSprite.idleHitbox.rectangle.X = DistanceToNearestInteger(movingSprite.position.X) + movingSprite.idleHitbox.offsetX;
            movingSprite.idleHitbox.rectangle.Y = DistanceToNearestInteger(movingSprite.position.Y) + movingSprite.idleHitbox.offsetY;


            // We create a list of "ghostPositions" and "ghostHitboxes"
            // These are places we could be between now and the next frame.

            List<Vector2> ghostPositions = new List<Vector2>();
            List<HitboxRectangle> ghostHitboxes = new List<HitboxRectangle>();

            for (int j = 0; j <= seeFurtherAhead * acc; j++)
            {

                Vector2 ghostPosition = new Vector2(movingSprite.position.X + movingSprite.displacement.X * ((float)j / acc), movingSprite.position.Y + movingSprite.displacement.Y * ((float)j / acc));
                ghostPositions.Add(ghostPosition);

            }

            for (int j = 0; j < ghostPositions.Count(); j++)
            {
                HitboxRectangle ghostHitbox = new HitboxRectangle(DistanceToNearestInteger(ghostPositions[j].X) + movingSprite.idleHitbox.offsetX, DistanceToNearestInteger(ghostPositions[j].Y) + movingSprite.idleHitbox.offsetY, movingSprite.idleHitbox.rectangle.Width, movingSprite.idleHitbox.rectangle.Height)
                {
                    offsetX = movingSprite.idleHitbox.offsetX,
                    offsetY = movingSprite.idleHitbox.offsetY,
                    isActive = true
                };

                ghostHitboxes.Add(ghostHitbox);
            }

            // Our method takes each terrain hitbox in turn and checks whether we collide with each one - here by collision we mean OVERLAP (not edges meeting).
            // If we do we create a new hitbox at the at the first ghostHitbox positions for which there was a collision, 
            // after adjusting this position along the displacement vector so that the edges are just meeting.
            // We then add a copy of this hitbox to the following list:
            List<HitboxRectangle> possibleHitboxesIfCollisionDetected = new List<HitboxRectangle>();


            foreach (HitboxRectangle hitbox2 in hitboxes)
            {
                for (int j = 0; j < ghostHitboxes.Count(); j++)
                {

                    if (hitbox2.isActive)
                    {
                        if (CheckForOverlap(ghostHitboxes[j], hitbox2))
                        {
                            Vector2 startingVec = new Vector2
                            {
                                X = ghostHitboxes[j].rectangle.X,
                                Y = ghostHitboxes[j].rectangle.Y
                            };

                            Vector2 displacementVec = new Vector2();

                            if (j == 0)
                            {
                                displacementVec.X = DistanceToNearestInteger(movingSprite.position.X) + movingSprite.idleHitbox.offsetX - ghostHitboxes[j].rectangle.X;
                                displacementVec.Y = DistanceToNearestInteger(movingSprite.position.Y) + movingSprite.idleHitbox.offsetY - ghostHitboxes[j].rectangle.Y;
                            }
                            else
                            {
                                displacementVec.X = ghostHitboxes[j - 1].rectangle.X - ghostHitboxes[j].rectangle.X;
                                displacementVec.Y = ghostHitboxes[j - 1].rectangle.Y - ghostHitboxes[j].rectangle.Y;

                            }

                            for (int k = 1; k <= acc; k++)
                            {
                                ghostHitboxes[j].rectangle.X = DistanceToNearestInteger(startingVec.X + ((float)k / acc) * displacementVec.X);
                                ghostHitboxes[j].rectangle.Y = DistanceToNearestInteger(startingVec.Y + ((float)k / acc) * displacementVec.Y);

                                if (!CheckForOverlap(ghostHitboxes[j], hitbox2))
                                {
                                    // I have moved a sufficient distance away so that there is no collision 
                                    // Now the aim is to move it closer so that we are just touching (this will count as a collision)
                                    // The idea is that the first collision we detect will be when we are just touching

                                    if (CheckForEdgesMeeting(ghostHitboxes[j], hitbox2))
                                    {
                                        HitboxRectangle newHitbox = new HitboxRectangle(ghostHitboxes[j].rectangle.X, ghostHitboxes[j].rectangle.Y, ghostHitboxes[j].rectangle.Width, ghostHitboxes[j].rectangle.Height);
                                        newHitbox.offsetX = ghostHitboxes[j].offsetX;
                                        newHitbox.offsetY = ghostHitboxes[j].offsetY;
                                        possibleHitboxesIfCollisionDetected.Add(newHitbox);

                                        ghostHitboxes[j].rectangle.X = (int)startingVec.X;
                                        ghostHitboxes[j].rectangle.Y = (int)startingVec.Y;
                                    }
                                    else
                                    {
                                        // we never reach here by looks of things?
                                        //Debug.WriteLine("here");
                                        // Maybe here we should set ghostHitboxes[j] to ghostHitboxes[j-1] and treat this as a bad case we should never be in
                                        for (int l = 1; l <= acc; l++)
                                        {

                                            ghostHitboxes[j].rectangle.X -= Math.Sign(displacementVec.X);
                                            ghostHitboxes[j].rectangle.Y -= Math.Sign(displacementVec.Y);

                                            if (CheckForCollision(ghostHitboxes[j], hitbox2))
                                            {


                                                HitboxRectangle newHitbox = new HitboxRectangle(ghostHitboxes[j].rectangle.X, ghostHitboxes[j].rectangle.Y, ghostHitboxes[j].rectangle.Width, ghostHitboxes[j].rectangle.Height);
                                                newHitbox.offsetX = ghostHitboxes[j].offsetX;
                                                newHitbox.offsetY = ghostHitboxes[j].offsetY;
                                                possibleHitboxesIfCollisionDetected.Add(newHitbox);

                                                ghostHitboxes[j].rectangle.X = (int)startingVec.X;
                                                ghostHitboxes[j].rectangle.Y = (int)startingVec.Y;

                                                break;
                                            }
                                        }
                                    }


                                    break;

                                }




                            }


                            break;



                        }
                    }

                }

            }


            // First let us treat the case that there has been no collisions
            // The hitbox we are now interested in is the last hitbox of ghostHitboxes

            if (possibleHitboxesIfCollisionDetected.Count() == 0)
            {
                foreach (HitboxRectangle hitbox2 in hitboxes)
                {
                    if (hitbox2.isActive)
                    {
                        if (CheckForCollision(ghostHitboxes.Last(), hitbox2))
                        {
                            ResetColliderBoolsForHitbox(ghostHitboxes.Last());
                            UpdateHitboxRectangleColliderBools2(ghostHitboxes.Last(), hitbox2);

                            if (ghostHitboxes.Last().CollidedOnBottom)
                            {
                                movingSprite.CollidedOnBottom = true;
                                movingSprite.velocity.Y = 0;
                                movingSprite.displacement.Y = 0;
                                //movingSprite.timeY = 0;
                            }
                            if (ghostHitboxes.Last().CollidedOnTop)
                            {
                                movingSprite.CollidedOnTop = true;
                                movingSprite.velocity.Y = 0;
                                movingSprite.displacement.Y = 0;
                                //movingSprite.timeY = 0;
                            }
                            if (ghostHitboxes.Last().CollidedOnRight)
                            {
                                movingSprite.CollidedOnRight = true;
                                movingSprite.velocity.X = 0;
                                movingSprite.displacement.X = 0;
                                //movingSprite.timeX = 0;
                            }
                            if (ghostHitboxes.Last().CollidedOnLeft)
                            {
                                movingSprite.CollidedOnLeft = true;
                                movingSprite.velocity.X = 0;
                                movingSprite.displacement.X = 0;
                                //movingSprite.timeX = 0;
                            }
                        }

                    }


                }

                movingSprite.position.X = positionOnNextFrameIfNoCollision.X;
                movingSprite.position.Y = positionOnNextFrameIfNoCollision.Y;


            }
            else
            {
                // In this case there has been collisions.
                // We are interested in the hitbox which is closest to the movingSprite so we first find that
                // and store the index as keyIndex.
                //Debug.WriteLine("here");
                int keyIndex = 0;

                Vector2 positionOnNextFrame = new Vector2
                {
                    X = possibleHitboxesIfCollisionDetected[0].rectangle.X - possibleHitboxesIfCollisionDetected[0].offsetX,
                    Y = possibleHitboxesIfCollisionDetected[0].rectangle.Y - possibleHitboxesIfCollisionDetected[0].offsetY
                };

                if (possibleHitboxesIfCollisionDetected.Count() == 1)
                {
                    keyIndex = 0;
                }
                else if (possibleHitboxesIfCollisionDetected.Count() > 1)
                {
                    for (int i = 1; i < possibleHitboxesIfCollisionDetected.Count(); i++)
                    {
                        Vector2 possiblePositionOnNextFrame = new Vector2(possibleHitboxesIfCollisionDetected[i].rectangle.X - possibleHitboxesIfCollisionDetected[i].offsetX, possibleHitboxesIfCollisionDetected[i].rectangle.Y - possibleHitboxesIfCollisionDetected[i].offsetY);

                        if (Vector2.Distance(movingSprite.position, possiblePositionOnNextFrame) <= Vector2.Distance(movingSprite.position, positionOnNextFrame))
                        {
                            positionOnNextFrame = possiblePositionOnNextFrame;
                            keyIndex = i;
                        }
                    }
                }



                foreach (HitboxRectangle hitbox2 in hitboxes)
                {

                    if (hitbox2.isActive)
                    {
                        if (CheckForCollision(possibleHitboxesIfCollisionDetected[keyIndex], hitbox2))
                        {
                            ResetColliderBoolsForHitbox(possibleHitboxesIfCollisionDetected[keyIndex]);
                            UpdateHitboxRectangleColliderBools2(possibleHitboxesIfCollisionDetected[keyIndex], hitbox2);

                            if (possibleHitboxesIfCollisionDetected[keyIndex].CollidedOnBottom)
                            {
                                movingSprite.CollidedOnBottom = true;
                                movingSprite.velocity.Y = 0;
                                movingSprite.displacement.Y = 0;
                                //movingSprite.timeY = 0;
                            }
                            if (possibleHitboxesIfCollisionDetected[keyIndex].CollidedOnTop)
                            {
                                movingSprite.CollidedOnTop = true;
                                movingSprite.velocity.Y = 0;
                                movingSprite.displacement.Y = 0;
                                //movingSprite.timeY = 0;
                            }
                            if (possibleHitboxesIfCollisionDetected[keyIndex].CollidedOnRight)
                            {
                                movingSprite.CollidedOnRight = true;
                                movingSprite.velocity.X = 0;
                                movingSprite.displacement.X = 0;
                                //movingSprite.timeX = 0;
                            }
                            if (possibleHitboxesIfCollisionDetected[keyIndex].CollidedOnLeft)
                            {
                                movingSprite.CollidedOnLeft = true;
                                movingSprite.velocity.X = 0;
                                movingSprite.displacement.X = 0;
                                //movingSprite.timeX = 0;
                            }
                        }
                    }

                }



                movingSprite.position.X = possibleHitboxesIfCollisionDetected[keyIndex].rectangle.X - possibleHitboxesIfCollisionDetected[keyIndex].offsetX;
                movingSprite.position.Y = possibleHitboxesIfCollisionDetected[keyIndex].rectangle.Y - possibleHitboxesIfCollisionDetected[keyIndex].offsetY;

            }


            movingSprite.idleHitbox.rectangle.X = DistanceToNearestInteger(movingSprite.position.X) + movingSprite.idleHitbox.offsetX;
            movingSprite.idleHitbox.rectangle.Y = DistanceToNearestInteger(movingSprite.position.Y) + movingSprite.idleHitbox.offsetY;

        }


        // Essentially, if our next move takes us OFF the platform we wish to not move at all
        // We may want to fine-tune this into the collision detection proper, so that 
        // we move along the displacement vector the right amount until we no longer hit etc
        // This will be an easy fix - perhaps better to refactor the code above first
        // (Basically discard a ghost hitbox if it has no collision with the platform.)
        public void AdjustForCollisionWithClimable(MovingGameObject movingSprite, AnimatedGameObject sprite, List<HitboxRectangle> hitboxes, int seeFurtherAhead, int acc)
        {

            // We first reset everything
            ResetColliderBoolsForSprite(movingSprite);
            ResetColliderBoolsForHitbox(movingSprite.idleHitbox);

            // Move idle hitbox to the next position

            movingSprite.idleHitbox.rectangle.X = DistanceToNearestInteger(movingSprite.position.X + movingSprite.displacement.X) + movingSprite.idleHitbox.offsetX;
            movingSprite.idleHitbox.rectangle.Y = DistanceToNearestInteger(movingSprite.position.Y + movingSprite.displacement.Y) + movingSprite.idleHitbox.offsetY;

            if (!CheckForCollision(movingSprite.idleHitbox, sprite.idleHitbox))
            {
                movingSprite.displacement.X = 0;
                movingSprite.displacement.Y = 0;
            }


            AdjustForCollisionsMovingSpriteAgainstListOfSprites(movingSprite, hitboxes, seeFurtherAhead, acc);
        }


        // ROPE COLLISION STUFF



        public void AdjustForRopeAgainstListOfSpritesForward(Rope rope, List<HitboxRectangle> hitboxes, int seeFurtherAhead, int acc)
        {
            for (int i = 0; i <= rope.IndexOfFirstEnabledRopeBit; i++)
            {
                if (rope.rope[i].pivotsBetweenThisRopeBitandOnePlus.Count() != 0)
                {
                    rope.rope[i].velocity.X = 0;
                    rope.rope[i].velocity.Y = 0;
                    rope.rope[i].displacement.X = 0;
                    rope.rope[i].displacement.Y = 0;
                }

                rope.rope[i].previousPosition = rope.rope[i].position;
                AdjustForCollisionsMovingSpriteAgainstListOfSprites(rope.rope[i], hitboxes, seeFurtherAhead, acc);
            }

            for (int i = 0; i <= rope.IndexOfFirstEnabledRopeBit; i++)
            {
                if (i != rope.IndexOfFirstRopeBitAnchor)
                {
                    if (rope.rope[i].pivotsBetweenThisRopeBitandOnePlus.Count() != 0 && rope.rope[i + 1].pivotsBetweenThisRopeBitandOneMinus.Count() != 0)
                    {
                        RemoveUnnecessaryPivotsForwards(rope, i, hitboxes);

                        if (rope.rope[i].pivotsBetweenThisRopeBitandOnePlus.Count() == 0)
                        {
                            continue;
                        }



                        float distanceToMove = (rope.rope[i].position - rope.rope[i].previousPosition).Length();

                        // Find the pivot point closest to us - this is the one we want to move towards (first)
                        Pivot pivot = rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[0];

                        for (int k = 0; k <= rope.rope[i].pivotsBetweenThisRopeBitandOnePlus.Count() - 1; k++)
                        {
                            if (Vector2.Distance(rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[k].position, rope.rope[i + 1].position) < Vector2.Distance(pivot.position, rope.rope[i + 1].position))
                            {
                                pivot = rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[k];
                            }

                        }

                        // We really want the direction between the points drawn as on screen - this is why we take integers
                        Vector2 direction = new Vector2
                        {
                            X = pivot.position.X - (rope.rope[i + 1].position.X + rope.rope[i + 1].idleHitbox.offsetX),
                            Y = pivot.position.Y - (rope.rope[i + 1].position.Y + rope.rope[i + 1].idleHitbox.offsetY)
                        };

                        // This means that we are on the pivot point - we will now remove the pivot as soon as we move away
                        if (direction.X == 0 && direction.Y == 0)
                        {
                            pivot.aboutToBeRemoved = true;

                            if (pivot.TopRight && rope.rope[i].position.Y <= rope.rope[i + 1].position.Y)
                            {
                                direction.X = -1;
                                direction.Y = 0;
                            }
                            else if (pivot.TopLeft && rope.rope[i].position.Y <= rope.rope[i + 1].position.Y)
                            {
                                direction.X = 1;
                                direction.Y = 0;
                            }
                            else if (pivot.TopRight && rope.rope[i].position.Y >= rope.rope[i + 1].position.Y)
                            {
                                direction.X = 0;
                                direction.Y = 1;
                            }
                            else if (pivot.TopLeft && rope.rope[i].position.Y >= rope.rope[i + 1].position.Y)
                            {
                                direction.X = 0;
                                direction.Y = 1;
                            }
                            else if (pivot.BottomRight && rope.rope[i].position.Y >= rope.rope[i + 1].position.Y)
                            {
                                direction.X = -1;
                                direction.Y = 0;
                            }
                            else if (pivot.BottomLeft && rope.rope[i].position.Y >= rope.rope[i + 1].position.Y)
                            {
                                direction.X = 1;
                                direction.Y = 0;
                            }
                            else if (pivot.BottomRight && rope.rope[i].position.Y <= rope.rope[i + 1].position.Y)
                            {
                                direction.X = 0;
                                direction.Y = -1;
                            }
                            else if (pivot.BottomLeft && rope.rope[i].position.Y <= rope.rope[i + 1].position.Y)
                            {
                                direction.X = 0;
                                direction.Y = -1;
                            }
                        }

                        if (direction.Length() > 0.001)
                        {
                            direction.Normalize();
                        }


                        // I only want to move if I move AWAY from the pivot.
                        if (Vector2.Distance(rope.rope[i].position, pivot.position) <= Vector2.Distance(rope.rope[i].previousPosition, pivot.position))
                        {
                            distanceToMove = 0;
                        }

                        rope.rope[i + 1].displacement = distanceToMove * direction;

                        AdjustForCollisionsMovingSpriteAgainstListOfSprites(rope.rope[i + 1], hitboxes, seeFurtherAhead, acc);


                        // Sometimes I would want to move but would get caught on a corner - say direction.X is very small after normalising 
                        // then we hardly move in the X direction, even though we would like to move at least one pixel
                        // The code chunk below fixes this

                        if (!pivot.aboutToBeRemoved && Math.Abs(distanceToMove) > 0.001)
                        {
                            if (direction.X > 0)
                            {
                                if (Math.Abs(rope.rope[i + 1].position.X - rope.rope[i + 1].previousPosition.X) < 0.001)
                                {
                                    rope.rope[i + 1].displacement.X = 1;
                                    rope.rope[i + 1].displacement.Y = 0;
                                    AdjustForCollisionsMovingSpriteAgainstListOfSprites(rope.rope[i + 1], hitboxes, seeFurtherAhead, acc);

                                }
                            }
                            if (direction.X < 0)
                            {
                                if (Math.Abs(rope.rope[i + 1].position.X - rope.rope[i + 1].previousPosition.X) < 0.001)
                                {
                                    rope.rope[i + 1].displacement.X = -1;
                                    rope.rope[i + 1].displacement.Y = 0;
                                    AdjustForCollisionsMovingSpriteAgainstListOfSprites(rope.rope[i + 1], hitboxes, seeFurtherAhead, acc);

                                }
                            }
                            if (direction.Y > 0)
                            {
                                if (Math.Abs(rope.rope[i + 1].position.Y - rope.rope[i + 1].previousPosition.Y) < 0.001)
                                {
                                    rope.rope[i + 1].displacement.X = 0;
                                    rope.rope[i + 1].displacement.Y = 1;
                                    AdjustForCollisionsMovingSpriteAgainstListOfSprites(rope.rope[i + 1], hitboxes, seeFurtherAhead, acc);

                                }
                            }
                            if (direction.Y < 0)
                            {
                                if (Math.Abs(rope.rope[i + 1].position.Y - rope.rope[i + 1].previousPosition.Y) < 0.001)
                                {
                                    rope.rope[i + 1].displacement.X = 0;
                                    rope.rope[i + 1].displacement.Y = -1;
                                    AdjustForCollisionsMovingSpriteAgainstListOfSprites(rope.rope[i + 1], hitboxes, seeFurtherAhead, acc);

                                }
                            }
                        }




                        if (!pivot.aboutToBeRemoved)
                        {
                            Vector2 newDirection = new Vector2
                            {
                                X = pivot.position.X - (rope.rope[i + 1].position.X + rope.rope[i + 1].idleHitbox.offsetX),
                                Y = pivot.position.Y - (rope.rope[i + 1].position.Y + rope.rope[i + 1].idleHitbox.offsetY)
                            };

                            if (!(Math.Sign(newDirection.X) == Math.Sign(direction.X) && Math.Sign(newDirection.Y) == Math.Sign(direction.Y)))
                            {
                                rope.rope[i + 1].position.X = pivot.position.X - rope.rope[i + 1].idleHitbox.offsetX;
                                rope.rope[i + 1].position.Y = pivot.position.Y - rope.rope[i + 1].idleHitbox.offsetY;
                            }
                        }
                        else
                        {
                            if (rope.rope[i + 1].position.X != pivot.position.X || rope.rope[i + 1].position.Y != pivot.position.Y)
                            {
                                rope.rope[i].pivotsBetweenThisRopeBitandOnePlus.Remove(pivot);
                                rope.rope[i + 1].pivotsBetweenThisRopeBitandOneMinus.Remove(pivot);
                            }
                        }


                    }
                    else
                    {

                        for (int j = 1; j <= acc; j++)
                        {
                            Vector2 newpoint1 = new Vector2
                            {
                                X = rope.rope[i].previousPosition.X + (float)j / acc * (rope.rope[i].position.X - rope.rope[i].previousPosition.X) + rope.rope[i].idleHitbox.offsetX,
                                Y = rope.rope[i].previousPosition.Y + (float)j / acc * (rope.rope[i].position.Y - rope.rope[i].previousPosition.Y) + rope.rope[i].idleHitbox.offsetY
                            };

                            Vector2 newpoint2 = new Vector2
                            {
                                X = rope.rope[i + 1].previousPosition.X + (float)j / acc * (rope.rope[i + 1].position.X - rope.rope[i + 1].previousPosition.X) + rope.rope[i + 1].idleHitbox.offsetX,
                                Y = rope.rope[i + 1].previousPosition.Y + (float)j / acc * (rope.rope[i + 1].position.Y - rope.rope[i + 1].previousPosition.Y) + rope.rope[i + 1].idleHitbox.offsetY
                            };


                            if (rope.rope[i].pivotsBetweenThisRopeBitandOnePlus.Count() == 0)
                            {
                                TakeTwoPointsAndCreateAPivotIfLineBetweenThemIntersectsTerrainForwards(newpoint1, newpoint2, hitboxes, i, rope, false);

                            }
                            else
                            {

                                // Find the pivot point closest to us - this is the one we want to check for straight lines against

                                Vector2 pivotPoint1 = rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[0].position;
                                Vector2 pivotPoint2 = rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[0].position;

                                for (int k = 0; k <= rope.rope[i].pivotsBetweenThisRopeBitandOnePlus.Count() - 1; k++)
                                {
                                    if (Vector2.Distance(rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[k].position, newpoint1) < Vector2.Distance(pivotPoint1, newpoint1))
                                    {
                                        pivotPoint1 = rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[k].position;
                                    }

                                    if (Vector2.Distance(rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[k].position, newpoint2) < Vector2.Distance(pivotPoint2, newpoint2))
                                    {
                                        pivotPoint2 = rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[k].position;
                                    }

                                }


                                TakeTwoPointsAndCreateAPivotIfLineBetweenThemIntersectsTerrainForwards(newpoint1, pivotPoint1, hitboxes, i, rope, true);
                                TakeTwoPointsAndCreateAPivotIfLineBetweenThemIntersectsTerrainForwards(pivotPoint2, newpoint2, hitboxes, i, rope, false);


                            }



                        }




                    }
                }


            }

            for (int i = rope.IndexOfFirstEnabledRopeBit; i >= 0; i--)
            {
                if (Math.Abs(rope.rope[i].position.X - rope.rope[i].previousPosition.X) < 0.001)
                {
                    rope.rope[i].velocity.X = 0;
                }
                if (Math.Abs(rope.rope[i].position.Y - rope.rope[i].previousPosition.Y) < 0.001)
                {
                    rope.rope[i].velocity.Y = 0;
                }

            }



        }



        public void AdjustForRopeAgainstListOfSpritesBackward(Rope rope, List<HitboxRectangle> hitboxes, int seeFurtherAhead, int acc)
        {
            for (int i = rope.IndexOfFirstEnabledRopeBit - 1; i >= 0; i--)
            {
                if (rope.rope[i].pivotsBetweenThisRopeBitandOnePlus.Count() != 0)
                {
                    // setting the velocities to zero has an effect on the air resistance ... 
                    rope.rope[i].velocity.X = 0;
                    rope.rope[i].velocity.Y = 0;
                    rope.rope[i].displacement.X = 0;
                    rope.rope[i].displacement.Y = 0;
                }

                rope.rope[i].previousPosition = rope.rope[i].position;
                AdjustForCollisionsMovingSpriteAgainstListOfSprites(rope.rope[i], hitboxes, seeFurtherAhead, acc);
            }

            for (int i = rope.IndexOfFirstEnabledRopeBit; i >= 1; i--)
            {
                if (rope.rope[i].pivotsBetweenThisRopeBitandOneMinus.Count() != 0 && rope.rope[i - 1].pivotsBetweenThisRopeBitandOnePlus.Count() != 0)
                {
                    RemoveUnnecessaryPivotsBackwards(rope, i, hitboxes);

                    if (rope.rope[i].pivotsBetweenThisRopeBitandOneMinus.Count() == 0)
                    {
                        continue;
                    }



                    float distanceToMove = (rope.rope[i].position - rope.rope[i].previousPosition).Length();

                    // Find the pivot point closest to us - this is the one we want to move towards (first)
                    Pivot pivot = rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[0];

                    for (int k = 0; k <= rope.rope[i].pivotsBetweenThisRopeBitandOneMinus.Count() - 1; k++)
                    {
                        if (Vector2.Distance(rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[k].position, rope.rope[i - 1].position) < Vector2.Distance(pivot.position, rope.rope[i - 1].position))
                        {
                            pivot = rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[k];
                        }

                    }

                    // We really want the direction between the points drawn as on screen - this is why we take integers
                    Vector2 direction = new Vector2
                    {
                        X = pivot.position.X - (rope.rope[i - 1].position.X + rope.rope[i - 1].idleHitbox.offsetX),
                        Y = pivot.position.Y - (rope.rope[i - 1].position.Y + rope.rope[i - 1].idleHitbox.offsetY)
                    };

                    // This means that we are on the pivot point - we will now remove the pivot as soon as we move away
                    if (direction.X == 0 && direction.Y == 0)
                    {
                        pivot.aboutToBeRemoved = true;

                        if (pivot.TopRight && rope.rope[i].position.Y <= rope.rope[i - 1].position.Y)
                        {
                            direction.X = -1;
                            direction.Y = 0;
                        }
                        else if (pivot.TopLeft && rope.rope[i].position.Y <= rope.rope[i - 1].position.Y)
                        {
                            direction.X = 1;
                            direction.Y = 0;
                        }
                        else if (pivot.TopRight && rope.rope[i].position.Y >= rope.rope[i - 1].position.Y)
                        {
                            direction.X = 0;
                            direction.Y = 1;
                        }
                        else if (pivot.TopLeft && rope.rope[i].position.Y >= rope.rope[i - 1].position.Y)
                        {
                            direction.X = 0;
                            direction.Y = 1;
                        }
                        else if (pivot.BottomRight && rope.rope[i].position.Y >= rope.rope[i - 1].position.Y)
                        {
                            direction.X = -1;
                            direction.Y = 0;
                        }
                        else if (pivot.BottomLeft && rope.rope[i].position.Y >= rope.rope[i - 1].position.Y)
                        {
                            direction.X = 1;
                            direction.Y = 0;
                        }
                        else if (pivot.BottomRight && rope.rope[i].position.Y <= rope.rope[i - 1].position.Y)
                        {
                            direction.X = 0;
                            direction.Y = -1;
                        }
                        else if (pivot.BottomLeft && rope.rope[i].position.Y <= rope.rope[i - 1].position.Y)
                        {
                            direction.X = 0;
                            direction.Y = -1;
                        }
                    }

                    if (direction.Length() > 0.001)
                    {
                        direction.Normalize();
                    }


                    // I only want to move if I move AWAY from the pivot.
                    if (Vector2.Distance(rope.rope[i].position, pivot.position) <= Vector2.Distance(rope.rope[i].previousPosition, pivot.position))
                    {
                        distanceToMove = 0;
                    }

                    rope.rope[i - 1].displacement = distanceToMove * direction;

                    AdjustForCollisionsMovingSpriteAgainstListOfSprites(rope.rope[i - 1], hitboxes, seeFurtherAhead, acc);


                    // Sometimes I would want to move but would get caught on a corner - say direction.X is very small after normalising 
                    // then we hardly move in the X direction, even though we would like to move at least one pixel
                    // The code chunk below fixes this

                    if (!pivot.aboutToBeRemoved && Math.Abs(distanceToMove) > 0.001)
                    {
                        if (direction.X > 0)
                        {
                            if (Math.Abs(rope.rope[i - 1].position.X - rope.rope[i - 1].previousPosition.X) < 0.001)
                            {
                                rope.rope[i - 1].displacement.X = 1;
                                rope.rope[i - 1].displacement.Y = 0;
                                AdjustForCollisionsMovingSpriteAgainstListOfSprites(rope.rope[i - 1], hitboxes, seeFurtherAhead, acc);

                            }
                        }
                        if (direction.X < 0)
                        {
                            if (Math.Abs(rope.rope[i - 1].position.X - rope.rope[i - 1].previousPosition.X) < 0.001)
                            {
                                rope.rope[i - 1].displacement.X = -1;
                                rope.rope[i - 1].displacement.Y = 0;
                                AdjustForCollisionsMovingSpriteAgainstListOfSprites(rope.rope[i - 1], hitboxes, seeFurtherAhead, acc);

                            }
                        }
                        if (direction.Y > 0)
                        {
                            if (Math.Abs(rope.rope[i - 1].position.Y - rope.rope[i - 1].previousPosition.Y) < 0.001)
                            {
                                rope.rope[i - 1].displacement.X = 0;
                                rope.rope[i - 1].displacement.Y = 1;
                                AdjustForCollisionsMovingSpriteAgainstListOfSprites(rope.rope[i - 1], hitboxes, seeFurtherAhead, acc);

                            }
                        }
                        if (direction.Y < 0)
                        {
                            if (Math.Abs(rope.rope[i - 1].position.Y - rope.rope[i - 1].previousPosition.Y) < 0.001)
                            {
                                rope.rope[i - 1].displacement.X = 0;
                                rope.rope[i - 1].displacement.Y = -1;
                                AdjustForCollisionsMovingSpriteAgainstListOfSprites(rope.rope[i - 1], hitboxes, seeFurtherAhead, acc);

                            }
                        }
                    }




                    if (!pivot.aboutToBeRemoved)
                    {
                        Vector2 newDirection = new Vector2
                        {
                            X = pivot.position.X - (rope.rope[i - 1].position.X + rope.rope[i - 1].idleHitbox.offsetX),
                            Y = pivot.position.Y - (rope.rope[i - 1].position.Y + rope.rope[i - 1].idleHitbox.offsetY)
                        };

                        if (!(Math.Sign(newDirection.X) == Math.Sign(direction.X) && Math.Sign(newDirection.Y) == Math.Sign(direction.Y)))
                        {
                            rope.rope[i - 1].position.X = pivot.position.X - rope.rope[i - 1].idleHitbox.offsetX;
                            rope.rope[i - 1].position.Y = pivot.position.Y - rope.rope[i - 1].idleHitbox.offsetY;
                        }
                    }
                    else
                    {
                        if (rope.rope[i - 1].position.X != pivot.position.X || rope.rope[i - 1].position.Y != pivot.position.Y)
                        {
                            rope.rope[i].pivotsBetweenThisRopeBitandOneMinus.Remove(pivot);
                            rope.rope[i - 1].pivotsBetweenThisRopeBitandOnePlus.Remove(pivot);
                        }
                    }


                }
                else
                {

                    for (int j = 1; j <= acc; j++)
                    {
                        Vector2 newpoint1 = new Vector2
                        {
                            X = rope.rope[i].previousPosition.X + (float)j / acc * (rope.rope[i].position.X - rope.rope[i].previousPosition.X) + rope.rope[i].idleHitbox.offsetX,
                            Y = rope.rope[i].previousPosition.Y + (float)j / acc * (rope.rope[i].position.Y - rope.rope[i].previousPosition.Y) + rope.rope[i].idleHitbox.offsetY
                        };

                        Vector2 newpoint2 = new Vector2
                        {
                            X = rope.rope[i - 1].previousPosition.X + (float)j / acc * (rope.rope[i - 1].position.X - rope.rope[i - 1].previousPosition.X) + rope.rope[i - 1].idleHitbox.offsetX,
                            Y = rope.rope[i - 1].previousPosition.Y + (float)j / acc * (rope.rope[i - 1].position.Y - rope.rope[i - 1].previousPosition.Y) + rope.rope[i - 1].idleHitbox.offsetY
                        };


                        if (rope.rope[i].pivotsBetweenThisRopeBitandOneMinus.Count() == 0)
                        {
                            TakeTwoPointsAndCreateAPivotIfLineBetweenThemIntersectsTerrainBackwards(newpoint1, newpoint2, hitboxes, i, rope, false);

                        }
                        else
                        {

                            // Find the pivot point closest to us - this is the one we want to check for straight lines against

                            Vector2 pivotPoint1 = rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[0].position;
                            Vector2 pivotPoint2 = rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[0].position;

                            for (int k = 0; k <= rope.rope[i].pivotsBetweenThisRopeBitandOneMinus.Count() - 1; k++)
                            {
                                if (Vector2.Distance(rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[k].position, newpoint1) < Vector2.Distance(pivotPoint1, newpoint1))
                                {
                                    pivotPoint1 = rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[k].position;
                                }

                                if (Vector2.Distance(rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[k].position, newpoint2) < Vector2.Distance(pivotPoint2, newpoint2))
                                {
                                    pivotPoint2 = rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[k].position;
                                }

                            }


                            TakeTwoPointsAndCreateAPivotIfLineBetweenThemIntersectsTerrainBackwards(newpoint1, pivotPoint1, hitboxes, i, rope, true);
                            TakeTwoPointsAndCreateAPivotIfLineBetweenThemIntersectsTerrainBackwards(pivotPoint2, newpoint2, hitboxes, i, rope, false);


                        }



                    }




                }


            }

            for (int i = rope.IndexOfFirstEnabledRopeBit - 1; i >= 0; i--)
            {
                if (Math.Abs(rope.rope[i].position.X - rope.rope[i].previousPosition.X) < 0.001)
                {
                    rope.rope[i].velocity.X = 0;
                }
                if (Math.Abs(rope.rope[i].position.Y - rope.rope[i].previousPosition.Y) < 0.001)
                {
                    rope.rope[i].velocity.Y = 0;
                }

            }



        }

        public void RemoveUnnecessaryPivotsBackwards(Rope rope, int i, List<HitboxRectangle> hitboxes)
        {
            // There are two cases: 
            // More than one pivot. Then we remove a pivot if the straight line between the current position and the pivot after the one we are looking at doesn't intersect any obstacles
            // Only one pivot active. Then we wish to remove the pivot if the straight line between the current rope positions doesn't intersect any obstacles
            // The way we insert / add pivots to our list means that we will always "choose" the correct pivots in this way.

            if (rope.rope[i].pivotsBetweenThisRopeBitandOneMinus.Count() > 1)
            {
                for (int k = rope.rope[i].pivotsBetweenThisRopeBitandOneMinus.Count() - 1; k >= 1; k--)
                {
                    Vector2 newvec1 = new Vector2();
                    Vector2 newvec2 = new Vector2();

                    if (rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[k].TopRight)
                    {
                        newvec1.X = rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[k - 1].position.X;
                        newvec1.Y = rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[k - 1].position.Y + 0.5f * rope.rope[i].idleHitbox.rectangle.Height;
                        newvec2.X = rope.rope[i - 1].position.X + rope.rope[i - 1].idleHitbox.offsetX;
                        newvec2.Y = rope.rope[i - 1].position.Y + rope.rope[i - 1].idleHitbox.offsetY + 0.5f * rope.rope[i - 1].idleHitbox.rectangle.Height;
                    }
                    else if (rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[k].TopLeft)
                    {
                        newvec1.X = rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[k - 1].position.X + 0.5f * rope.rope[i].idleHitbox.rectangle.Width;
                        newvec1.Y = rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[k - 1].position.Y + 0.5f * rope.rope[i].idleHitbox.rectangle.Height;
                        newvec2.X = rope.rope[i - 1].position.X + rope.rope[i - 1].idleHitbox.offsetX + 0.5f * rope.rope[i - 1].idleHitbox.rectangle.Width;
                        newvec2.Y = rope.rope[i - 1].position.Y + rope.rope[i - 1].idleHitbox.offsetY + 0.5f * rope.rope[i - 1].idleHitbox.rectangle.Height;
                    }
                    else if (rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[k].BottomRight)
                    {
                        newvec1.X = rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[k - 1].position.X;
                        newvec1.Y = rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[k - 1].position.Y;
                        newvec2.X = rope.rope[i - 1].position.X + rope.rope[i - 1].idleHitbox.offsetX;
                        newvec2.Y = rope.rope[i - 1].position.Y + rope.rope[i - 1].idleHitbox.offsetY;
                    }
                    else if (rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[k].BottomLeft)
                    {
                        newvec1.X = rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[k - 1].position.X + 0.5f * rope.rope[i].idleHitbox.rectangle.Width;
                        newvec1.Y = rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[k - 1].position.Y;
                        newvec2.X = rope.rope[i - 1].position.X + rope.rope[i - 1].idleHitbox.offsetX + 0.5f * rope.rope[i - 1].idleHitbox.rectangle.Width;
                        newvec2.Y = rope.rope[i - 1].position.Y + rope.rope[i - 1].idleHitbox.offsetY;
                    }

                    if (!CheckIfPixelLineBetweenTwoPointsIntersectsListOfHitboxes(newvec1, newvec2, hitboxes))
                    {

                        rope.rope[i].pivotsBetweenThisRopeBitandOneMinus.RemoveAt(k);
                        rope.rope[i - 1].pivotsBetweenThisRopeBitandOnePlus.RemoveAt(k);

                    }

                    if (rope.rope[i].pivotsBetweenThisRopeBitandOneMinus.Count() == 1)
                    {
                        break;
                    }
                }
            }




            if (rope.rope[i].pivotsBetweenThisRopeBitandOneMinus.Count() == 1)
            {
                Vector2 newvec1 = new Vector2();
                Vector2 newvec2 = new Vector2();

                if (rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[0].TopRight)
                {
                    newvec1.X = rope.rope[i].position.X + rope.rope[i].idleHitbox.offsetX;
                    newvec1.Y = rope.rope[i].position.Y + rope.rope[i].idleHitbox.offsetY + 0.5f * rope.rope[i].idleHitbox.rectangle.Height;
                    newvec2.X = rope.rope[i - 1].position.X + rope.rope[i - 1].idleHitbox.offsetX;
                    newvec2.Y = rope.rope[i - 1].position.Y + rope.rope[i - 1].idleHitbox.offsetY + 0.5f * rope.rope[i - 1].idleHitbox.rectangle.Height;
                }
                else if (rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[0].TopLeft)
                {
                    newvec1.X = rope.rope[i].position.X + rope.rope[i].idleHitbox.offsetX + 0.5f * rope.rope[i].idleHitbox.rectangle.Width;
                    newvec1.Y = rope.rope[i].position.Y + rope.rope[i].idleHitbox.offsetY + 0.5f * rope.rope[i].idleHitbox.rectangle.Height;
                    newvec2.X = rope.rope[i - 1].position.X + rope.rope[i - 1].idleHitbox.offsetX + 0.5f * rope.rope[i - 1].idleHitbox.rectangle.Width;
                    newvec2.Y = rope.rope[i - 1].position.Y + rope.rope[i - 1].idleHitbox.offsetY + 0.5f * rope.rope[i - 1].idleHitbox.rectangle.Height;
                }
                else if (rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[0].BottomRight)
                {
                    newvec1.X = rope.rope[i].position.X + rope.rope[i].idleHitbox.offsetX;
                    newvec1.Y = rope.rope[i].position.Y + rope.rope[i].idleHitbox.offsetY;
                    newvec2.X = rope.rope[i - 1].position.X + rope.rope[i - 1].idleHitbox.offsetX;
                    newvec2.Y = rope.rope[i - 1].position.Y + rope.rope[i - 1].idleHitbox.offsetY;
                }
                else if (rope.rope[i].pivotsBetweenThisRopeBitandOneMinus[0].BottomLeft)
                {
                    newvec1.X = rope.rope[i].position.X + rope.rope[i].idleHitbox.offsetX + 0.5f * rope.rope[i].idleHitbox.rectangle.Width;
                    newvec1.Y = rope.rope[i].position.Y + rope.rope[i].idleHitbox.offsetY;
                    newvec2.X = rope.rope[i - 1].position.X + rope.rope[i - 1].idleHitbox.offsetX + 0.5f * rope.rope[i - 1].idleHitbox.rectangle.Width;
                    newvec2.Y = rope.rope[i - 1].position.Y + rope.rope[i - 1].idleHitbox.offsetY;
                }

                if (!CheckIfPixelLineBetweenTwoPointsIntersectsListOfHitboxes(newvec1, newvec2, hitboxes))
                {

                    rope.rope[i].pivotsBetweenThisRopeBitandOneMinus.Clear();
                    rope.rope[i - 1].pivotsBetweenThisRopeBitandOnePlus.Clear();

                }



            }

        }

        public void RemoveUnnecessaryPivotsForwards(Rope rope, int i, List<HitboxRectangle> hitboxes)
        {
            // There are two cases: 
            // More than one pivot. Then we remove a pivot if the straight line between the current position and the pivot after the one we are looking at doesn't intersect any obstacles
            // Only one pivot active. Then we wish to remove the pivot if the straight line between the current rope positions doesn't intersect any obstacles
            // The way we insert / add pivots to our list means that we will always "choose" the correct pivots in this way.

            if (rope.rope[i].pivotsBetweenThisRopeBitandOnePlus.Count() > 1)
            {
                for (int k = rope.rope[i].pivotsBetweenThisRopeBitandOnePlus.Count() - 1; k >= 1; k--)
                {
                    Vector2 newvec1 = new Vector2();
                    Vector2 newvec2 = new Vector2();

                    if (rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[k].TopRight)
                    {
                        newvec1.X = rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[k - 1].position.X;
                        newvec1.Y = rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[k - 1].position.Y + 0.5f * rope.rope[i].idleHitbox.rectangle.Height;
                        newvec2.X = rope.rope[i + 1].position.X + rope.rope[i + 1].idleHitbox.offsetX;
                        newvec2.Y = rope.rope[i + 1].position.Y + rope.rope[i + 1].idleHitbox.offsetY + 0.5f * rope.rope[i + 1].idleHitbox.rectangle.Height;
                    }
                    else if (rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[k].TopLeft)
                    {
                        newvec1.X = rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[k - 1].position.X + 0.5f * rope.rope[i].idleHitbox.rectangle.Width;
                        newvec1.Y = rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[k - 1].position.Y + 0.5f * rope.rope[i].idleHitbox.rectangle.Height;
                        newvec2.X = rope.rope[i + 1].position.X + rope.rope[i + 1].idleHitbox.offsetX + 0.5f * rope.rope[i + 1].idleHitbox.rectangle.Width;
                        newvec2.Y = rope.rope[i + 1].position.Y + rope.rope[i + 1].idleHitbox.offsetY + 0.5f * rope.rope[i + 1].idleHitbox.rectangle.Height;
                    }
                    else if (rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[k].BottomRight)
                    {
                        newvec1.X = rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[k - 1].position.X;
                        newvec1.Y = rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[k - 1].position.Y;
                        newvec2.X = rope.rope[i + 1].position.X + rope.rope[i + 1].idleHitbox.offsetX;
                        newvec2.Y = rope.rope[i + 1].position.Y + rope.rope[i + 1].idleHitbox.offsetY;
                    }
                    else if (rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[k].BottomLeft)
                    {
                        newvec1.X = rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[k - 1].position.X + 0.5f * rope.rope[i].idleHitbox.rectangle.Width;
                        newvec1.Y = rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[k - 1].position.Y;
                        newvec2.X = rope.rope[i + 1].position.X + rope.rope[i + 1].idleHitbox.offsetX + 0.5f * rope.rope[i + 1].idleHitbox.rectangle.Width;
                        newvec2.Y = rope.rope[i + 1].position.Y + rope.rope[i + 1].idleHitbox.offsetY;
                    }

                    if (!CheckIfPixelLineBetweenTwoPointsIntersectsListOfHitboxes(newvec1, newvec2, hitboxes))
                    {

                        rope.rope[i].pivotsBetweenThisRopeBitandOnePlus.RemoveAt(k);
                        rope.rope[i + 1].pivotsBetweenThisRopeBitandOneMinus.RemoveAt(k);

                    }

                    if (rope.rope[i].pivotsBetweenThisRopeBitandOnePlus.Count() == 1)
                    {
                        break;
                    }
                }
            }




            if (rope.rope[i].pivotsBetweenThisRopeBitandOnePlus.Count() == 1)
            {
                Vector2 newvec1 = new Vector2();
                Vector2 newvec2 = new Vector2();

                if (rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[0].TopRight)
                {
                    newvec1.X = rope.rope[i].position.X + rope.rope[i].idleHitbox.offsetX;
                    newvec1.Y = rope.rope[i].position.Y + rope.rope[i].idleHitbox.offsetY + 0.5f * rope.rope[i].idleHitbox.rectangle.Height;
                    newvec2.X = rope.rope[i + 1].position.X + rope.rope[i + 1].idleHitbox.offsetX;
                    newvec2.Y = rope.rope[i + 1].position.Y + rope.rope[i + 1].idleHitbox.offsetY + 0.5f * rope.rope[i + 1].idleHitbox.rectangle.Height;
                }
                else if (rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[0].TopLeft)
                {
                    newvec1.X = rope.rope[i].position.X + rope.rope[i].idleHitbox.offsetX + 0.5f * rope.rope[i].idleHitbox.rectangle.Width;
                    newvec1.Y = rope.rope[i].position.Y + rope.rope[i].idleHitbox.offsetY + 0.5f * rope.rope[i].idleHitbox.rectangle.Height;
                    newvec2.X = rope.rope[i + 1].position.X + rope.rope[i + 1].idleHitbox.offsetX + 0.5f * rope.rope[i + 1].idleHitbox.rectangle.Width;
                    newvec2.Y = rope.rope[i + 1].position.Y + rope.rope[i + 1].idleHitbox.offsetY + 0.5f * rope.rope[i + 1].idleHitbox.rectangle.Height;
                }
                else if (rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[0].BottomRight)
                {
                    newvec1.X = rope.rope[i].position.X + rope.rope[i].idleHitbox.offsetX;
                    newvec1.Y = rope.rope[i].position.Y + rope.rope[i].idleHitbox.offsetY;
                    newvec2.X = rope.rope[i + 1].position.X + rope.rope[i + 1].idleHitbox.offsetX;
                    newvec2.Y = rope.rope[i + 1].position.Y + rope.rope[i + 1].idleHitbox.offsetY;
                }
                else if (rope.rope[i].pivotsBetweenThisRopeBitandOnePlus[0].BottomLeft)
                {
                    newvec1.X = rope.rope[i].position.X + rope.rope[i].idleHitbox.offsetX + 0.5f * rope.rope[i].idleHitbox.rectangle.Width;
                    newvec1.Y = rope.rope[i].position.Y + rope.rope[i].idleHitbox.offsetY;
                    newvec2.X = rope.rope[i + 1].position.X + rope.rope[i + 1].idleHitbox.offsetX + 0.5f * rope.rope[i + 1].idleHitbox.rectangle.Width;
                    newvec2.Y = rope.rope[i + 1].position.Y + rope.rope[i + 1].idleHitbox.offsetY;
                }

                if (!CheckIfPixelLineBetweenTwoPointsIntersectsListOfHitboxes(newvec1, newvec2, hitboxes))
                {

                    rope.rope[i].pivotsBetweenThisRopeBitandOnePlus.Clear();
                    rope.rope[i + 1].pivotsBetweenThisRopeBitandOneMinus.Clear();

                }



            }

        }

        public void TakeTwoPointsAndCreateAPivotIfLineBetweenThemIntersectsTerrainBackwards(Vector2 newpoint1, Vector2 newpoint2, List<HitboxRectangle> hitboxes, int i, Rope rope, bool addToStart)
        {
            Vector2 displacementVec = newpoint2 - newpoint1;
            int maxInt = (int)Math.Floor(displacementVec.Length());

            if (maxInt >= 1)
            {
                bool breakEarly = false;

                for (int k = 0; k < maxInt - 1; k++)
                {
                    Rectangle testRect = new Rectangle
                    {
                        X = DistanceToNearestInteger(newpoint1.X + ((float)(k + 1) / maxInt) * displacementVec.X),
                        Y = DistanceToNearestInteger(newpoint1.Y + ((float)(k + 1) / maxInt) * displacementVec.Y),
                        Width = 1,
                        Height = 1
                    };


                    foreach (HitboxRectangle hitbox in hitboxes)
                    {
                        if (hitbox.rectangle.Contains(testRect))
                        {
                            CreatePivotBackwards(testRect, hitbox, i, rope, addToStart);
                            rope.rope[i].pivotsBetweenThisRopeBitandOneMinusActive = true;
                            rope.rope[i - 1].pivotsBetweenThisRopeBitandOnePlusActive = true;
                            breakEarly = true;
                            break;
                        }
                    }

                    if (breakEarly)
                    {
                        break;
                    }

                }
            }
        }

        public void TakeTwoPointsAndCreateAPivotIfLineBetweenThemIntersectsTerrainForwards(Vector2 newpoint1, Vector2 newpoint2, List<HitboxRectangle> hitboxes, int i, Rope rope, bool addToStart)
        {
            Vector2 displacementVec = newpoint2 - newpoint1;
            int maxInt = (int)Math.Floor(displacementVec.Length());

            if (maxInt >= 1)
            {
                bool breakEarly = false;

                for (int k = 0; k < maxInt - 1; k++)
                {
                    Rectangle testRect = new Rectangle
                    {
                        X = DistanceToNearestInteger(newpoint1.X + ((float)(k + 1) / maxInt) * displacementVec.X),
                        Y = DistanceToNearestInteger(newpoint1.Y + ((float)(k + 1) / maxInt) * displacementVec.Y),
                        Width = 1,
                        Height = 1
                    };


                    foreach (HitboxRectangle hitbox in hitboxes)
                    {
                        if (hitbox.rectangle.Contains(testRect))
                        {
                            CreatePivotForwards(testRect, hitbox, i, rope, addToStart);
                            rope.rope[i].pivotsBetweenThisRopeBitandOnePlusActive = true;
                            rope.rope[i + 1].pivotsBetweenThisRopeBitandOneMinusActive = true;
                            breakEarly = true;
                            break;
                        }
                    }

                    if (breakEarly)
                    {
                        break;
                    }

                }
            }
        }

        public void CreatePivotBackwards(Rectangle testRect, HitboxRectangle hitbox, int i, Rope rope, bool addToStart)
        {
            // Top - right corner
            if (testRect.X >= hitbox.rectangle.Center.X && testRect.Y <= hitbox.rectangle.Center.Y)
            {

                Rectangle test = new Rectangle(hitbox.rectangle.X + hitbox.rectangle.Width, hitbox.rectangle.Y - 2, 2, 2);

                if (!hitbox.rectangle.Contains(test))
                {
                    Pivot pivot = new Pivot();
                    pivot.position.X = hitbox.rectangle.X + hitbox.rectangle.Width;
                    pivot.position.Y = hitbox.rectangle.Y - 2;
                    pivot.isPivotActive = true;
                    pivot.TopRight = true;

                    if (addToStart)
                    {
                        rope.rope[i].pivotsBetweenThisRopeBitandOneMinus.Insert(0, pivot);
                        rope.rope[i - 1].pivotsBetweenThisRopeBitandOnePlus.Insert(0, pivot);
                    }
                    else
                    {
                        rope.rope[i].pivotsBetweenThisRopeBitandOneMinus.Add(pivot);
                        rope.rope[i - 1].pivotsBetweenThisRopeBitandOnePlus.Add(pivot);
                    }

                }


            }
            // Bottom - right corner
            else if (testRect.X >= hitbox.rectangle.Center.X && testRect.Y >= hitbox.rectangle.Center.Y)
            {
                Rectangle test = new Rectangle(hitbox.rectangle.X + hitbox.rectangle.Width, hitbox.rectangle.Y + hitbox.rectangle.Height, 2, 2);

                if (!hitbox.rectangle.Contains(test))
                {
                    Pivot pivot = new Pivot();
                    pivot.position.X = hitbox.rectangle.X + hitbox.rectangle.Width;
                    pivot.position.Y = hitbox.rectangle.Y + hitbox.rectangle.Height;
                    pivot.isPivotActive = true;
                    pivot.BottomRight = true;

                    if (addToStart)
                    {
                        rope.rope[i].pivotsBetweenThisRopeBitandOneMinus.Insert(0, pivot);
                        rope.rope[i - 1].pivotsBetweenThisRopeBitandOnePlus.Insert(0, pivot);
                    }
                    else
                    {
                        rope.rope[i].pivotsBetweenThisRopeBitandOneMinus.Add(pivot);
                        rope.rope[i - 1].pivotsBetweenThisRopeBitandOnePlus.Add(pivot);
                    }
                }



            }
            // Top - left corner
            else if (testRect.X <= hitbox.rectangle.Center.X && testRect.Y <= hitbox.rectangle.Center.Y)
            {
                Rectangle test = new Rectangle(hitbox.rectangle.X - 2, hitbox.rectangle.Y - 2, 2, 2);

                if (!hitbox.rectangle.Contains(test))
                {

                    Pivot pivot = new Pivot();
                    pivot.position.X = hitbox.rectangle.X - 2;
                    pivot.position.Y = hitbox.rectangle.Y - 2;
                    pivot.isPivotActive = true;
                    pivot.TopLeft = true;

                    if (addToStart)
                    {
                        rope.rope[i].pivotsBetweenThisRopeBitandOneMinus.Insert(0, pivot);
                        rope.rope[i - 1].pivotsBetweenThisRopeBitandOnePlus.Insert(0, pivot);
                    }
                    else
                    {
                        rope.rope[i].pivotsBetweenThisRopeBitandOneMinus.Add(pivot);
                        rope.rope[i - 1].pivotsBetweenThisRopeBitandOnePlus.Add(pivot);
                    }
                }


            }
            // Bottom - left corner
            else if (testRect.X <= hitbox.rectangle.Center.X && testRect.Y >= hitbox.rectangle.Center.Y)
            {
                Rectangle test = new Rectangle(hitbox.rectangle.X - 2, hitbox.rectangle.Y + hitbox.rectangle.Height, 2, 2);

                if (!hitbox.rectangle.Contains(test))
                {

                    Pivot pivot = new Pivot();
                    pivot.position.X = hitbox.rectangle.X - 2;
                    pivot.position.Y = hitbox.rectangle.Y + hitbox.rectangle.Height;
                    pivot.isPivotActive = true;
                    pivot.BottomLeft = true;

                    if (addToStart)
                    {
                        rope.rope[i].pivotsBetweenThisRopeBitandOneMinus.Insert(0, pivot);
                        rope.rope[i - 1].pivotsBetweenThisRopeBitandOnePlus.Insert(0, pivot);
                    }
                    else
                    {
                        rope.rope[i].pivotsBetweenThisRopeBitandOneMinus.Add(pivot);
                        rope.rope[i - 1].pivotsBetweenThisRopeBitandOnePlus.Add(pivot);
                    }
                }

            }
        }


        public void CreatePivotForwards(Rectangle testRect, HitboxRectangle hitbox, int i, Rope rope, bool addToStart)
        {
            // Top - right corner
            if (testRect.X >= hitbox.rectangle.Center.X && testRect.Y <= hitbox.rectangle.Center.Y)
            {

                Rectangle test = new Rectangle(hitbox.rectangle.X + hitbox.rectangle.Width, hitbox.rectangle.Y - 2, 2, 2);

                if (!hitbox.rectangle.Contains(test))
                {
                    Pivot pivot = new Pivot();
                    pivot.position.X = hitbox.rectangle.X + hitbox.rectangle.Width;
                    pivot.position.Y = hitbox.rectangle.Y - 2;
                    pivot.isPivotActive = true;
                    pivot.TopRight = true;

                    if (addToStart)
                    {
                        rope.rope[i].pivotsBetweenThisRopeBitandOnePlus.Insert(0, pivot);
                        rope.rope[i + 1].pivotsBetweenThisRopeBitandOneMinus.Insert(0, pivot);
                    }
                    else
                    {
                        rope.rope[i].pivotsBetweenThisRopeBitandOnePlus.Add(pivot);
                        rope.rope[i + 1].pivotsBetweenThisRopeBitandOneMinus.Add(pivot);
                    }

                }


            }
            // Bottom - right corner
            else if (testRect.X >= hitbox.rectangle.Center.X && testRect.Y >= hitbox.rectangle.Center.Y)
            {
                Rectangle test = new Rectangle(hitbox.rectangle.X + hitbox.rectangle.Width, hitbox.rectangle.Y + hitbox.rectangle.Height, 2, 2);

                if (!hitbox.rectangle.Contains(test))
                {
                    Pivot pivot = new Pivot();
                    pivot.position.X = hitbox.rectangle.X + hitbox.rectangle.Width;
                    pivot.position.Y = hitbox.rectangle.Y + hitbox.rectangle.Height;
                    pivot.isPivotActive = true;
                    pivot.BottomRight = true;

                    if (addToStart)
                    {
                        rope.rope[i].pivotsBetweenThisRopeBitandOnePlus.Insert(0, pivot);
                        rope.rope[i + 1].pivotsBetweenThisRopeBitandOneMinus.Insert(0, pivot);
                    }
                    else
                    {
                        rope.rope[i].pivotsBetweenThisRopeBitandOnePlus.Add(pivot);
                        rope.rope[i + 1].pivotsBetweenThisRopeBitandOneMinus.Add(pivot);
                    }
                }



            }
            // Top - left corner
            else if (testRect.X <= hitbox.rectangle.Center.X && testRect.Y <= hitbox.rectangle.Center.Y)
            {
                Rectangle test = new Rectangle(hitbox.rectangle.X - 2, hitbox.rectangle.Y - 2, 2, 2);

                if (!hitbox.rectangle.Contains(test))
                {

                    Pivot pivot = new Pivot();
                    pivot.position.X = hitbox.rectangle.X - 2;
                    pivot.position.Y = hitbox.rectangle.Y - 2;
                    pivot.isPivotActive = true;
                    pivot.TopLeft = true;

                    if (addToStart)
                    {
                        rope.rope[i].pivotsBetweenThisRopeBitandOnePlus.Insert(0, pivot);
                        rope.rope[i + 1].pivotsBetweenThisRopeBitandOneMinus.Insert(0, pivot);
                    }
                    else
                    {
                        rope.rope[i].pivotsBetweenThisRopeBitandOnePlus.Add(pivot);
                        rope.rope[i + 1].pivotsBetweenThisRopeBitandOneMinus.Add(pivot);
                    }
                }


            }
            // Bottom - left corner
            else if (testRect.X <= hitbox.rectangle.Center.X && testRect.Y >= hitbox.rectangle.Center.Y)
            {
                Rectangle test = new Rectangle(hitbox.rectangle.X - 2, hitbox.rectangle.Y + hitbox.rectangle.Height, 2, 2);

                if (!hitbox.rectangle.Contains(test))
                {

                    Pivot pivot = new Pivot();
                    pivot.position.X = hitbox.rectangle.X - 2;
                    pivot.position.Y = hitbox.rectangle.Y + hitbox.rectangle.Height;
                    pivot.isPivotActive = true;
                    pivot.BottomLeft = true;

                    if (addToStart)
                    {
                        rope.rope[i].pivotsBetweenThisRopeBitandOnePlus.Insert(0, pivot);
                        rope.rope[i + 1].pivotsBetweenThisRopeBitandOneMinus.Insert(0, pivot);
                    }
                    else
                    {
                        rope.rope[i].pivotsBetweenThisRopeBitandOnePlus.Add(pivot);
                        rope.rope[i + 1].pivotsBetweenThisRopeBitandOneMinus.Add(pivot);
                    }
                }

            }
        }



        public bool CheckIfPixelLineBetweenTwoPointsIntersectsListOfHitboxes(Vector2 position1, Vector2 position2, List<HitboxRectangle> hitboxes)
        {
            Vector2 displacementVec = position2 - position1;
            int maxInt = (int)Math.Floor(displacementVec.Length());

            if (maxInt >= 1)
            {

                for (int k = 0; k < maxInt - 1; k++)
                {
                    Rectangle testRect = new Rectangle
                    {
                        X = DistanceToNearestInteger(position1.X + ((float)(k + 1) / maxInt) * displacementVec.X),
                        Y = DistanceToNearestInteger(position1.Y + ((float)(k + 1) / maxInt) * displacementVec.Y),
                        Width = 1,
                        Height = 1
                    };


                    foreach (HitboxRectangle hitbox in hitboxes)
                    {
                        if (hitbox.rectangle.Contains(testRect))
                        {
                            return true;
                        }
                    }


                }
            }

            return false;

        }

        public Vector2 CheckIfPixelLineBetweenTwoPointsIntersectsListOfHitboxesAndReturnContactPoint(Vector2 position1, Vector2 position2, List<HitboxRectangle> hitboxes)
        {
            Vector2 displacementVec = position2 - position1;
            int maxInt = (int)Math.Floor(displacementVec.Length());

            if (maxInt >= 1)
            {


                for (int k = 0; k < maxInt - 1; k++)
                {
                    Rectangle testRect = new Rectangle
                    {
                        X = DistanceToNearestInteger(position1.X + ((float)(k + 1) / maxInt) * displacementVec.X),
                        Y = DistanceToNearestInteger(position1.Y + ((float)(k + 1) / maxInt) * displacementVec.Y),
                        Width = 1,
                        Height = 1
                    };


                    foreach (HitboxRectangle hitbox in hitboxes)
                    {
                        if (hitbox.rectangle.Contains(testRect))
                        {

                            // Top - right corner
                            if (testRect.X >= hitbox.rectangle.Center.X && testRect.Y <= hitbox.rectangle.Center.Y)
                            {
                                return new Vector2(hitbox.rectangle.X + hitbox.rectangle.Width, hitbox.rectangle.Y - 2);
                            }

                            // Bottom - right corner
                            if (testRect.X >= hitbox.rectangle.Center.X && testRect.Y >= hitbox.rectangle.Center.Y)
                            {
                                return new Vector2(hitbox.rectangle.X + hitbox.rectangle.Width, hitbox.rectangle.Y + hitbox.rectangle.Height);
                            }

                            // Top - left corner
                            if (testRect.X <= hitbox.rectangle.Center.X && testRect.Y <= hitbox.rectangle.Center.Y)
                            {
                                return new Vector2(hitbox.rectangle.X - 2, hitbox.rectangle.Y - 2);
                            }

                            // Bottom - left corner
                            if (testRect.X <= hitbox.rectangle.Center.X && testRect.Y >= hitbox.rectangle.Center.Y)
                            {
                                return new Vector2(hitbox.rectangle.X - 2, hitbox.rectangle.Y + hitbox.rectangle.Height);
                            }

                            //Vector2 vec1 = new Vector2(testRect.X, testRect.Y);
                            //return vec1;
                        }
                    }

                }
            }

            return new Vector2(0, 0);

        }

        public Vector2 CheckIfPixelLineBetweenTwoPointsIntersectsListOfHitboxesAndCreateAPivotIfSo(Rope rope, int i, Vector2 position1, Vector2 position2, List<HitboxRectangle> hitboxes)
        {
            //Vector2 position1 = new Vector2
            //{
            //    X = rope.rope[i].previousSpritePosition.X + (float)j / acc * (rope.rope[i].spritePosition.X - rope.rope[i].previousSpritePosition.X) + rope.rope[i].idleHitbox.offsetX,
            //    Y = rope.rope[i].previousSpritePosition.Y + (float)j / acc * (rope.rope[i].spritePosition.Y - rope.rope[i].previousSpritePosition.Y) + rope.rope[i].idleHitbox.offsetY
            //};

            //Vector2 position2 = new Vector2
            //{
            //    X = rope.rope[i - 1].previousSpritePosition.X + (float)j / acc * (rope.rope[i - 1].spritePosition.X - rope.rope[i - 1].previousSpritePosition.X) + rope.rope[i - 1].idleHitbox.offsetX,
            //    Y = rope.rope[i - 1].previousSpritePosition.Y + (float)j / acc * (rope.rope[i - 1].spritePosition.Y - rope.rope[i - 1].previousSpritePosition.Y) + rope.rope[i - 1].idleHitbox.offsetY
            //};


            Vector2 displacementVec = position2 - position1;
            int maxInt = (int)Math.Floor(displacementVec.Length());

            if (maxInt >= 1)
            {


                for (int k = 0; k < maxInt - 1; k++)
                {
                    Rectangle testRect = new Rectangle
                    {
                        X = DistanceToNearestInteger(position1.X + ((float)(k + 1) / maxInt) * displacementVec.X),
                        Y = DistanceToNearestInteger(position1.Y + ((float)(k + 1) / maxInt) * displacementVec.Y),
                        Width = 1,
                        Height = 1
                    };


                    foreach (HitboxRectangle hitbox in hitboxes)
                    {
                        if (hitbox.rectangle.Contains(testRect))
                        {

                            // Top - right corner
                            if (testRect.X >= hitbox.rectangle.Center.X && testRect.Y <= hitbox.rectangle.Center.Y)
                            {
                                return new Vector2(hitbox.rectangle.X + hitbox.rectangle.Width, hitbox.rectangle.Y - 2);
                            }

                            // Bottom - right corner
                            if (testRect.X >= hitbox.rectangle.Center.X && testRect.Y >= hitbox.rectangle.Center.Y)
                            {
                                return new Vector2(hitbox.rectangle.X + hitbox.rectangle.Width, hitbox.rectangle.Y + hitbox.rectangle.Height);
                            }

                            // Top - left corner
                            if (testRect.X <= hitbox.rectangle.Center.X && testRect.Y <= hitbox.rectangle.Center.Y)
                            {
                                return new Vector2(hitbox.rectangle.X - 2, hitbox.rectangle.Y - 2);
                            }

                            // Bottom - left corner
                            if (testRect.X <= hitbox.rectangle.Center.X && testRect.Y >= hitbox.rectangle.Center.Y)
                            {
                                return new Vector2(hitbox.rectangle.X - 2, hitbox.rectangle.Y + hitbox.rectangle.Height);
                            }

                            //Vector2 vec1 = new Vector2(testRect.X, testRect.Y);
                            //return vec1;
                        }
                    }

                }
            }

            return new Vector2(0, 0);

        }




        public bool CheckIfPixelLineBetweenTwoPointsTouchesListOfHitboxes(Rope rope, Vector2 position1, Vector2 position2, List<HitboxRectangle> hitboxes)
        {
            Vector2 displacementVec = position2 - position1;
            int maxInt = (int)Math.Floor(displacementVec.Length());
            bool lineIntersectsTerrain = false;

            for (int k = 0; k < maxInt; k++)
            {
                HitboxRectangle testHitbox = new HitboxRectangle(DistanceToNearestInteger(position1.X + rope.rope[0].idleHitbox.offsetX + ((float)(k + 1) / maxInt) * displacementVec.X), DistanceToNearestInteger(position1.Y + rope.rope[0].idleHitbox.offsetY + ((float)(k + 1) / maxInt) * displacementVec.Y), 1, 1);


                foreach (HitboxRectangle hitbox in hitboxes)
                {
                    lineIntersectsTerrain = CheckForEdgesMeeting(testHitbox, hitbox);

                    if (lineIntersectsTerrain)
                    {

                        return true;
                    }
                }

                if (lineIntersectsTerrain)
                {
                    return true;
                }


            }

            return false;

        }


        public void CheckIfPixelLineBetweenTwoPointsIntersectsListOfHitboxesAndUpdateListOfRectangles(Vector2 position1, Vector2 position2, List<HitboxRectangle> hitboxes, List<Rectangle> rectangles)
        {

            Vector2 displacementVec = position2 - position1;
            int maxInt = (int)Math.Floor(displacementVec.Length());

            if (maxInt >= 1)
            {

                for (int k = 0; k < maxInt - 1; k++)
                {
                    Rectangle testRect = new Rectangle
                    {
                        X = DistanceToNearestInteger(position1.X + ((float)(k + 1) / maxInt) * displacementVec.X),
                        Y = DistanceToNearestInteger(position1.Y + ((float)(k + 1) / maxInt) * displacementVec.Y),
                        Width = 1,
                        Height = 1
                    };


                    foreach (HitboxRectangle hitbox in hitboxes)
                    {
                        if (hitbox.isActive && hitbox.rectangle.Contains(testRect))
                        {
                            //Debug.WriteLine("Here");
                            return;
                        }

                    }

                    rectangles.Add(testRect);

                }
            }


        }















    }
}
