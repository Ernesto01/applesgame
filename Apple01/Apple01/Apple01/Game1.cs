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

namespace Apple01
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteManager spriteManager;

        enum GameState { Start, InGame, GameOver };
        GameState currentGameState = GameState.Start;

        int currentScore = 0;
        SpriteFont scoreFont;

        // Apple stuff
        Texture2D apple;
        Vector2 applePos;
        float appleSpeed = 1.75f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);

            // Disable Added GameComponent update and draw method
            spriteManager.Enabled = false;
            spriteManager.Visible = false;

            applePos = new Vector2(150, 150);

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
            scoreFont = Content.Load<SpriteFont>(@"fonts\score");
            apple = Content.Load<Texture2D>(@"images\apple");
            
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            switch (currentGameState)
            {
                case GameState.Start:
                    // Move the apple
                    applePos.Y += appleSpeed;
                    if (applePos.Y > Window.ClientBounds.Height /* - apple.Height*/)
                        applePos.Y = -(apple.Height);
                    if (Keyboard.GetState().GetPressedKeys().Length > 0)
                    {
                        currentGameState = GameState.InGame;
                        spriteManager.Enabled = true;
                        spriteManager.Visible = true;
                    }
                    break;
                case GameState.InGame:

                    break;
                case GameState.GameOver:

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
            GraphicsDevice.Clear(Color.White);

            switch (currentGameState)
            {
                case GameState.Start:
                    GraphicsDevice.Clear(Color.AliceBlue);

                    // Draw Text for intro splash screen
                    spriteBatch.Begin();
                    spriteBatch.Draw(apple,
                        applePos,
                        Color.White);

                    string text = "Catch as many apples as you can before timer runs out";
                    spriteBatch.DrawString(scoreFont, text, new Vector2((Window.ClientBounds.Width / 2)
                        - (scoreFont.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2) - (scoreFont.MeasureString(text).Y / 2)),
                        Color.SaddleBrown);
                    text = "(Press any key to begin)";
                    spriteBatch.DrawString(scoreFont, text, new Vector2((Window.ClientBounds.Width / 2)
                        - (scoreFont.MeasureString(text).X / 2), (Window.ClientBounds.Height / 2)
                        - (scoreFont.MeasureString(text).Y / 2) + 30), Color.SaddleBrown);

                    spriteBatch.End();
                    break;

                case GameState.InGame:
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Begin();
                    spriteBatch.DrawString(scoreFont, "Score: " + currentScore, new Vector2(10, 10),
                        Color.DarkBlue, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    spriteBatch.End();

                    break;
                    
                case GameState.GameOver:

                    break;
            }

            base.Draw(gameTime);
        }
    }
}
