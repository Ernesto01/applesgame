using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Apple01
{
    class UserAnimatedSprite : Sprite
    {
        public bool movingRight = false;
        public bool movingLeft = false;
        public bool jumping = false;

        public UserAnimatedSprite(Texture2D textImage, Vector2 pos, Point frameSize,
                int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textImage, pos, frameSize, collisionOffset, currentFrame, sheetSize, speed) { }
        public UserAnimatedSprite(Texture2D textImage, Vector2 pos, Point frameSize,
                int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
            : base(textImage, pos, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame) { }

        public override Vector2 direction
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    inputDirection.X -= 1;
                    movingLeft = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    inputDirection.X += 1;
                    movingRight = true;
                }
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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
            
            
        }


    }
}
