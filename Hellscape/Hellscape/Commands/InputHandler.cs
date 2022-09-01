using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Hellscape

    /*
     * Input handler class, used to record inputs and return commands of 
     * the associated input type
     */
{
    public class InputHandler
    {
        Keys upKey = Keys.W;
        Keys downKey = Keys.S;
        Keys leftKey = Keys.A;
        Keys rightKey = Keys.D;

        Keys lightAttackKey = Keys.K;
        Keys heavyAttackKey = Keys.L;

        // 4 move command objects for each option
        MoveCommand moveLEFT;
        MoveCommand moveRIGHT;
        MoveCommand moveUP;
        MoveCommand moveDOWN;

        AttackCommand lightAttack;
        AttackCommand heavyAttack;
        List<Command> previousMoves;
        MainGame.Direction facing = MainGame.Direction.LEFT;
        // 2 attack command objects

        


        private KeyboardState prevKeyboardState;
        private KeyboardState keyboardState;

        

        Vector2 standardHeavyRange = new Vector2(2, 1);
        Vector2 combo1Range = new Vector2(2,2);
        Vector2 combo2Range = new Vector2(3,3);
        Vector2 combo3Range = new Vector2(4,3);


        public InputHandler()
        {
            moveLEFT = new MoveCommand((int)MainGame.Direction.LEFT);
            moveRIGHT = new MoveCommand((int)MainGame.Direction.RIGHT);
            moveUP = new MoveCommand((int)MainGame.Direction.UP);
            moveDOWN = new MoveCommand((int)MainGame.Direction.DOWN);

            lightAttack = new AttackCommand((int)facing);
            heavyAttack = new AttackCommand((int)facing, standardHeavyRange , true);

            
            previousMoves = new List<Command> {
            new Command(),
            new Command(),
            new Command(),
            new Command()
            };
        }

        public bool IsKeyDown(Keys key)
        {
            return (keyboardState.IsKeyDown(key));
        }

        public bool IsHoldingKey(Keys key)
        {
            return (keyboardState.IsKeyDown(key) &&
                prevKeyboardState.IsKeyDown(key));
        }

        public bool WasKeyPressed(Keys key)
        {
            return (keyboardState.IsKeyDown(key) &&
                prevKeyboardState.IsKeyUp(key));
        }

        public bool HasReleasedKey(Keys key)
        {
            return (keyboardState.IsKeyUp(key) &&
                prevKeyboardState.IsKeyDown(key));
        }

       
        

        //returns command
        public void Update()
        {
            //set our previous state to our new state
            prevKeyboardState = keyboardState;

            //get our new state
            keyboardState = Keyboard.GetState();

            
        }

        public Command checkForInput()
        {

            // if button pressed return command
            Update();
            Command currentCommand = null;
            if (IsKeyDown(upKey))
            { currentCommand = moveUP;}
            if (IsKeyDown(downKey)) 
            { currentCommand = moveDOWN;}
            if (IsKeyDown(leftKey)) { currentCommand = moveLEFT; }
            if (IsKeyDown(rightKey)) { currentCommand = moveRIGHT; }

            if (IsKeyDown(lightAttackKey)) { currentCommand = lightAttack; }

            //combo algorithm
            if(IsKeyDown(heavyAttackKey)) {

                int previousLightAttacks = 0;

                for(int i = previousMoves.Count(); i >= previousMoves.Count - 3; i--)
                {
                    Debug.WriteLine(previousMoves.Count());
                    if(previousMoves[i-1].isAttack && previousMoves[i-1].isLight)
                    {
                        previousLightAttacks++;
                    }
                    else { break; }
                }

                switch (previousLightAttacks)
                {
                    case 0:
                        heavyAttack.setRange(standardHeavyRange);
                        break;

                    case 1:
                        heavyAttack.setRange(combo1Range);
                        break;

                    case 2:
                        heavyAttack.setRange(combo2Range);
                        break;

                    case 3:
                        heavyAttack.setRange(combo3Range);
                        break;
                }

                

                
                currentCommand = heavyAttack; 
            }
            if (currentCommand != null)
            {
                previousMoves.Add(currentCommand);
                facing = (MainGame.Direction)currentCommand.getDirection();
                lightAttack = new AttackCommand((int)facing);
                heavyAttack = new AttackCommand((int)facing, standardHeavyRange, true);
            }
            return currentCommand;
        }

        public Vector2 getStandardRange()
        {
            return standardHeavyRange;
        }

        

    }
}
