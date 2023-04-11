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
using System.Diagnostics;

namespace Adventure
{
    public class AnimatedGameObject : GameObject
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 previousPosition;
        public string filename;
        // This is the position in which we draw to the screen.
        // This will be the position vector clamped to the nearest integer - this is necessary to avoid drawing at sub-pixels and hence avoid blurring effects
        public Vector2 animationPosition;

        // If the object can be climbed upon this will be set to true
        public bool Climable = false;

        // If the object is a hazard - i.e. kills player on contact - this will be set to true
        public bool Hazard = false;

        // Every sprite will have a list of hitboxes (nearly all sprites will only have a single one)
        public List<HitboxRectangle> hitboxes = new List<HitboxRectangle>();

        // Every sprite will have a "base" hitbox which we call idleHitbox
        public HitboxRectangle idleHitbox;


        // We set CollisionSprite to true if we want the player / other objects to collide with this sprite
        public bool CollisionObject = false;


        // The drawHitboxes bool is for DEBUGGING purposes
        // At some point we may want to draw the hitboxes to the screen to see exactly what is going on 
        public bool drawHitboxes = false;

        // The highlight bool is for DEBUGGING purposes
        // E.g. in collision detection code we can test for if (sprite.Highlight) to isolate that particular sprite in the code
        public bool Highlight = false;


        // Each object of this type will have a list of animations as created in the Aseprite file
        // An AnimatedSprite object is essentially just an animation - it contains information to play / pause / stop etc.
        // Every object will have at least one animation, which we call animation_Idle
        public SpriteSheet spriteSheet;
        public AnimatedSprite animation_Idle;
        // This will be a reference to the animation we want to be playing
        public AnimatedSprite animation_playing;



        public AnimatedGameObject()
        {
        }

        public AnimatedGameObject(Vector2 position)
        {
            this.position = position;
            previousPosition = position;
            animationPosition = FindNearestIntegerVector(position);

        }


        public AnimatedGameObject(Vector2 position, string filename, AssetManager assetManager, ColliderManager colliderManager = null, InputManager inputManager = null, ScreenManager screenManager = null, Player player = null)
        {
            this.position = position;
            this.filename = filename;
            this.assetManager = assetManager;
            this.colliderManager = colliderManager;
            this.inputManager = inputManager;
            this.screenManager = screenManager;
            this.player = player;
            previousPosition = position;
            animationPosition = FindNearestIntegerVector(position);

        }

        public AnimatedGameObject(string filename)
        {
            this.filename = filename;
        }

        public override void LoadContent()
        {
            // Obtain the correct spritesheet from the assetManager
            spriteSheet = assetManager.spriteSheets[filename];
            // We then form AnimatedSprites from each animation in the spritesheet - these are called by the tag names we give the animation in Aseprite
            animation_Idle = spriteSheet.CreateAnimatedSprite("Idle");
            animation_Idle.Play();
            animation_playing = animation_Idle;

            // Create the idleHitbox
            idleHitbox = new HitboxRectangle((int)position.X, (int)position.Y, animation_Idle.Width, animation_Idle.Height);
            idleHitbox.texture = assetManager.hitboxTexture;
            hitboxes.Add(idleHitbox);

            if (Climable)
            {
                idleHitbox.isActive = true;
            }

        }


        public virtual void ManageAnimations()
        {

        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            animation_playing.Update(gameTime);
            animationPosition = FindNearestIntegerVector(position);

            if (Climable)
            {
                UpdateClimable();
            }

            if (Hazard)
            {
                UpdateHazard();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (drawHitboxes)
            {
                spriteBatch.Draw(idleHitbox.texture, idleHitbox.rectangle, Color.Red);
            }
            if (Enabled)
            {
                animation_playing.Draw(spriteBatch, animationPosition);
            }
        }


        public int FindNearestInteger(float x)
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

        public Vector2 FindNearestIntegerVector(Vector2 vec)
        {
            return new Vector2(FindNearestInteger(vec.X), FindNearestInteger(vec.Y));
        }

        public void UpdateVectorToNearestInteger(Vector2 vec)
        {
            vec.X = FindNearestInteger(vec.X);
            vec.Y = FindNearestInteger(vec.Y);
        }

        public void UpdatePlayingAnimation(AnimatedSprite animation, int i = 0)
        {
            if (animation_playing != animation)
            {
                //if (animation_playing == References.player.animation_Dead)
                //{
                //    Debug.WriteLine("here");

                //}

                animation_playing.Stop();
                animation_playing.Reset();
                animation_playing = animation;
                animation_playing.Play(i);
            }
        }


        // SEPARATE THIS OUT INTO ITS OWN CLASS
        public void UpdateClimable()
        {
            if (!player.playerStateManager.climbingState.Active)
            {


                if (colliderManager.CheckForCollision(player.idleHitbox, idleHitbox))
                {
                    if (player.CollidedOnTop && player.velocity.Y < 0)
                    {
                        player.playerStateManager.DeactivatePlayerStates();
                        player.playerStateManager.climbingState.Activate();
                        player.playerStateManager.climbingState.platform = this;
                        return;
                    }
                    if (player.CollidedOnRight || player.CollidedOnLeft)
                    {
                        player.playerStateManager.DeactivatePlayerStates();
                        player.playerStateManager.climbingState.Activate();
                        player.playerStateManager.climbingState.platform = this;
                        return;
                    }
                    else if (player.CollidedOnBottom)
                    {
                        if ((player.spriteDirectionX == 1 || player.spriteDirectionY == 1) && player.idleHitbox.rectangle.X >= idleHitbox.rectangle.X + idleHitbox.rectangle.Width - player.distanceCanStartClimbing)
                        {
                            player.idleHitbox.rectangle.X = idleHitbox.rectangle.X + idleHitbox.rectangle.Width;
                            player.idleHitbox.rectangle.Y = idleHitbox.rectangle.Y;
                            player.position.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                            player.position.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;
                            player.playerStateManager.DeactivatePlayerStates();
                            player.playerStateManager.climbingState.Activate();
                            player.playerStateManager.climbingState.platform = this;
                            return;
                        }
                        else if ((player.spriteDirectionX == -1 || player.spriteDirectionY == 1) && player.idleHitbox.rectangle.X <= idleHitbox.rectangle.X - player.distanceCanStartClimbing)
                        {
                            player.idleHitbox.rectangle.X = idleHitbox.rectangle.X - player.idleHitbox.rectangle.Width;
                            player.idleHitbox.rectangle.Y = idleHitbox.rectangle.Y;
                            player.position.X = player.idleHitbox.rectangle.X - player.idleHitbox.offsetX;
                            player.position.Y = player.idleHitbox.rectangle.Y - player.idleHitbox.offsetY;
                            player.playerStateManager.DeactivatePlayerStates();
                            player.playerStateManager.climbingState.Activate();
                            player.playerStateManager.climbingState.platform = this;
                            return;
                        }

                    }
                }
            }

        }

        public void UpdateHazard()
        {
            if (!player.playerStateManager.deadState.Active)
            {
                if (colliderManager.CheckForCollision(player.idleHitbox, idleHitbox))
                {
                    player.Dead = true;
                    player.playerStateManager.DeactivatePlayerStates();
                    player.playerStateManager.deadState.Activate();
                }
            }
        }
    }
}
