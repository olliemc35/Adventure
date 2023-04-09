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

        public Ladder(Vector2 initialPosition, int numberOfRungs, AssetManager assetManager, ColliderManager colliderManager, Player player) : base(initialPosition)
        {
            this.colliderManager = colliderManager;
            this.assetManager = assetManager;
            this.player = player;

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
                    listOfRungs.Add(new AnimatedGameObject(location, "LadderTopRung", assetManager));
                }
                else if (j == numberOfRungs - 1)
                {
                    listOfRungs.Add(new AnimatedGameObject(location, "LadderBottomRung", assetManager));
                }
                else
                {
                    listOfRungs.Add(new AnimatedGameObject(location, "Ladder", assetManager));
                }

            }

            positionOfTopLeftCorner.X = initialPosition.X;
            positionOfTopLeftCorner.Y = initialPosition.Y;

            filename = "Ladder";

        }

        public override void LoadContent()
        {
            foreach (AnimatedGameObject rung in listOfRungs)
            {
                rung.LoadContent();
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
            if (player.idleHitbox.rectangle.Y + player.idleHitbox.rectangle.Height <= positionOfTopLeftCorner.Y && player.spriteDirectionY != 1)
            {
                idleHitbox.isActive = true;
            }
            else
            {
                idleHitbox.isActive = false;
            }


            if (!player.playerStateManager.climbingLadderState.Active)
            {
                if (colliderManager.CheckForCollision(player.idleHitbox, idleHitbox))
                {
                    // I'm climbing starting from the top
                    if (player.idleHitbox.rectangle.Y + player.idleHitbox.rectangle.Height <= positionOfTopLeftCorner.Y && player.spriteDirectionY == 1)
                    {
                        player.playerStateManager.climbingLadderState.ladder = this;
                        idleHitbox.isActive = false;
                        player.playerStateManager.DeactivatePlayerStates();
                        player.playerStateManager.climbingLadderState.Activate();
                        return;
                    }

                    // I'm climbing starting from somewhere below the top
                    if (player.idleHitbox.rectangle.Y + player.idleHitbox.rectangle.Height > positionOfTopLeftCorner.Y && player.spriteDirectionY != 0)
                    {
                        player.playerStateManager.climbingLadderState.ladder = this;
                        player.playerStateManager.DeactivatePlayerStates();
                        player.playerStateManager.climbingLadderState.Activate();
                        return;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            foreach (AnimatedGameObject rung in listOfRungs)
            {
                rung.Draw(spriteBatch);
            }

        }



    }
}
