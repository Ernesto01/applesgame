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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        const int GROUND_LEVEL = 500;
        SpriteBatch spriteBatch;
        UserControlledSprite player;
        List<Sprite> spriteList = new List<Sprite>();


        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            // Load player controlled character
            player = new UserControlledSprite(Game.Content.Load<Texture2D>(@"Images/char01"),
                        new Vector2(0,GROUND_LEVEL), new Point(57, 95), 10, new Point(0, 0), new Point(1, 1),
                        new Vector2(6, 6));

            // Load game controlled sprites
            for(int i = 0; i < 10; ++i)
                spriteList.Add(new AppleSprite(Game.Content.Load<Texture2D>(@"Images\apple"), 
                    new Point(28, 32), 5, new Vector2(0, 2), Game.Window.ClientBounds));

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime, Game.Window.ClientBounds);

            for(int i = 0; i < spriteList.Count; ++i)
            {
                Sprite sprite = spriteList[i];
                sprite.Update(gameTime, Game.Window.ClientBounds);

                // Check for colllisions
                if (sprite.collisionRect.Intersects(player.collisionRect))
                {
                    spriteList.RemoveAt(i);
                    --i;
                }
                if (sprite.collisionRect.Bottom >= GROUND_LEVEL+85)
                {
                    sprite.speed.Y = 0;
                }

            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            // Draw the player
            player.Draw(gameTime, spriteBatch);
            // Draw the apples
            foreach (Sprite sprite in spriteList)
                sprite.Draw(gameTime, spriteBatch);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
