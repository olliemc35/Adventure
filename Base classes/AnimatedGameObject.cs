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
        public Vector2 drawPosition;

        // If the object can be climbed upon this will be set to true
        public bool Climable = false;

        // If the object is a hazard - i.e. kills player on contact - this will be set to true
        public bool Hazard = false;

        // Every sprite will have a list of hitboxes - most AnimatedGameObjects will only have a single one which we call idleHitbox but more complicated
        // AnimatedGameObjects might have more - e.g. the OrganTop has a idleHitbox along with a baseHitbox (corresponding to the base part of the sprite).
        public List<HitboxRectangle> hitboxes = new List<HitboxRectangle>();

        // Every sprite will have a "base" hitbox which we call idleHitbox
        public HitboxRectangle idleHitbox;

        // We set CollisionSprite to true if we want the player / other objects to collide with this sprite
        // In this case the player will detect collisions with EVERY hitbox in the list hitboxes with has isActive set to TRUE.
        public bool CollisionObject = false;

        // The drawHitboxes bool is for DEBUGGING purposes
        // At some point we may want to draw the hitboxes to the screen to see exactly what is going on 
        public bool drawHitboxes = false;

        // The highlight bool is for DEBUGGING purposes
        // E.g. in collision detection code we can test for if (sprite.Highlight) to isolate that particular sprite in the code
        public bool Highlight = false;


        // Each object of this type will have at least one animation as created in the Aseprite file
        // Note that an AnimatedSprite object is essentially just an animation - it contains information to play / pause / stop etc.
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
            drawPosition = FindNearestIntegerVector(position);

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
            drawPosition = FindNearestIntegerVector(position);

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
            // If there are multiple animations for a given class we create these manually by overriding the LoadContent method
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
            // It is important that we draw at an integer position to avoid blurring effects which occur when the GraphicsDevice attempts to draw at sub-pixels.
            drawPosition = FindNearestIntegerVector(position);


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
                animation_playing.Draw(spriteBatch, drawPosition);
            }
        }

        public override void MoveManually(Vector2 moveVector)
        {
            position.X += moveVector.X;
            idleHitbox.rectangle.X = FindNearestInteger(position.X) + idleHitbox.offsetX;
            position.Y += moveVector.Y;
            idleHitbox.rectangle.Y = FindNearestInteger(position.Y) + idleHitbox.offsetY;
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

        public void UpdatePlayingAnimation(AnimatedSprite animation, int i = 0)
        {
            if (animation_playing != animation)
            {
                animation_playing.Stop();
                animation_playing.Reset();
                animation_playing = animation;
                animation_playing.Play(i);
            }
        }


        // If an AnimatedGameObject is climable we want to detect when we are in contact with the player and then tell the player to check whether we want to climb or not.
        public void UpdateClimable()
        {
            if (!player.playerStateManager.climbingState.Active)
            {
                if (colliderManager.CheckForCollision(player.idleHitbox, idleHitbox))
                {
                    player.Climb(this);
                }
            }

        }

        // If an AnimatedGameObject is a hazard then we want to detect when we are in contact with the player and then tell the player to kill themselves!
        public void UpdateHazard()
        {
            if (!player.playerStateManager.deadState.Active)
            {
                if (colliderManager.CheckForCollision(player.hurtHitbox, idleHitbox))
                {
                    player.KillPlayer();
                }
            }
        }


        // This is code which is used by the ActionScreenBuilder only.
        public override void AdjustHorizontally(ref List<int> ints)
        {
            position.X += ints[0];
            ints.RemoveAt(0);
        }
        public override void AdjustVertically(ref List<int> ints)
        {
            position.Y += ints[0];
            ints.RemoveAt(0);
        }




    }
}
