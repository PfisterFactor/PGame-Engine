// .NET stuff
using System;
using System.Collections.Generic;
// XNA stuff
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PfisterGameEngine
{

    public class Entity : GameObject // Base class for all non-UI objects. 
    {
        // Static Properties
        public static List<Entity> Entities = new List<Entity>();
        public const int DefaultDepth = 101; // The default depth for Entity if none is specified
        // 

        // Constructors/Deconstructors
        public Entity(string externalName, AnimatedTexture texture, Rectangle bounds, string internalName = "", int depth = DefaultDepth)
            : base(externalName, texture, bounds, internalName)
        {
            Depth = depth;
            DrawArguments.Depth = 1f / depth;
            Entities.Add(this);

        }

        // 

        // Static Methods
        public static void InitializeAndLoadEntities(ContentManager content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content),"Content Manager cannot be null!");
            }

            AnimatedTexture groundTex = new AnimatedTexture(content.Load<Texture2D>(@"Art\Entities\Ground"));
            AnimatedTexture testPlayerTex = new AnimatedTexture(content.Load<Texture2D>(@"Art\Entities\ExtractFrameTest"), 50, 50, true, 0.1f);

            Entity ground = new Entity("Ground", groundTex, new Rectangle(0, 550, 800, 50), "entity_ground");
            Entity testPlayer = new Entity("Test Player", testPlayerTex, new Rectangle(40, 486, 64, 64), "test_player");
            
        }
        // 

        // Methods
        public void Remove() // Removes current Entity from Entities
        {
            Entities.Remove(this);
        }
        // 
        
        
    }
}
