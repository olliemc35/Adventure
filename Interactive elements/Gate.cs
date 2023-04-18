﻿using Microsoft.Xna.Framework.Content;
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

        public Gate(Vector2 initialPosition, Vector2 endPosition, string filename, AssetManager assetManager, ColliderManager colliderManager, Player player) : base(new List<Vector2>() { initialPosition, endPosition }, new List<int>() { 0, 1 }, filename, 0, 1, 0, assetManager, colliderManager, player)
        {

        }


        public override void Update(GameTime gameTime)
        {

            if (open && !opened)
            {
                base.Update(gameTime);

                if (globalDirection == GlobalDirection.horizontal)
                {
                    if (position.X == positions[1].X)
                    {
                        opened = true;
                    }
                }
                else if (globalDirection == GlobalDirection.vertical)
                {
                    if (position.Y == positions[1].Y)
                    {
                        opened = true;
                    }
                }
            }

            if (!open && opened)
            {
                base.Update(gameTime);

                if (globalDirection == GlobalDirection.horizontal)
                {
                    if (position.X == positions[0].X)
                    {
                        opened = false;
                    }
                }
                else if (globalDirection == GlobalDirection.vertical)
                {
                    if (position.Y == positions[0].Y)
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
