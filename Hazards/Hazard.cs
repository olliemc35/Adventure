using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Aseprite;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Linq;
using MonoGame.Aseprite.Sprites;
using System.Xml;

namespace Adventure
{
    public class Hazard : AnimatedGameObject
    {
        public Hazard(Vector2 initialPosition, string filename, ColliderManager colliderManager) : base(initialPosition, filename, colliderManager)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!References.player.playerStateManager.deadState.Active && colliderManager.CheckForCollision(References.player.idleHitbox, idleHitbox))
            {
                References.player.Dead = true;
                References.player.playerStateManager.DeactivatePlayerStates();
                References.player.playerStateManager.deadState.Activate();
            }
        }

    }
}
