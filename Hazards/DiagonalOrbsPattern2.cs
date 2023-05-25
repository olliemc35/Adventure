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
    public class DiagonalOrbsPattern2 : GameObject
    {
        public List<MovingPlatform_ABWrapAround> orbs = new List<MovingPlatform_ABWrapAround>();
        public List<OrbReceptor> orbReceptors = new List<OrbReceptor>();
        public float horizontalSpacing;
        public float verticalSpacing;
        public Vector2 startStream;
        public int numberOfOrbs;
        public int indexOfOrbClosestToStart = 0;

        public DiagonalOrbsPattern2(Vector2 startStream, Vector2 endStream, float horizontalSpacing, float verticalSpacing, float speed, SoundManager soundManager, AssetManager assetManager, ColliderManager colliderManager, ScreenManager screenManager, Player player) : base()
        {
            this.horizontalSpacing = horizontalSpacing;
            this.verticalSpacing = verticalSpacing;
            this.startStream = startStream;
            this.soundManager = soundManager;
            this.assetManager = assetManager;
            this.colliderManager = colliderManager;
            this.player = player;

            // This is minimum number
            numberOfOrbs = (int)Math.Floor(Math.Abs(startStream.X - endStream.X) / (8 * horizontalSpacing));
            // We want to increase to make it a multiple of 3 as there are 3 orbs in each pattern block
            // If we don't do this then the pattern will break at the endpoints
            if (numberOfOrbs % 3 != 0)
            {
                numberOfOrbs = numberOfOrbs + (3 - numberOfOrbs % 3);
            }

            for (int i = 0; i < numberOfOrbs; i++)
            {
                Vector2 startPosition = new Vector2();
                startPosition.X = startStream.X;
                Vector2 endPosition = new Vector2();
                endPosition.X = endStream.X;

                if (i % 3 == 0)
                {
                    startPosition.Y = startStream.Y;
                    endPosition.Y = endStream.Y;
                    MovingPlatform_ABWrapAround orb = new MovingPlatform_ABWrapAround(startPosition, endPosition, "RedSquare", speed, new List<int>() { 0, 0 }, assetManager, colliderManager, screenManager, player, "HighCBell", soundManager);
                    orbs.Add(orb);

                }
                else if (i % 3 == 1)
                {
                    startPosition.Y = startStream.Y - 8 * verticalSpacing;
                    endPosition.Y = endStream.Y - 8 * verticalSpacing;
                    MovingPlatform_ABWrapAround orb = new MovingPlatform_ABWrapAround(startPosition, endPosition, "RedSquare", speed, new List<int>() { 0, 0 }, assetManager, colliderManager, screenManager, player, "HighEflatBell", soundManager);
                    orbs.Add(orb);
                }
                else if (i % 3 == 2)
                {
                    startPosition.Y = startStream.Y - 2 * 8 * verticalSpacing;
                    endPosition.Y = endStream.Y - 2 * 8 * verticalSpacing;
                    MovingPlatform_ABWrapAround orb = new MovingPlatform_ABWrapAround(startPosition, endPosition, "RedSquare", speed, new List<int>() { 0, 0 }, assetManager, colliderManager, screenManager, player, "HighGBell", soundManager);
                    orbs.Add(orb);
                }

            }

            orbs[0].movePlatform = true;
            indexOfOrbClosestToStart = 0;

            for (int i = 0; i < 3; i++)
            {
                orbReceptors.Add(new OrbReceptor(new Vector2(startStream.X, startStream.Y - 8 * i * verticalSpacing), "OrbReceptors", assetManager));
                orbReceptors.Add(new OrbReceptor(new Vector2(endStream.X, endStream.Y - 8 * i * verticalSpacing), "OrbReceptors", assetManager));
            }


        }

        public override void LoadContent()
        {
            foreach (MovingPlatform_ABWrapAround orb in orbs)
            {
                orb.LoadContent();
                orb.idleHitbox.rectangle.Width = 4;
                orb.idleHitbox.rectangle.Height = 4;
                orb.idleHitbox.offsetX = 2;
                orb.idleHitbox.offsetY = 2;
            }

            foreach (AnimatedGameObject sprite in orbReceptors)
            {
                sprite.LoadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {

            if (Vector2.Distance(orbs[indexOfOrbClosestToStart].position, orbs[indexOfOrbClosestToStart].positions[0]) >= 8 * horizontalSpacing)
            {
                if (indexOfOrbClosestToStart == orbs.Count - 1)
                {
                    orbs[0].movePlatform = true;
                    indexOfOrbClosestToStart = 0;
                }
                else
                {
                    orbs[indexOfOrbClosestToStart + 1].movePlatform = true;
                    indexOfOrbClosestToStart += 1;
                }
            }


            foreach (MovingPlatform_ABWrapAround orb in orbs)
            {
                orb.Update(gameTime);

                if (orb.hitFlag)
                {
                    orb.hitFlag = false;

                    if (orb.position.Y == startStream.Y)
                    {
                        orbReceptors[1].UpdatePlayingAnimation(orbReceptors[1].animation_Hit, 1);

                        //orbReceptors[1].nameOfCurrentAnimationSprite = "Hit";

                        //orbReceptors[1].animatedSprite_Idle.Play("Hit");
                        //orbReceptors[1].currentFrame = orbReceptors[1].frameAndTag["Hit"].From;
                        //orbReceptors[1].tagOfCurrentFrame = "Hit";
                    }
                    else if (orb.position.Y == startStream.Y - 8 * verticalSpacing)
                    {
                        orbReceptors[3].UpdatePlayingAnimation(orbReceptors[3].animation_Hit, 1);

                        //orbReceptors[3].nameOfCurrentAnimationSprite = "Hit";


                        //orbReceptors[3].animatedSprite_Idle.Play("Hit");
                        //orbReceptors[3].currentFrame = orbReceptors[3].frameAndTag["Hit"].From;
                        //orbReceptors[3].tagOfCurrentFrame = "Hit";

                    }
                    else if (orb.position.Y == startStream.Y - 2 * 8 * verticalSpacing)
                    {
                        orbReceptors[5].UpdatePlayingAnimation(orbReceptors[5].animation_Hit, 1);

                        //orbReceptors[5].nameOfCurrentAnimationSprite = "Hit";


                        //orbReceptors[5].animatedSprite_Idle.Play("Hit");
                        //orbReceptors[5].currentFrame = orbReceptors[5].frameAndTag["Hit"].From;
                        //orbReceptors[5].tagOfCurrentFrame = "Hit";
                    }
                }

            }

            foreach (OrbReceptor orbReceptor in orbReceptors)
            {
                orbReceptor.Update(gameTime);



                //sprite.animatedSprite_Idle.OnAnimationLoop = () =>
                //{
                //    if (sprite.tagOfCurrentFrame == "Hit")
                //    {
                //        sprite.animatedSprite_Idle.Play("Idle");
                //        sprite.currentFrame = sprite.frameAndTag["Idle"].From;
                //        sprite.tagOfCurrentFrame = "Idle";
                //        sprite.animatedSprite_Idle.OnAnimationLoop = null;
                //    }
                //};

            }



        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (MovingPlatform_ABWrapAround orb in orbs)
            {
                orb.Draw(spriteBatch);
            }

            foreach (AnimatedGameObject sprite in orbReceptors)
            {
                sprite.Draw(spriteBatch);
            }

        }




    }
}
