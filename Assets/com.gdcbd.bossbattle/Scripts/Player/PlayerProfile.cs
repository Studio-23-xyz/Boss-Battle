using UnityEngine;

namespace BossBattle
{
    [CreateAssetMenu(fileName = "New Player Profile", menuName = "BossBattle/PlayerProfile", order = 0)]
    public class PlayerProfile : ScriptableObject
    {
        // LAYERS
        [Tooltip("Player layer")]
        public LayerMask PlayerLayer;

        // MOVEMENT
        [Tooltip("Max speed")]
        public float MaxSpeed = 14;
        [Tooltip("Max speed")]
        public float SprintSpeed = 21;
        
        [Tooltip("Acceleration")]
        public float Acceleration = 120;

        [Tooltip("Ground deceleration")]
        public float GroundDeceleration = 60;

        [Tooltip("Air deceleration")]
        public float AirDeceleration = 30;

        [Tooltip("Grounding force"), Range(0f, -10f)]
        public float GroundingForce = -1.5f;

        [Tooltip("Grounder distance"), Range(0f, 0.5f)]
        public float GrounderDistance = 0.05f;

        // JUMP
        [Tooltip("Jump power")]
        public float JumpPower = 36;

        [Tooltip("Max fall speed")]
        public float MaxFallSpeed = 40;

        [Tooltip("Fall acceleration")]
        public float FallAcceleration = 110;
       
        public float CoyoteTime = .15f;
        public float JumpBuffer = .2f;
       
        public float DashSpeed =  50f; // Speed during dash
        public float DashDuration = 0.2f; // Duration of the dash
    }
}