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

namespace Adventure
{
    public class AnimationSprite : Sprite
    {
        // animatedSprite has its own position vector which needs to be updated after we update the spritePosition.
        public AnimatedSprite animatedSprite;
        public AsepriteFrame baseFrame;
        public int currentFrame;
        public string tagOfCurrentFrame;
        public IDictionary<string, AsepriteTag> frameAndTag = new Dictionary<string, AsepriteTag>();
        public Vector2 animationPosition = new Vector2();
        public int previousFrameNumber = 0;

        public AnimationSprite() : base() { }
        public AnimationSprite(Vector2 initialPosition) : base(initialPosition) { }

        public AnimationSprite(Vector2 initialPosition, string filename) : base(initialPosition, filename) { }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            asepriteFile = contentManager.Load<AsepriteFile>(spriteFilename);
            animatedSprite = new AnimatedSprite();
            animatedSprite = new AnimatedSprite(asepriteFile);
            baseFrame = asepriteFile.Frames[0];
            currentFrame = 0;
            tagOfCurrentFrame = "Idle";
            frameAndTag = asepriteFile.Tags;

            spriteTexture = asepriteDocument.Texture;

            // Create the idleHitbox
            idleHitbox = new HitboxRectangle((int)spritePosition.X, (int)spritePosition.Y, baseFrame.Width, baseFrame.Height);
            spriteHitboxes.Add(idleHitbox);

            spriteHitboxTexture = new Texture2D(graphicsDevice, 1, 1);
            spriteHitboxTexture.SetData(new Color[] { Color.White });

            animationPosition.X = DistanceToNearestInteger(spritePosition.X);
            animationPosition.Y = DistanceToNearestInteger(spritePosition.Y);
            animatedSprite.Position = animationPosition;


            if (climable)
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
            animatedSprite.Update(gameTime);
            //animatedSprite.Position = spritePosition;
            // We set the position to be the nearest integer
            // This helps remove any pixel blurring. Essentially the computer doesn't know what to do if we ask it to draw at a sub-pixel and it draws
            // as both the nearest two pixels, creating this blurring effect. Hence it is necessary to draw at the nearest integer. Note that we don't want the actual sprite position
            // to be set as the nearest integer as otherwise the updates in spriteCollider would never work if we moved too slowly. 
            // This is often referred to as "clamping."
            animationPosition.X = DistanceToNearestInteger(spritePosition.X);
            animationPosition.Y = DistanceToNearestInteger(spritePosition.Y);
            animatedSprite.Position = animationPosition;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (drawHitboxes)
            {
                spriteBatch.Draw(spriteHitboxTexture, idleHitbox.rectangle, Color.Red);
            }
            if (Enabled)
            {
                animatedSprite.Render(spriteBatch);
            }
        }

    }
}
