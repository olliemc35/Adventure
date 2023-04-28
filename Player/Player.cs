using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite;
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Aseprite.Content.Processors;
using Microsoft.Xna.Framework.Input;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
//using System.Numerics;

namespace Adventure
{
    public class Player : SwingingGameObject
    {
        public AnimatedSprite animation_IdleLeft;
        public AnimatedSprite animation_MoveRight;
        public AnimatedSprite animation_MoveLeft;
        public AnimatedSprite animation_JumpRight;
        public AnimatedSprite animation_JumpLeft;
        public AnimatedSprite animation_Respawn;
        public AnimatedSprite animation_Dead;
        public AnimatedSprite animation_ClimbingLadder;
        public AnimatedSprite animation_Teleport;
        public AnimatedSprite animation_SlideRight;
        public AnimatedSprite animation_SlideLeft;
        public AnimatedSprite animation_ClimbTop;
        public AnimatedSprite animation_Landed;
        public AnimatedSprite animation_LandedLeft;
        public AnimatedSprite animation_FallingRight;
        public AnimatedSprite animation_FallingLeft;


        public Vector2 ropeAnchor = new Vector2(0, 0);

        public bool DirectionChangedX;
        public bool DirectionChangedY;


        // These will be +/-1 or 0
        public int directionX;
        public int directionY;
        public int previousDirectionX;
        public int previousDirectionY;

        public float jumpDuration;
        public float jumpHeight;
        public float jumpSpeed;

        public bool jumpButtonPressed = false;
        public bool runButtonPressed = false;
        public bool flagJumpButtonPressed = false;
        public bool flagLeftMouseClick = false;
        public bool flagTeleportButtonPressed = false;
        public bool flagBombButtonPressed = false;
        public bool flagHookButtonPressed = false;
        public bool climbButtonPressed = false;

        public bool launchFlag = false;

        public int distanceCanStartClimbing = 6; // This is the distance away from the edge of a climbable platform at which the player can start climbing



        public bool twoKeysPressedFirstTimeX = true;



        public float distanceFromRopeHookWhenAttached = 0;



        public RopeForPlayer rope;

        public List<Ribbon> ribbons;
        public int ribbonIndex = 0;
        public bool ribbonInHand = false;


        public Bomb bomb;
        public bool bombPlanted = false;


        public PlayerStateManager playerStateManager;

        public HitboxRectangle hurtHitbox = new HitboxRectangle();




        public Player(Vector2 initialPosition, string filename, AssetManager assetManager, ColliderManager colliderManager, InputManager inputManager, ScreenManager screenManager) : base(initialPosition, filename, assetManager)
        {
            this.colliderManager = colliderManager;
            this.inputManager = inputManager;
            this.screenManager = screenManager;


            maxHorizontalSpeed = 60;

            jumpDuration = 0.5f;
            jumpHeight = 24;

            gravityConstant = 750;
            mass = 8 * jumpHeight / (jumpDuration * jumpDuration * gravityConstant);
            jumpSpeed = 0.5f * mass * gravityConstant * jumpDuration;




            maxVerticalSpeed = 300;


            //mass = 1;
            //staffHitboxes = new List<HitboxRectangle>();

            //lambdaDecelFromSwing = 0.05f * lambdaDecel;


            swingForceDuration = 0.5f;
            swingForceMaximum = 5;


            //swingForceMaximum = 5f / (2 * swingForceDuration);


            //Highlight = true;
            rope = new RopeForPlayer(position, this, assetManager, screenManager);
            //gun = new Gun(this, assetManager);
            bomb = new Bomb(position, "NoteBomb", assetManager, colliderManager, inputManager, screenManager, this);
            ribbons = new List<Ribbon>()
            {
                new Ribbon(this, position, assetManager, colliderManager, inputManager, screenManager){Enabled = false },
                new Ribbon(this, position, assetManager, colliderManager, inputManager, screenManager){Enabled = false },
                new Ribbon(this, position, assetManager,colliderManager, inputManager, screenManager){Enabled = false },
            };

            //rope.LoadContent(References.content, References.graphicsDevice);


            // There was an issue where deltaTime was not getting updated on the first frame, and this meant that if we fell from a high ground
            // timeY would never be incremented (as deltaTime was zero). This is because we are now calling the base.Update (which updated deltaTime)
            // at the end of the frame. A fix would be to just update deltaTime at the beginning of the frame.
            deltaTime = (float)1 / 60;



            playerStateManager = new PlayerStateManager(this, screenManager, assetManager);

        }






        public void ReturnToIdleAnimation(AnimatedSprite animation)
        {
            UpdatePlayingAnimation(animation_Idle);
        }
        public void ReturnToIdleLeftAnimation(AnimatedSprite animation)
        {
            UpdatePlayingAnimation(animation_IdleLeft);
        }

        public void ReturnToRespawnAnimation(AnimatedSprite animation)
        {
            //Debug.WriteLine("here");
            //Dead = false;
            UpdatePlayingAnimation(animation_Respawn, 1);
            position.X = screenManager.activeScreen.respawnPoint.X;
            position.Y = screenManager.activeScreen.respawnPoint.Y;
            idleHitbox.rectangle.X = (int)position.X + idleHitbox.offsetX;
            idleHitbox.rectangle.Y = (int)position.Y + idleHitbox.offsetY;
            hurtHitbox.rectangle.X = (int)position.X + hurtHitbox.offsetX;
            hurtHitbox.rectangle.Y = (int)position.Y + hurtHitbox.offsetY;
            //player.animatedSprite_Idle.Position = player.spritePosition;
            velocity.X = 0;
            velocity.Y = 0;
        }
        //public void ReturnToIdleAnimation(AnimatedSprite animation)
        //{
        //    animation.Stop();
        //    animation_Idle.Play();
        //}

        public override void LoadContent()
        {

            base.LoadContent();



            animation_IdleLeft = spriteSheet.CreateAnimatedSprite("IdleLeft");
            animation_MoveRight = spriteSheet.CreateAnimatedSprite("MoveRight");
            animation_MoveLeft = spriteSheet.CreateAnimatedSprite("MoveLeft");
            animation_JumpRight = spriteSheet.CreateAnimatedSprite("JumpRight");
            animation_JumpLeft = spriteSheet.CreateAnimatedSprite("JumpLeft");
            animation_Respawn = spriteSheet.CreateAnimatedSprite("Respawn");
            animation_Dead = spriteSheet.CreateAnimatedSprite("Dead");
            animation_ClimbingLadder = spriteSheet.CreateAnimatedSprite("ClimbingLadder");
            animation_Teleport = spriteSheet.CreateAnimatedSprite("Teleport");
            animation_SlideRight = spriteSheet.CreateAnimatedSprite("SlideRight");
            animation_SlideLeft = spriteSheet.CreateAnimatedSprite("SlideLeft");
            animation_ClimbTop = spriteSheet.CreateAnimatedSprite("ClimbTop");
            animation_Landed = spriteSheet.CreateAnimatedSprite("Landed");
            animation_LandedLeft = spriteSheet.CreateAnimatedSprite("LandedLeft");
            animation_FallingRight = spriteSheet.CreateAnimatedSprite("FallingRight");
            animation_FallingLeft = spriteSheet.CreateAnimatedSprite("FallingLeft");


            animation_Landed.OnAnimationEnd = ReturnToIdleAnimation;
            animation_LandedLeft.OnAnimationEnd = ReturnToIdleLeftAnimation;
            animation_Respawn.OnAnimationEnd = ReturnToIdleAnimation;
            animation_Dead.OnAnimationEnd = ReturnToRespawnAnimation;

            idleHitbox.rectangle.Width = 10;
            idleHitbox.offsetX = 3;
            idleHitbox.rectangle.Height = 14;
            idleHitbox.offsetY = 2;
            idleHitbox.rectangle.X = (int)position.X + idleHitbox.offsetX;
            idleHitbox.rectangle.Y = (int)position.Y + idleHitbox.offsetY;
            idleHitbox.isActive = true;

            hurtHitbox.texture = assetManager.hitboxTexture;
            hurtHitbox.rectangle.Width = 6;
            hurtHitbox.offsetX = 5;
            hurtHitbox.rectangle.Height = 11;
            hurtHitbox.offsetY = 4;
            hurtHitbox.rectangle.X = (int)position.X + hurtHitbox.offsetX;
            hurtHitbox.rectangle.Y = (int)position.Y + hurtHitbox.offsetY;
            hurtHitbox.isActive = true;


            bomb.LoadContent();

            foreach (Ribbon ribbon in ribbons)
            {
                ribbon.LoadContent();
            }



        }







        public void MovePlayer(GameTime gameTime)
        {
            bool oldCollidedOnLeft = CollidedOnLeft;
            bool oldCollidedOnRight = CollidedOnRight;
            bool oldCollidedOnTop = CollidedOnTop;
            bool oldCollidedOnBottom = CollidedOnBottom;

            previousVelocity = velocity;
            previousPosition = position;

            playerStateManager.Update(gameTime);


            flagCollidedOnLeft = !oldCollidedOnLeft && CollidedOnLeft;
            flagCollidedOnRight = !oldCollidedOnRight && CollidedOnRight;
            flagCollidedOnTop = !oldCollidedOnTop && CollidedOnTop;
            flagCollidedOnBottom = !oldCollidedOnBottom && CollidedOnBottom;

        }



        public override void Update(GameTime gameTime)
        {
            //Debug.WriteLine("A: " + animation_playing.Name);
            //Debug.WriteLine("B: " + spriteDirectionX);
            //Debug.WriteLine("C: " + playerStateManager.normalState.Active);
            //Debug.WriteLine(velocity.X);
            bomb.Update(gameTime);
            foreach (Ribbon ribbon in ribbons)
            {
                if (ribbon.Enabled)
                {
                    ribbon.Update(gameTime);
                }
            }

            //if (playerStateManager.climbingState.Active)
            //{
            //    Debug.WriteLine(playerStateManager.climbingState.Active);

            //}

            ropeAnchor.X = position.X + idleHitbox.offsetX + 1;
            ropeAnchor.Y = position.Y + idleHitbox.offsetY + idleHitbox.rectangle.Height / 2 - 2;



            MovePlayer(gameTime);

            base.Update(gameTime);


        }



        public override void Draw(SpriteBatch spriteBatch)
        {

            //Debug.WriteLine(animation_playing.Name);

            //if (!drawHitboxes)
            //{
            //    foreach (HitboxRectangle hitbox in spriteHitboxes)
            //    {
            //        if (hitbox.isActive)
            //        {
            //            spriteBatch.Draw(spriteHitboxTexture, hitbox.rectangle, Color.Red);
            //        }
            //    }

            //}

            //spriteBatch.Draw(idleHitbox.texture, idleHitbox.rectangle, Color.Red);
            //spriteBatch.Draw(hurtHitbox.texture, hurtHitbox.rectangle, Color.Blue);



            playerStateManager.Draw(spriteBatch);


            base.Draw(spriteBatch);

            //animatedSpriteAndTag[nameOfCurrentAnimationSprite].Draw(spriteBatch, animationPosition);
            rope.Draw(spriteBatch);

            bomb.Draw(spriteBatch);

            foreach (Ribbon ribbon in ribbons)
            {
                if (ribbon.Enabled)
                {
                    ribbon.Draw(spriteBatch);
                }
            }

            //gun.Draw(spriteBatch);

        }



        public void KillPlayer()
        {
            playerStateManager.DeactivatePlayerStates();
            playerStateManager.deadState.Activate();
        }

        public void Climb(AnimatedGameObject platform)
        {


            // We deal with the collider bools on the PLATFORM hitbox
            // This is much better/easier - as the player hitbox will be collided with other objects such as the terrain 
            // E.g. player.idleHitbox.CollidedOnBottom will be true if we are standing on any ground, whereas we would like to test when we are standing on top of the CLIMABLE object
            // We can do this by looking at platform.idleHitbox.CollidedOnTop

            // Another issue: platform can detect CollidedOnTop in situations where the player is not on top (e.g. try jumping at the ivy, if we hit mid-fall often fall through. This must be because platofrm is detecting collided on TOP)
            colliderManager.UpdateHitboxRectangleColliderBools2(platform.idleHitbox, idleHitbox);


            if (idleHitbox.rectangle.Y > platform.idleHitbox.rectangle.Y - idleHitbox.rectangle.Height)
            {

                if (platform.idleHitbox.CollidedOnLeft && directionX == 1)
                {
                    playerStateManager.DeactivatePlayerStates();
                    playerStateManager.climbingState.Activate();
                    playerStateManager.climbingState.platform = platform;
                    playerStateManager.climbingState.orientation = ClimbingState.Orientation.right;
                }
                else if (platform.idleHitbox.CollidedOnRight && directionX == -1)
                {
                    playerStateManager.DeactivatePlayerStates();
                    playerStateManager.climbingState.Activate();
                    playerStateManager.climbingState.platform = platform;
                    playerStateManager.climbingState.orientation = ClimbingState.Orientation.left;
                }
                else if (platform.idleHitbox.CollidedOnBottom && directionY == 1 && platform.idleHitbox.rectangle.Width > 8)
                {
                    playerStateManager.DeactivatePlayerStates();
                    playerStateManager.climbingState.Activate();
                    playerStateManager.climbingState.platform = platform;
                    playerStateManager.climbingState.orientation = ClimbingState.Orientation.top;
                }
            }
            else
            {

                //In this case the player is standing on top of a platform which is climable
                //If they press the down key, and are standing in the right position, we want them to move to a CLIMB state
                //If the platform is wide enough the "right position" simply depends on which corner we are closest to
                //Otherwise, we need to check collisions with other terrain elements in the level, in order to determine which side of the platform is free and the side we should be climbing (think of ivy for this case)

                if (directionY == 1)
                {
                    if (platform.idleHitbox.rectangle.Width > 8)
                    {
                        if (position.X < platform.idleHitbox.rectangle.X + distanceCanStartClimbing)
                        {
                            playerStateManager.DeactivatePlayerStates();
                            playerStateManager.climbingState.Activate();
                            playerStateManager.climbingState.platform = platform;
                            playerStateManager.climbingState.orientation = ClimbingState.Orientation.right;

                        }
                        else if (position.X > platform.idleHitbox.rectangle.X + platform.idleHitbox.rectangle.Width - distanceCanStartClimbing)
                        {
                            playerStateManager.DeactivatePlayerStates();
                            playerStateManager.climbingState.Activate();
                            playerStateManager.climbingState.platform = platform;
                            playerStateManager.climbingState.orientation = ClimbingState.Orientation.left;
                        }

                    }
                    else
                    {


                        colliderManager.ResetColliderBoolsForHitbox(platform.idleHitbox);

                        foreach (HitboxRectangle hitbox in screenManager.activeScreen.hitboxesToCheckCollisionsWith)
                        {
                            colliderManager.UpdateHitboxRectangleColliderBools2(platform.idleHitbox, hitbox);
                        }

                        if (platform.idleHitbox.CollidedOnRight)
                        {
                            playerStateManager.DeactivatePlayerStates();
                            playerStateManager.climbingState.Activate();
                            playerStateManager.climbingState.platform = platform;
                            playerStateManager.climbingState.orientation = ClimbingState.Orientation.right;
                        }
                        else if (platform.idleHitbox.CollidedOnLeft)
                        {
                            playerStateManager.DeactivatePlayerStates();
                            playerStateManager.climbingState.Activate();
                            playerStateManager.climbingState.platform = platform;
                            playerStateManager.climbingState.orientation = ClimbingState.Orientation.left;
                        }

                    }
                }

            }

            colliderManager.ResetColliderBoolsForHitbox(platform.idleHitbox);

        }

    }
}
