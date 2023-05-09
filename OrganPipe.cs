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
    public class OrganPipe : GameObject
    {
        public List<AnimatedGameObject> pipeTiles_BetweenBaseAndPlatform = new List<AnimatedGameObject>();
        public List<AnimatedGameObject> pipeTiles_BetweenPlatformAndTop = new List<AnimatedGameObject>();
        public List<AnimatedGameObject> pipeTiles_Middle = new List<AnimatedGameObject>();


        public AnimatedGameObject pipeBottom = new AnimatedGameObject();
        public MovingPlatform platform;

        // The idleHitbox corresponds to the platform (the top). We also need a hitbox for the tube.
        public HitboxRectangle pipeHitbox1 = new HitboxRectangle();
        public HitboxRectangle pipeHitbox2 = new HitboxRectangle();
        public HitboxRectangle pipeHitbox3 = new HitboxRectangle();

        public int distanceFromBase;

        public enum ExtendingDirection
        {
            topToBottom,
            bottomToTop,
            leftToRight,
            rightToLeft,
        };
        public ExtendingDirection direction;


        public OrganPipe(Vector2 startPoint, Vector2 endPoint, float speed, List<int> stationaryTimes, AssetManager assetManager, ColliderManager colliderManager, Player player, int distanceFromBase, int behaviour)
        {

            this.assetManager = assetManager;
            this.colliderManager = colliderManager;
            this.player = player;
            this.distanceFromBase = distanceFromBase;
            LoadFirst = true;

            if (behaviour == 0)
            {
                platform = new MovingPlatform_AB(startPoint, endPoint, "OrganPipe_Platform", speed, stationaryTimes, assetManager, colliderManager, player);

            }
            else if (behaviour == 1)
            {
                platform = new MovingPlatform_ABA(startPoint, endPoint, "OrganPipe_Platform", speed, stationaryTimes, assetManager, colliderManager, player);

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

            }



        }

        public override void LoadContent()
        {
            base.LoadContent();
            platform.LoadContent();


            if (direction == ExtendingDirection.topToBottom || direction == ExtendingDirection.bottomToTop)
            {
                if (direction == ExtendingDirection.topToBottom)
                {
                    CreateTopToBottom();
                }
                else
                {
                    CreateBottomToTop();
                }
                //int Height = 0;

                //if (direction == ExtendingDirection.bottomToTop)
                //{
                //    if (platform.positions[1].Y < (int)platform.positions[0].Y)
                //    {
                //        Height = ((int)platform.positions[0].Y - (int)platform.positions[1].Y) / 8 + distanceFromBase / 8;
                //    }
                //    else
                //    {
                //        Height = distanceFromBase / 8;

                //    }
                //}
                //else
                //{
                //    if (platform.positions[1].Y > (int)platform.positions[0].Y)
                //    {
                //        Height = ((int)platform.positions[1].Y - (int)platform.positions[0].Y) / 8 - distanceFromBase / 8 + 1;
                //    }
                //    else
                //    {
                //        Height = -distanceFromBase / 8;

                //    }

                //}

                //int sign = Math.Sign(distanceFromBase);

                //for (int i = 1; i <= Height; i++)
                //{
                //    AnimatedGameObject animatedGameObject;

                //    if (direction == ExtendingDirection.bottomToTop)
                //    {
                //        animatedGameObject = new AnimatedGameObject(new Vector2(platform.positions[0].X + 8, platform.positions[0].Y + sign * (8 + 8 * i)), "OrganStopMiddle", assetManager);
                //    }
                //    else
                //    {
                //        animatedGameObject = new AnimatedGameObject(new Vector2(platform.positions[0].X + 8, platform.positions[0].Y + sign * (8 * i)), "OrganStopMiddle", assetManager);
                //    }

                //    animatedGameObject.LoadContent();
                //    pipeTiles_BetweenBaseAndPlatform.Add(animatedGameObject);
                //    platform.attachedGameObjects.Add(animatedGameObject);
                //}
                //pipeBottom = new AnimatedGameObject(new Vector2(platform.positions[0].X + 8, platform.positions[0].Y + distanceFromBase), "OrganStopBottom", assetManager);
                //pipeBottom.LoadContent();

                //if (direction == ExtendingDirection.bottomToTop)
                //{
                //    pipeHitbox1 = new HitboxRectangle((int)pipeTiles_BetweenBaseAndPlatform[0].position.X, (int)pipeTiles_BetweenBaseAndPlatform[0].position.Y, pipeTiles_BetweenBaseAndPlatform[0].idleHitbox.rectangle.Width, 8 * (Height - 1));
                //}
                //else
                //{
                //    pipeHitbox1 = new HitboxRectangle((int)pipeBottom.position.X, (int)pipeTiles_BetweenBaseAndPlatform.Last().position.Y + 8, pipeTiles_BetweenBaseAndPlatform[0].idleHitbox.rectangle.Width, 8 * (Height - 1));

                //}
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
                        Width = -distanceFromBase / 8;

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
                    pipeTiles_BetweenBaseAndPlatform.Add(animatedGameObject);
                    platform.attachedGameObjects.Add(animatedGameObject);
                }

                pipeBottom = new AnimatedGameObject(new Vector2(platform.positions[0].X + distanceFromBase, platform.positions[0].Y + 8), "OrganStopBottom_Vertical", assetManager);
                pipeBottom.LoadContent();

                if (direction == ExtendingDirection.rightToLeft)
                {
                    pipeHitbox1 = new HitboxRectangle((int)pipeTiles_BetweenBaseAndPlatform[0].position.X, (int)pipeTiles_BetweenBaseAndPlatform[0].position.Y, 8 * (Width - 1), pipeTiles_BetweenBaseAndPlatform[0].idleHitbox.rectangle.Height);
                }
                else
                {
                    pipeHitbox1 = new HitboxRectangle((int)pipeTiles_BetweenBaseAndPlatform.Last().position.X + 8, (int)pipeBottom.position.Y, 8 * (Width - 1), pipeTiles_BetweenBaseAndPlatform[0].idleHitbox.rectangle.Height);
                }
            }


            pipeHitbox1.texture = assetManager.hitboxTexture;
            pipeHitbox1.isActive = true;
            pipeHitbox2.texture = assetManager.hitboxTexture;
            pipeHitbox2.isActive = true;
            pipeHitbox3.texture = assetManager.hitboxTexture;
            pipeHitbox3.isActive = true;

        }
        public override void Update(GameTime gameTime)
        {
            platform.Update(gameTime);

            //if (platform.direction == MovingPlatform.Direction.moveRight)
            //{
            //    Debug.WriteLine(platform.drawPosition.X);
            //}

            foreach (AnimatedGameObject animatedGameObject in pipeTiles_BetweenBaseAndPlatform)
            {
                animatedGameObject.Update(gameTime);
            }
            foreach (AnimatedGameObject animatedGameObject in pipeTiles_BetweenPlatformAndTop)
            {
                animatedGameObject.Update(gameTime);
            }
            foreach (AnimatedGameObject animatedGameObject in pipeTiles_Middle)
            {
                animatedGameObject.Update(gameTime);
            }
            switch (direction)
            {
                case ExtendingDirection.bottomToTop:
                    {
                        pipeHitbox1.rectangle.X = (int)pipeTiles_BetweenBaseAndPlatform[0].position.X;
                        pipeHitbox1.rectangle.Y = (int)pipeTiles_BetweenBaseAndPlatform[0].position.Y;
                        pipeHitbox2.rectangle.X = (int)pipeTiles_BetweenPlatformAndTop.Last().position.X;
                        pipeHitbox2.rectangle.Y = (int)pipeTiles_BetweenPlatformAndTop.Last().position.Y;
                        pipeHitbox3.rectangle.X = (int)pipeTiles_Middle.Last().position.X;
                        pipeHitbox3.rectangle.Y = (int)pipeTiles_Middle.Last().position.Y;
                        break;
                    }

                case ExtendingDirection.topToBottom:
                    {
                        pipeHitbox1.rectangle.X = (int)pipeTiles_BetweenBaseAndPlatform.Last().position.X;
                        pipeHitbox1.rectangle.Y = (int)pipeTiles_BetweenBaseAndPlatform.Last().position.Y + 8;
                        pipeHitbox2.rectangle.X = (int)pipeTiles_BetweenPlatformAndTop.Last().position.X;
                        pipeHitbox2.rectangle.Y = (int)pipeTiles_BetweenPlatformAndTop.First().position.Y;
                        pipeHitbox3.rectangle.X = (int)pipeTiles_Middle.Last().position.X;
                        pipeHitbox3.rectangle.Y = (int)pipeTiles_Middle.Last().position.Y;
                        break;
                    }

                case ExtendingDirection.rightToLeft:
                    {
                        pipeHitbox1.rectangle.X = (int)pipeTiles_BetweenBaseAndPlatform[0].position.X;
                        pipeHitbox1.rectangle.Y = (int)pipeTiles_BetweenBaseAndPlatform[0].position.Y;
                        pipeHitbox2.rectangle.X = (int)pipeTiles_BetweenPlatformAndTop[0].position.X;
                        pipeHitbox2.rectangle.Y = (int)pipeTiles_BetweenPlatformAndTop[0].position.Y;
                        pipeHitbox3.rectangle.X = (int)pipeTiles_Middle[0].position.X;
                        pipeHitbox3.rectangle.Y = (int)pipeTiles_Middle[0].position.Y;
                        break;
                    }
                case ExtendingDirection.leftToRight:
                    {

                        pipeHitbox1.rectangle.X = (int)pipeTiles_BetweenBaseAndPlatform.Last().position.X + 8;
                        pipeHitbox1.rectangle.Y = (int)pipeTiles_BetweenBaseAndPlatform.Last().position.Y;
                        pipeHitbox2.rectangle.X = (int)pipeTiles_BetweenPlatformAndTop.Last().position.X + 8;
                        pipeHitbox2.rectangle.Y = (int)pipeTiles_BetweenPlatformAndTop.Last().position.Y;
                        pipeHitbox3.rectangle.X = (int)pipeTiles_Middle.Last().position.X + 8;
                        pipeHitbox3.rectangle.Y = (int)pipeTiles_Middle.Last().position.Y;
                        break;
                    }
            }


        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            platform.Draw(spriteBatch);

            foreach (AnimatedGameObject animatedGameObject in pipeTiles_BetweenBaseAndPlatform)
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

            pipeBottom.Draw(spriteBatch);

            foreach (AnimatedGameObject animatedGameObject in pipeTiles_BetweenPlatformAndTop)
            {
                animatedGameObject.Draw(spriteBatch);
            }
            foreach (AnimatedGameObject animatedGameObject in pipeTiles_Middle)
            {
                animatedGameObject.Draw(spriteBatch);
            }

            //spriteBatch.Draw(pipeHitbox1.texture, pipeHitbox1.rectangle, Color.Red);
            //spriteBatch.Draw(pipeHitbox2.texture, pipeHitbox2.rectangle, Color.Red);
            //spriteBatch.Draw(pipeHitbox3.texture, pipeHitbox3.rectangle, Color.Red);

        }


        public override void AddAttachedGameObject(GameObject gameObject)
        {
            platform.AddAttachedGameObject(gameObject);
        }

        public override void HandleNoteTrigger()
        {
            platform.HandleNoteTrigger();
        }


        public void CreateTopToBottom()
        {

            int HeightFromBase = 0;


            if (platform.positions[1].Y > (int)platform.positions[0].Y)
            {
                HeightFromBase = ((int)platform.positions[1].Y - (int)platform.positions[0].Y) / 8 - distanceFromBase / 8 + 1;
            }
            else
            {
                HeightFromBase = -distanceFromBase / 8;

            }

            AnimatedGameObject animatedGameObject2 = new AnimatedGameObject(new Vector2(platform.positions[0].X + 2 * 8, platform.positions[0].Y), "OrganPipe_Tile", assetManager);
            animatedGameObject2.LoadContent();
            pipeTiles_Middle.Add(animatedGameObject2);
            platform.attachedGameObjects.Add(animatedGameObject2);

            int sign = Math.Sign(distanceFromBase);

            for (int i = 1; i <= HeightFromBase; i++)
            {
                if (i == 1 || i == 2)
                {
                    AnimatedGameObject animatedGameObject = new AnimatedGameObject(new Vector2(platform.positions[0].X + 2 * 8, platform.positions[0].Y + sign * (8 * i)), "OrganPipe_Tile", assetManager);
                    animatedGameObject.LoadContent();
                    pipeTiles_Middle.Add(animatedGameObject);
                    platform.attachedGameObjects.Add(animatedGameObject);
                }
                else
                {
                    AnimatedGameObject animatedGameObject = new AnimatedGameObject(new Vector2(platform.positions[0].X, platform.positions[0].Y + sign * (8 * i)), "OrganPipe_TileRow", assetManager);
                    animatedGameObject.LoadContent();
                    pipeTiles_BetweenBaseAndPlatform.Add(animatedGameObject);
                    platform.attachedGameObjects.Add(animatedGameObject);
                }

            }


            for (int i = 1; i <= 5; i++)
            {

                AnimatedGameObject animatedGameObject = new AnimatedGameObject(new Vector2(platform.positions[0].X, platform.positions[0].Y - sign * (8 * i)), "OrganPipe_TileRow", assetManager);
                animatedGameObject.LoadContent();
                pipeTiles_BetweenPlatformAndTop.Add(animatedGameObject);
                platform.attachedGameObjects.Add(animatedGameObject);


            }

            pipeHitbox1 = new HitboxRectangle((int)pipeBottom.position.X, (int)pipeTiles_BetweenBaseAndPlatform.Last().position.Y + 8, pipeTiles_BetweenBaseAndPlatform[0].idleHitbox.rectangle.Width, 8 * (HeightFromBase - 2 - 1));
            pipeHitbox2 = new HitboxRectangle((int)pipeBottom.position.X, (int)pipeTiles_BetweenPlatformAndTop.Last().position.Y + 8, pipeTiles_BetweenPlatformAndTop[0].idleHitbox.rectangle.Width, 8 * (5));
            pipeHitbox3 = new HitboxRectangle((int)pipeTiles_Middle.Last().position.X, (int)pipeTiles_Middle.Last().position.Y, pipeTiles_Middle[0].idleHitbox.rectangle.Width, 8 * 3);


            pipeBottom = new AnimatedGameObject(new Vector2(platform.positions[0].X, platform.positions[0].Y + distanceFromBase), "OrganPipe_Base", assetManager);
            pipeBottom.LoadContent();


        }


        public void CreateBottomToTop()
        {
            int HeightFromBase = 0;

            if (platform.positions[1].Y < (int)platform.positions[0].Y)
            {
                HeightFromBase = ((int)platform.positions[0].Y - (int)platform.positions[1].Y) / 8 + distanceFromBase / 8;
            }
            else
            {
                HeightFromBase = distanceFromBase / 8;

            }

            AnimatedGameObject animatedGameObject2 = new AnimatedGameObject(new Vector2(platform.positions[0].X - 8, platform.positions[0].Y), "OrganPipe_Tile", assetManager);
            animatedGameObject2.LoadContent();
            pipeTiles_Middle.Add(animatedGameObject2);
            platform.attachedGameObjects.Add(animatedGameObject2);

            int sign = Math.Sign(distanceFromBase);

            for (int i = 1; i <= HeightFromBase; i++)
            {
               
                
                    AnimatedGameObject animatedGameObject = new AnimatedGameObject(new Vector2(platform.positions[0].X - 8, platform.positions[0].Y + sign * (8 * i)), "OrganPipe_TileRowLeft", assetManager);
                    animatedGameObject.LoadContent();
                    pipeTiles_BetweenBaseAndPlatform.Add(animatedGameObject);
                    platform.attachedGameObjects.Add(animatedGameObject);
                

            }


            for (int i = 1; i <= 5; i++)
            {
                if (i == 1 || i == 2)
                {
                    AnimatedGameObject animatedGameObject = new AnimatedGameObject(new Vector2(platform.positions[0].X - 8, platform.positions[0].Y - sign * (8 * i)), "OrganPipe_Tile", assetManager);
                    animatedGameObject.LoadContent();
                    pipeTiles_Middle.Add(animatedGameObject);
                    platform.attachedGameObjects.Add(animatedGameObject);
                }
                else
                {
                    AnimatedGameObject animatedGameObject = new AnimatedGameObject(new Vector2(platform.positions[0].X - 8, platform.positions[0].Y - sign * (8 * i)), "OrganPipe_TileRowLeft", assetManager);
                    animatedGameObject.LoadContent();
                    pipeTiles_BetweenPlatformAndTop.Add(animatedGameObject);
                    platform.attachedGameObjects.Add(animatedGameObject);
                }


            }


            pipeHitbox1 = new HitboxRectangle((int)pipeBottom.position.X, (int)pipeTiles_BetweenBaseAndPlatform.Last().position.Y + 8, pipeTiles_BetweenBaseAndPlatform[0].idleHitbox.rectangle.Width, 8 * (HeightFromBase));
            pipeHitbox2 = new HitboxRectangle((int)pipeBottom.position.X, (int)pipeTiles_BetweenPlatformAndTop.Last().position.Y + 8, pipeTiles_BetweenPlatformAndTop[0].idleHitbox.rectangle.Width, 8 * (3));
            pipeHitbox3 = new HitboxRectangle((int)pipeTiles_Middle.Last().position.X, (int)pipeTiles_Middle.Last().position.Y, pipeTiles_Middle[0].idleHitbox.rectangle.Width, 8 * 3);


            pipeBottom = new AnimatedGameObject(new Vector2(platform.positions[0].X - 8, platform.positions[0].Y + distanceFromBase), "OrganPipe_Base", assetManager);
            pipeBottom.LoadContent();
        }
    }
}
