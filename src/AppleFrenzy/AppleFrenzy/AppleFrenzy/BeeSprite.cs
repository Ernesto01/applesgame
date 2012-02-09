﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Apple01;

namespace AppleFrenzy
{
    class BeeSprite : Sprite
    {
        public BeeSprite(Texture2D image, Vector2 position, Point frameSize, int collisionOffset,
            Point currentFrame, Point sheetSize, Vector2 velocity, float size) 
            : base(image, position, frameSize, collisionOffset, currentFrame, sheetSize, 
                velocity, size) { }

        public override Vector2 direction
        {
            get { return velocity; }
        }
    }
}