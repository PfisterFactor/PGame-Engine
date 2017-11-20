// .NET stuff
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// XNA stuff
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace PfisterGameEngine
{
    public class UIButtonEventArgs : EventArgs // Used for switch event isClicked
    {
        // Properties
        public enum ButtonType { UIButton, UISwitch };

        public bool State // Used only for UISwitch, is switch on or off?
        {
            get;
            private set;
        }
        // 

        public UIButtonEventArgs(bool state)
        {
            State = state;
        }
    }
    public abstract class UIObject : GameObject // Base UI Object
    {
        // Static Properties
        public static List<UIObject> UIObjects = new List<UIObject>();
        public const int DefaultDepth = 1; // The default depth for UIObjects if none is specified
        // 

        // Constructors/Destructors
        protected UIObject(string externalName, Rectangle bounds, AnimatedTexture texture, string internalName = "", int depth = DefaultDepth)
            : base(externalName, texture, bounds, internalName)
        {
            MouseManager.OnReleased += OnClick; // Subscribes onClick to our MouseManager
            Depth = depth;
            DrawArguments.Depth = 1f / depth;
            UIObjects.Add(this);
        }
        // 

        // Event Handling
        protected virtual void OnClick(object sender, MouseEventArgs e) { } // Method to be overrided in inherited classes. Fired on event onMouseReleased
        // 

        // Static Methods
        public static void InitializedAndLoadUIObjects(ContentManager content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content), "Content Manager cannot be null!");
            }
            UIText.DefaultFont = content.Load<SpriteFont>(@"Art\UI\Default");
            AnimatedTexture muteButtonTexture = new AnimatedTexture(content.Load<Texture2D>(@"Art\UI\Mute Button"), 64, 64, false);
            AnimatedTexture backgroundUITexture = new AnimatedTexture(content.Load<Texture2D>(@"Art\UI\Background UI"));


            UISwitch muteButton = new UISwitch("Mute Button", muteButtonTexture, new Rectangle(760, 8, 32, 32), "UI_button_mute", 1);
            UIImage hudBackground = new UIImage("HUD Background", backgroundUITexture, new Rectangle(0, 0, 800, 100), "UI_image_hudbackground", 2);
            UIText testText = new UIText("Test Text", "Hello! I am a test of the new String drawing!", new Vector2(16, 48), 1, 50, "UI_text_debugtext",1);
            UIText fpsText = new UIText("FPS", "aaaa", new Vector2(8, 8), 1f, 20, "UI_text_fps", 1);
            fpsText.DrawArguments.DrawingColor = Color.White;
            testText.IsDynamic = true;
        }
        // 

        // Methods
        public void Remove()// Removes current UIObject from UIObjects
        {
            UIObjects.Remove(this);
        } 
        // 
    }
    public class UIButton : UIObject // Class for button in UI, fires isClicked event when clicked on.
    {
        // Constructors/Deconstructors
        public UIButton(string externalName, AnimatedTexture texture, Rectangle bounds, string internalName = "", int depth = DefaultDepth)
            : base(externalName, bounds, texture, internalName, depth)
        {
            // Do nothing, base constructor takes care of everything
        }
        // 

        // Event Handling
        public virtual event EventHandler<UIButtonEventArgs> IsClicked; // Fires when clicked

        protected virtual void Click() // Null checks and executes isClicked event
        {
            if (IsClicked != null)
            {
                IsClicked(this, new UIButtonEventArgs(false));
            }
        }
        protected override void OnClick(object sender, MouseEventArgs e)
        {
            if (!Active) return;
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e), "MouseEventArgs cannot be null!");
            }
            if (Bounds.Contains(e.MousePosition)) // Is the UIObject being clicked on?
            {
                Click();
            }
        }
        // 


    }
    public class UISwitch : UIButton // Class for switch in UI, toggles property state true and false when clicked on and fires isClicked event
    {
        
        // Properties
        public bool State
        {
            get;
            set;
        }
        // 

        // Constructors/Deconstructors
        public UISwitch(string externalName, AnimatedTexture texture, Rectangle bounds, string internalName = "", int depth = DefaultDepth)
            : base(externalName, texture, bounds, internalName, depth)
        {
            Bounds = bounds;
            Texture = texture; // Use first texture for starting texture
            State = false;
        }
        // 

        // Event Handling
        public override event EventHandler<UIButtonEventArgs> IsClicked;
        protected override void Click()
        {
            if (IsClicked != null)
            {
                IsClicked(this, new UIButtonEventArgs(State));
            }
        }
        protected override void OnClick(object sender, MouseEventArgs e)
        {
            if (!Active) return;
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e), "MouseEventArgs cannot be null!");
            }
            if (Bounds.Contains(e.MousePosition)) // Is the UIObject being clicked on?
            {
                State = !State;
                Click();
            }
        }
        // 

        // Methods
        public override void DrawObject(SpriteBatch spriteBatch, DrawingArguments drawingArguments, float elapsed)
        {
            if (!Active) return; // Dont draw object if its not active
            int frame = (State) ? 1 : 0;
            Texture.DrawFrame(spriteBatch, Bounds, frame, drawingArguments);
        }
        // 


    }
    public class UIText : UIObject
    {
        // Static Properties
        public static SpriteFont DefaultFont;
        // 

        // Backing properties
        private string _text;
        // 

        // Properties
        public string Text // Current text being displayed
        {
            set { _text = (value.Length <= MaxLength) ? value : _text; }
            get { return _text; }
        }
        public new Vector2 Position
        {
            get;
            set;
        }
        public new Rectangle Bounds // Bounds of UIText object. Cannot be set because of how XNA handles string drawing. Is instead resized to meet font size and text length.
        {
            get
            {
                if (Text == null) return Rectangle.Empty;
                Vector2 a = Font.MeasureString(new string('W', MaxLength)) * Scale;
                return new Rectangle((int)Position.X, (int)Position.Y, (int)a.X, (int)a.Y);
            }
        }
        public bool IsBeingEdited // Is the object currently being edited?
        {
            protected set;
            get;
        }
        public float Scale // Scale of object when drawn, default (1) is font size 14
        {
            protected set;
            get;
        }
        public SpriteFont Font
        {
            get;
            protected set;
        }

        public int MaxLength  // Max length in characters of text
        {
            get;
            set;
        } 
        public bool IsDynamic // Can the object be edited?
        {
            get;
            set;
        }

        private const float keyDelay = 0.5f; // Delay between key pressed and when to repeat characters
        private const float keySpeed = 0.03f; // How fast characters are repeated
        private const float blinker = 0.53f;
        private float keyDelayTimer = 0f; // Timer for our key delay.
        private float keySpeedTimer = 0f; // Timer for our keyspeed
        private float blinkerTimer = 0f;
        // 

        // Constructors/Destructors
        public UIText(string externalName, string text, Vector2 position, float scale = 1.0f, int maxLength = -1, string internalName = "", int depth = DefaultDepth)
            : base(externalName, Rectangle.Empty, null, internalName, depth)
        {
            Font = DefaultFont;
            MaxLength = (maxLength != -1) ? maxLength : text.Length; // If maxLength is specified, use it. Otherwise set it to passed text length
            Text = (text.Length <= maxLength) ? text : text.Substring(0, maxLength); // If passed text is less-than or equal to passed maxLength, use it. Otherwise cut it to fit
            Position = position;
            Scale = scale;
            IsDynamic = false;
            DrawArguments.DrawingColor = Color.Black;
        }
        // 

        // Event Handling
        protected override void OnClick(object sender, MouseEventArgs e)
        {
            if (!Active) return;
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e), "MouseEventArgs cannot be null!");
            }
            if (!(e.Button == XInputManager.MouseButton.Left)) return; // If mouse button clicked isn't left mouse button, do nothing

            if (Bounds.Contains(e.MousePosition)) // Is the UIObject being clicked on?
            {
                if (IsDynamic) // If object is editable
                {
                    if (IsBeingEdited == false) // If the object isn't currently being edited so we don't subscribe to the same event twice
                    {
                        KeyboardManager.OnKeyTyped += OnType; // Subscribe to the onKeyTyped event
                        KeyboardManager.OnKeyPressed += OnKeyHeld;
                        IsBeingEdited = true; 
                    }
                }
                
            }
            else
            {
                if (IsDynamic) // If object is editable
                {
                    if (IsBeingEdited == true) // If the object isn't currently being edited
                    {
                        KeyboardManager.OnKeyTyped -= OnType; // Unsubscribe to onKeyTyped event
                        KeyboardManager.OnKeyPressed -= OnKeyHeld;
                        blinkerTimer = 0f;
                        IsBeingEdited = false;
                    }
                }
                    
            }
            
        }
        protected void OnKeyHeld(object sender, KeyboardEventArgs e)
        {
            if (!Active) return;
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e), "KeyboardEventArgs cannot be null");
            }
            blinkerTimer = 0.53f;
            if (keyDelayTimer >= keyDelay)
            {
                if (keySpeedTimer >= keySpeed)
                {
                    keySpeedTimer = 0f;
                    if (e.Key == Keys.Back)
                    {
                        Text = (Text.Length > 0) ? Text.Substring(0, Text.Length - 1) : Text; // If text length is greated than zero, remove a letter. Otherwise keep it the same
                    }
                    else
                    {
                        string key = KeyboardManager.GetKeyString(e.Key, e.IsShifted); // Get character value for key, taking capitalization into consideration
                        Text += key; // Append the typed character to text
                    }
                }
                else
                {
                    keySpeedTimer += e.Elapsed;
                }
            }
            else
            {
                keyDelayTimer += e.Elapsed;
            }
        }
        protected void OnType(object sender, KeyboardEventArgs e) // Fires every time onKeyType is called
        {
            if (!Active) return;
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e), "KeyboardEventArgs cannot be null!");
            }
            blinkerTimer = 0;
            keyDelayTimer = 0;
            if (e.Key != Keys.Back) // If key isn't backspace
            {
                string key = KeyboardManager.GetKeyString(e.Key, e.IsShifted); // Get character value for key, taking capitalization into consideration
                Text += key; // Append the typed character to text
            }
            else
            {
                Text = (Text.Length > 0) ? Text.Substring(0, Text.Length - 1) : Text; // If text length is greated than zero, remove a letter. Otherwise keep it the same
            }
        }
        // 

        // Methods
        public override void DrawObject(SpriteBatch spriteBatch, DrawingArguments drawingArguments, float elapsed)
        {
            if (!Active) return; // Dont draw object if its not active
            if (spriteBatch == null)
            {
                throw new ArgumentNullException(nameof(spriteBatch), "SpriteBatch cannot be null!");
            }
            string textToDraw = Text;
            if (IsBeingEdited)
            {
                if (blinkerTimer >= blinker)
                {
                    if (blinkerTimer >= blinker * 2)
                    {
                        blinkerTimer = 0;
                    }
                    else
                    {
                        textToDraw += "_";
                    }
                    
                }
                blinkerTimer += elapsed;
            }
            spriteBatch.DrawString(Font,
                textToDraw,
                Position,
                DrawArguments.DrawingColor,
                DrawArguments.Rotation,
                DrawArguments.Origin,
                Scale, DrawArguments.SpriteEffects,
                1.0f / Depth);
        }
        // 
        
    }
    public class UIImage : UIObject
    {
        // Properties

        // 

        public UIImage(string externalName, AnimatedTexture texture, Rectangle bounds, string internalName = "", int depth = DefaultDepth)
            : base(externalName, bounds, texture, internalName, depth)
        {
            // Do nothing, base constructor takes care of everything
        }

        // Methods

        // 
    }
}
