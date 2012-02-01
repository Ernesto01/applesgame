using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Apple01
{
    class AutomatedSprite : Sprite
    {
        public AutomatedSprite(Texture2D textImage, Vector2 pos, Point frameSize,
                int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textImage, pos, frameSize, collisionOffset, currentFrame, sheetSize, speed) { }
        public AutomatedSprite(Texture2D textImage, Vector2 pos, Point frameSize,
                int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
            : base(textImage, pos, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame) { }

        public override Vector2 direction
        {
            get { return speed; }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += direction;
            
            base.Update(gameTime, clientBounds);
        }

    }
}
