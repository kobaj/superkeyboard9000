using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Keyboard;

namespace TestGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public enum KeyboardType { Default, Super, None }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;
        KeyboardType keyboardType;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Components.Add(new GamerServicesComponent(this));
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            keyboardType = KeyboardType.None;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("Background");
            KeyboardManager.Initialize(Content);
            

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardManager.Update(gameTime);

            switch (keyboardType)
            {
                case KeyboardType.None:
                    if (InputManager.BackPressed(PlayerIndex.One))
                        this.Exit();
                    if (InputManager.APressed(PlayerIndex.One))
                    {
                        KeyboardManager.ShowKeyboard();
                        keyboardType = KeyboardType.Super;
                    }
                    if (InputManager.XPressed(PlayerIndex.One))
                    {
                        if (!Guide.IsVisible)
                        {
                            Guide.BeginShowKeyboardInput(PlayerIndex.One, "Microsoft Keyboard", "Test the defualt Microsoft Keyboard", "", null, null);
                            keyboardType = KeyboardType.Default;
                        }
                    }
                    break;
                case KeyboardType.Default:
                    if (!Guide.IsVisible)
                        keyboardType = KeyboardType.None;
                    break;
                case KeyboardType.Super:
                    if(!KeyboardManager.IsActive)
                        keyboardType = KeyboardType.None;
                    if (InputManager.BPressed(PlayerIndex.One))
                        KeyboardManager.HideKeyboard();
                    break;
            }
                
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            KeyboardManager.Draw(spriteBatch);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
