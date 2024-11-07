using UnityEngine;

namespace MyThings.Locomotion
{

    /// <summary>
    /// Static Class Which Deals With The Enemy Locomotion
    /// </summary>
    public static class TargetLocomotion
    {
        /// <summary>
        /// The Modes Of Movement For Basic Flying Enemy 
        /// </summary>
        public enum FlyingRB
        {
            ForceInDirectionOfTarget,//The Force Will Be Applied In The Direction Of The Object To Follow
            ForceInDirectionToAddToTarget,//The Force Will Be Applied In The Direction So That The Sum Of The Force And Velocity Is In THe DIrection Of Object To Follow
            VelocityInDirection// The Direction Will Be Used Directly As THe Velocity So Enemy Can Be Bit Snapy
        }


        /// <summary>
        /// Move The Enemy In Using A Movemet Mode (RigidBody Based)
        /// </summary>
        /// <param name="flying">The Mode</param>
        /// <param name="FollowTransform">Object To Follow</param>
        /// <param name="ObjectFollowing">The Object Following</param>
        /// <param name="RB">The Rigid Body Of The Object Following</param>
        /// <param name="Speed">The Speed At Which The Object Is Following</param>
        public static void UseForce(FlyingRB flying, Transform FollowTransform, Transform ObjectFollowing, Rigidbody RB, float Speed)
        {
            Vector3 dir, ForceDir;
            switch (flying)
            {
                case FlyingRB.ForceInDirectionOfTarget:

                    dir = FollowTransform.position - ObjectFollowing.position;
                    RB.AddForce(dir.normalized * Speed, ForceMode.Force);

                    break;
                case FlyingRB.ForceInDirectionToAddToTarget:

                    dir = FollowTransform.position - ObjectFollowing.position;
                    ForceDir = 2 * dir - RB.linearVelocity;
                    RB.AddForce(ForceDir.normalized * Speed, ForceMode.Force);

                    break;
                case FlyingRB.VelocityInDirection:

                    dir = FollowTransform.position - ObjectFollowing.position;
                    Vector3 Diff = dir - RB.linearVelocity;

                    // change The Speed Fast If Angle is Huge
                    if (Vector3.Angle(dir, RB.linearVelocity) > 30)
                        RB.AddForce(Diff.normalized * Speed, ForceMode.Impulse);

                    RB.AddForce(dir.normalized * Speed, ForceMode.Force);
                    break;
            }
        }
    }
}
