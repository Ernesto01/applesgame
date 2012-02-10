#region 
// Animation.cs 
// This is a class to hold information about animation and loop 
// through a sprite sheet of frames.
// 
// The Object Role Stereotype of this object is Information Holder
#endregion
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppleFrenzy
{
    class Animation
    {
        // Define animation speed
        const int DefaultMillisecondsPerFrame = 48;

        // Local data
        public Texture2D Image { get; private set; }
        private Point frameSize, currentFrame, sheetSize;
        int millisecondsPerFrame, timeSinceLastFrame = 0;

        // Properties
        public int FrameWidth
        {
            get { return frameSize.X; }
        }

        public int FrameHeight
        {
            get { return frameSize.Y; }
        }

        public Rectangle frameRectangle
        {
            get
            {
                return new Rectangle(currentFrame.X * frameSize.X,
                                    currentFrame.Y * frameSize.Y,
                                    frameSize.X, frameSize.Y);
            }
        }

        // Constructor
        public Animation(Texture2D image, Point frameSize, Point currentFrame,
                Point sheetSize, int millisecondsPerFrame)
        {
            this.Image = image;
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }
        public Animation(Texture2D image, Point frameSize, Point currentFrame,
                Point sheetSize)
            : this(image, frameSize, currentFrame,
               sheetSize, DefaultMillisecondsPerFrame) { }

        // Functions
        // Move to the next frame in the sprite sheet if enough time has passed
        public void nextFrame(GameTime gameTime)
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

    }
}
