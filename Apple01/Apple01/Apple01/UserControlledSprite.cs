using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Apple01
{
    class UserControlledSprite : Sprite
    {
        public UserControlledSprite(Texture2D textImage, Vector2 pos, Point frameSize,
                int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textImage, pos, frameSize, collisionOffset, currentFrame, sheetSize, speed) { }
        public UserControlledSprite(Texture2D textImage, Vector2 pos, Point frameSize,
                int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
            : base(textImage, pos, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame) { }


        public override Vector2 direction
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    inputDirection.X -= 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    inputDirection.X += 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    inputDirection.Y -= 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    inputDirection.Y += 1;

                GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
                if (gamepadState.ThumbSticks.Left.X != 0)
                    inputDirection.X += gamepadState.ThumbSticks.Left.X;
                if (gamepadState.ThumbSticks.Left.Y != 0)
                    inputDirection.Y -= gamepadState.ThumbSticks.Left.Y;

                return inputDirection * speed;
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // Move sprite based on direction
            position += direction;

            // If sprite is offscreen move back within game window
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > clientBounds.Width - frameSize.X)
                position.X = clientBounds.Width - frameSize.X;
            if (position.Y > clientBounds.Height - frameSize.Y)
                position.Y = clientBounds.Height - frameSize.Y;

            base.Update(gameTime, clientBounds);
        }
    }
}
