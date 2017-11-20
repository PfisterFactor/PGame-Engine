// .NET stuff
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// XNA stuff
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PfisterGameEngine
{
    public class DrawingArguments // Contains arguments for SpriteBatch.Draw() method minus texture and bounds, as these are specified by the object
    {
        public Color DrawingColor
        {
            get;
            set;
        }
        public int Rotation
        {
            get;
            set;
        }
        public Vector2 Origin
        {
            get;
            set;
        }
        public SpriteEffects SpriteEffects
        {
            get;
            set;
        }
        public float Depth
        {
            get;
            set;
        }

        public DrawingArguments() // Basically a struct with parameters
        {
            DrawingColor = Color.White;
            Rotation = 0;
            Origin = Vector2.Zero;
            SpriteEffects = SpriteEffects.None;
            Depth = 1f;
        }
    }

    public class GameObject // Base class for all game-related things drawn
    {
        // Static Properties
        public static List<GameObject> Objects
        {
            get
            {
                return Entity.Entities.Cast<GameObject>().Union(UIObject.UIObjects).ToList(); // Casts Entities to an IEnumerable of Objects, then casts UIObjects as Objects and combines it with Entities. Finally, casts the IEnumerable as a list of objects and returns.
            }
        }
        // 

        // Backing Properties

        // 

        // Properties
        public AnimatedTexture Texture // Texture of object, used for drawing
        {
            get;
            set;
        }

        public Rectangle Bounds // Bounding box of object, used for game logic and drawing
        {
            get;
            set;
        }

        public Vector2 Position // Position of object, refers to bounds.X and bounds.Y, used for game logic and drawing
        {
            get { return new Vector2(Bounds.X, Bounds.Y); }
            set { Bounds = new Rectangle((int)value.X, (int)value.Y, Bounds.Width, Bounds.Height); }
        }
        
        public string InternalName // Internal name of object, is not shown to player
        {
            get;
            protected set;
        }
        
        public string Name // External name of object, is shown to player
        {
            get;
            protected set;
        }

        public bool Active  // If false, object is not drawn and event functions do not fire.
        {
            get;
            set;
        }
        public int Depth // Determines which sprite is drawn on top. Lower values are drawn on top.
        {
            get;
            set;
        }

        public DrawingArguments DrawArguments
        {
            get;
            set;
        }
        // 

        // Constructors/Deconstructors
        protected GameObject(string externalName, AnimatedTexture texture, Rectangle bounds, string internalName = "")
        {
            Name = externalName;
            Active = true;
            InternalName = (!string.IsNullOrEmpty(internalName)) ? internalName : "object" + (Entity.Entities.Count + UIObject.UIObjects.Count + 1); // If internal name is passed use it, otherwise use generated name
            Texture = texture;
            Bounds = bounds;
            Position = new Vector2(bounds.X, bounds.Y);
            DrawArguments = new DrawingArguments();

        }
        // Static Methods
        public static GameObject GetByIntName(string internalName)
        {
            GameObject returnObject = Objects.Find(x => x.InternalName == internalName);
            if (returnObject == null)
            {
                throw new KeyNotFoundException("Can't find object: (" + internalName + ")");
            }
            return returnObject;
        }
        // 

        // Methods
        public virtual void DrawObject(SpriteBatch spriteBatch, DrawingArguments drawingArguments, float elapsed) // If any object has a special drawing requirement they can override this method and specify
        {

            if (Texture.IsAnimated)
            {
                Texture.DrawAnimation(spriteBatch, Bounds, elapsed, drawingArguments);
            }
            else
            {
                Texture.DrawFrame(spriteBatch, Bounds, drawingArguments);
            }
            
        }
        // 
        

        
    }
}
