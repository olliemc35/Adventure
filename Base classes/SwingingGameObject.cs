using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    public class SwingingGameObject : MovingGameObject
    {
        // Below are properties for swinging objects
        public float swingForceDuration;
        public float swingAngle = 0;
        public float swingAngleDot = 0;
        public float length;
        public float lengthDot = 0;
        public float lengthForceDuration;
        public float lengthForceMaximum;
        public float lengthForce = 0;
        public bool firstLoopOnSwing = true;
        public float timeAngle = 0;
        public float timeLength = 0;
        public bool attachedToASwingingPivot = false;
        public bool swinging = false;
        public bool impulseWindow = false;
        public float impulseAngle = 0;
        public int swingDirection = 0;
        public float swingDrivingForce = 0;
        public float swingForceMaximum;
        public float swingFrictionConstant = 0.5f;


        public SwingingGameObject() : base()
        {
        }

        public SwingingGameObject(Vector2 initialPosition) : base(initialPosition)
        {
        }

        public SwingingGameObject(Vector2 position, string filename, AssetManager assetManager, ColliderManager colliderManager = null, InputManager inputManager = null, ScreenManager screenManager = null, Player player = null) : base(position, filename, assetManager, colliderManager, inputManager, screenManager, player)
        {
        }



    }
}
