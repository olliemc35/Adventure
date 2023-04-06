using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class GlobalTeleport : Teleport
    {
        Vector2 shootOffDirection = new Vector2();

        public GlobalTeleport(Vector2 initialPosition, string filename) : base(initialPosition, filename)
        {
            InRange = true;
        }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            base.LoadContent(contentManager, graphicsDevice);
            UpdatePlayingAnimation(animation_InRange);
        }


        public override void Update(GameTime gameTime)
        {
           

            //animatedSprite_Idle.Play("InRange");
            //currentFrame = frameAndTag["InRange"].From;
            //tagOfCurrentFrame = "InRange";

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }



    }
}
