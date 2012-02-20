using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Apple01;

namespace AppleFrenzy
{
    class Timer
    {
        // Timer and Timer Font
        SpriteFont timerFont;
        public float TimeRemaining { get; set; }

        /// <summary>
        /// Initialize Timer state here
        /// </summary>
        /// <param name="timeRemaining"></param>
        public Timer(float timeRemaining)
        {
            this.TimeRemaining = timeRemaining;
        }

        /// <summary>
        /// Load Content
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            // Load timerFont
            timerFont = content.Load<SpriteFont>(@"fonts\score");
        }

        /// <summary>
        /// Reduce TimeRemainingby total seconds that have passed since last call
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            TimeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }


        /// <summary>
        /// Draw time remanining to the display
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(timerFont, "Time: " + TimeRemaining.ToString("0.00"),
                                        new Vector2(985, 10), Color.Red);
        }

        /// <summary>
        /// Check for when timer ends 
        /// </summary>
        /// <returns>true if timer has reach the end, false otherwise</returns>
        public bool EndTime()
        {
            if (TimeRemaining <= 0)
                return true;
            return false;
        }

    }
}
