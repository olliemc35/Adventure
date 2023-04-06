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
    public class MovingGameObject : AnimatedGameObject
    {
        public Vector2 velocity = new Vector2();
        public Vector2 previousVelocity;
        public Vector2 displacement; // This is the displacement between NOW and the NEXT FRAME.
        public Vector2 previousDisplacement;        

        public float maxHorizontalSpeed;
        public float maxVerticalSpeed;



        // These bools will keep a global record of whether I am currently collided with ANY sprite (e.g. the floor)
        public bool CollidedOnTop = false;
        public bool CollidedOnBottom = false;
        public bool CollidedOnRight = false;
        public bool CollidedOnLeft = false;

        // These bools will FLAG when a GlobalCollider bool changes from FALSE to TRUE
        public bool flagCollidedOnTop = false;
        public bool flagCollidedOnBottom = false;
        public bool flagCollidedOnRight = false;
        public bool flagCollidedOnLeft = false;

        public bool ResetColliderBools = true;


        public ColliderManager colliderManager = new ColliderManager();
        public float deltaTime;


        public float gravityConstant;
        public float mass;

        public Color[] colorMapOfSpriteSheet;
        public List<List<HitboxRectangle>> hitboxesForGunlineForEachFrame = new List<List<HitboxRectangle>>();

        public MovingGameObject() : base()
        {
        }

        public MovingGameObject(Vector2 initialPosition) : base(initialPosition)
        {
        }

        public MovingGameObject(Vector2 initialPosition, string filename) : base(initialPosition, filename)
        {
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }


       



        public void EnableGunlineHitboxes()
        {
            foreach (List<HitboxRectangle> hitboxes in hitboxesForGunlineForEachFrame)
            {
                foreach (HitboxRectangle hitbox in hitboxes)
                {
                    hitbox.isActive = false;
                }
            }


            //foreach (HitboxRectangle hitbox in hitboxesForGunlineForEachFrame[animatedSprite_Idle.CurrentFrameIndex])
            //{
            //    hitbox.isActive = true;
            //}

        }

        public void UpdateGunlineHitboxes()
        {

            //foreach (HitboxRectangle hitbox in hitboxesForGunlineForEachFrame[previousFrameNumber])
            //{
            //    hitbox.isActive = false;
            //}

            //for (int k = frameAndTag[tagOfCurrentFrame].From; k <= frameAndTag[tagOfCurrentFrame].To; k++)
            //{
            //    foreach (HitboxRectangle hitbox in hitboxesForGunlineForEachFrame[k])
            //    {
            //        hitbox.rectangle.X = (int)spritePosition.X + hitbox.offsetX;
            //        hitbox.rectangle.Y = (int)spritePosition.Y + hitbox.offsetY;
            //    }
            //}

            //foreach (HitboxRectangle hitbox in hitboxesForGunlineForEachFrame[animatedSprite_Idle.CurrentFrameIndex])
            //{
            //    hitbox.isActive = true;
            //}

        }

        public virtual void HandleGunshot(Vector2 collisionPoint)
        {
        }


        // The following code takes as input a Texture2D object 
        // and returns a 2-dim array of pixel colours

        public Color[,] TextureTo2DArrayOfColors(Texture2D texture)
        {
            // First we create 1-dim array which can be obtained via the standard GetData method of Texture2D
            Color[] colorsOne = new Color[texture.Width * texture.Height];
            texture.GetData(colorsOne);

            // Now we convert this into an easier 2-dim array
            Color[,] colorsTwo = new Color[texture.Width, texture.Height]; //The new, easy to read 2D array

            for (int i = 0; i < texture.Width; i++)
            {
                for (int j = 0; j < texture.Height; j++)
                {
                    colorsTwo[i, j] = colorsOne[i + j * texture.Width];
                }
            }

            return colorsTwo;
        }



        //public Color[,] TextureTo2DArrayOfColors(Texture2D texture, int frameNumber)
        //{
        //    // Get bounds of the frame on the sprite map
        //    int x = animatedSprite_Idle.Frames[frameNumber].Bounds.X;
        //    int y = animatedSprite_Idle.Frames[frameNumber].Bounds.Y;

        //    // First we create 1-dim array which can be obtained via the standard GetData method of Texture2D
        //    Color[] colorsOne = new Color[texture.Width * texture.Height];
        //    texture.GetData(colorsOne);

        //    // Now we convert this into an easier 2-dim array
        //    Color[,] colorsTwo = new Color[baseFrame.Width, baseFrame.Height];

        //    for (int i = 0; i < baseFrame.Width; i++)
        //    {
        //        for (int j = 0; j < baseFrame.Height; j++)
        //        {
        //            // colorsTwo[i, j] = colorsOne[i + j * texture.Width];
        //            colorsTwo[i, j] = colorsOne[x + i + (y + j) * texture.Width];
        //        }
        //    }

        //    return colorsTwo;
        //}




        public bool CheckIfThereIsColor(Color color)
        {
            if (color.R == 0 && color.B == 0 && color.A == 0 && color.G == 0)
            {
                return false;
            }

            return true;

        }


        public void SetColorToZero(Color color)
        {
            color.R = 0;
            color.B = 0;
            color.A = 0;
            color.G = 0;
        }


        public void BuildHitboxSetFromColourMap(Color[,] colorMap, List<HitboxRectangle> hitboxes)
        {
            bool GotHeight = false;

            for (int i = 0; i < colorMap.GetLength(0); i++)
            {
                for (int j = 0; j < colorMap.GetLength(1); j++)
                {

                    GotHeight = false;

                    if (CheckIfThereIsColor(colorMap[i, j]))
                    {
                        HitboxRectangle newHitbox = new HitboxRectangle(0, 0, 0, 0);
                        newHitbox.rectangle.X = i;
                        newHitbox.rectangle.Y = j;


                        // Get correct width
                        if (i < colorMap.GetLength(0) - 1)
                        {
                            for (int k = i + 1; k < colorMap.GetLength(0); k++)
                            {
                                if (!CheckIfThereIsColor(colorMap[k, j]))
                                {
                                    newHitbox.rectangle.Width = (k - i);
                                    break;
                                }

                                if (k == colorMap.GetLength(0) - 1 && CheckIfThereIsColor(colorMap[k, j]))
                                {
                                    newHitbox.rectangle.Width = (k - i + 1);
                                    break;
                                }

                            }
                        }
                        else
                        {
                            newHitbox.rectangle.Width = 1;
                        }


                        // Get correct height
                        if (j < colorMap.GetLength(1) - 1)
                        {
                            for (int h = j + 1; h < colorMap.GetLength(1); h++)
                            {
                                for (int l = i; l < i + newHitbox.rectangle.Width; l++)
                                {
                                    if (!CheckIfThereIsColor(colorMap[l, h]))
                                    {
                                        newHitbox.rectangle.Height = (h - j);
                                        GotHeight = true;
                                        break;
                                    }
                                }

                                if (GotHeight)
                                {
                                    break;
                                }

                                // I only reach this point if everything is fine
                                if (h == colorMap.GetLength(1) - 1)
                                {
                                    newHitbox.rectangle.Height = (h - j + 1);
                                }

                            }

                        }
                        else
                        {
                            newHitbox.rectangle.Height = 1;
                        }


                        for (int j2 = j; j2 < j + newHitbox.rectangle.Height; j2++)
                        {
                            for (int i2 = i; i2 < i + newHitbox.rectangle.Width; i2++)
                            {
                                SetColorToZero(colorMap[i2, j2]);
                            }
                        }

                        newHitbox.offsetX = newHitbox.rectangle.X;
                        newHitbox.offsetY = newHitbox.rectangle.Y;

                        newHitbox.rectangle.X = (int)position.X + newHitbox.offsetX;
                        newHitbox.rectangle.Y = (int)position.Y + newHitbox.offsetY;


                        hitboxes.Add(newHitbox);



                    }
                }


            }
        }


        //public void UpdateColorMapAtThisPoint(Vector2 collisionPoint)
        //{
        //    // want to operate on enemy texture 2d directly
        //    collisionPoint.X = (int)collisionPoint.X - spritePosition.X;
        //    collisionPoint.Y = (int)collisionPoint.Y - spritePosition.Y;
        //    int x = animatedSprite_Idle.Frames[animatedSprite_Idle.CurrentFrameIndex].Bounds.X;
        //    int y = animatedSprite_Idle.Frames[animatedSprite_Idle.CurrentFrameIndex].Bounds.Y;

        //    // E.g. to turn it red ...
        //    colorMapOfSpriteSheet[x + (int)collisionPoint.X + (y + (int)collisionPoint.Y) * animatedSprite_Idle.Texture.Width].R = 255;
        //    colorMapOfSpriteSheet[x + (int)collisionPoint.X + (y + (int)collisionPoint.Y) * animatedSprite_Idle.Texture.Width].G = 0;
        //    colorMapOfSpriteSheet[x + (int)collisionPoint.X + (y + (int)collisionPoint.Y) * animatedSprite_Idle.Texture.Width].B = 0;

        //    animatedSprite_Idle.Texture.SetData<Color>(colorMapOfSpriteSheet);
        //}



        //public void CreateHitboxesForGunLine()
        //{
        //    for (int i = 0; i < animatedSprite_Idle.Frames.Count; i++)
        //    {
        //        List<HitboxRectangle> hitboxes = new List<HitboxRectangle>();
        //        Color[,] colorMap = TextureTo2DArrayOfColors(animatedSprite_Idle.Texture, i);
        //        BuildHitboxSetFromColourMap(colorMap, hitboxes);
        //        hitboxesForGunlineForEachFrame.Add(hitboxes);

        //    }
        //}

    }
}
