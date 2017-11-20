// .NET stuff
using System;
using System.Collections.Generic;
using System.Linq;
// XNA stuff
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
// Makes XNA input less horrible
using XInputManager;

namespace PfisterGameEngine
{


    public class MainGame : Game
    {

        // Properties
        GraphicsDeviceManager graphics; // Manages our graphics card
        SpriteBatch spriteBatch; // Used to draw sprites to screen
        Xin xinInstance; // Part of XInputManager, used to make XNA input less horrible
        float elapsed;
        private float fps;
        // 

        // Constructors/Destructors
        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content"; // Sets root directory for assets
            xinInstance = new Xin(this); // Creates a new instance of our xin input
            Components.Add(xinInstance); // Adds xin to our components to load

        }
        // 


        // Methods
        protected override void Initialize() // Intialize everything
        {
            base.Initialize(); // Enumerates through Components and calls their Intialize method

            IsMouseVisible = true; // Mouse is now displayed in window

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 800; // Window size X
            graphics.PreferredBackBufferHeight = 600; // Window size Y
            graphics.ApplyChanges();

            a.Add(new Vector2(400, 300));
            a.Add(new Vector2(394, 230));
            a.Add(new Vector2(600, 250));
            GameLogic.StartBackgroundMusic();
            ((UISwitch)GameObject.GetByIntName("UI_button_mute")).IsClicked += ToggleBackgroundMusic;


        }

        protected override void LoadContent() // Load everything
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
           
            Entity.InitializeAndLoadEntities(Content);
            UIObject.InitializedAndLoadUIObjects(Content);


            try // If user has no audio device this line will fail and be caught
            {
                GameLogic.SetBackgroundMusic(Content.Load<SoundEffect>(@"Music\Mega Rust")); 
            }
            catch (NoAudioHardwareException e)
            {
                Console.WriteLine("Error initializing audio!: " + e.Message);
            }


        }

        protected override void UnloadContent() // Unload any non-content-manager stuff
        {

        }


        public void ToggleBackgroundMusic(object sender, UIButtonEventArgs e)
        {
            if (sender.GetType().Name == "UISwitch")
                GameLogic.PauseBackgroundMusic(e.State);
        }
        void PositionUpdate() // Updates Entities positions according to velocity and acceleration
        {
            //Stub
        }
        protected override void Update(GameTime gameTime) // Called every frame. Used for game logic
        {
            elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds; // Get time in seconds from start of last frame
            fps = 1f/elapsed;
            ((UIText)UIObject.GetByIntName("UI_text_fps")).Text = "FPS: " + ((int)(fps+.01)).ToString();
            MouseManager.CheckMouse(); // Check if any mouse buttons are pressed/released and fire the appropriate event in MouseManager.cs
            KeyboardManager.CheckKeys(elapsed); // Check if any keyboard keys are pressed/released and fire the appropriate event in KeyboardManager.cs
            PositionUpdate(); // Update entities' positions

            base.Update(gameTime);
        }


        private List<Vector2> a = new List<Vector2>();
        private void DrawBorder(Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)// Draws a border around a rectangle. For debugging usage
        {
            Texture2D pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color); // Create new 1x1 texture\
            pixel.SetData(new[] { borderColor }); // Set the 1x1 texture to green

            // Draw top line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), null, borderColor, 0, Vector2.Zero, SpriteEffects.None, 1);

            // Draw left line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), null, borderColor, 0, Vector2.Zero, SpriteEffects.None, 1);

            // Draw right line
            spriteBatch.Draw(pixel, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
                                            rectangleToDraw.Y,
                                            thicknessOfBorder,
                                            rectangleToDraw.Height), null, borderColor, 0, Vector2.Zero, SpriteEffects.None, 1);
            // Draw bottom line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X,
                                            rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                                            rectangleToDraw.Width,
                                            thicknessOfBorder), null, borderColor, 0, Vector2.Zero, SpriteEffects.None, 1);
        }

        public  void DrawLineTo(SpriteBatch sb, Texture2D texture, Vector2 src, Vector2 dst, Color color)
        {
            //direction is destination - source vectors
            Vector2 direction = dst - src;
            //get the angle from 2 specified numbers (our point)
            var angle = (float)Math.Atan2(direction.Y, direction.X);
            //calculate the distance between our two vectors
            float distance;
            Vector2.Distance(ref src, ref dst, out distance);

            //draw the sprite with rotation
            sb.Draw(texture, src, new Rectangle((int)src.X, (int)src.Y, (int)distance, 1), Color.White, angle, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }

        public void DrawLines(List<Vector2> lines)
        {
            Texture2D pixel = new Texture2D(GraphicsDevice, 1, 1);
            Color[] pixelArray = { Color.Green };
            pixel.SetData(pixelArray);
            
            for (int i = 0; i < lines.Count; i++)
            {
                Vector2 a = lines.ElementAtOrDefault(i);
                Vector2 b = lines.ElementAtOrDefault(i + 1);

                if (b == default(Vector2))
                {
                    DrawLineTo(spriteBatch, pixel, a, a, Color.Green);
                }
                else
                {
                    DrawLineTo(spriteBatch, pixel, a, b, Color.Green);
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue); // Clear the screen and set the background to blue

            int viewportWidth = GraphicsDevice.Viewport.Bounds.Width; // Get our viewport width  (typically 800)
            int viewportHeight = GraphicsDevice.Viewport.Bounds.Height; // Get our viewport height (typically 600)



            base.Draw(gameTime); // Tells game when to draw frame. Yeah. I don't really know

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend); // Begin specifying objects to draw, sorting them by FrontToBack (lower sprite depths have priority), and blended with AlphaBlend (which I think justs makes transparency available)

            DrawLines(a);
            //Delete this code
            Texture2D pixel = new Texture2D(GraphicsDevice, 1, 1);
            Color[] pixelArray = { Color.Green };
            pixel.SetData(pixelArray);
            MouseState s = Mouse.GetState();
            DrawLineTo(spriteBatch, pixel, new Vector2(0, 0), new Vector2(s.X, s.Y), Color.Green);
            DrawLineTo(spriteBatch, pixel, new Vector2(800, 600), new Vector2(s.X, s.Y), Color.Green);
            DrawLineTo(spriteBatch, pixel, new Vector2(0, 600), new Vector2(s.X, s.Y), Color.Green);
            DrawLineTo(spriteBatch, pixel, new Vector2(800, 0), new Vector2(s.X, s.Y), Color.Green);
            //Done deleting code
            foreach (GameObject obj in GameObject.Objects)
            {
                obj.DrawObject(spriteBatch, obj.DrawArguments, elapsed);
                DrawBorder(obj.Bounds, 1, Color.Green);
            }
            
            spriteBatch.End(); // End specifying objects to draw and actually draw the objects to screen
           
        }
    }
    // 
}
