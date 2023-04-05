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

namespace Adventure
{
    public class AnimationSprite : Sprite
    {
        public SpriteSheet spriteSheet;
        public List<AnimatedSprite> animations = new List<AnimatedSprite>();
        public AnimatedSprite animation_Idle;
        public IDictionary<string, AnimatedSprite> animatedSpriteAndTag = new Dictionary<string, AnimatedSprite>();
        public string nameOfCurrentAnimationSprite;



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
            //asepriteFile = contentManager.Load<AsepriteFile>(spriteFilename);
            //animatedSprite = (AnimatedSprite)SpriteProcessor.Process(graphicsDevice, asepriteFile, aseFrameIndex: 0);

            //  Load the Aseprite file
            asepriteFile = contentManager.Load<AsepriteFile>(spriteFilename);
            //  Use the SpriteSheetProcessor to process the SpriteSheet from the Aseprite file.
            spriteSheet = SpriteSheetProcessor.Process(graphicsDevice, asepriteFile);
            animation_Idle = spriteSheet.CreateAnimatedSprite("Idle");
            animatedSpriteAndTag.Add("Idle", animation_Idle);
            nameOfCurrentAnimationSprite = "Idle";
            animation_Idle.Play();
            animations.Add(animation_Idle);


 

            // Load in the relevant aseprite file
            //asepriteFile = contentManager.Load<AsepriteFile>(spriteFilename);
            // textureAtlas = TextureAtlasProcessor.Process(graphicsDevice, asepriteFile);
            //spriteTexture = textureAtlas.GetRegion(0).Texture;
            //animatedSprite = (AnimatedSprite)textureAtlas.CreateSprite(regionIndex: 0);
            //AsepriteTag tag = asepriteFile.GetTag(0);
            //animatedSprite = new AnimatedSprite(tag);


            baseFrame = asepriteFile.Frames[0];
            currentFrame = 0;
            tagOfCurrentFrame = "Idle";


            //frameAndTag = asepriteFile.Tags;

            //spriteTexture = asepriteDocument.Texture;

            // Create the idleHitbox
            idleHitbox = new HitboxRectangle((int)spritePosition.X, (int)spritePosition.Y, baseFrame.Width, baseFrame.Height);
            spriteHitboxes.Add(idleHitbox);

            spriteHitboxTexture = new Texture2D(graphicsDevice, 1, 1);
            spriteHitboxTexture.SetData(new Color[] { Color.White });

            animationPosition.X = DistanceToNearestInteger(spritePosition.X);
            animationPosition.Y = DistanceToNearestInteger(spritePosition.Y);
            //animatedSprite.Position = animationPosition;


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

            //animatedSprite_Idle.Update(gameTime);
            //animatedSprite_Idle.Play();

            foreach (AnimatedSprite anim in animations) { anim.Update(gameTime); }
            //animatedSpriteAndTag[nameOfCurrentAnimationSprite].Update(gameTime);
            //animatedSpriteAndTag[nameOfCurrentAnimationSprite].Play();
            //animatedSprite.Update(gameTime);
            //animatedSprite.Position = spritePosition;
            // We set the position to be the nearest integer
            // This helps remove any pixel blurring. Essentially the computer doesn't know what to do if we ask it to draw at a sub-pixel and it draws
            // as both the nearest two pixels, creating this blurring effect. Hence it is necessary to draw at the nearest integer. Note that we don't want the actual sprite position
            // to be set as the nearest integer as otherwise the updates in spriteCollider would never work if we moved too slowly. 
            // This is often referred to as "clamping."
            animationPosition.X = DistanceToNearestInteger(spritePosition.X);
            animationPosition.Y = DistanceToNearestInteger(spritePosition.Y);
           // animatedSprite.Position = animationPosition;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (drawHitboxes)
            {
                spriteBatch.Draw(spriteHitboxTexture, idleHitbox.rectangle, Color.Red);
            }
            if (Enabled)
            {
                foreach (AnimatedSprite animation in animations)
                {
                    if (animation.IsAnimating)
                    {
                        animation.Draw(spriteBatch, animationPosition);
                    }
                }
                //animatedSpriteAndTag[nameOfCurrentAnimationSprite].Draw(spriteBatch, animationPosition);
            }
        }

    }
}
