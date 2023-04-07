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

        public Bomb(Vector2 initialPosition, string filename, ColliderManager colliderManager, InputManager inputManager) : base(initialPosition, filename, colliderManager, inputManager)
        {
        }


        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            base.LoadContent(contentManager, graphicsDevice);

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



            if (!References.player.bombPlanted)
            {
                if (References.activeScreen.screenNotes.Count > 0)
                {
                    foreach (Note note in References.activeScreen.screenNotes)
                    {
                        // What to do if I press B next to a note (i.e. bomb)
                        if (colliderManager.CheckForCollision(References.player.idleHitbox, note.key.idleHitbox) && inputManager.OnKeyUp(Keys.B))
                        {
                            References.player.bombPlanted = true;
                            position = References.player.position;
                            attachedNote = note;
                        }

                    }
                }
            }
            else
            {
                if (inputManager.OnKeyUp(Keys.B))
                {
                    detonate = true;
                    References.player.bombPlanted = false;
                    attachedNote.flagPlayerInteractedWith = true;
                }
            }

            //if (References.player.bombPlanted)
            //{
            //    if (inputManager.OnKeyUp(Keys.B))
            //    {
            //        detonate = true;
            //        References.player.bombPlanted = false;
            //        attachedNote.flagPlayerInteractedWith = true;
            //    }
            //}

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (References.player.bombPlanted || detonate)
            {
                base.Draw(spriteBatch);
            }
        }


    }
}
