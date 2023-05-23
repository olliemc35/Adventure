using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.Sprites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Adventure
{
    public class MovingOrbReceptor : MovingPlatform_AB
    {
        public AnimatedSprite animation_Hit;

        public MovingOrbReceptor(Vector2 initialPosition, Vector2 endPoint, string filename, float speed, List<int> stationaryTimes, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player) : base(initialPosition, endPoint, filename, speed, stationaryTimes, assetManager, colliderManager, screenManager, player)
        {
            movePlatform = false;
        }


        public override void LoadContent()
        {
            base.LoadContent();

            animation_Hit = spriteSheet.CreateAnimatedSprite("Hit");

            animation_Hit.OnAnimationEnd = (hello) =>
            {
                UpdatePlayingAnimation(animation_Idle);
            };

        }


        public override void ManageAnimations()
        {
            if (animation_playing == animation_Hit)
            {
                // do nothing
            }
            else if (!movePlatform || direction == Direction.stationary || halt)
            {
                UpdatePlayingAnimation(animation_Idle);
            }
            else
            {
                UpdatePlayingAnimation(animation_Moving);
            }

        }
    }
}
