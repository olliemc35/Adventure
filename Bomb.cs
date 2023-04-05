using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class Bomb : AnimationSprite
    {
        public bool detonate = false;
        public bool readyToRemove = false;
        public Note attachedNote;

        public AnimatedSprite animatedSprite_Detonate;
        public AnimatedSprite animatedSprite_Planted;


        public Bomb() : base()
        {

        }

        public Bomb(Vector2 initialPosition) : base(initialPosition)
        {

        }

        public Bomb(Vector2 initialPosition, string filename) : base(initialPosition, filename)
        {

        }

        public Bomb(Vector2 initialPosition, string filename, Note note) : base(initialPosition, filename)
        {
            attachedNote = note;
        }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            base.LoadContent(contentManager, graphicsDevice);

            animatedSprite_Detonate = spriteSheet.CreateAnimatedSprite("Detonate");
            animatedSpriteAndTag.Add("Detonate", animatedSprite_Detonate);
            animatedSprite_Planted = spriteSheet.CreateAnimatedSprite("Planted");
            animatedSpriteAndTag.Add("Planted", animatedSprite_Planted);

        }


        public override void Update(GameTime gameTime)
        {
            if (detonate)
            {
                nameOfCurrentAnimationSprite = "Detonate";
                //animatedSprite_Idle.Play("Detonate");
                //currentFrame = frameAndTag["Detonate"].From;
                //tagOfCurrentFrame = "Detonate";
            }
            else
            {
                nameOfCurrentAnimationSprite = "Planted";

                //animatedSprite_Idle.Play("Planted");
                //currentFrame = frameAndTag["Planted"].From;
                //tagOfCurrentFrame = "Planted";
            }


            animatedSprite_Detonate.OnAnimationLoop = (animatedSprite_Detonate) =>
            {
                
                    readyToRemove = true;
                    animatedSprite_Detonate.OnAnimationLoop = null;
                
            };

            base.Update(gameTime);
        }


    }
}
