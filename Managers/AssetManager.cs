using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Aseprite;
using MonoGame.Aseprite.Sprites;
using MonoGame.Aseprite.Content.Processors;
using MonoGame.Aseprite.AsepriteTypes;
using System.Reflection.Metadata;

namespace Adventure
{
    public class AssetManager
    {
        public List<string> spriteFilenames;
        public Texture2D hitboxTexture;
        public IDictionary<string, SpriteSheet> spriteSheets = new Dictionary<string, SpriteSheet>();

        public AssetManager()
        {



            spriteFilenames = new List<string>()
            {
                "AKeyRound",
                "AncientDoor",
                "Beam",
                "BottomBeam",
                "BouncingOrb",
                "CKeyRound",
                "ClimableBox",
                "ClimablePlatform",
                "Door",
                "EKeyRound",
                "FKeyRound",
                "GKeyRound",
                "hoodedoldman",
                "hoodedoldmanv2",
                "Ivy",
                "Ladder",
                "LadderBottomRung",
                "LadderTopRung",
                "LaunchPad",
                "MovingClimablePlatform",
                "movingPlatform1",
                "movingPlatform1Long",
                "NoteBomb",
                "NoteLine",
                "OrangeOrb",
                "Orb",
                "OrbMiddle",
                "OrbReceptors",
                "OrbVesselEndLeft",
                "OrbVesselEndRight",
                "OrbWindowLeft",
                "OrbWindowRight",
                "OrbWindowMiddle",
                "OrganStopBottom",
                "OrganStopBottom_Vertical",
                "OrganStopMiddle",
                "OrganStopMiddle_Vertical",
                "OrganStopTop",
                "OrganStopTop_Vertical",
                "OrganPipe_Base",
                "OrganPipe_Platform",
                "OrganPipe_Tile",
                "OrganPipe_TileRow",
                "OrganPipe_TileRowLeft",
                "platform_breaking",
                "Post",
                "PostDown",
                "RedDot",
                "RedDot2",
                "RedSquare",
                "Respawn",
                "RopeRing",
                "RopeRing2",
                "rune_A",
                "rune_C",
                "rune_E",
                "rune_F",
                "rune_G",
                "Spike",
                "symbolPlate",
                "symbolPlateLong",
                "Tile_air",
                "Tile_grass",
                "Tile_ground",
                "TopBeam",
                "TuningForkC",
                "TuningForkE",
                "TuningForkG",
                "whiteDot",
                "WoodenGate",
                "WoodenGateHorizontal",
                "YellowDot"
            };

            //Debug.WriteLine(spriteFilenames.Contains("RedDot"));


        }

        public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {

            hitboxTexture = new Texture2D(graphicsDevice, 1, 1);
            hitboxTexture.SetData(new Color[] { Color.White });

            foreach (string filename in spriteFilenames)
            {
                AsepriteFile asepriteFile = contentManager.Load<AsepriteFile>(filename);
                SpriteSheet spriteSheet = SpriteSheetProcessor.Process(graphicsDevice, asepriteFile);
                spriteSheets.Add(filename, spriteSheet);
            }



        }








    }
}
