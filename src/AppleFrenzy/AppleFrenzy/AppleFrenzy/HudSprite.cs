using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Apple01;

namespace AppleFrenzy
{
    class HudSprite : Sprite
    {
        // AppleSprite Constructor
        public HudSprite(Texture2D image, Vector2 position, Point frameSize, float size) :
            base(image, position, frameSize, 0, new Point(0, 0),
                new Point(1, 1), new Vector2(0,0), size) { }

        // Don't need it for this type of sprite so set to zero
        public override Vector2 direction
        {
            get { return new Vector2(0,0); }
        }
    }
}
