using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AppleFrenzy
{
    class PlayerPhysics
    {
        // Constants for handling horizontal movement
        private const float MoveAcceleration = 13000f;
        private const float MaxMoveSpeed = 1750f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

        // Constant for handling vertical movement
        private const float MaxJumpTime = 0.35f;
        private const float JumpLaunchVelocity = -3500f;
        private const float GravityAcceleration = 3400f;
        private const float MaxFallSpeed = 550f;
        private const float JumpControlPower = 0.14f;

        public bool OnGround { get; private set; }

        // Jumping state
        public bool isJumping;
        public bool wasJumping;
        public float jumpTime;

        public void ApplyPhysics(GameTime gameTime, ref Vector2 position, ref Vector2 velocity, float movement)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 previousPosition = position;

            velocity.X += movement * MoveAcceleration * elapsed;
            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed,
                                        -MaxFallSpeed, MaxFallSpeed);

            velocity.Y = Jump(velocity.Y, gameTime);

            // Apply drag
            if (OnGround)
                velocity.X *= GroundDragFactor;
            else if(!OnGround)
                velocity.X *= AirDragFactor;

            // Cap player speed
            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            // Apply velocity
            position += velocity * elapsed;
            position.X = (float)Math.Round(position.X);
            position.Y = (float)Math.Round(position.Y);

            // if collisiong stopped us from moving, reset velocity
            //if (position.X == previousPosition.X)
            //    velocity.X = 0;
            //if (position.Y == previousPosition.Y)
            //    velocity.Y = 0;
            

        }

        private float Jump(float velocityY, GameTime gameTime)
        {
            return 0;
        }
    }
}
