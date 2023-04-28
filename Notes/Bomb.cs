using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Aseprite.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class Bomb : AnimatedGameObject
    {
        public bool detonate = false;
        public bool readyToRemove = false;
        public Note attachedNote;

        public AnimatedSprite animation_Detonate;
        public AnimatedSprite animation_Planted;


        public Bomb() : base()
        {

        }

        public Bomb(Vector2 initialPosition) : base(initialPosition)
        {

        }

        public Bomb(Vector2 initialPosition, string filename, AssetManager assetManager, ColliderManager colliderManager, InputManager inputManager, ScreenManager screenManager, Player player) : base(initialPosition, filename, assetManager, colliderManager, inputManager, screenManager, player)
        {
        }


        public override void LoadContent()
        {
            base.LoadContent();

            animation_Detonate = spriteSheet.CreateAnimatedSprite("Detonate");
            animation_Planted = spriteSheet.CreateAnimatedSprite("Planted");

            animation_Detonate.OnAnimationEnd = (hello) =>
            {
                detonate = false;
                //animation_Detonate.OnAnimationEnd = null;
            };

        }

       


        public override void Update(GameTime gameTime)
        {
            if (detonate)
            {
                UpdatePlayingAnimation(animation_Detonate, 1);
            }
            else
            {
                UpdatePlayingAnimation(animation_Planted);
            }



            if (!player.bombPlanted)
            {
                if (screenManager.activeScreen.screenNotes.Count > 0)
                {
                    foreach (Note note in screenManager.activeScreen.screenNotes)
                    {
                        // What to do if I press B next to a note (i.e. bomb)
                        if (colliderManager.CheckForCollision(player.idleHitbox, note.key.idleHitbox) && inputManager.OnKeyDown(Keys.B))
                        {
                            player.bombPlanted = true;
                            position = player.position;
                            attachedNote = note;
                        }

                    }
                }
            }
            else
            {
                if (inputManager.OnKeyDown(Keys.B))
                {
                    detonate = true;
                    player.bombPlanted = false;
                    attachedNote.flagPlayerInteractedWith = true;
                }
            }



            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (player.bombPlanted || detonate)
            {
                base.Draw(spriteBatch);
            }
        }


    }
}
