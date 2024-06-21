using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace com.gdcbd.bossbattle.player
{
    [CreateAssetMenu(fileName = "New Player Health", menuName = "BossBattle/PlayerHealth", order = 0)]
    public class PlayerHealth : ScriptableObject
    {
        public string PlayerName = "Knight 1";
        // LAYERS
        [Tooltip("Player layer")]
        public LayerMask PlayerLayer;

        // MOVEMENT
        [Tooltip("Max speed")]
        public float Speed = 14;

        [Tooltip("Crouch Speed")]
        public float CrouchSpeed = 7f;
        
        [Tooltip("Sprint speed")]
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
        [Tooltip("How many jump player can air"), Range(1,2)]
        public int MaxJumpInAir = 1; 
        [Tooltip("Jump power")]
        public float JumpPower = 36;

        [Tooltip("Max fall speed")]
        public float MaxFallSpeed = 40;

        [Tooltip("Fall acceleration")]
        public float FallAcceleration = 110;
       
        public float CoyoteTime = .15f;
        public float JumpBuffer = .2f;
       
        [Tooltip("How many dash player can in air"), Range(1,5)]
        public int MaxDashInAir = 1;
        public float DashSpeed =  50f; // Speed during dash
        public float DashDuration = 0.2f; // Duration of the dash
        
      
    }
}