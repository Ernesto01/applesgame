using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using AppleFrenzy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Apple01
{
    class UserControlledSprite : Sprite
    {
        // idle, run, die and jump animations are loaded for this character
        public Animation idle, run, die, jump;
        private SpriteEffects flip = SpriteEffects.None;
        float movement;
     
        PlayerPhysics physics;

        // Address animations 
        public enum AnimationType { idle, run, die, jump };

        /* Constructors */
        public UserControlledSprite(Texture2D textImage, Vector2 pos, Point frameSize,
                int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, float size)
            : base(textImage, pos, frameSize, collisionOffset, currentFrame, sheetSize, speed, size) 
        {
            physics = new PlayerPhysics(this);
            IsAlive = true;
        }
        public UserControlledSprite(Texture2D textImage, Vector2 pos, Point frameSize,
                int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, float size, int millisecondsPerFrame)
            : base(textImage, pos, frameSize, collisionOffset, currentFrame, sheetSize, speed, size, millisecondsPerFrame) 
        {
            physics = new PlayerPhysics(this);
            IsAlive = true;
        }

        /// <summary>
        /// Set and initialize animation if it's not already running
        /// </summary>
        /// <param name="animation"></param>
        public void setAnimation(Animation animation)
        {
            if (currentAnimation == animation)
                return;

            currentAnimation = animation;
            currentAnimation.Initialize();
        }

        // Resets character to passed-in position
        public void Reset(Vector2 position)
        {
            this.position = position;
            velocity = Vector2.Zero;
            IsAlive = true;
            currentAnimation = idle;
        }

        // Load and initialize animations, sounds, etc...
        public override void Initialize(IServiceProvider serviceProvider)
        {
            
      
            ContentManager content = new ContentManager(serviceProvider, "Content");
            idle = new Animation(content.Load<Texture2D>(@"Images/Idle"), new Point(64, 64),
                                new Point(0, 0), new Point(1, 1));
            run = new Animation(content.Load<Texture2D>(@"Images/Run"), new Point(64, 64),
                                new Point(0, 0), new Point(10, 1));
            jump = new Animation(content.Load<Texture2D>(@"Images/Jump"), new Point(64, 64),
                                new Point(0, 0), new Point(11, 1), 70);
            die = new Animation(content.Load<Texture2D>(@"Images/Die"), new Point(64, 64),
                                new Point(0, 0), new Point(12, 1));
        }

        /// <summary>
        /// Get user input for movement of the user controlled sprite
        /// This is also where changes would be made if we add Windows 
        /// Phone or XBOX functionality
        /// </summary>
        /// <param name="keyboardState"></param>
        public void getInput(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Right))
                movement = 1f;
            if (keyboardState.IsKeyDown(Keys.Left))
                movement = -1f;
            // Handles jump
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                physics.isJumping = true;

        }

        /// <summary>
        /// Update sprite according to user input, check for screen bounds
        /// so character stays within the screen coordinates, apple gravity,
        /// friction and drag. 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="clientBounds"></param>
        public void Update(GameTime gameTime, Rectangle clientBounds, List<Sprite> tiles)
        {
            getInput(Keyboard.GetState());   

            physics.ApplyPhysics(gameTime, movement, tiles);

            if (IsAlive && physics.OnGround)
            {
                if (Math.Abs(velocity.X) - 0.02f > 0)
                    currentAnimation = run;
                else
                    currentAnimation = idle;
            }

            // Clear input
            movement = 0f;
            physics.isJumping = false;

            // Make sure player doesn't move out of window
            moveWithinBoundary(clientBounds);

            base.Update(gameTime, clientBounds);
        }

        void moveWithinBoundary(Rectangle clientBounds)
        {
            // If sprite is offscreen move back within game window
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > clientBounds.Width - currentAnimation.FrameWidth)
                position.X = clientBounds.Width - currentAnimation.FrameWidth;
            if (position.Y > clientBounds.Height - currentAnimation.FrameHeight)
                position.Y = clientBounds.Height - currentAnimation.FrameHeight;
        }

        /// <summary>
        /// Draw player sprite, and flip accordingly when player changes direction
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Flip sprite to face the way we're moving
            if (velocity.X > 0)
                flip = SpriteEffects.FlipHorizontally;
            else if (velocity.X < 0)
                flip = SpriteEffects.None;

            spriteBatch.Draw(currentAnimation.Image, position, currentAnimation.frameRectangle,
                            Color.White, 0f, Vector2.Zero, size, flip, 0f);
        }
    }
}
