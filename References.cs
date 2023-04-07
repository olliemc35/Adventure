using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public static class References
    {
        public static Player player;


        public static ContentManager content;
        public static AssetManager assetManager;

        public static ColliderManager colliderManager;

        public static GraphicsDevice graphicsDevice;
        public static Game1 game;

        public static GameScreen activeScreen;

        public static Camera camera;

        public static RenderTarget2D renderTarget;

        public static int ScreenNumber;
        public static int PreviousScreenNumber;
        public static int DoorNumberToMoveTo;



        public static SoundManager soundManager;

        public static int counter;





    }
}
