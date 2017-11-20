// .NET stuff
using System;
using System.Collections.Generic;
using System.Linq;
// XNA stuff
using Microsoft.Xna.Framework.Input;
// Makes XNA input less horrible
using XInputManager;

namespace PfisterGameEngine
{

    public class KeyboardEventArgs : EventArgs // Data to be passed with events onKeyTyped, onKeyPressed, and onKeyReleased
    {
        // Properties
        public Keys Key // Key that is pressed/released
        {
            protected set;
            get;
        }
        public float Elapsed 
        {
            get;
            protected set;
        } // Elapsed game time between frames
        public bool IsShifted // True if shift was pressed with key, false otherwise
        {
            protected set;
            get;
        }
        // 
        
        // Constructors/Destructors
        public KeyboardEventArgs(Keys key, bool isShifted, float elapsed)
        {
            Key = key;
            IsShifted = isShifted;
            Elapsed = elapsed;
        }
        // 
    }
    public static class KeyboardManager // Manages keyboard typing and keyboard-related event firing
    {

        // Event Handling
        public static event EventHandler<KeyboardEventArgs> OnKeyTyped; // Fires on key pressed this frame, but not last frame
        public static event EventHandler<KeyboardEventArgs> OnKeyPressed; // Fires on key pressed this frame
        public static event EventHandler<KeyboardEventArgs> OnKeyReleased; // Fires on key pressed last frame, but not this frame

        private static void KeyTyped(Keys key, bool isShifted, float elapsed) // Does null checking and calls onKeyTyped event
        {
            OnKeyTyped?.Invoke(null, new KeyboardEventArgs(key, isShifted, elapsed));
        }

        private static void KeyPressed(Keys key, bool isShifted, float elapsed) // Does null checking and calls onKeyPressed event
        {
            OnKeyPressed?.Invoke(null, new KeyboardEventArgs(key, isShifted, elapsed));
        }

        private static void KeyReleased(Keys key, bool isShifted, float elapsed) // Does null checking and calls onKeyReleased event
        {
            OnKeyReleased?.Invoke(null, new KeyboardEventArgs(key, isShifted, elapsed));
        }


        // 

        // Static Methods
        public static string GetKeyString(Keys key, bool isShifted)
        {
            string keyString = "";
            Console.WriteLine(Keys.A.ToString());
            switch (key) // Messy, but if key has a character value associated, return it
            {
                   // Alphabet keys
                   case Keys.A: if (isShifted) { keyString = "A"; } else { keyString = "a"; } break;
                   case Keys.B: if (isShifted) { keyString = "B"; } else { keyString = "b"; } break;
                   case Keys.C: if (isShifted) { keyString = "C"; } else { keyString = "c"; } break;
                   case Keys.D: if (isShifted) { keyString = "D"; } else { keyString = "d"; } break;
                   case Keys.E: if (isShifted) { keyString = "E"; } else { keyString = "e"; } break;
                   case Keys.F: if (isShifted) { keyString = "F"; } else { keyString = "f"; } break;
                   case Keys.G: if (isShifted) { keyString = "G"; } else { keyString = "g"; } break;
                   case Keys.H: if (isShifted) { keyString = "H"; } else { keyString = "h"; } break;
                   case Keys.I: if (isShifted) { keyString = "I"; } else { keyString = "i"; } break;
                   case Keys.J: if (isShifted) { keyString = "J"; } else { keyString = "j"; } break;
                   case Keys.K: if (isShifted) { keyString = "K"; } else { keyString = "k"; } break;
                   case Keys.L: if (isShifted) { keyString = "L"; } else { keyString = "l"; } break;
                   case Keys.M: if (isShifted) { keyString = "M"; } else { keyString = "m"; } break;
                   case Keys.N: if (isShifted) { keyString = "N"; } else { keyString = "n"; } break;
                   case Keys.O: if (isShifted) { keyString = "O"; } else { keyString = "o"; } break;
                   case Keys.P: if (isShifted) { keyString = "P"; } else { keyString = "p"; } break;
                   case Keys.Q: if (isShifted) { keyString = "Q"; } else { keyString = "q"; } break;
                   case Keys.R: if (isShifted) { keyString = "R"; } else { keyString = "r"; } break;
                   case Keys.S: if (isShifted) { keyString = "S"; } else { keyString = "s"; } break;
                   case Keys.T: if (isShifted) { keyString = "T"; } else { keyString = "t"; } break;
                   case Keys.U: if (isShifted) { keyString = "U"; } else { keyString = "u"; } break;
                   case Keys.V: if (isShifted) { keyString = "V"; } else { keyString = "v"; } break;
                   case Keys.W: if (isShifted) { keyString = "W"; } else { keyString = "w"; } break;
                   case Keys.X: if (isShifted) { keyString = "X"; } else { keyString = "x"; } break;
                   case Keys.Y: if (isShifted) { keyString = "Y"; } else { keyString = "y"; } break;
                   case Keys.Z: if (isShifted) { keyString = "Z"; } else { keyString = "z"; } break;
 
                   // Decimal keys
                   case Keys.D0: if (isShifted) { keyString = ")"; } else { keyString = "0"; } break;
                   case Keys.D1: if (isShifted) { keyString = "!"; } else { keyString = "1"; } break;
                   case Keys.D2: if (isShifted) { keyString = "@"; } else { keyString = "2"; } break;
                   case Keys.D3: if (isShifted) { keyString = "#"; } else { keyString = "3"; } break;
                   case Keys.D4: if (isShifted) { keyString = "$"; } else { keyString = "4"; } break;
                   case Keys.D5: if (isShifted) { keyString = "%"; } else { keyString = "5"; } break;
                   case Keys.D6: if (isShifted) { keyString = "^"; } else { keyString = "6"; } break;
                   case Keys.D7: if (isShifted) { keyString = "&"; } else { keyString = "7"; } break;
                   case Keys.D8: if (isShifted) { keyString = "*"; } else { keyString = "8"; } break;
                   case Keys.D9: if (isShifted) { keyString = "("; } else { keyString = "9"; } break;
 
                   // Decimal numpad keys
                   case Keys.NumPad0: keyString = "0"; break;
                   case Keys.NumPad1: keyString = "1"; break;
                   case Keys.NumPad2: keyString = "2"; break;
                   case Keys.NumPad3: keyString = "3"; break;
                   case Keys.NumPad4: keyString = "4"; break;
                   case Keys.NumPad5: keyString = "5"; break;
                   case Keys.NumPad6: keyString = "6"; break;
                   case Keys.NumPad7: keyString = "7"; break;
                   case Keys.NumPad8: keyString = "8"; break;
                   case Keys.NumPad9: keyString = "9"; break;
                    
                   // Special keys
                   case Keys.OemTilde: if (isShifted) { keyString = "~"; } else { keyString = "`"; } break;
                   case Keys.OemSemicolon: if (isShifted) { keyString = ":"; } else { keyString = ";"; } break;
                   case Keys.OemQuotes: if (isShifted) { keyString = "\""; } else { keyString = "\'"; } break;
                   case Keys.OemQuestion: if (isShifted) { keyString = "?"; } else { keyString = "/"; } break;
                   case Keys.OemPlus: if (isShifted) { keyString = "+"; } else { keyString = "="; } break;
                   case Keys.OemPipe: if (isShifted) { keyString = "|"; } else { keyString = "\\"; } break;
                   case Keys.OemPeriod: if (isShifted) { keyString = ">"; } else { keyString = "."; } break;
                   case Keys.OemOpenBrackets: if (isShifted) { keyString = "{"; } else { keyString = "["; } break;
                   case Keys.OemCloseBrackets: if (isShifted) { keyString = "}"; } else { keyString = "]"; } break;
                   case Keys.OemMinus: if (isShifted) { keyString = "_"; } else { keyString = "-"; } break;
                   case Keys.OemComma: if (isShifted) { keyString = "<"; } else { keyString = ","; } break;
                   case Keys.Space: keyString = " "; break;                                       
            }
            return keyString;
         }

        public static void CheckKeys(float elapsed) // Checks if any keys are pressed/released and fires the associated event
        {
            Keys[] pressedKeys = Xin.GetPressedKeys(); // Keys pressed in current frame
            Keys[] lastPressedKeys = Xin.GetLastPressedKeys(); // Keys pressed in last frame
            bool lastisShifted = Xin.LastIsKeyDown(Keys.LeftShift) || Xin.LastIsKeyDown(Keys.RightShift); // Was shift down last frame?
            bool isShifted = Xin.IsKeyDown(Keys.LeftShift) || Xin.IsKeyDown(Keys.RightShift); // Is shift down this frame?

            foreach (Keys key in lastPressedKeys) 
            {
                if (!pressedKeys.Contains(key)) // If key was down last frame, then up this frame
                {
                    KeyReleased(key, lastisShifted, elapsed);
                }
            }
            foreach (Keys key in pressedKeys)
            {
                if (!lastPressedKeys.Contains(key)) // If key is down this frame, but not last frame
                {
                    KeyTyped(key, isShifted, elapsed);
                }
                KeyPressed(key, isShifted, elapsed);
            }
        }
        // 
    }
}
