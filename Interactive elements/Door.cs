﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using MonoGame.Aseprite.Sprites;

namespace Adventure
{
    public class Door : AnimatedGameObject
    {
        public int doorNumber;
        public int screenNumberToMoveTo;
        public int doorNumberToMoveTo;
        public bool locked = false;

        public AnimatedSprite animation_Locked;
        public Door(Vector2 initialPosition, string filename, int doorNumber, int screenNumberToMoveTo, int doorNumberToMoveTo, AssetManager assetManager, ColliderManager colliderManager, InputManager inputManager, ScreenManager screenManager, Player player) : base(initialPosition, filename, assetManager)
        {
            this.doorNumber = doorNumber;
            this.screenNumberToMoveTo = screenNumberToMoveTo;
            this.doorNumberToMoveTo = doorNumberToMoveTo;
            //CollisionObject = true;
            this.colliderManager = colliderManager;
            this.inputManager = inputManager;
            this.screenManager = screenManager;
            this.player = player;

            // Why does the order matter ???
            // This is a cheap way of ensuring that the doors are added to the list of screen GameObjects in the right order
            // But only works properly if there are two doors per screen. 
            // So with more complex levels would have to think of a different way to do this
            if (doorNumber == 2)
            {
                LoadLast = true;
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (spriteSheet.ContainsAnimationTag("Locked"))
            {
                animation_Locked = spriteSheet.CreateAnimatedSprite("Locked");
                locked = true;
                UpdatePlayingAnimation(animation_Locked);
            }
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            if (colliderManager.CheckForCollision(player.idleHitbox, idleHitbox) && inputManager.OnKeyUp(Keys.Up) && !locked)
            {
                screenManager.activeScreen.ChangeScreenFlag = true;
                screenManager.PreviousScreenNumber = screenManager.activeScreen.screenNumber;
                screenManager.ScreenNumber = screenNumberToMoveTo;
                screenManager.DoorNumberToMoveTo = doorNumberToMoveTo;
            }



        }

        public override void HandleNoteTrigger()
        {
            locked = false;
            UpdatePlayingAnimation(animation_Idle);
        }


    }
}
