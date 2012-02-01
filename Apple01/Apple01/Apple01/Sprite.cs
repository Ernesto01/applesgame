using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Apple01
{
    abstract class Sprite
    {
        Texture2D textImage;
        protected Point frameSize;
        Point currentFrame, sheetSize;
        int collisionOffset, millisecondsPerFrame, timeSinceLastFrame = 0;
        const int defaultMillisecondsPerFrame = 16;
        protected Vector2 speed, position;

        /* Properties */
        public abstract Vector2 direction
        {
            get;
        }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                        (int)position.X + collisionOffset,
                        (int)position.Y + collisionOffset,
                        frameSize.X - (collisionOffset * 2),
                        frameSize.Y - (collisionOffset * 2));
            }
        }

        /* Constructors */
        public Sprite(Texture2D textImage, Vector2 pos, Point frameSize, int collisionOffset,
                Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
        {
            this.textImage = textImage;
            this.position = pos;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }

        public Sprite(Texture2D textImage, Vector2 pos, Point frameSize,
                int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : this(textImage, pos, frameSize, collisionOffset, currentFrame, sheetSize,
                    speed, defaultMillisecondsPerFrame) { }


        /* Functions */
        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                        currentFrame.Y = 0;
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textImage, position,
                new Rectangle(currentFrame.X * frameSize.X,
                            currentFrame.Y * frameSize.Y,
                            frameSize.X, frameSize.Y),
                Color.White, 0, Vector2.Zero,
                1f, SpriteEffects.None, 0);
        }

    }
}
