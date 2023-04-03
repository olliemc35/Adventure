using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class HookPoint : AnimationSprite
    {
        public float radius;
        public bool InRange = false;

        public HookPoint(Vector2 initialPosition, string filename) : base(initialPosition, filename)
        {
            radius = 8 * 10;
        }

        public override void Update(GameTime gameTime)
        {

            if (Vector2.Distance(References.player.spritePosition, spritePosition) <= radius)
            {
                InRange = true;
                animatedSprite.Play("InRange");
                currentFrame = frameAndTag["InRange"].From;
                tagOfCurrentFrame = "InRange";
            }
            else
            {
                InRange = false;
                animatedSprite.Play("Idle");
                currentFrame = frameAndTag["Idle"].From;
                tagOfCurrentFrame = "Idle";
            }

            base.Update(gameTime);
        }
    }
}
