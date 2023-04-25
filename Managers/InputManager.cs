using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Adventure
{
    public class InputManager
    {
        public KeyboardState keyboardState;
        public KeyboardState previousKeyboardState;
        public MouseState mouseState;
        public MouseState previousMouseState;
        public Vector2 mousePosition = new Vector2();

        public InputManager()
        {

        }

        public void Update()
        {
            previousKeyboardState = keyboardState;
            previousMouseState = mouseState;
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
        }

        public bool IsKeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return keyboardState.IsKeyUp(key);
        }

        public bool OnKeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        }

        public bool OnKeyUp(Keys key)
        {
            return keyboardState.IsKeyUp(key) && previousKeyboardState.IsKeyDown(key);
        }

        public void UpdatePlayerInput(Player player)
        {
            //Debug.WriteLine(velocityOffSetDueToMovingPlatform.Y);
            // We will use these to check whether we have changed direction
            int testDirectionX = player.directionX;
            int testDirectionY = player.directionY;



            // Horizontal direction
            // This slightly more complicated code accounts for what happens if we have pressed the LEFT and RIGHT key simultaneously
            // Essentially we want to move in the direction the LAST key was pressed down
            // Our previous method gave priority to the left key which is not desirable
            if (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Right) && player.twoKeysPressedFirstTimeX)
            {
                if (testDirectionX != -1)
                {
                    player.directionX = -1;
                }
                else if (testDirectionX != 1)
                {
                    player.directionX = 1;
                }
                else
                {
                    player.directionX = 0;
                }

                player.twoKeysPressedFirstTimeX = false;
            }
            else if (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Right) && !player.twoKeysPressedFirstTimeX)
            {
                // do nothing to change keyboard state in this case
            }
            else if (keyboardState.IsKeyDown(Keys.Left))
            {
                player.directionX = -1;
                player.twoKeysPressedFirstTimeX = true;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                player.directionX = 1;
                player.twoKeysPressedFirstTimeX = true;
            }
            else
            {
                player.directionX = 0;
                player.twoKeysPressedFirstTimeX = true;
            }


            //if (keyboardState.IsKeyDown(Keys.Left)) { spriteDirectionX = -1; }
            //else if (keyboardState.IsKeyDown(Keys.Right)) { spriteDirectionX = 1; }
            //else { spriteDirectionX = 0; }

            // Vertical direction
            if (keyboardState.IsKeyDown(Keys.Up)) { player.directionY = -1; }
            else if (keyboardState.IsKeyDown(Keys.Down)) { player.directionY = 1; }
            else { player.directionY = 0; }

            player.jumpButtonPressed = keyboardState.IsKeyDown(Keys.Space);
            //runButtonPressed = keyboardState.IsKeyDown(Keys.LeftShift);
            player.gunButtomPressed = keyboardState.IsKeyDown(Keys.G);
            player.flagJumpButtonPressed = previousKeyboardState.IsKeyUp(Keys.Space) && keyboardState.IsKeyDown(Keys.Space);
            player.flagLeftDashButtonPressed = previousKeyboardState.IsKeyUp(Keys.Z) && keyboardState.IsKeyDown(Keys.Z);
            player.flagGunButtonPressed = previousKeyboardState.IsKeyUp(Keys.G) && keyboardState.IsKeyDown(Keys.G);
            player.flagTeleportButtonPressed = previousKeyboardState.IsKeyUp(Keys.T) && keyboardState.IsKeyDown(Keys.T);
            player.flagBombButtonPressed = previousKeyboardState.IsKeyUp(Keys.B) && keyboardState.IsKeyDown(Keys.B);
            player.flagHookButtonPressed = previousKeyboardState.IsKeyUp(Keys.H) && keyboardState.IsKeyDown(Keys.H);
            player.climbButtonPressed = keyboardState.IsKeyDown(Keys.LeftShift);


            player.flagLeftMouseClick = mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;

            // Keep track of previous direction
            if (player.directionX != testDirectionX)
            {
                player.previousDirectionX = testDirectionX;
                player.DirectionChangedX = true;
            }
            else
            {
                player.DirectionChangedX = false;
            }
            if (player.directionY != testDirectionY)
            {
                player.previousDirectionY = testDirectionY;
                player.DirectionChangedY = true;
            }
            else
            {
                player.DirectionChangedY = false;
            }

            // To get the correct mousePosition relative to our coordinates we must take into account the fact we have rescaled the screen
            mousePosition.X = ((float)2 / 5) * mouseState.X;
            mousePosition.Y = ((float)3 / 8) * mouseState.Y;



            if (player.runButtonPressed)
            {
                player.maxHorizontalSpeed = 180;
            }
            else
            {
                player.maxHorizontalSpeed = 120;
            }



        }




    }
}
