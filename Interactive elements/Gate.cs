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
    public class Gate : MovingPlatformLooping
    {
        public bool open = false;
        public bool opened = false;

        public bool moveHorizontally = false;
        public bool moveVertically = false;

        public Gate(Vector2 initialPosition, string filename, Vector2 endPosition, AssetManager assetManager, ColliderManager colliderManager, Player player) : base(initialPosition, filename, endPosition, 0, 1, assetManager, colliderManager, player)
        {

        }


        public override void Update(GameTime gameTime)
        {

            if (open && !opened)
            {
                base.Update(gameTime);

                if (horizontalMovement)
                {
                    if (position.X == endPosition.X)
                    {
                        opened = true;
                    }
                }
                else if (verticalMovement)
                {
                    if (position.Y == endPosition.Y)
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
                    if (position.X == startPosition.X)
                    {
                        opened = false;
                    }
                }
                else if (verticalMovement)
                {
                    if (position.Y == startPosition.Y)
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
