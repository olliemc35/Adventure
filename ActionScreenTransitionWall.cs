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


namespace Adventure
{
    public class ActionScreenTransitionWall : AnimatedGameObject
    {
        
        public enum Direction
        {
            left,
            right,
            top,
            bottom
        };

        public Direction direction;


        public int wallNumber;
        public int screenNumberToMoveTo;
        public int wallNumberToMoveTo;
        public int width;
        public int height;


        public Vector2 spawnPoint = new Vector2();

        public ActionScreenTransitionWall(Vector2 initialPosition, string direction, int width, int height, int wallNumber, int screenNumberToMoveTo, int wallNumberToMoveTo, ColliderManager colliderManager, InputManager inputManager, ScreenManager screenManager, Player player) : base()
        {
            this.position = initialPosition;
            this.wallNumber = wallNumber;
            this.screenNumberToMoveTo = screenNumberToMoveTo;
            this.wallNumberToMoveTo = wallNumberToMoveTo;
            this.width = width;
            this.height = height;
            //CollisionObject = true;
            this.colliderManager = colliderManager;
            this.inputManager = inputManager;
            this.screenManager = screenManager;
            this.player = player;

            // This is a cheap way of ensuring that the doors are added to the list of screen GameObjects in the right order
            // But only works properly if there are two doors per screen. 
            // So with more complex levels would have to think of a different way to do this
            if (wallNumber == 2)
            {
                LoadLast = true;
            }

            if (direction == "left")
            {
                this.direction = Direction.left;
            }
            else if (direction == "right")
            {
                this.direction= Direction.right;
            }
            else if (direction == "top")
            {
                this.direction= Direction.top;
            }
            else if (direction == "bottom")
            {
                this.direction= Direction.bottom;
            }


        }

        public override void LoadContent()
        {
            switch (direction)
            {
                case Direction.left:
                    {
                        idleHitbox = new HitboxRectangle((int)position.X -  tileSize, (int)position.Y, width, height);
                        spawnPoint.X = (int)position.X;
                        spawnPoint.Y = (int)position.Y + height - 2 * tileSize;
                        break;
                    }
                case Direction.right:
                    {
                        idleHitbox = new HitboxRectangle((int)position.X + tileSize, (int)position.Y, width, height);
                        spawnPoint.X = (int)position.X - tileSize;
                        spawnPoint.Y = (int)position.Y + height - 2 * tileSize;
                        break;
                    }
                case Direction.top:
                    {
                        idleHitbox = new HitboxRectangle((int)position.X, (int)position.Y - tileSize, width, height);
                        spawnPoint.X = (int)position.X;
                        spawnPoint.Y = (int)position.Y + 2 * tileSize;
                        break;
                    }
                case Direction.bottom:
                    {
                        idleHitbox = new HitboxRectangle((int)position.X, (int)position.Y + tileSize, width, height);
                        spawnPoint.X = (int)position.X;
                        spawnPoint.Y = (int)position.Y - 2 * tileSize;
                        break;
                    }

            }

            //Debug.WriteLine(spawnPoint.Y);
            


        }

        public override void Update(GameTime gameTime)
        {
            if (colliderManager.CheckForOverlap(player.idleHitbox, idleHitbox))
            {
                screenManager.activeScreen.ChangeScreenFlag_Wall = true;
                screenManager.PreviousScreenNumber = screenManager.activeScreen.screenNumber;
                screenManager.ScreenNumber = screenNumberToMoveTo;
                screenManager.WallNumberToMoveTo = wallNumberToMoveTo;
            }



        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // do nothing...
        }


    }
}
