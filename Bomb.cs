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
    public class Bomb : AnimatedGameObject
    {
        public bool detonate = false;
        public bool readyToRemove = false;
        public Note attachedNote;

        public AnimatedSprite animation_Detonate;
        public AnimatedSprite animation_Planted;


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

            animation_Detonate = spriteSheet.CreateAnimatedSprite("Detonate");
            animation_Planted = spriteSheet.CreateAnimatedSprite("Planted");

            animation_Detonate.OnAnimationLoop = (animatedSprite_Detonate) =>
            {

                readyToRemove = true;
                animatedSprite_Detonate.OnAnimationLoop = null;

            };

        }


        public override void Update(GameTime gameTime)
        {
            if (detonate)
            {
                UpdatePlayingAnimation(animation_Detonate);
            }
            else
            {
                UpdatePlayingAnimation(animation_Planted);
            }


      

            base.Update(gameTime);
        }


    }
}
