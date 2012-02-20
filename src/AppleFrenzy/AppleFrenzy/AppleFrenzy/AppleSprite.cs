using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Apple01
{
    class AppleSprite : Sprite
    {
        // Random number generator - Needed to randomly select spawn points for this sprite
        static Random random = new Random();
        
        // AppleSprite Constructor
        public AppleSprite(Texture2D image, Point frameSize, int collisionOffset, Vector2 velocity, Rectangle clientBounds, float size) :
            base(image, new Vector2(getRandom(clientBounds.Width-15), getRandom(120)), frameSize, collisionOffset, new Point(0, 0),
                new Point(1, 1), velocity, size) { }

        public AppleSprite(Texture2D image, Vector2 position, Point frameSize, int collisionOffset, Vector2 velocity, Rectangle clientBounds, float size) :
            base(image, position, frameSize, collisionOffset, new Point(0, 0),
                new Point(1, 1), velocity, size) { }

        // Helper Function to get random position - Needs to be static
        static int getRandom(int max)
        {
            return random.Next(max);
        }

        /// <summary>
        /// Return the velocity vector for its direction, 
        /// this allows us to add direction to our posistion vector
        /// for movement. Notice that direction points the same way as
        /// velocity, the magnitude of the vector is used for movement.
        /// </summary>
        public override Vector2 direction
        {
            get { return velocity; }
        }

        /// <summary>
        /// Move the apples depending on velocity/direction
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="clientBounds"></param>
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if(!IsOnGround)
                position += direction;

            base.Update(gameTime, clientBounds);
        }
    }
}
