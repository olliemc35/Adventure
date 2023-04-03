using Microsoft.Xna.Framework;
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

        public override void Update(GameTime gameTime)
        {
            if (detonate)
            {
                animatedSprite.Play("Detonate");
                currentFrame = frameAndTag["Detonate"].From;
                tagOfCurrentFrame = "Detonate";
            }
            else
            {
                animatedSprite.Play("Planted");
                currentFrame = frameAndTag["Planted"].From;
                tagOfCurrentFrame = "Planted";
            }

            animatedSprite.OnAnimationLoop = () =>
            {
                if (tagOfCurrentFrame == "Detonate")
                {
                    readyToRemove = true;
                    animatedSprite.OnAnimationLoop = null;
                }
            };

            base.Update(gameTime);
        }


    }
}
