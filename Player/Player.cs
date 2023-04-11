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


        public KeyboardState keyboardState;
        public KeyboardState oldKeyboardState;
        public MouseState mouseState;
        public MouseState oldMouseState;


        public Vector2 ropeAnchor = new Vector2(0, 0);

        public bool DirectionChangedX;
        public bool DirectionChangedY;


        // These will be +/-1 or 0
        public int spriteDirectionX;
        public int spriteDirectionY;
        public int previousSpriteDirectionX;
        public int previousSpriteDirectionY;

        public float jumpDuration;
        public float jumpHeight;
        public float jumpSpeed;
        public float wallJumpWithRopeSpeedX;

        public bool jumpButtonPressed = false;
        public bool runButtonPressed = false;
        public bool gunButtomPressed = false;
        public bool flagJumpButtonPressed = false;
        public bool flagLeftDashButtonPressed = false;
        public bool flagGunButtonPressed = false;
        public bool flagLeftMouseClick = false;
        public bool flagTeleportButtonPressed = false;
        public bool jumpingWithRopeAttachedToTheLeft = false;
        public bool flagBombButtonPressed = false;
        public bool flagHookButtonPressed = false;
        public bool climbButtonPressed = false;

        public bool launchFlag = false;

        public int distanceCanStartClimbing = 6;


        public bool Respawn = false;
        public bool Dead = false;

        public bool twoKeysPressedFirstTimeX = true;


        public bool gunEquipped = false;

        public float distanceFromRopeHookWhenAttached = 0;
        //public float knockBackInterpolator = 0f;


        public bool startedJumpingAgainstWall = false;

        public float jump;
        public float leftDash;
        public float climbingSpeed = 60;





        public int health = 3;


        //public Vector2 mousePosition = new Vector2();


        public RopeForPlayer rope;
        public Gun gun;
        public bool fire = false;

        // This will be a reference / pointer to the ladder we are climbing
        public Ladder ladder;

        public bool ropeActive = false;
        public bool pullBackRope = false;




        public float lambdaDecelFromSwing;

        public float speedToStartOfFrom = 0;
        public float initialVelocity = 0;
        public Vector2 velocityAtTheBeginningOfTheInterval = new Vector2(0, 0);
        public float timerXAtTheBeginningOfTheInterval = 0;





        public bool onMovingPlatform = false;
        public Vector2 velocityOffSetDueToMovingPlatform = new Vector2(0, 0);

        public bool ribbonInHand = false;
        public List<Ribbon> ribbons;
        public int ribbonIndex = 0;

        public Bomb bomb;
        public bool bombPlanted = false;


        public PlayerStateManager playerStateManager;





        public Player(Vector2 initialPosition, string filename, AssetManager assetManager, ColliderManager colliderManager, InputManager inputManager, ScreenManager screenManager) : base(initialPosition, filename, assetManager)
        {
            this.colliderManager = colliderManager;
            this.inputManager = inputManager;
            this.screenManager = screenManager;


            maxHorizontalSpeed = 60;

            jumpDuration = 0.5f;
            jumpHeight = 24;
            leftDash = 500;

            gravityConstant = 750;
            mass = 8 * jumpHeight / (jumpDuration * jumpDuration * gravityConstant);
            jumpSpeed = 0.5f * mass * gravityConstant * jumpDuration;
            wallJumpWithRopeSpeedX = jumpSpeed;


            jump = 0;


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

        public void ReturnToRespawnAnimation(AnimatedSprite animation)
        {
            //Debug.WriteLine("here");
            //Dead = false;
            UpdatePlayingAnimation(animation_Respawn, 1);
            position.X = screenManager.activeScreen.respawnPoint.X;
            position.Y = screenManager.activeScreen.respawnPoint.Y;
            idleHitbox.rectangle.X = (int)position.X + idleHitbox.offsetX;
            idleHitbox.rectangle.Y = (int)position.Y + idleHitbox.offsetY;
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
            animation_LandedLeft.OnAnimationEnd = ReturnToIdleAnimation;
            animation_Respawn.OnAnimationEnd = ReturnToIdleAnimation;
            animation_Dead.OnAnimationEnd = ReturnToRespawnAnimation;

            idleHitbox.rectangle.Width = 10;
            idleHitbox.offsetX = 3;
            idleHitbox.rectangle.Height = 14;
            idleHitbox.offsetY = 2;
            idleHitbox.rectangle.X = (int)position.X + idleHitbox.offsetX;
            idleHitbox.rectangle.Y = (int)position.Y + idleHitbox.offsetY;
            idleHitbox.isActive = true;


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
            
            bomb.Update(gameTime);
            foreach (Ribbon ribbon in ribbons)
            {
                if (ribbon.Enabled)
                {
                    ribbon.Update(gameTime);
                }
            }

            ropeAnchor.X = position.X + idleHitbox.offsetX + 1;
            ropeAnchor.Y = position.Y + idleHitbox.offsetY + idleHitbox.rectangle.Height / 2 - 2;


            base.Update(gameTime);

            inputManager.PlayerMovementInput(this);
            MovePlayer(gameTime);



        }



        public override void Draw(SpriteBatch spriteBatch)
        {



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
            //spriteBatch.Draw(spriteHitboxTexture, idleHitbox.rectangle, Color.Red);

            playerStateManager.Draw(spriteBatch);


            base.Draw(spriteBatch);

            //animatedSpriteAndTag[nameOfCurrentAnimationSprite].Draw(spriteBatch, animationPosition);
            rope.Draw(spriteBatch);

            bomb.Draw(spriteBatch);

            foreach(Ribbon ribbon in ribbons)
            {
                if (ribbon.Enabled)
                {
                    ribbon.Draw(spriteBatch);
                }
            }

            //gun.Draw(spriteBatch);

        }
    }
}
