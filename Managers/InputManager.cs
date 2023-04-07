using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class InputManager
    {
        public KeyboardState keyboardState;
        public KeyboardState previousKeyboardState;
        public MouseState mouseState;
        public MouseState previousMouseState;

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

        //public void PlayerMovementInput()
        //{
        //    //Debug.WriteLine(velocityOffSetDueToMovingPlatform.Y);
        //    // We will use these to check whether we have changed direction
        //    int testDirectionX = spriteDirectionX;
        //    int testDirectionY = spriteDirectionY;

     

        //    // Horizontal direction
        //    // This slightly more complicated code accounts for what happens if we have pressed the LEFT and RIGHT key simultaneously
        //    // Essentially we want to move in the direction the LAST key was pressed down
        //    // Our previous method gave priority to the left key which is not desirable
        //    if (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Right) && twoKeysPressedFirstTimeX)
        //    {
        //        if (testDirectionX != -1)
        //        {
        //            spriteDirectionX = -1;
        //        }
        //        else if (testDirectionX != 1)
        //        {
        //            spriteDirectionX = 1;
        //        }
        //        else
        //        {
        //            spriteDirectionX = 0;
        //        }

        //        twoKeysPressedFirstTimeX = false;
        //    }
        //    else if (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Right) && !twoKeysPressedFirstTimeX)
        //    {
        //        // do nothing to change keyboard state in this case
        //    }
        //    else if (keyboardState.IsKeyDown(Keys.Left))
        //    {
        //        spriteDirectionX = -1;
        //        twoKeysPressedFirstTimeX = true;
        //    }
        //    else if (keyboardState.IsKeyDown(Keys.Right))
        //    {
        //        spriteDirectionX = 1;
        //        twoKeysPressedFirstTimeX = true;
        //    }
        //    else
        //    {
        //        spriteDirectionX = 0;
        //        twoKeysPressedFirstTimeX = true;
        //    }


        //    //if (keyboardState.IsKeyDown(Keys.Left)) { spriteDirectionX = -1; }
        //    //else if (keyboardState.IsKeyDown(Keys.Right)) { spriteDirectionX = 1; }
        //    //else { spriteDirectionX = 0; }

        //    // Vertical direction
        //    if (keyboardState.IsKeyDown(Keys.Up)) { spriteDirectionY = -1; }
        //    else if (keyboardState.IsKeyDown(Keys.Down)) { spriteDirectionY = 1; }
        //    else { spriteDirectionY = 0; }

        //    jumpButtonPressed = keyboardState.IsKeyDown(Keys.Space);
        //    //runButtonPressed = keyboardState.IsKeyDown(Keys.LeftShift);
        //    gunButtomPressed = keyboardState.IsKeyDown(Keys.G);
        //    flagJumpButtonPressed = oldKeyboardState.IsKeyUp(Keys.Space) && keyboardState.IsKeyDown(Keys.Space);
        //    flagLeftDashButtonPressed = oldKeyboardState.IsKeyUp(Keys.Z) && keyboardState.IsKeyDown(Keys.Z);
        //    flagGunButtonPressed = oldKeyboardState.IsKeyUp(Keys.G) && keyboardState.IsKeyDown(Keys.G);
        //    flagTeleportButtonPressed = oldKeyboardState.IsKeyUp(Keys.T) && keyboardState.IsKeyDown(Keys.T);
        //    flagBombButtonPressed = oldKeyboardState.IsKeyUp(Keys.B) && keyboardState.IsKeyDown(Keys.B);
        //    flagHookButtonPressed = oldKeyboardState.IsKeyUp(Keys.H) && keyboardState.IsKeyDown(Keys.H);
        //    climbButtonPressed = keyboardState.IsKeyDown(Keys.LeftShift);


        //    flagLeftMouseClick = mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released;

        //    // Keep track of previous direction
        //    if (spriteDirectionX != testDirectionX)
        //    {
        //        previousSpriteDirectionX = testDirectionX;
        //        DirectionChangedX = true;
        //    }
        //    else
        //    {
        //        DirectionChangedX = false;
        //    }
        //    if (spriteDirectionY != testDirectionY)
        //    {
        //        previousSpriteDirectionY = testDirectionY;
        //        DirectionChangedY = true;
        //    }
        //    else
        //    {
        //        DirectionChangedY = false;
        //    }

        //    // To get the correct mousePosition relative to our coordinates we must take into account the fact we have rescaled the screen
        //    mousePosition.X = ((float)2 / 5) * mouseState.X;
        //    mousePosition.Y = ((float)3 / 8) * mouseState.Y;



        //    if (runButtonPressed)
        //    {
        //        maxHorizontalSpeed = 180;
        //    }
        //    else
        //    {
        //        maxHorizontalSpeed = 120;
        //    }


           
        //}




    }
}
