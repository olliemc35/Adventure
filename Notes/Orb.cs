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
    public class Orb : MovingGameObject
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


            if (position.X <= orbVessel.orbEndLeft)
            {
                if (setToTurnInActive)
                {
                    setToTurnInActive = false;
                    isActive = false;
                    return;
                }
                else
                {
                    position.X = orbVessel.orbEndRight;
                }

            }
            else
            {
                position.X -= speed;
                idleHitbox.rectangle.X = (int)position.X + idleHitbox.offsetX;
            }

            if (position.X >= orbVessel.orbWindowLeft && position.X <= orbVessel.orbWindowRight)
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
