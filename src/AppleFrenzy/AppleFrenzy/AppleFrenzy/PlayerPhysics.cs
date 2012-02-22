using System;
using System.Collections.Generic;
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

        public void ApplyPhysics(GameTime gameTime, float movement, List<Sprite> tiles)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 previousPosition = player.position;

            player.xVelocity += movement * MoveAcceleration * elapsed;
            player.yVelocity = MathHelper.Clamp(player.yVelocity + GravityAcceleration * elapsed,
                                       -MaxFallSpeed, MaxFallSpeed);

            // Handle jumps
            player.yVelocity = Jump(player.yVelocity, gameTime);

            // Apply drag
            if (OnGround)
                player.xVelocity *= GroundDragFactor;
            else if(!OnGround)
                player.xVelocity *= AirDragFactor;

            // Cap player speed
            player.xVelocity = MathHelper.Clamp(player.xVelocity, -MaxMoveSpeed, MaxMoveSpeed);

            // Apply velocity to player position <----
            player.position += player.velocity * elapsed;
            player.position = new Vector2((float)Math.Round(player.xPosition),
                                (float)Math.Round(player.yPosition));

            // Handle ground and tile (platform) collision detection
            handleCollision(tiles);

            if (player.yPosition == previousPosition.Y)
                player.yVelocity = 0;
        }


        /// <summary>
        /// Handle collision with ground or platforms
        /// </summary>
        /// <param name="tiles"></param>
        void handleCollision(List<Sprite> tiles)
        {
            OnGround = false;

            if (player.yPosition >= GROUND_LEVEL)
            {
                OnGround = true;
                player.position = new Vector2(player.xPosition, GROUND_LEVEL);
            }
            else
            {
                tileCollision(tiles);
            }
            
        }


        /// <summary>
        /// Do hit detection with platforms, if player lands above, then modify 
        /// position so gravity doesn't affect it. If player is under the 
        /// platform, don't let him jump through it.
        /// </summary>
        /// <param name="tiles"></param>
        void tileCollision(List<Sprite> tiles)
        {
            foreach (Sprite tile in tiles)
            {
                float top = tile.RectangleTop - player.currentAnimation.FrameHeight - 4.0f;

                if (tile.RectangleLeft < player.RectangleRight && tile.RectangleRight > player.RectangleLeft)
                {
                    // If player is under the platform 
                    if (player.yPosition <= tile.RectangleBottom-15 &&
                        player.yPosition > tile.RectangleTop)
                    {
                        player.position = new Vector2(player.xPosition, tile.RectangleBottom-15);
                    }
                    else if (player.yPosition >= top && 
                        (player.yPosition + player.currentAnimation.FrameHeight < tile.RectangleBottom))
                    {   // Player is above the platform
                        OnGround = true;
                        player.position = new Vector2(player.xPosition, top);
                    }
                }
            }
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
                    player.setAnimation(player.animations[2]); // Set Jump animation
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
