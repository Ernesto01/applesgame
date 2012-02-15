using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AppleFrenzy;
using System;

namespace Apple01
{
    abstract class Sprite
    {
        // Current animation, only one animation may play at a time.
        public Animation currentAnimation;   
        int collisionOffset;
        public Vector2 velocity; 
        public Vector2 position;
        protected float size;

        /* Properties */
        public bool IsAlive { get; set; }

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
                        currentAnimation.FrameWidth - (collisionOffset * 2),
                        currentAnimation.FrameHeight - (collisionOffset * 2));
            }
        }

        /* Constructors */
        public Sprite(Texture2D image, Vector2 position, Point frameSize, int collisionOffset,
                Point currentFrame, Point sheetSize, Vector2 velocity, float size, int millisecondsPerFrame)
        {
            currentAnimation = new Animation(image, frameSize, currentFrame, sheetSize, millisecondsPerFrame);
            this.position = position;
            this.collisionOffset = collisionOffset;
            this.velocity = velocity;
            this.size = size;
        }

        public Sprite(Texture2D image, Vector2 position, Point frameSize,
                int collisionOffset, Point currentFrame, Point sheetSize, Vector2 velocity, float size)
        {
            currentAnimation = new Animation(image, frameSize, currentFrame, sheetSize);
            this.position = position;
            this.collisionOffset = collisionOffset;
            this.velocity = velocity;
            this.size = size;
        }


        /* Functions */
        // Initialize will be used to initialize any class variables and states
        public virtual void Initialize(IServiceProvider serviceProvider) { }

        // Set vertical velocity to zero
        public void stopVerticalMovement()
        {
            velocity.Y = 0;
        }

        // Update method will iterate through sprites in spritesheet to do animation
        // and move between sprites given a set framerate
        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // If enough time has passed, render next frame
            currentAnimation.nextFrame(gameTime);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(currentAnimation.Image, position,
                currentAnimation.frameRectangle,
                Color.White, 0, Vector2.Zero,
                size, SpriteEffects.None, 0);
        }

    }
}
