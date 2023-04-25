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
    public class TeleportState : State
    {
        public bool teleportX = false;
        public bool teleportY = false;


        public bool isTeleportGlobal;
        public Teleport portal;
        public Vector2 teleportPosition;
        public Vector2 teleportDirection;
        public Vector2 globalTeleportBoostDirection = new Vector2();
        public bool teleportFlagX;
        public bool teleportFlagY;
        public int teleportCounter = 0;
        public int teleportEffectAnimationCounter = 0;
        public int teleportEffectLength = 10;
        public List<Rectangle> teleportEffectRectangles = new List<Rectangle>();
        public Vector2 lastPositionBeforeTeleport = new Vector2();
        public int framesStoppedWhilstTeleporting = 10;
        public float teleportSpeed = 300;





        public TeleportState(Player player, ScreenManager screenManager) : base(player, screenManager)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (enterStateFlag)
            {
                enterStateFlag = false;

                if (!isTeleportGlobal)
                {

                    Vector2 vec1 = player.position;
                    vec1.X += player.idleHitbox.offsetX + 0.5f * player.idleHitbox.rectangle.Width;
                    vec1.Y += player.idleHitbox.offsetY + 0.5f * player.idleHitbox.rectangle.Height;

                    Vector2 vec2 = portal.position;
                    vec2.X += 0.5f * portal.idleHitbox.rectangle.Width;
                    vec2.Y += 0.5f * portal.idleHitbox.rectangle.Height;

                    CreateTeleportEffect(vec1, vec2);
                    lastPositionBeforeTeleport = vec1;

                    teleportPosition = portal.position;
                    teleportDirection = vec2 - vec1;
                    teleportDirection.Normalize();

                    player.velocity.X = 0;
                    player.velocity.Y = 0;
                    player.displacement.X = 0;
                    player.displacement.Y = 0;
                    player.position = portal.position;


                }
                else if (isTeleportGlobal)
                {

                    Vector2 vec1 = player.position;
                    vec1.X += player.idleHitbox.offsetX + 0.5f * player.idleHitbox.rectangle.Width;
                    vec1.Y += player.idleHitbox.offsetY + 0.5f * player.idleHitbox.rectangle.Height;

                    Vector2 vec2 = portal.position;
                    vec2.X += 0.5f * portal.idleHitbox.rectangle.Width;
                    vec2.Y += 0.5f * portal.idleHitbox.rectangle.Height;

                    CreateTeleportEffect(vec1, vec2);
                    lastPositionBeforeTeleport = vec1;

                    player.velocity.X = 0;
                    player.velocity.Y = 0;
                    player.displacement.X = 0;
                    player.displacement.Y = 0;
                    player.position = portal.position;

                }
            }

            UpdateAnimations();
            UpdateVelocityAndDisplacement();

            player.colliderManager.AdjustForCollisionsMovingSpriteAgainstListOfSprites(player, screenManager.activeScreen.hitboxesToCheckCollisionsWith, 1, 10);

        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            if (teleportCounter < teleportEffectLength)
            {
                foreach (Rectangle rect in teleportEffectRectangles)
                {
                    Vector2 vec = new Vector2
                    {
                        X = rect.X,
                        Y = rect.Y
                    };

                    if (Vector2.Distance(lastPositionBeforeTeleport, vec) < (float)teleportEffectAnimationCounter / teleportEffectLength * Vector2.Distance(lastPositionBeforeTeleport, teleportPosition))
                    {
                        spriteBatch.Draw(player.idleHitbox.texture, rect, Color.White);
                    }

                }

                teleportEffectAnimationCounter += 1;
            }


        }


        public override void UpdateVelocityAndDisplacement()
        {



            if (isTeleportGlobal)
            {

                teleportDirection.X = player.directionX;

                if (player.directionX != 0)
                {
                    teleportDirection.Y = -1;
                }
                else
                {
                    if (player.directionY == -1)
                    {
                        teleportDirection.Y = -1;

                    }
                    else
                    {
                        teleportDirection.Y = 0;
                    }
                }


                if (teleportDirection.Length() > 0.001)
                {
                    teleportDirection.Normalize();
                }



            }

            if (teleportCounter < framesStoppedWhilstTeleporting)
            {
                teleportCounter += 1;
            }



        }

        public override void UpdateExits()
        {

            if (teleportCounter >= framesStoppedWhilstTeleporting)
            {

                player.velocity.X = teleportSpeed * teleportDirection.X;
                player.displacement.X = player.velocity.X * player.deltaTime;
                player.velocity.Y = teleportSpeed * teleportDirection.Y;
                player.displacement.Y = player.velocity.Y * player.deltaTime;
                player.launchFlag = true;
                teleportCounter = 0;
                exits = Exits.exitToNormalState;


            }



        }




        public void CreateTeleportEffect(Vector2 playerPosition, Vector2 teleportPosition)
        {
            teleportEffectRectangles.Clear();
            teleportEffectAnimationCounter = 0;

            Vector2 displacement = teleportPosition - playerPosition;

            Vector2 normalVec = new Vector2
            {
                X = -displacement.Y,
                Y = displacement.X
            };

            normalVec.Normalize();

            Random random = new Random();


            for (int j = 1; j <= displacement.Length(); j++)
            {
                Vector2 vec = playerPosition + (float)j / displacement.Length() * displacement;



                for (int k = -3; k <= 3; k++)
                {
                    float f = (float)random.NextDouble();



                    if ((float)j / displacement.Length() < 0.25f)
                    {
                        if (f < 0.4)
                        {
                            teleportEffectRectangles.Add(new Rectangle((int)(vec.X + k * normalVec.X), (int)(vec.Y + k * normalVec.Y), 1, 1));
                        }
                    }
                    else if ((float)j / displacement.Length() >= 0.25f && (float)j / displacement.Length() < 0.5f)
                    {
                        if (f < 0.2)
                        {
                            teleportEffectRectangles.Add(new Rectangle((int)(vec.X + k * normalVec.X), (int)(vec.Y + k * normalVec.Y), 1, 1));
                        }
                    }
                    else if ((float)j / displacement.Length() >= 0.5f && (float)j / displacement.Length() < 0.75f)
                    {
                        if (f < 0.05)
                        {
                            teleportEffectRectangles.Add(new Rectangle((int)(vec.X + k * normalVec.X), (int)(vec.Y + k * normalVec.Y), 1, 1));
                        }
                    }
                    else if ((float)j / displacement.Length() >= 0.75f && (float)j / displacement.Length() < 1f)
                    {
                        if (f < 0.01)
                        {
                            teleportEffectRectangles.Add(new Rectangle((int)(vec.X + k * normalVec.X), (int)(vec.Y + k * normalVec.Y), 1, 1));
                        }
                    }


                }
            }


        }


        public override void UpdateAnimations()
        {
            player.UpdatePlayingAnimation(player.animation_Teleport);

        }


    }
}
