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
    public class Beam : AnimatedGameObject
    {

        public bool horizontalBeam = false;
        public bool verticalBeam = false;
        public Vector2 startPosition = new Vector2();
        public Vector2 endPosition = new Vector2();
        public List<AnimatedGameObject> listOfBeamSquares = new List<AnimatedGameObject>();
        public int IndexOfBeamToUpdateTo = 0;

        public HitboxRectangle startHitbox;
        public HitboxRectangle endHitbox;


        public Beam(Vector2 startPosition, Vector2 endPosition, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player) : base()
        {
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.colliderManager = colliderManager;
            this.assetManager = assetManager;
            this.screenManager = screenManager;
            this.player = player;
        }

        public override void LoadContent()
        {

            if (startPosition.X == endPosition.X)
            {
                verticalBeam = true;
                int numberOfBeams = (int)Math.Abs(endPosition.Y - startPosition.Y) / 8;
                int sign = Math.Sign(endPosition.Y - startPosition.Y);

                for (int i = 0; i <= numberOfBeams; i++)
                {
                    Vector2 location = startPosition;
                    location.Y += sign * 8 * i;

                    if (i == 0)
                    {
                        listOfBeamSquares.Add(new AnimatedGameObject(location, "TopBeam", assetManager));

                    }
                    else if (i == numberOfBeams)
                    {
                        listOfBeamSquares.Add(new AnimatedGameObject(location, "BottomBeam", assetManager));

                    }
                    else
                    {
                        listOfBeamSquares.Add(new AnimatedGameObject(location, "Beam", assetManager, colliderManager, null, null, player) { Hazard = true });
                    }
                }

            }
            else
            {
                horizontalBeam = true;
                int numberOfBeams = (int)Math.Abs(endPosition.X - startPosition.X) / 8;
                int sign = Math.Sign(endPosition.X - startPosition.X);

                for (int i = 0; i <= numberOfBeams; i++)
                {
                    Vector2 location = startPosition;
                    location.X += sign * 8 * i;

                    if (i == 0)
                    {
                        listOfBeamSquares.Add(new AnimatedGameObject(location, "TopBeam", assetManager));

                    }
                    else if (i == numberOfBeams)
                    {
                        listOfBeamSquares.Add(new AnimatedGameObject(location, "BottomBeam", assetManager));

                    }
                    else
                    {
                        listOfBeamSquares.Add(new AnimatedGameObject(location, "Beam", assetManager, colliderManager, null, null, player) { Hazard = true});
                    }
                }


            }

            foreach (AnimatedGameObject sprite in listOfBeamSquares)
            {
                sprite.LoadContent();
            }

            startHitbox = listOfBeamSquares[0].idleHitbox;
            endHitbox = listOfBeamSquares[listOfBeamSquares.Count - 1].idleHitbox;

            if (verticalBeam)
            {

                Vector2 startLocation;

                if (startPosition.Y < endPosition.Y)
                {
                    startLocation = startPosition;
                }
                else
                {
                    startLocation = endPosition;
                }

                startLocation.Y += listOfBeamSquares[0].idleHitbox.rectangle.Height;
                idleHitbox = new HitboxRectangle((int)startLocation.X, (int)startLocation.Y, listOfBeamSquares[0].idleHitbox.rectangle.Width, listOfBeamSquares[0].idleHitbox.rectangle.Height * (listOfBeamSquares.Count - 2));

            }
            else
            {
                Vector2 startLocation;

                if (startPosition.X < endPosition.X)
                {
                    startLocation = startPosition;
                }
                else
                {
                    startLocation = endPosition;
                }

                startLocation.X += listOfBeamSquares[0].idleHitbox.rectangle.Width;
                idleHitbox = new HitboxRectangle((int)startLocation.X, (int)startLocation.Y, listOfBeamSquares[0].idleHitbox.rectangle.Width * (listOfBeamSquares.Count - 2), listOfBeamSquares[0].idleHitbox.rectangle.Height);


            }

            idleHitbox.isActive = true;
            startHitbox.isActive = true;
            endHitbox.isActive = true;

        }



        public override void Update(GameTime gameTime)
        {
            listOfBeamSquares[0].Update(gameTime);
            listOfBeamSquares[listOfBeamSquares.Count - 1].Update(gameTime);

            IndexOfBeamToUpdateTo = listOfBeamSquares.Count - 2;

            for (int i = 1; i < listOfBeamSquares.Count - 1; i++)
            {
                bool breakEarly = false;

                foreach (HitboxRectangle hitbox in screenManager.activeScreen.hitboxesToCheckCollisionsWith)
                {
                    if (colliderManager.CheckForOverlap(hitbox, listOfBeamSquares[i].idleHitbox))
                    {
                        breakEarly = true;
                        break;
                    }
                }

                if (breakEarly)
                {
                    IndexOfBeamToUpdateTo = i - 1;
                    break;
                }
                else
                {
                    listOfBeamSquares[i].Update(gameTime);
                }

            }

            if (verticalBeam)
            {
                if (startPosition.Y > endPosition.Y)
                {
                    idleHitbox.rectangle.Y = listOfBeamSquares[IndexOfBeamToUpdateTo].idleHitbox.rectangle.Y;
                }

                idleHitbox.rectangle.Height = listOfBeamSquares[0].idleHitbox.rectangle.Height * IndexOfBeamToUpdateTo;
            }
            else if (horizontalBeam)
            {
                if (startPosition.X > endPosition.X)
                {
                    idleHitbox.rectangle.X = listOfBeamSquares[IndexOfBeamToUpdateTo].idleHitbox.rectangle.X;
                }

                idleHitbox.rectangle.Width = listOfBeamSquares[0].idleHitbox.rectangle.Width * IndexOfBeamToUpdateTo;
            }





        }


        public override void Draw(SpriteBatch spriteBatch)
        {

            listOfBeamSquares[0].Draw(spriteBatch);
            listOfBeamSquares[listOfBeamSquares.Count - 1].Draw(spriteBatch);

            for (int i = 1; i <= IndexOfBeamToUpdateTo; i++)
            {
                listOfBeamSquares[i].Draw(spriteBatch);
                //spriteBatch.Draw(References.player.spriteHitboxTexture, listOfBeamSquares[i].idleHitbox.rectangle, Color.Red);
            }

            // spriteBatch.Draw(References.player.spriteHitboxTexture, idleHitbox.rectangle, Color.Red);


        }



    }
}
