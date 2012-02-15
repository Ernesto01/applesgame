using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Apple01;


namespace AppleFrenzy
{
    class BirdSprite : Sprite
    {
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
    }
}
