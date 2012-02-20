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
        // XNA system variables for connecting to GPU and drawing textures to GPU
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Game Component for handling sprites
        SpriteManager spriteManager;

        enum GameState { Start, ControlScreen, AboutScreen, InGame, GameOver };
        GameState currentGameState = GameState.Start;

        public int currentScore = 0;
        public SpriteFont scoreFont;

        BackgroundSprite layer0, layer1, layer2;

        // Start Menu title
        Texture2D TitleScreenLogo;
        // Apple Controls title
        Texture2D ControlScreenLogo;
        // About screen title
        Texture2D AboutScreenLogo;
        // Game Over Title
        Texture2D EndScreenLogo;

        //Song (.mp3) file        
        SoundEffect FrenzyAudio;        
        SoundEffectInstance frenzyAudioInstance;

        // Apple stuff
        AppleSprite fallingApple;


        // Timer
        Timer timer;

        // Constructor 
        public ApplesGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 680;
            graphics.PreferredBackBufferWidth = 1124;
        }

        /// <summary>
        /// Update the score
        /// </summary>
        /// <param name="score"></param>
        public void AddScore(int score)
        {
            currentScore += score;
        }


        /// <summary>
        /// Signal end of In-game state
        /// </summary>
        public void EndGame()
        {
            currentGameState = GameState.GameOver;
        }

        /// <summary>
        /// Reset the game state
        /// </summary>
        void Reset()
        {
            // Disable Added GameComponent update and draw method
            spriteManager.Enabled = false;
            spriteManager.Visible = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                // Initialize timer
                timer.TimeRemaining = 60;
                // Initialize score
                currentScore = 0;
                // Initialize Game State
                currentGameState = GameState.Start;
                // Reset Sprite manager
                spriteManager.Reset();
            }
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

            // Initialize timer
            timer = new Timer(60);
            // Initialize score
            currentScore = 0;
            // Initialize Game State
            currentGameState = GameState.Start;
         
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

            // Load Background: 
            layer0 = new BackgroundSprite(Content.Load<Texture2D>(@"Images\Layer0_0"),
                    new Point(800, 480), 1.418f);
            layer1 = new BackgroundSprite(Content.Load<Texture2D>(@"Images\Layer1_0"),
                    new Point(800, 480), 1.418f, 0.9f);
            layer2 = new BackgroundSprite(Content.Load<Texture2D>(@"Images\Layer2_0"),
                    new Point(800, 480), 1.418f, 0.8f);

            // Load Timer
            //timerFont = Content.Load<SpriteFont>(@"fonts\score");
            timer.LoadContent(Content);
            // Load Title Screen Background
            TitleScreenLogo = Content.Load<Texture2D>(@"images\FrenzyTitleScreen");
            // Load Game Controls title
            ControlScreenLogo = Content.Load<Texture2D>(@"images\appleControls");
            // Load About Screen title
            AboutScreenLogo = Content.Load<Texture2D>(@"images\aboutApple");
            // Load Game Over Screen title
            EndScreenLogo = Content.Load<Texture2D>(@"images\appleGameOver");

            //Load Sound Effect           
            FrenzyAudio = Content.Load<SoundEffect>(@"Sounds\Background");            
            frenzyAudioInstance = FrenzyAudio.CreateInstance();            
            frenzyAudioInstance.IsLooped = true;  
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        // Set state of game and do proper initialization
        void setGameState(GameState state)
        {
            if (state == GameState.InGame)
            {
                spriteManager.Enabled = true;
                spriteManager.Visible = true;
            }
            else
            {
                spriteManager.Enabled = false;
                spriteManager.Visible = false;
            }
            currentGameState = state;
            
        }

        void startGame()
        {
            setGameState(GameState.InGame);
        }

        void controlGame()
        {
            setGameState(GameState.ControlScreen);     
        }

        void aboutGame()
        {
            setGameState(GameState.AboutScreen);
        }

        void returnHome()
        {
            setGameState(GameState.Start);
        }


        void handleInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                startGame();
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                controlGame();
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                aboutGame();
            if (Keyboard.GetState().IsKeyDown(Keys.B))
                returnHome();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
        }

        void checkForEnd()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.B))
                returnHome();
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

                    // Start menu options:
                    // Enter - Start game, Space - Controls, A - About Game
                    handleInput();
                    break;

                case GameState.ControlScreen:
                    // player views keyboard configuration
                    handleInput();
                    break;

                case GameState.AboutScreen:
                    // player views extraneous game information
                    handleInput();   // press b or B to return to start menu
                    break;

                case GameState.InGame:
                    // Plays background music                     
                    frenzyAudioInstance.Play();
                    // Handle timer
                    timer.Update(gameTime);
                    if (timer.EndTime())
                        currentGameState = GameState.GameOver;
                    break;
                case GameState.GameOver:
                    // Stop background music                     
                    frenzyAudioInstance.Stop();
                    // Reset game state
                    Reset();
                    checkForEnd();
                    break;
            }
            base.Update(gameTime);
        }

        // Draw String to Display
        void DisplayCenteredString(SpriteBatch spriteBatch, String message, int yOffset)
        {
            spriteBatch.DrawString(scoreFont, message, new Vector2((Window.ClientBounds.Width / 2)
                        - (scoreFont.MeasureString(message).X / 2), (Window.ClientBounds.Height / 2)
                        - (scoreFont.MeasureString(message).Y / 2) + yOffset), Color.SaddleBrown);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            string text;
            switch (currentGameState)
            {
                case GameState.Start:
                    GraphicsDevice.Clear(Color.AliceBlue);

                    // Draw text for intro splash screen
                    spriteBatch.Begin();
                    spriteBatch.Draw(TitleScreenLogo, new Vector2(225, 150), Color.White);
                    fallingApple.Draw(gameTime, spriteBatch);

                    text = "Press Enter to START";
                    DisplayCenteredString(spriteBatch, text, 30);

                    text = "Press Space for Game Controls";
                    DisplayCenteredString(spriteBatch, text, 60);

                    text = "Press A for Information about the Game";
                    DisplayCenteredString(spriteBatch, text, 90);

                    text = "Press Escape to Exit";
                    DisplayCenteredString(spriteBatch, text, 120);

                    spriteBatch.End();
                    break;

                case GameState.ControlScreen:
                    GraphicsDevice.Clear(Color.White);

                    // Display control screen logo
                    spriteBatch.Begin();
                    spriteBatch.Draw(ControlScreenLogo, new Vector2(25, 50), Color.White);
                    text = "The UP directional arrow allows the character to JUMP.";
                    DisplayCenteredString(spriteBatch, text, -30);

                    text = "The RIGHT directional arrow allows the character to move RIGHT.";
                    DisplayCenteredString(spriteBatch, text, 0);

                    text = "The LEFT directional arrow allows the character to move LEFT.";
                    DisplayCenteredString(spriteBatch, text, 30);

                    text = "*Jumping to catch the apples will earn you DOUBLE points!";
                    DisplayCenteredString(spriteBatch, text, 60);

                    text = "(Press B to return to the Start Menu)";
                    DisplayCenteredString(spriteBatch, text, 120);

                    spriteBatch.End();
                    break;

                case GameState.AboutScreen:
                    GraphicsDevice.Clear(Color.White);

                    // Display about screen title
                    spriteBatch.Begin();
                    spriteBatch.Draw(AboutScreenLogo, new Vector2(25, 50), Color.White);
                    
                    text = "Development by: Ernesto Pavon, Corbin Benally & Alex Solis";
                    DisplayCenteredString(spriteBatch, text, -30);

                    text = "Tools:                                   ";
                    DisplayCenteredString(spriteBatch, text, 0);

                    text = " Microsoft Visual Studio 2010";
                    DisplayCenteredString(spriteBatch, text, 30);

                    text = " XNA 4.0 framework and C#    ";
                    DisplayCenteredString(spriteBatch, text, 60);

                    text = " Github                      ";
                    DisplayCenteredString(spriteBatch, text, 90);
                    
                    text = "(Press B to return to the Start Menu)";
                    DisplayCenteredString(spriteBatch, text, 150);

                    spriteBatch.End();
                    break;

                case GameState.InGame:
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Begin();

                    // Draw Background: SpriteManager draws OVER this class, so need to draw BG here
                    layer0.Draw(gameTime, spriteBatch);
                    layer1.Draw(gameTime, spriteBatch);
                    //layer2.Draw(gameTime, spriteBatch); // Enable this to allow a different look

                    // Draw the score
                    spriteBatch.DrawString(scoreFont, "Score: " + currentScore, new Vector2(10, 10),
                        Color.DarkBlue, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    
                    // Draw the time remaining
                    timer.Draw(spriteBatch);
                    spriteBatch.End();

                    break;
                    
                case GameState.GameOver:
                    GraphicsDevice.Clear(Color.AliceBlue);
                    // Draw Score at End Screen
                    spriteBatch.Begin();
                    spriteBatch.Draw(EndScreenLogo, new Vector2(225, 150), Color.White);
                    
                    DisplayCenteredString(spriteBatch, "Score: " + currentScore, -50);

                    text = "(Press Enter key to try again)";
                    DisplayCenteredString(spriteBatch, text, 30);
                    
                    text = "(Press B to return to Start Menu)";
                    DisplayCenteredString(spriteBatch, text, 60);

                    text = "(Press Escape to Exit)";
                    DisplayCenteredString(spriteBatch, text, 90);
                    

                    spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);
        }
    }
}