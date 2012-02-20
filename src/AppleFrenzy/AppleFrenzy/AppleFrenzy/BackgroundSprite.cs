using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Apple01;

namespace AppleFrenzy
{
    class BackgroundSprite : Sprite
    {
        /// <summary>
        /// Depth data for sprite so it knows not to be drawn on top of other sprites
        /// </summary>
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

        /// <summary>
        /// No direction is necessary for this kind of sprite
        /// </summary>
        public override Vector2 direction
        {
            get { return new Vector2(0, 0); }
        }

        /// <summary>
        /// Draw sprite to display
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(currentAnimation.Image, position,
                currentAnimation.frameRectangle,
                Color.White, 0, Vector2.Zero,
                size, SpriteEffects.None, depth);
        }
    }
}
