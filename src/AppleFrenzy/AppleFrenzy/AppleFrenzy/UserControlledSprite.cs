using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using AppleFrenzy;
using System;

namespace Apple01
{
    class UserControlledSprite : Sprite
    {
        public Animation idle, run, die, jump;
        private SpriteEffects flip = SpriteEffects.None;
        float movement;
     
        PlayerPhysics physics;

        /* Constructors */
        public UserControlledSprite(Texture2D textImage, Vector2 pos, Point frameSize,
                int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, float size)
            : base(textImage, pos, frameSize, collisionOffset, currentFrame, sheetSize, speed, size) 
        {
            physics = new PlayerPhysics(this);
        }
        public UserControlledSprite(Texture2D textImage, Vector2 pos, Point frameSize,
                int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, float size, int millisecondsPerFrame)
            : base(textImage, pos, frameSize, collisionOffset, currentFrame, sheetSize, speed, size, millisecondsPerFrame) 
        {
            physics = new PlayerPhysics(this);
        }


        public override Vector2 direction
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    inputDirection.X = -1;
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    inputDirection.X = 1;
                
                GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
                if (gamepadState.ThumbSticks.Left.X != 0)
                    inputDirection.X += gamepadState.ThumbSticks.Left.X;
                if (gamepadState.ThumbSticks.Left.Y != 0)
                    inputDirection.Y -= gamepadState.ThumbSticks.Left.Y;

                return inputDirection * velocity;
            }
        }

        // Resets character to passed position
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
            IsAlive = true;
      
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

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            getInput(Keyboard.GetState());   

            physics.ApplyPhysics(gameTime, ref position, ref velocity, movement);

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

            // If sprite is offscreen move back within game window
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > clientBounds.Width - currentAnimation.FrameWidth)
                position.X = clientBounds.Width - currentAnimation.FrameWidth;
            if (position.Y > clientBounds.Height - currentAnimation.FrameHeight)
                position.Y = clientBounds.Height - currentAnimation.FrameHeight;

            base.Update(gameTime, clientBounds);
        }

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
