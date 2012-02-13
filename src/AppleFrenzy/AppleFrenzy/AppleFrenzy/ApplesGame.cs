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
using AppleFrenzy;

namespace Apple01
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ApplesGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteManager spriteManager;

        enum GameState { Start, InGame, GameOver };
        GameState currentGameState = GameState.Start;

        public int currentScore = 0;
        public SpriteFont scoreFont;

        BackgroundSprite layer0, layer1, layer2;

        // Timer and Title Background
        SpriteFont timerFont;
        Texture2D TitleScreenLogo;
        public float gameTimer = 45;


        // Apple stuff
        AppleSprite fallingApple;

        
        public ApplesGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 680;
            graphics.PreferredBackBufferWidth = 1124;
        }

        public void AddScore(int score)
        {
            currentScore += score;
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
           
            // Load Initial screen apple
            fallingApple = new AppleSprite(Content.Load<Texture2D>(@"images\apple"), new Vector2(180, 180),
                            new Point(28, 32), 0, new Vector2(0, 2), Window.ClientBounds, 1f);
            // Load Title Screen Background
            TitleScreenLogo = Content.Load<Texture2D>(@"images\FrenzyTitleScreen");

            // Load Background: 
            layer0 = new BackgroundSprite(Content.Load<Texture2D>(@"Images\Layer0_0"),
                    new Point(800, 480), 1.418f);
            layer1 = new BackgroundSprite(Content.Load<Texture2D>(@"Images\Layer1_0"),
                    new Point(800, 480), 1.418f, 0.9f);
            layer2 = new BackgroundSprite(Content.Load<Texture2D>(@"Images\Layer2_0"),
                    new Point(800, 480), 1.418f, 0.8f);

            // Load Timer
            timerFont = Content.Load<SpriteFont>(@"fonts\score");
            
            
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
                    fallingApple.Update(gameTime, Window.ClientBounds);
                    if (fallingApple.position.Y > Window.ClientBounds.Height)
                        fallingApple.position.Y = -19;

                    if (Keyboard.GetState().GetPressedKeys().Length > 0)
                    {
                        currentGameState = GameState.InGame;
                        spriteManager.Enabled = true;
                        spriteManager.Visible = true;
                    }
                    break;
                case GameState.InGame:
                    // Handle timer
                    gameTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (gameTimer <= 0)
                        currentGameState = GameState.GameOver;
                    

                    break;
                case GameState.GameOver:
                    spriteManager.Enabled = false;
                    spriteManager.Visible = false;
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
                    GraphicsDevice.Clear(Color.White);

                    // Draw Text for intro splash screen
                    spriteBatch.Begin();

                    fallingApple.Draw(gameTime, spriteBatch);

                    spriteBatch.Draw(TitleScreenLogo, new Vector2(200, 200), Color.White);

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

                    // Draw Background: SpriteManager draws OVER this class, so need to draw BG here
                    layer0.Draw(gameTime, spriteBatch);
                    layer1.Draw(gameTime, spriteBatch);
                    //layer2.Draw(gameTime, spriteBatch);
                    // Draw the score
                    spriteBatch.DrawString(scoreFont, "Score: " + currentScore, new Vector2(10, 10),
                        Color.DarkBlue, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(timerFont, "Time: " + gameTimer.ToString("0.00"),
                                        new Vector2(985, 10), Color.Red);

                    spriteBatch.End();

                    break;
                    
                case GameState.GameOver:
                    GraphicsDevice.Clear(Color.AliceBlue);
                    // Draw Score at End Screen
                    spriteBatch.Begin();
                    spriteBatch.DrawString(scoreFont, "Score: " + currentScore, 
                        new Vector2(Window.ClientBounds.Width/2 - 50, Window.ClientBounds.Height/2 - 50),
                        Color.DarkBlue, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);

                    spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);
        }
    }
}
