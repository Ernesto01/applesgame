using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Apple01;

namespace AppleFrenzy
{
    class PlayerPhysics
    {
        // Constants for handling horizontal movement
        private const float MoveAcceleration = 13000f;
        private const float MaxMoveSpeed = 1750f;
        private const float GroundDragFactor = 0.58f;
        private const float AirDragFactor = 0.58f;

        // Constant for handling vertical movement
        private const float MaxJumpTime = 0.35f;
        private const float JumpLaunchVelocity = -3500f;
        private const float GravityAcceleration = 3400f;
        private const float MaxFallSpeed = 550f;
        private const float JumpControlPower = 0.14f;

        const int GROUND_LEVEL = 580;

        public bool OnGround { get; private set; }

        // Jumping state
        public bool isJumping;
        public bool wasJumping;
        public float jumpTime;

        UserControlledSprite player;

        // Make default c'tor private so it cannot be called
        private PlayerPhysics() { } 

        public PlayerPhysics(UserControlledSprite player)
        {
            this.player = player;
            isJumping = false;
            wasJumping = false;
            jumpTime = 0;
            OnGround = true;
        }

        public void ApplyPhysics(GameTime gameTime, ref Vector2 position, ref Vector2 velocity, float movement)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 previousPosition = position;

            player.velocity.X += movement * MoveAcceleration * elapsed;
            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed,
                                       -MaxFallSpeed, MaxFallSpeed);

            // Handle jumps
            velocity.Y = Jump(velocity.Y, gameTime);
            
            // Check that player doesn't fall under GROUND_LEVEL
            if (player.position.Y >= GROUND_LEVEL)
            {
                player.position.Y = GROUND_LEVEL;
                OnGround = true;
            }
            else
            {
                OnGround = false;
            }

            
            // Apply drag
            if (OnGround)
                player.velocity.X *= GroundDragFactor;
            else if(!OnGround)
                player.velocity.X *= AirDragFactor;

            // Cap player speed
            player.velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            // Apply velocity to player position <----
            player.position += velocity * elapsed;
            player.position = new Vector2((float)Math.Round(player.position.X),
                                (float)Math.Round(player.position.Y));



        }

        void handleCollision()
        {

        }


        /// <summary>
        /// Handle player jumps and returns vertical velocity
        /// </summary>
        /// <param name="velocityY"></param>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        private float Jump(float velocityY, GameTime gameTime)
        {
            if (isJumping)
            {
                if ((!wasJumping && OnGround) || jumpTime > 0f)
                {
                    jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    player.currentAnimation = player.jump;
                }
                // if we are in the ascent of the the jump
                if (0f < jumpTime && jumpTime <= MaxJumpTime)
                {
                    // Override vertical velocity with power curve
                    velocityY = JumpLaunchVelocity * (1f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
                }
                else
                {   // Reached apex of the jump
                    jumpTime = 0f;
                }
            }
            else
            {   // Continues not jumping or cancels a jump in progress
                jumpTime = 0f;
            }
            wasJumping = isJumping;
            return velocityY;
        }
    }
}
