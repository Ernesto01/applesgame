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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public const int GROUND_LEVEL = 580;
        // System provided variable for drawing to GPU
        SpriteBatch spriteBatch;
        // Sprites
        UserControlledSprite player;
        List<Sprite> apples = new List<Sprite>();
        BeeSprite bee1, bee2;
        List<Sprite> lives = new List<Sprite>();
        BirdSprite bird;
        SoundEffect beeHit;
        SoundEffect appleCollected;
        SoundEffect deadBird;
        

        public SpriteManager(Game game) : base(game)
        {
         
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {

            base.Initialize();
        }

        /// <summary>
        /// Reset game state to initial state
        /// </summary>
        public void Reset()
        {   // This is a point of possible performance optimization by NOT allocating new memory
            // when resetting, if you think about porting to WP, change this, you will need
            // to add initialization routines to several sprite ADTs. Memory operations are 
            // more expensive than CPU operations. 
            apples = new List<Sprite>();
            lives = new List<Sprite>();
            LoadContent();
        }

        protected override void LoadContent()
        {
        
            // Initialize spriteBatch object to correct GPU
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            // Load Life sprite - Player always starts with 3 lives
            lives.Add(new HudSprite(Game.Content.Load<Texture2D>(@"Images/Heart"),
                        new Vector2(20, 30), new Point(101, 171), 0.19f));
            lives.Add(new HudSprite(Game.Content.Load<Texture2D>(@"Images/Heart"),
                        new Vector2(40, 30), new Point(101, 171), 0.19f));
            lives.Add(new HudSprite(Game.Content.Load<Texture2D>(@"Images/Heart"),
                        new Vector2(60, 30), new Point(101, 171), 0.19f));

            // Load bees sprites
            bee1 = new BeeSprite(Game.Content.Load<Texture2D>(@"Images/Bee1"),
                        new Vector2(600, GROUND_LEVEL+5), new Point(24,24), 5, new Point(0,0), 
                        new Point(3,1), new Vector2(-3,0), 1f);
            bee2 = new BeeSprite(Game.Content.Load<Texture2D>(@"Images/Bee2"),
                        new Vector2(900, GROUND_LEVEL + 35), new Point(24, 24), 5, new Point(0, 0),
                        new Point(3, 1), new Vector2(-2, 0), 1f);

            // Load bird sprite
            bird = new BirdSprite(Game.Content.Load<Texture2D>(@"Images/Bird5"),
                        new Vector2(500, 530), new Point(47, 44), 5, new Point(0, 0),
                        new Point(9, 1), new Vector2(0, 0), 1.17f);

            // Load player controlled character
            player = new UserControlledSprite(Game.Content.Load<Texture2D>(@"Images/Idle"),
                        new Vector2(0,GROUND_LEVEL), new Point(64, 64), 10, new Point(0, 0), new Point(1, 1),
                        new Vector2(6, 6), 1f);
            player.Initialize(Game.Services);

            // Load Apples
            loadApples();


            // Load Sound Effects
            beeHit = Game.Content.Load<SoundEffect>(@"Sounds\Hit3");
            appleCollected = Game.Content.Load<SoundEffect>(@"Sounds\AppleCollected");
            deadBird = Game.Content.Load<SoundEffect>(@"Sounds\Bird03");

            base.LoadContent();
        }

        /// <summary>
        /// Loads apples when starting a game and when
        /// there are only two remaining apples on the screen
        /// </summary>
        private void loadApples()
        {
            for (int i = 0; i < 10; ++i)
                apples.Add(new AppleSprite(Game.Content.Load<Texture2D>(@"Images\Apple"),
                    new Point(28, 32), 5, new Vector2(0, 2), Game.Window.ClientBounds, 0.60f));
        }

        // Check for apple collisions
        void AppleCollision(Sprite sprite, ref int i)
        {
            // Check for collisions
            if (sprite.collisionRect.Intersects(player.collisionRect))
            { 
                if (sprite.collisionRect.Bottom < GROUND_LEVEL + 65)
                    ((ApplesGame)Game).AddScore(4);
                else
                    ((ApplesGame)Game).AddScore(2);

                // Play Apple Collected Sound
                appleCollected.Play();

                // Remove apple from apples list
                apples.RemoveAt(i);
                --i;

                // Check to see if we need to drop more apples
                if (apples.Count <= 2)
                    loadApples();
            }
            if (sprite.collisionRect.Bottom >= GROUND_LEVEL + 78)
                sprite.stopVerticalMovement();
            
        }

        void onPlayerHit()
        {
            player.Reset(new Vector2(0, GROUND_LEVEL));
            if (lives.Count != 0)
                lives.RemoveAt(lives.Count - 1);
            if (lives.Count == 0)
                player.IsAlive = false;
            ((ApplesGame)Game).AddScore(-4);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime, Game.Window.ClientBounds);

            // Update bees
            bee1.Update(gameTime, Game.Window.ClientBounds);
            bee2.Update(gameTime, Game.Window.ClientBounds);

            // update birds
            if (bird.IsAlive)
            {
                bird.Update(gameTime, Game.Window.ClientBounds);

                // Check for bird collision detection
                if (bird.collisionRect.Intersects(player.collisionRect))
                {
                    if (Math.Abs(player.collisionRect.Bottom - bird.collisionRect.Top) <= 3f)
                    {
                        bird.IsAlive = false;
                        deadBird.Play();
                        ((ApplesGame)Game).AddScore(8);
                    }
                    else
                    {
                        onPlayerHit();
                    }
                }

            }

            // Check for bees' intersection with player
            if (bee1.collisionRect.Intersects(player.collisionRect) ||
                bee2.collisionRect.Intersects(player.collisionRect))
            {
                beeHit.Play();
                player.Reset(new Vector2(0, GROUND_LEVEL));
                if(lives.Count != 0)
                    lives.RemoveAt(lives.Count - 1);
                if (lives.Count == 0)
                    player.IsAlive = false;
                ((ApplesGame)Game).AddScore(-4);

            }

            // Update apples
            for(int i = 0; i < apples.Count; ++i)
            {
                Sprite sprite = apples[i];
                sprite.Update(gameTime, Game.Window.ClientBounds);

                // Check for collisions and update appropriately
                AppleCollision(sprite, ref i);
            }


            // Check if player is dead and end game if so
            if (!player.IsAlive)
                ((ApplesGame)Game).gameTimer = 0;

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw all sprites including player, enemies, apples, etc.. to
        /// the screen. For each sprite call it's corresponding Draw method.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            // Draw heart (life) hud
            foreach (Sprite sprite in lives)
                sprite.Draw(gameTime, spriteBatch);

            // Draw the player
            player.Draw(gameTime, spriteBatch);

            // Draw bird
            if(bird.IsAlive)
                bird.Draw(gameTime, spriteBatch);

            // Draw the bees
            bee1.Draw(gameTime, spriteBatch);
            bee2.Draw(gameTime, spriteBatch);

            // Draw the apples
            foreach (Sprite sprite in apples)
                sprite.Draw(gameTime, spriteBatch);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
