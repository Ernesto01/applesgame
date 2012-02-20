using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Apple01;

namespace AppleFrenzy
{
    class PlatformSprite : Sprite
    {
        // number of blocks that make up a platform
        private int BlockSize = 5; 

        public PlatformSprite(Texture2D image, Vector2 position, Point frameSize, int collisionOffset,
                                Vector2 velocity, Rectangle clientBounds, float size) :
            base(image, position, frameSize, collisionOffset, new Point(0, 0),
                new Point(1, 1), velocity, size) { }

        public PlatformSprite(Texture2D image, Vector2 position, Point frameSize, int collisionOffset,
                                Vector2 velocity, float size, int blockSize) :
            base(image, position, frameSize, collisionOffset, new Point(0, 0),
                new Point(1, 1), velocity, size) 
        {
            BlockSize = blockSize;
        }


        /// <summary>
        /// No direction is necessary for this kind of sprite
        /// </summary>
        public override Vector2 direction
        {
            get { return velocity; }
        }

        /// <summary>
        /// Gets updated collision rectangle for this sprite.
        /// Notice that this sprite requires an override of this property since 
        /// it consists of an array of images as opposed to a single image.
        /// Make sure to multiply BlockSize BEFORE subtracting collisionOffset,
        /// otherwise the offset is subtracted per block and collision detection
        /// will be wrong, leaving large portion of the figure without hit detection
        /// </summary>
        public override Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                        (int)position.X + collisionOffset*2,
                        (int)position.Y + collisionOffset,
                        (currentAnimation.FrameWidth * BlockSize - collisionOffset*2),
                        currentAnimation.FrameHeight);
            }
        }

        /// <summary>
        /// Draw an array of blocks that form the platform by using the starting position vector
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < BlockSize; ++i)
            {
                spriteBatch.Draw(currentAnimation.Image,
                        new Vector2(position.X + i*currentAnimation.FrameWidth, position.Y), 
                        Color.White);
            }
            
        }
    }
}
