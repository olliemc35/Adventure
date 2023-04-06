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
    public class LocalTeleport : Teleport
    {

        public List<AnimatedGameObject> arrow = new List<AnimatedGameObject>();


        public LocalTeleport(Vector2 initialPosition, string filename) : base(initialPosition, filename)
        {
            radius = 8 * 10;
            speedBoost = 100;
            InRange = false;

            for (int i = 0; i < 20; i++)
            {
                AnimatedGameObject sprite = new AnimatedGameObject(initialPosition, "whiteDot");
                arrow.Add(sprite);

            }
        }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            for (int i = 0; i < arrow.Count(); i++)
            {
                arrow[i].LoadContent(contentManager, graphicsDevice);
            }

            base.LoadContent(contentManager, graphicsDevice);
        }


        public override void Update(GameTime gameTime)
        {

            if (InRange)
            {
                Vector2 vec1 = References.player.position;
                vec1.X += References.player.idleHitbox.offsetX + 0.5f * References.player.idleHitbox.rectangle.Width;
                vec1.Y += References.player.idleHitbox.offsetY + 0.5f * References.player.idleHitbox.rectangle.Height;

                Vector2 vec2 = position;
                vec2.X += 0.5f * idleHitbox.rectangle.Width;
                vec2.Y += 0.5f * idleHitbox.rectangle.Height;


                Vector2 direction = vec2 - vec1;

                direction.Normalize();
                for (int i = 0; i < arrow.Count(); i++)
                {
                    arrow[i].position = position + i * direction;
                    arrow[i].position.X += 0.5f * idleHitbox.rectangle.Width;
                    arrow[i].position.Y += 0.5f * idleHitbox.rectangle.Height;
                    arrow[i].position.X = FindNearestInteger(arrow[i].position.X);
                    arrow[i].position.Y = FindNearestInteger(arrow[i].position.Y);

                }

            }
            if (Vector2.Distance(References.player.position, position) <= radius)
            {
                InRange = true;
                UpdatePlayingAnimation(animation_InRange);
                //nameOfCurrentAnimationSprite = "InRange";

                //animatedSprite_Idle.Play("InRange");
                //currentFrame = frameAndTag["InRange"].From;
                //tagOfCurrentFrame = "InRange";
            }
            else
            {
                InRange = false;
                UpdatePlayingAnimation(animation_Idle);
               // nameOfCurrentAnimationSprite = "Idle";

                //animatedSprite_Idle.Play("Idle");
                //currentFrame = frameAndTag["Idle"].From;
                //tagOfCurrentFrame = "Idle";
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (InRange && !References.player.playerStateManager.teleportState.Active)
            {
                for (int i = 0; i < arrow.Count(); i++)
                {
                    arrow[i].Draw(spriteBatch);
                }

            }

            base.Draw(spriteBatch);
        }


    }
}
