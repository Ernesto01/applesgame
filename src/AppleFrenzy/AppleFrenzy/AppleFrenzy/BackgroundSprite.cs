using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Apple01;

namespace AppleFrenzy
{
    class BackgroundSprite : Sprite
    {
        private float depth = 1;

        public BackgroundSprite(Texture2D image, Point frameSize, float size)
            : base(image, new Vector2(0, 0), frameSize, 0, new Point(0, 0), new Point(1, 1),
                new Vector2(0, 0), size) { }

        public BackgroundSprite(Texture2D image, Point frameSize, float size, float depth)
            : base(image, new Vector2(0, 0), frameSize, 0, new Point(0, 0), new Point(1, 1),
                new Vector2(0, 0), size) 
        {
            this.depth = depth;
        }

        public override Vector2 direction
        {
            get { return new Vector2(0, 0); }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(currentAnimation.Image, position,
                currentAnimation.frameRectangle,
                Color.White, 0, Vector2.Zero,
                size, SpriteEffects.None, depth);
        }
    }
}
