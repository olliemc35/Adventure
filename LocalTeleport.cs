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

        public List<Sprite> arrow = new List<Sprite>();


        public LocalTeleport(Vector2 initialPosition, string filename) : base(initialPosition, filename)
        {
            radius = 8 * 10;
            speedBoost = 100;
            InRange = false;

            for (int i = 0; i < 20; i++)
            {
                Sprite sprite = new Sprite(initialPosition, "whiteDot");
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
                Vector2 vec1 = References.player.spritePosition;
                vec1.X += References.player.idleHitbox.offsetX + 0.5f * References.player.idleHitbox.rectangle.Width;
                vec1.Y += References.player.idleHitbox.offsetY + 0.5f * References.player.idleHitbox.rectangle.Height;

                Vector2 vec2 = spritePosition;
                vec2.X += 0.5f * idleHitbox.rectangle.Width;
                vec2.Y += 0.5f * idleHitbox.rectangle.Height;


                Vector2 direction = vec2 - vec1;

                direction.Normalize();
                for (int i = 0; i < arrow.Count(); i++)
                {
                    arrow[i].spritePosition = spritePosition + i * direction;
                    arrow[i].spritePosition.X += 0.5f * idleHitbox.rectangle.Width;
                    arrow[i].spritePosition.Y += 0.5f * idleHitbox.rectangle.Height;
                    arrow[i].spritePosition.X = DistanceToNearestInteger(arrow[i].spritePosition.X);
                    arrow[i].spritePosition.Y = DistanceToNearestInteger(arrow[i].spritePosition.Y);

                }

            }
            if (Vector2.Distance(References.player.spritePosition, spritePosition) <= radius)
            {
                InRange = true;
                animatedSprite_Idle.Play("InRange");
                currentFrame = frameAndTag["InRange"].From;
                tagOfCurrentFrame = "InRange";
            }
            else
            {
                InRange = false;
                animatedSprite_Idle.Play("Idle");
                currentFrame = frameAndTag["Idle"].From;
                tagOfCurrentFrame = "Idle";
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
