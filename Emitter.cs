using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using System.Threading.Tasks;

namespace Adventure
{
    public class Emitter : MovingPlatform
    {
        // Emitters are used by SeriesOfMovingPlatforms_ABWrapAround and derived classes thereof.
        // They are simply MovingPlatforms with access to more animations which are used in puzzles etc.

        public AnimatedSprite animation_Hit;
        public AnimatedSprite animation_Active;
        public AnimatedSprite animation_Success;

        public Emitter(Vector2 position, string filename, AssetManager assetManager) : base(position, filename, assetManager)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();

            animation_Hit = spriteSheet.CreateAnimatedSprite("Hit");
            animation_Active = spriteSheet.CreateAnimatedSprite("Active");
            animation_Success = spriteSheet.CreateAnimatedSprite("Success");

            animation_Hit.OnAnimationEnd = (hello) =>
            {
                UpdatePlayingAnimation(animation_Idle);
            };

        }


    }
}
