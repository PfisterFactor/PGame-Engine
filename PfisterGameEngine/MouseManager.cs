// .NET stuff
using System;
using System.Collections.Generic;
using System.Linq;
// XNA stuff
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
// Makes XNA input less horrible
using XInputManager;

namespace PfisterGameEngine
{
    public class MouseEventArgs : EventArgs {
        public MouseButton Button {
            get;
            protected set;
        }
        public Point MousePosition {
            get;
            protected set;
        }
        public MouseEventArgs(MouseButton button, Point mousePosition) {
            Button = button;
            MousePosition = mousePosition;
        }
    }
    public static class MouseManager
    {
        // Event Handling
        public static event EventHandler<MouseEventArgs> OnPress;
        public static event EventHandler<MouseEventArgs> OnReleased;

        private static void MousePressed(MouseButton pressed, Point pos)
        {
            OnPress?.Invoke(null, new MouseEventArgs(pressed, pos));
        }

        private static void MouseReleased(MouseButton released, Point pos)
        {
            OnReleased?.Invoke(null, new MouseEventArgs(released, pos));
        }

        // 

        // Static Methods
        public static void CheckMouse() // Checks if any mouse buttons are pressed/released and fires the associated event if neccesary
        {
            Point mousePos = Xin.MouseAsPoint;
            foreach (MouseButton mb in Enum.GetValues(typeof(MouseButton)))
            {
                if (Xin.IsMouseDown(mb))
                {
                    MousePressed(mb, mousePos);
                }
                if (Xin.CheckMousePress(mb))
                {
                    MouseReleased(mb, mousePos);
                }
            }
        }
        // 
    }
}
