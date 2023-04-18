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
    public class DiagonalOrbsPattern1 : GameObject
    {
        public List<MusicalMovingPlatformNoLoop> orbs = new List<MusicalMovingPlatformNoLoop>();
        public List<OrbReceptor> orbReceptors = new List<OrbReceptor>();
        public float horizontalSpacing;
        public float verticalSpacing;
        public Vector2 startStream;
        public int numberOfOrbs;
        public int indexOfOrbClosestToStart = 0;

        public DiagonalOrbsPattern1(Vector2 startStream, Vector2 endStream, float horizontalSpacing, float verticalSpacing, float speed, AssetManager assetManager, ColliderManager colliderManager, SoundManager soundManager, Player player) : base()
        {
            this.horizontalSpacing = horizontalSpacing;
            this.verticalSpacing = verticalSpacing;
            this.startStream = startStream;
            this.soundManager = soundManager;
            this.assetManager = assetManager;
            this.player = player;
            this.colliderManager = colliderManager;

            // This is minimum number
            numberOfOrbs = (int)Math.Floor(Math.Abs(startStream.X - endStream.X) / (8 * horizontalSpacing));
            // We want to increase to make it a multiple of 4 as there are 4 orbs in each pattern block
            // If we don't do this then the pattern will break at the endpoints
            if (numberOfOrbs % 4 != 0)
            {
                numberOfOrbs = numberOfOrbs + (4 - numberOfOrbs % 4);
            }

            for (int i = 0; i < numberOfOrbs; i++)
            {
                Vector2 startPosition = new Vector2();
                startPosition.X = startStream.X;
                Vector2 endPosition = new Vector2();
                endPosition.X = endStream.X;

                if (i % 4 == 0)
                {
                    startPosition.Y = startStream.Y;
                    endPosition.Y = endStream.Y;
                    MusicalMovingPlatformNoLoop orb = new MusicalMovingPlatformNoLoop(startPosition, "RedSquare", endPosition, 0, speed, "HighCBell", soundManager, assetManager, colliderManager, player);
                    orbs.Add(orb);

                }
                else if (i % 4 == 1 || i % 4 == 3)
                {
                    startPosition.Y = startStream.Y - 8 * verticalSpacing;
                    endPosition.Y = endStream.Y - 8 * verticalSpacing;
                    MusicalMovingPlatformNoLoop orb = new MusicalMovingPlatformNoLoop(startPosition, "RedSquare", endPosition, 0, speed, "HighEflatBell", soundManager, assetManager, colliderManager, player);
                    orbs.Add(orb);
                }
                else if (i % 4 == 2)
                {
                    startPosition.Y = startStream.Y - 2 * 8 * verticalSpacing;
                    endPosition.Y = endStream.Y - 2 * 8 * verticalSpacing;
                    MusicalMovingPlatformNoLoop orb = new MusicalMovingPlatformNoLoop(startPosition, "RedSquare", endPosition, 0, speed, "HighGBell", soundManager, assetManager, colliderManager, player);
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
            foreach (MusicalMovingPlatformNoLoop orb in orbs)
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

            if (Vector2.Distance(orbs[indexOfOrbClosestToStart].position, orbs[indexOfOrbClosestToStart].startPosition) >= 8 * horizontalSpacing)
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


            foreach (MusicalMovingPlatformNoLoop orb in orbs)
            {
                orb.Update(gameTime);

                if (orb.hitFlag)
                {
                    orb.hitFlag = false;

                    if (orb.position.Y == startStream.Y)
                    {
                        orbReceptors[1].UpdatePlayingAnimation(orbReceptors[1].animation_Hit, 1);
                    }
                    else if (orb.position.Y == startStream.Y - 8 * verticalSpacing)
                    {
                        orbReceptors[3].UpdatePlayingAnimation(orbReceptors[3].animation_Hit, 1);
                    }
                    else if (orb.position.Y == startStream.Y - 2 * 8 * verticalSpacing)
                    {
                        orbReceptors[5].UpdatePlayingAnimation(orbReceptors[5].animation_Hit, 1);
                    }
                }

            }

            foreach (OrbReceptor orbReceptor in orbReceptors)
            {
                orbReceptor.Update(gameTime);
              

            }



        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (MusicalMovingPlatformNoLoop orb in orbs)
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
