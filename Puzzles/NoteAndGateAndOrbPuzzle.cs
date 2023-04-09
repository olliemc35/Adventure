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
    public class NoteAndGateAndOrbPuzzle : GameObject
    {
        public AssetManager assetManager;

        public List<Note> notes;
        public Gate gate;
        public OrbVessel orbVessel;
        public AnimatedGameObject symbolPlate;
        public List<Symbol> symbols = new List<Symbol>();
        public List<Orb> orbs = new List<Orb>();


        public int indexWhichMustBePlayed = 0;
        public int indexWhichMustBePlayedNext = 0;



        public int indexOfNextOrbToBeSetActive = 1;
        public int indexOfOrbOnScreen = 0;


        public bool resetToStart = false;
        public bool doneNothing = true;
        public bool winConditionSet = false;


        public NoteAndGateAndOrbPuzzle(Vector2 symbolPlatePosition, string symbolPlateFilename, List<Note> notes, Gate gate, OrbVessel orbVessel, AssetManager assetManager)
        {
            this.notes = notes;
            this.gate = gate;
            this.orbVessel = orbVessel;
            symbolPlate = new AnimatedGameObject(symbolPlatePosition, symbolPlateFilename, assetManager);
            this.assetManager = assetManager;
        }

        public override void LoadContent()
        {

            orbVessel.LoadContent();

            symbolPlate.LoadContent();

            for (int i = 0; i < notes.Count; i++)
            {
                Symbol symbol = new Symbol(new Vector2(symbolPlate.position.X + 12 * (i + 1), symbolPlate.position.Y + 4), notes[i].symbolFilename, assetManager);
                symbol.LoadContent();
                symbols.Add(symbol);
            }

            for (int i = 0; i < notes.Count; i++)
            {
                Orb orb = new Orb(new Vector2(orbVessel.orbEndRight, 0), notes[i].orbFilename, notes[i].orbSpeed, orbVessel, assetManager);
                orb.LoadContent();
                orbs.Add(orb);
            }

            orbs[0].isActive = true;

        }

        public override void Update(GameTime gameTime)
        {
            // We must update the puzzle logic first as otherwise we will never detect
            // when notes[i].flagPlayerInteractedWith is true/false . (The order of the update steps are absolutely crucial.) 

            if (!gate.open)
            {
                PuzzleLogic();
            }
            if (gate.open && !winConditionSet)
            {
                //References.soundManager.playCMajArpeggio = true;
                //References.soundManager.flagPlayCMajArpeggio = true;
                winConditionSet = true;
            }

            for (int i = 0; i < notes.Count; i++)
            {
                symbols[i].Update(gameTime);

                if (orbs[i].isActive)
                {
                    orbs[i].Update(gameTime);
                }
            }



        }



        public override void Draw(SpriteBatch spriteBatch)
        {

            orbVessel.Draw(spriteBatch);

            symbolPlate.Draw(spriteBatch);

            for (int i = 0; i < notes.Count; i++)
            {
                symbols[i].Draw(spriteBatch);

                if (orbs[i].isActive)
                {
                    orbs[i].Draw(spriteBatch);
                }

            }



        }


        public void PuzzleLogic()
        {


            if (!orbs[indexWhichMustBePlayed].isActive)
            {
                if (indexWhichMustBePlayed > 0)
                {
                    if (doneNothing)
                    {
                        for (int j = 0; j < notes.Count; j++)
                        {
                            symbols[j].TurnedOn = false;
                        }

                        indexWhichMustBePlayedNext = 0;
                    }
                    else
                    {
                        doneNothing = true;
                    }


                }



                indexWhichMustBePlayed = indexWhichMustBePlayedNext;
                orbs[indexWhichMustBePlayed].isActive = true;
                orbs[indexWhichMustBePlayed].position.X = orbVessel.orbEndRight;
            }


            if (indexWhichMustBePlayed == 0)
            {
                if (notes[0].flagPlayerInteractedWith && orbs[0].inWindow)
                {
                    orbs[0].setToTurnInActive = true;
                    symbols[0].TurnedOn = true;
                    indexWhichMustBePlayedNext = 1;
                }
            }
            else
            {

                orbs[indexWhichMustBePlayed].setToTurnInActive = true;


                for (int i = 0; i < notes.Count; i++)
                {
                    if (notes[i].flagPlayerInteractedWith)
                    {
                        doneNothing = false;

                        if (!orbs[i].inWindow || i != indexWhichMustBePlayed)
                        {
                            for (int j = 0; j < notes.Count; j++)
                            {
                                symbols[j].TurnedOn = false;
                            }

                            orbs[indexWhichMustBePlayed].setToTurnInActive = false;
                            orbs[indexWhichMustBePlayed].isActive = false;
                            orbs[0].position.X = orbs[indexWhichMustBePlayed].position.X;
                            orbs[0].isActive = true;
                            indexWhichMustBePlayed = 0;
                            indexWhichMustBePlayedNext = 0;
                            break;
                        }
                        else
                        {
                            symbols[i].TurnedOn = true;
                            indexWhichMustBePlayedNext = indexWhichMustBePlayed + 1;
                        }

                    }
                }


            }


            if (indexWhichMustBePlayedNext == notes.Count)
            {
                gate.open = true;
                orbs[indexWhichMustBePlayed].setToTurnInActive = true;
                return;
            }



        }




    }
}
