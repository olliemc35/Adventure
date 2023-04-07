using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Framework.Utilities.Deflate;

namespace Adventure
{
    public class Ladder : AnimatedGameObject
    {
        public int numberOfRungs;


        public Vector2 positionOfTopLeftCorner = new Vector2();
        public List<AnimatedGameObject> listOfRungs = new List<AnimatedGameObject>();

        public bool topRungActive = true;

        public Ladder() : base()
        {
        }

        public Ladder(Vector2 initialPosition) : base(initialPosition)
        {
        }

        public Ladder(Vector2 initialPosition, int numberOfRungs, ColliderManager colliderManager) : base(initialPosition)
        {
            this.colliderManager = colliderManager;

            CollisionObject = true;

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
                    listOfRungs.Add(new AnimatedGameObject(location, "LadderTopRung"));
                }
                else if (j == numberOfRungs - 1)
                {
                    listOfRungs.Add(new AnimatedGameObject(location, "LadderBottomRung"));
                }
                else
                {
                    listOfRungs.Add(new AnimatedGameObject(location, "Ladder"));
                }

            }

            positionOfTopLeftCorner.X = initialPosition.X;
            positionOfTopLeftCorner.Y = initialPosition.Y;

            filename = "Ladder";

        }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            foreach (AnimatedGameObject rung in listOfRungs)
            {
                rung.LoadContent(contentManager, graphicsDevice);
            }

            idleHitbox = new HitboxRectangle((int)positionOfTopLeftCorner.X, (int)positionOfTopLeftCorner.Y, listOfRungs[0].idleHitbox.rectangle.Width, listOfRungs[0].idleHitbox.rectangle.Height * numberOfRungs);
            idleHitbox.isActive = true;

        }


        public override void Update(GameTime gameTime)
        {
            // this causes error - try to call animation_playing which is null
            //base.Update(gameTime);

            foreach (AnimatedGameObject rung in listOfRungs)
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


            if (!References.player.playerStateManager.climbingLadderState.Active)
            {
                if (colliderManager.CheckForCollision(References.player.idleHitbox, idleHitbox))
                {
                    // I'm climbing starting from the top
                    if (References.player.idleHitbox.rectangle.Y + References.player.idleHitbox.rectangle.Height <= positionOfTopLeftCorner.Y && References.player.spriteDirectionY == 1)
                    {
                        References.player.playerStateManager.climbingLadderState.ladder = this;
                        idleHitbox.isActive = false;
                        References.player.playerStateManager.DeactivatePlayerStates();
                        References.player.playerStateManager.climbingLadderState.Activate();
                        return;
                    }

                    // I'm climbing starting from somewhere below the top
                    if (References.player.idleHitbox.rectangle.Y + References.player.idleHitbox.rectangle.Height > positionOfTopLeftCorner.Y && References.player.spriteDirectionY != 0)
                    {
                        References.player.playerStateManager.climbingLadderState.ladder = this;
                        References.player.playerStateManager.DeactivatePlayerStates();
                        References.player.playerStateManager.climbingLadderState.Activate();
                        return;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            //spriteBatch.Draw(References.player.spriteHitboxTexture, idleHitbox.rectangle, Color.Red);

            foreach (AnimatedGameObject rung in listOfRungs)
            {
                rung.Draw(spriteBatch);
            }

        }



    }
}
