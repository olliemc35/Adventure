using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Adventure
{
    public class Orb : MovingSprite
    {
        public float speed;
        public OrbVessel orbVessel;
        public bool inWindow = false;
        public bool isActive = false;

        public bool setToTurnInActive = false;


        public Orb(Vector2 initialPosition, string filename, float speed, OrbVessel orbVessel) : base(initialPosition, filename)
        {
            this.speed = speed;
            this.orbVessel = orbVessel;
        }


        public override void Update(GameTime gameTime)
        {


            if (spritePosition.X <= orbVessel.orbEndLeft)
            {
                if (setToTurnInActive)
                {
                    setToTurnInActive = false;
                    isActive = false;
                    return;
                }
                else
                {
                    spritePosition.X = orbVessel.orbEndRight;
                }

            }
            else
            {
                spritePosition.X -= speed;
                idleHitbox.rectangle.X = (int)spritePosition.X + idleHitbox.offsetX;
            }

            if (spritePosition.X >= orbVessel.orbWindowLeft && spritePosition.X <= orbVessel.orbWindowRight)
            {
                inWindow = true;
            }
            else
            {
                inWindow = false;
            }


            base.Update(gameTime);

        }


    }
}
