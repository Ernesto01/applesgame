using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using AppleFrenzy;
using Microsoft.Xna.Framework;

namespace AppleFrenzy
{
    class Score
    {
        // Public property to keep track of score
        public int currentScore { get; set; }
        // score font
        public SpriteFont scoreFont;

        // Constructors
        private Score() { }

        public Score(ContentManager content)
        {
            currentScore = 0;
            scoreFont = content.Load<SpriteFont>(@"Fonts\Score");   
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(scoreFont, "Score: " + currentScore, new Vector2(10, 10),
                        Color.DarkBlue, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
        }
    
    }
}
