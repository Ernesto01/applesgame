using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Apple01;

namespace AppleFrenzy
{
    class BeeSprite : Sprite
    {
        private SpriteEffects flip = SpriteEffects.None;

        // Constructor 
        public BeeSprite(Texture2D image, Vector2 position, Point frameSize, int collisionOffset,
            Point currentFrame, Point sheetSize, Vector2 velocity, float size) 
            : base(image, position, frameSize, collisionOffset, currentFrame, sheetSize, 
                velocity, size) {}

        public override Vector2 direction
        {
            get { return velocity; }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // Update bee
            position += direction;

            if (position.X < 100 || position.X > 1000)
            {
                velocity.X *= -1;
            }

            base.Update(gameTime, clientBounds);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Flip sprite to face the way we're moving
            if (position.X < 100 || position.X > 1000)
                flip = SpriteEffects.FlipHorizontally;
            else if (velocity.X < 0)
                flip = SpriteEffects.None;

            spriteBatch.Draw(currentAnimation.Image, position, currentAnimation.frameRectangle,
                            Color.White, 0f, Vector2.Zero, size, flip, 0f);
        }
    }
}
