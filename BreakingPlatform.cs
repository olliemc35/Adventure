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

namespace Adventure
{
    public class BreakingPlatform : AnimatedGameObject
    {
        public AnimatedSprite animation_Breaking;
        public int numberOfFramesUntilBreak;
        public int counter = 0;
        public bool notTouchedYet = true;
        public bool DrawMe = true;


        public BreakingPlatform(Vector2 position, string filename, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player, int numberOfFramesUntilBreak = 60) : base(position, filename, assetManager, colliderManager, null, screenManager, player)
        {
            this.numberOfFramesUntilBreak = numberOfFramesUntilBreak;
            CollisionObject = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            idleHitbox.isActive = true;
            animation_Breaking = spriteSheet.CreateAnimatedSprite("Breaking");

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (notTouchedYet && colliderManager.CheckForCollision(player.idleHitbox, idleHitbox))
            {
                notTouchedYet = false;
                counter += 1;
                UpdatePlayingAnimation(animation_Breaking);
            }

            if (counter > 0)
            {
                if (counter == 2 * numberOfFramesUntilBreak)
                {
                    idleHitbox.isActive = true;
                    //Enabled = true;
                    notTouchedYet = true;
                    DrawMe = true;
                    UpdatePlayingAnimation(animation_Idle);
                    counter = 0;
                }
                else if (counter == numberOfFramesUntilBreak)
                {
                    idleHitbox.isActive = false;
                    DrawMe = false;
                    //Enabled = false;
                    counter += 1;
                }
                else
                {
                    counter += 1;
                }
            }




        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (DrawMe)
            {
                base.Draw(spriteBatch);
            }

            //spriteBatch.Draw(idleHitbox.texture, idleHitbox.rectangle, Color.Blue);
        }








    }
}
