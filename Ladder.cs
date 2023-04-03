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
    public class Ladder : Sprite
    {
        public int numberOfRungs;


        public Vector2 positionOfTopLeftCorner = new Vector2();
        public List<AnimationSprite> listOfRungs = new List<AnimationSprite>();

        public bool topRungActive = true;

        public Ladder() : base()
        {
        }

        public Ladder(Vector2 initialPosition) : base(initialPosition)
        {
        }

        public Ladder(Vector2 initialPosition, int numberOfRungs) : base(initialPosition)
        {
            CollisionSprite = true;

            this.numberOfRungs = numberOfRungs;

            for (int j = 0; j < numberOfRungs; j++)
            {
                Vector2 location = new Vector2
                {
                    X = initialPosition.X,
                    Y = initialPosition.Y + 8 * j
                };

                if (j == 0)
                {
                    listOfRungs.Add(new AnimationSprite(location, "LadderTopRung"));
                }
                else if (j == numberOfRungs - 1)
                {
                    listOfRungs.Add(new AnimationSprite(location, "LadderBottomRung"));
                }
                else
                {
                    listOfRungs.Add(new AnimationSprite(location, "Ladder"));
                }

            }

            positionOfTopLeftCorner.X = initialPosition.X;
            positionOfTopLeftCorner.Y = initialPosition.Y;

            spriteFilename = "Ladder";

        }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            foreach (Sprite rung in listOfRungs)
            {
                rung.LoadContent(contentManager, graphicsDevice);
            }

            idleHitbox = new HitboxRectangle((int)positionOfTopLeftCorner.X, (int)positionOfTopLeftCorner.Y, listOfRungs[0].baseFrame.Width, listOfRungs[0].baseFrame.Height * numberOfRungs);
            idleHitbox.isActive = true;

        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (Sprite rung in listOfRungs)
            {
                rung.Update(gameTime);
            }

            // This is allowing the player to stand on the top of a ladder, but move from underneath the ladder 
            if (References.player.idleHitbox.rectangle.Y + References.player.idleHitbox.rectangle.Height <= positionOfTopLeftCorner.Y && References.player.spriteDirectionY != 1)
            {
                idleHitbox.isActive = true;
            }
            else
            {
                idleHitbox.isActive = false;
            }





        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            //spriteBatch.Draw(References.player.spriteHitboxTexture, idleHitbox.rectangle, Color.Red);

            foreach (Sprite rung in listOfRungs)
            {
                rung.Draw(spriteBatch);
            }

        }



    }
}
