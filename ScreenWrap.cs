using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.IO.Enumeration;
using System.Text.Encodings.Web;

namespace Adventure
{
    public class ScreenWrap : GameObject
    {
        int width;
        int height;

        public List<AnimatedGameObject> sprites = new List<AnimatedGameObject>();
        public Vector2 position;

        public enum Location
        {
            left,
            right,
            top,
            bottom
        };

        public Location location;

        public ScreenWrap(Vector2 initialPosition, string filename, string location, int width, int height, AssetManager assetManager, ScreenManager screenManager, Player player)
        {
            //Debug.WriteLine(initialPosition.X);

            this.width = width;
            this.height = height;
            this.assetManager = assetManager;
            this.screenManager = screenManager;
            this.player = player;
            position = initialPosition;

            if (location == "left")
            {
                this.location = Location.left;
            }
            else if (location == "right")
            {
                this.location = Location.right;
            }
            else if (location == "top")
            {
                this.location = Location.top;
            }
            else if (location == "bottom")
            {
                this.location = Location.bottom;
            }

            for (int i = 0; i < width / tileSize; i++)
            {
                for (int j = 0; j < height / tileSize; j++)
                {
                    Vector2 position = new Vector2(initialPosition.X + i * tileSize, initialPosition.Y + j * tileSize);
                    sprites.Add(new AnimatedGameObject(position, filename, assetManager));
                }
            }

        }


        public override void LoadContent()
        {
            foreach (AnimatedGameObject sprite in sprites)
            {
                sprite.LoadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (AnimatedGameObject sprite in sprites)
            {
                sprite.Update(gameTime);
            }

            //Debug.WriteLine(position.X);

            switch (location)
            {
                case Location.left:
                    {
                        if (player.idleHitbox.rectangle.Y >= position.Y && player.idleHitbox.rectangle.Y <= position.Y + height)
                        {
                            if (player.idleHitbox.rectangle.X <= position.X - width && player.velocity.X < 0)
                            {
                                player.position.X = screenManager.activeScreen.actualScreenWidth - width;
                            }
                        }
                        break;
                    }
                case Location.right:
                    {
                        if (player.idleHitbox.rectangle.Y >= position.Y && player.idleHitbox.rectangle.Y <= position.Y + height)
                        {
                            if (player.idleHitbox.rectangle.X >= position.X + width && player.velocity.X > 0)
                            {
                                player.position.X = -width;
                            }
                        }
                        break;
                    }
                case Location.top:
                    {
                        if (player.idleHitbox.rectangle.X >= position.X && player.idleHitbox.rectangle.X <= position.X + width)
                        {
                            if (player.idleHitbox.rectangle.Y <= position.Y - height && player.velocity.Y < 0)
                            {
                                player.position.Y = screenManager.activeScreen.actualScreenHeight - 2 * tileSize;
                            }
                        }
                        break;
                    }
                case Location.bottom:
                    {
                        if (player.idleHitbox.rectangle.X >= position.X && player.idleHitbox.rectangle.X <= position.X + width)
                        {
                            if (player.idleHitbox.rectangle.Y >= position.Y + height && player.velocity.Y > 0)
                            {
                                player.position.Y = -2 * tileSize;
                            }
                        }
                        break;
                    }

            }


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (AnimatedGameObject sprite in sprites)
            {
                sprite.Draw(spriteBatch);
            }
        }


    }
}
