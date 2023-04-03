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
    public class Gate : MovingPlatform
    {
        public bool open = false;
        public bool opened = false;

        public bool moveHorizontally = false;
        public bool moveVertically = false;

        public Gate(Vector2 initialPosition, string filename, Vector2 endPosition) : base(initialPosition, filename, endPosition, 0, 1)
        {

        }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            base.LoadContent(contentManager, graphicsDevice);


            animationPosition.X = DistanceToNearestInteger(spritePosition.X);
            animationPosition.Y = DistanceToNearestInteger(spritePosition.Y);
            animatedSprite_Idle.Position = animationPosition;

        }

        public override void Update(GameTime gameTime)
        {

            if (open && !opened)
            {
                base.Update(gameTime);

                if (horizontalMovement)
                {
                    if (spritePosition.X == endPosition.X)
                    {
                        opened = true;
                    }
                }
                else if (verticalMovement)
                {
                    if (spritePosition.Y == endPosition.Y)
                    {
                        opened = true;
                    }
                }
            }

            if (!open && opened)
            {
                base.Update(gameTime);

                if (horizontalMovement)
                {
                    if (spritePosition.X == startPosition.X)
                    {
                        opened = false;
                    }
                }
                else if (verticalMovement)
                {
                    if (spritePosition.Y == startPosition.Y)
                    {
                        opened = false;
                    }
                }
            }




        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }


    }
}
