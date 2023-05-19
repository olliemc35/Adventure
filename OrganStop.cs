using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite;
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Aseprite.Content.Processors;
using System.Diagnostics;
using System.Net;

namespace Adventure
{
    public class OrganStop : GameObject
    {
        public List<AnimatedGameObject> organTiles = new List<AnimatedGameObject>();
        public AnimatedGameObject organBottom = new AnimatedGameObject();
        public MovingPlatform platform;

        // The idleHitbox corresponds to the platform (the top). We also need a hitbox for the tube.
        public HitboxRectangle tubeHitbox = new HitboxRectangle();
        public int distanceFromBase;

        public enum ExtendingDirection
        {
            topToBottom,
            bottomToTop,
            leftToRight,
            rightToLeft,
        };
        public ExtendingDirection direction;


        public OrganStop(Vector2 startPoint, Vector2 endPoint, float speed, List<int> stationaryTimes, AssetManager assetManager, ColliderManager colliderManager, Player player, int distanceFromBase, int behaviour) 
        {

            this.assetManager = assetManager;
            this.colliderManager = colliderManager;
            this.player = player;
            this.distanceFromBase = distanceFromBase;
            LoadFirst = true;

            if (behaviour == 0)
            {
                platform = new MovingPlatform_AB(startPoint, endPoint, "OrganStop", speed, stationaryTimes, assetManager, colliderManager, player);

            }
            else if (behaviour == 1)
            {
                platform = new MovingPlatform_ABA(startPoint, endPoint, "OrganStop", speed, stationaryTimes, assetManager, colliderManager, player);

            }


            if (startPoint.X == endPoint.X)
            {

                if (distanceFromBase > 0)
                {
                    direction = ExtendingDirection.bottomToTop;
                }
                else
                {
                    direction = ExtendingDirection.topToBottom;
                }

                platform.filename = "OrganStopTop";
            }
            else
            {

                if (distanceFromBase > 0)
                {
                    direction = ExtendingDirection.rightToLeft;
                }
                else
                {
                    direction = ExtendingDirection.leftToRight;
                }

                platform.Highlight = true;
                platform.filename = "OrganStopTop_Vertical";

            }
            

        }

        public override void LoadContent()
        {
            base.LoadContent();
            platform.LoadContent();


            if (direction == ExtendingDirection.topToBottom || direction == ExtendingDirection.bottomToTop)
            {
                int Height = 0;

                if (direction == ExtendingDirection.bottomToTop)
                {
                    if (platform.positions[1].Y < (int)platform.positions[0].Y)
                    {
                        Height = ((int)platform.positions[0].Y - (int)platform.positions[1].Y) / 8 + distanceFromBase / 8;
                    }
                    else
                    {
                        Height = distanceFromBase / 8;

                    }
                }
                else
                {
                    if (platform.positions[1].Y > (int)platform.positions[0].Y)
                    {
                        Height = ((int)platform.positions[1].Y - (int)platform.positions[0].Y) / 8 - distanceFromBase / 8 + 1;
                    }
                    else
                    {
                        Height = - distanceFromBase / 8;

                    }

                }

                int sign = Math.Sign(distanceFromBase);

                for (int i = 1; i <= Height; i++)
                {
                    AnimatedGameObject animatedGameObject;

                    if (direction == ExtendingDirection.bottomToTop)
                    {
                        animatedGameObject = new AnimatedGameObject(new Vector2(platform.positions[0].X + 8, platform.positions[0].Y + sign * (8 + 8 * i)), "OrganStopMiddle", assetManager);
                    }
                    else
                    {
                        animatedGameObject = new AnimatedGameObject(new Vector2(platform.positions[0].X + 8, platform.positions[0].Y + sign * (8 * i)), "OrganStopMiddle", assetManager);
                    }

                    animatedGameObject.LoadContent();
                    organTiles.Add(animatedGameObject);
                    platform.attachedGameObjects.Add(animatedGameObject);
                }
                organBottom = new AnimatedGameObject(new Vector2(platform.positions[0].X + 8, platform.positions[0].Y + distanceFromBase), "OrganStopBottom", assetManager);
                organBottom.LoadContent();

                if (direction == ExtendingDirection.bottomToTop)
                {
                    tubeHitbox = new HitboxRectangle((int)organTiles[0].position.X, (int)organTiles[0].position.Y, organTiles[0].idleHitbox.rectangle.Width, 8 * (Height - 1));
                }
                else
                {
                    tubeHitbox = new HitboxRectangle((int)organBottom.position.X, (int)organTiles.Last().position.Y + 8, organTiles[0].idleHitbox.rectangle.Width, 8 * (Height - 1));

                }
            }
            else
            {

                int Width = 0;

                if (direction == ExtendingDirection.leftToRight)
                {
                    if (platform.positions[1].X > platform.positions[0].X)
                    {
                        Width = ((int)platform.positions[1].X - (int)platform.positions[0].X) / 8 - distanceFromBase / 8 + 1;
                    }
                    else
                    {
                        Width = - distanceFromBase / 8;

                    }
                }
                else
                {
                    if (platform.positions[1].X < platform.positions[0].X)
                    {
                        Width = ((int)platform.positions[0].X - (int)platform.positions[1].X) / 8 + distanceFromBase / 8;
                    }
                    else
                    {
                        Width = distanceFromBase / 8;

                    }
                }

                int sign = Math.Sign(distanceFromBase);

                for (int i = 1; i <= Width; i++)
                {
                    AnimatedGameObject animatedGameObject;

                    if (direction == ExtendingDirection.rightToLeft)
                    {
                        animatedGameObject = new AnimatedGameObject(new Vector2(platform.positions[0].X + sign * (8 + 8 * i), platform.positions[0].Y + 8), "OrganStopMiddle_Vertical", assetManager);
                    }
                    else
                    {
                        animatedGameObject = new AnimatedGameObject(new Vector2(platform.positions[0].X + sign * (8 * i), platform.positions[0].Y + 8), "OrganStopMiddle_Vertical", assetManager);
                    }
                    animatedGameObject.LoadContent();
                    organTiles.Add(animatedGameObject);
                    platform.attachedGameObjects.Add(animatedGameObject);
                }

                organBottom = new AnimatedGameObject(new Vector2(platform.positions[0].X + distanceFromBase, platform.positions[0].Y + 8), "OrganStopBottom_Vertical", assetManager);
                organBottom.LoadContent();

                if (direction == ExtendingDirection.rightToLeft)
                {
                    tubeHitbox = new HitboxRectangle((int)organTiles[0].position.X, (int)organTiles[0].position.Y, 8 * (Width - 1), organTiles[0].idleHitbox.rectangle.Height);
                }
                else
                {
                    tubeHitbox = new HitboxRectangle((int)organTiles.Last().position.X + 8, (int)organBottom.position.Y, 8 * (Width - 1), organTiles[0].idleHitbox.rectangle.Height);
                }
            }



            tubeHitbox.texture = assetManager.hitboxTexture;
            tubeHitbox.isActive = true;

        }
        public override void Update(GameTime gameTime)
        {
            platform.Update(gameTime);

            //if (platform.direction == MovingPlatform.Direction.moveRight)
            //{
            //    Debug.WriteLine(platform.drawPosition.X);
            //}

            foreach (AnimatedGameObject animatedGameObject in organTiles)
            {
                animatedGameObject.Update(gameTime);
            }
            switch (direction)
            {
                case ExtendingDirection.bottomToTop:
                    {
                        tubeHitbox.rectangle.X = (int)organTiles[0].position.X;
                        tubeHitbox.rectangle.Y = (int)organTiles[0].position.Y;
                        break;
                    }

                case ExtendingDirection.topToBottom:
                    {
                        tubeHitbox.rectangle.X = (int)organTiles.Last().position.X;
                        tubeHitbox.rectangle.Y = (int)organTiles.Last().position.Y + 8;
                        break;
                    }

                case ExtendingDirection.rightToLeft:
                    {
                        tubeHitbox.rectangle.X = (int)organTiles[0].position.X;
                        tubeHitbox.rectangle.Y = (int)organTiles[0].position.Y;
                        break;
                    }
                case ExtendingDirection.leftToRight:
                    {
                        
                        tubeHitbox.rectangle.X = (int)organTiles.Last().position.X + 8;
                        tubeHitbox.rectangle.Y = (int)organTiles.Last().position.Y;
                        break;
                    }
            }
            

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            platform.Draw(spriteBatch);

            foreach (AnimatedGameObject animatedGameObject in organTiles)
            {
                switch (direction)
                {
                    case ExtendingDirection.bottomToTop:
                        {
                            if (animatedGameObject.position.Y <= platform.positions[0].Y + distanceFromBase)
                            {
                                animatedGameObject.Draw(spriteBatch);
                            }
                            break;
                        }

                    case ExtendingDirection.topToBottom:
                        {
                            if (animatedGameObject.position.Y >= platform.positions[0].Y + distanceFromBase)
                            {
                                animatedGameObject.Draw(spriteBatch);
                            }
                            break;
                        }

                    case ExtendingDirection.rightToLeft:
                        {
                            if (animatedGameObject.position.X <= platform.positions[0].X + distanceFromBase)
                            {
                                animatedGameObject.Draw(spriteBatch);
                            }
                            break;
                        }
                    case ExtendingDirection.leftToRight:
                        {
                            if (animatedGameObject.position.X >= platform.positions[0].X + distanceFromBase)
                            {
                                animatedGameObject.Draw(spriteBatch);
                            }
                            break;
                        }
                }

            }

            organBottom.Draw(spriteBatch);

            //spriteBatch.Draw(tubeHitbox.texture, tubeHitbox.rectangle, Color.Blue);

        }


        public override void AddAttachedGameObject(GameObject gameObject)
        {
            platform.AddAttachedGameObject(gameObject);
        }

        public override void HandleNoteTrigger()
        {
            platform.HandleNoteTrigger();
        }
    }
}
