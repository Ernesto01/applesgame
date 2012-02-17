using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Apple01;


namespace AppleFrenzy
{
    class BirdSprite : Sprite
    {
        // Constant to control bird movement
        const int LeftBound = 75;
        const int RightBound = 1080;

        private SpriteEffects flip = SpriteEffects.None;


        // Constructor 
        public BirdSprite(Texture2D image, Vector2 position, Point frameSize, int collisionOffset,
            Point currentFrame, Point sheetSize, Vector2 velocity, float size) 
            : base(image, position, frameSize, collisionOffset, currentFrame, sheetSize, 
                velocity, size) 
        {
            IsAlive = true;
        }

        public override Vector2 direction
        {
            get { return velocity; }
        }

        /// <summary>
        /// This function applies linear tranformation to
        /// convert screen coordinates to a specific coordinate system to
        /// do trigonometric functions sin and cos. I'm only converting x coordinates
        /// from (0,1000) -> (-3, 3) 
        /// </summary>
        float mapScreenCoordinatesToGame(float x)
        {
            return 0.0065f * x - 3;
        }

        // Do Linear transformation for Y coordinates and convert back to Screen
        // coordinates
        float mapGameCoordinatesToScreen(float y)
        {
            return (int)(y * (-100) + 485);
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // Update bird's X position
            position += direction;
            // Make sure bird does not go out of bounds
            if (position.X < LeftBound || position.X > RightBound)
            {
                velocity.X *= -1;
            }

            // As the sprite moves horizontally, it's movement vertically should 
            // follow the Sin wave
            float yCoordinate = (float)Math.Sin(mapScreenCoordinatesToGame(position.X));
            position.Y = mapGameCoordinatesToScreen(yCoordinate);

            
            base.Update(gameTime, clientBounds);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Flip sprite to face the way we're moving
            if (position.X <= LeftBound || position.X >= RightBound)
                flip = SpriteEffects.FlipHorizontally;
            else if (velocity.X < 0)
                flip = SpriteEffects.None;

            spriteBatch.Draw(currentAnimation.Image, position, currentAnimation.frameRectangle,
                            Color.White, 0f, Vector2.Zero, size, flip, 0f);
        }
    }
}
