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
        const int GROUND_LEVEL = 500;

        SpriteBatch spriteBatch;
        UserControlledSprite player;
        List<Sprite> apples = new List<Sprite>();
        Sprite bee;

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

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            bee = new BeeSprite(Game.Content.Load<Texture2D>(@"Images/Bee1"),
                        new Vector2(20, GROUND_LEVEL-70), new Point(24,24), 5, new Point(0,0), 
                        new Point(3,1), new Vector2(0,0), 1f);

            // Load player controlled character
            player = new UserControlledSprite(Game.Content.Load<Texture2D>(@"Images/Idle"),
                        new Vector2(0,GROUND_LEVEL), new Point(64, 64), 10, new Point(0, 0), new Point(1, 1),
                        new Vector2(6, 6), 1f);
            player.Initialize(Game.Services);

            // Load game controlled sprites
            for(int i = 0; i < 10; ++i)
                apples.Add(new AppleSprite(Game.Content.Load<Texture2D>(@"Images\Apple"), 
                    new Point(28, 32), 5, new Vector2(0, 2), Game.Window.ClientBounds, 0.60f));

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime, Game.Window.ClientBounds);
            bee.Update(gameTime, Game.Window.ClientBounds);

            for(int i = 0; i < apples.Count; ++i)
            {
                Sprite sprite = apples[i];
                sprite.Update(gameTime, Game.Window.ClientBounds);

                // Check for collisions
                if (sprite.collisionRect.Intersects(player.collisionRect))
                {
                    apples.RemoveAt(i);
                    --i;
                    //player.Reset(new Vector2(0, GROUND_LEVEL));
                }
                if (sprite.collisionRect.Bottom >= GROUND_LEVEL+70)
                {
                    sprite.velocity.Y = 0;
                }

            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            // Draw the player
            player.Draw(gameTime, spriteBatch);
            bee.Draw(gameTime, spriteBatch);

            // Draw the apples
            foreach (Sprite sprite in apples)
                sprite.Draw(gameTime, spriteBatch);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
