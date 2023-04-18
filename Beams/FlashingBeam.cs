using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Adventure
{
    public class FlashingBeam : Beam
    {

        public int counter = 0;
        public int durationOn;
        public int durationOff;
        public int delay;
        public bool beforeDelay;
        public bool on;


        public FlashingBeam(Vector2 startPosition, int durationOn, int durationOff, Vector2 endPosition, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player, int delay = 0) : base(startPosition, endPosition, assetManager, colliderManager, screenManager, player)
        {
            this.durationOn = durationOn;
            this.durationOff = durationOff;
            this.delay = delay;
            beforeDelay = false;
            on = true;
        }





        public override void Update(GameTime gameTime)
        {

            if (beforeDelay)
            {
                if (counter > delay)
                {
                    beforeDelay = false;
                    counter = 0;
                }

            }
            else
            {
                if (!on)
                {
                    if (counter > durationOff)
                    {
                        on = true;
                        counter = 0;
                        idleHitbox.isActive = true;
                    }
                }
                else
                {
                    if (counter > durationOn)
                    {
                        on = false;
                        counter = 0;
                        idleHitbox.isActive = false;
                    }
                }
            }


            if (on)
            {
                base.Update(gameTime);
            }



            counter += 1;

        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            listOfBeamSquares[0].Draw(spriteBatch);
            listOfBeamSquares[listOfBeamSquares.Count - 1].Draw(spriteBatch);

            if (on)
            {
                for (int i = 1; i <= IndexOfBeamToUpdateTo; i++)
                {
                    listOfBeamSquares[i].Draw(spriteBatch);
                }
            }

        }








    }
}
