using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace BossBattle
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerProfile _profile;
        [FormerlySerializedAs("_playerGunController")] [SerializeField] private PlayerGunManager playerGunManager;
        [SerializeField] private Transform _gunTransform;
        
        private Rigidbody2D _rigidbody;
        private CapsuleCollider2D _colider;
        private InputManager _input;

        private Vector2 _velocity;
        private bool _isGrounded;
        private bool _isReadyToJump;  
        private bool _isInSprint;
        private bool _isInFire;
        private bool _isDashing;
        private bool _isInCrouch;
        private bool _isStartedMoving;
        
        private bool _cachedQueryStartInColliders; // ignore itself collision
       
        private float _time;
        private float _timeJumpStart;
       
        private float _inAirTime = float.MinValue;
        private float _dashTime;
        private bool _bufferedJumpUsable;
        private bool _coyoteUsable;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_profile == null) Debug.LogWarning("Player profile not found!", this);
        }
#endif

        #region Input

        private void OnEnable()
        {
            _input = InputManager.Instance;
            if (_input != null)
            {
                _input.JumpPressedAction += OnJumpPressed;
                _input.SprintPressedAction += OnSprintPressed;
                _input.SprintReleasedAction += OnSprintReleased;
                _input.FirePressedAction += OnFirePressed;
                _input.FireReleasedAction += OnFireReleased;
                _input.DashPressedAction += OnDashPressed;
                _input.CrouchPressedAction += OnCrouchPressed;
                _input.CrouchReleasedAction += OnCrouchReleased;
                _input.MovePressedAction += OnMovePressed;
                _input.MoveReleaseAction += OnMoveReleased;
            }
        }
        private void OnDisable()
        {
            if (_input != null)
            {
                _input.JumpPressedAction -= OnJumpPressed;
                _input.SprintPressedAction -= OnSprintPressed;
                _input.SprintReleasedAction -= OnSprintReleased;
                _input.FirePressedAction -= OnFirePressed;
                _input.FireReleasedAction -= OnFireReleased;
                _input.DashPressedAction -= OnDashPressed;
                _input.CrouchPressedAction -= OnCrouchPressed;
                _input.CrouchReleasedAction -= OnCrouchReleased;
                _input.MovePressedAction -= OnMovePressed;
                _input.MoveReleaseAction -= OnMoveReleased;
            }
        }
        #endregion
        private void OnJumpPressed()
        {
            _isReadyToJump = true;
            _timeJumpStart = _time;
        }
        private void OnSprintPressed() => _isInSprint = true;

        private void OnSprintReleased() => _isInSprint = false;

        private void OnFirePressed()
        {
            playerGunManager.Fire();
            _isInFire = true;
        }
        private void OnFireReleased()  => _isInFire = false;
        
        private void OnDashPressed()
        {
            if (_isGrounded)
            {
                _isDashing = true;
                _dashTime = _time;
                var _dashDirection = Mathf.Sign(_input.Move().x); // TODO: dash will be in player forward
                _velocity.x = _dashDirection * _profile.DashSpeed;
            }
        }

        private void OnMovePressed() => _isStartedMoving = true;
        private void OnMoveReleased()
        {
            
        }

        
        private void OnCrouchPressed() => _isInCrouch = true;
        private void OnCrouchReleased() => _isInCrouch = false ;

       

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _colider = GetComponent<CapsuleCollider2D>();
            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        }

        private void Update()
        {
            _time += Time.deltaTime;
        }

        private void FixedUpdate()
        {
            if (_isDashing)
            {
                CheckDash();
            }
            else
            {
                DetectGround();
                HandleJump();
                HandleMovement();
                HandleGravity();
                FlipSprite(); 
            }
            ApplyForce();
            
           
        }
        private void FlipSprite()
        {
            if (_input.Move().x > 0 && _gunTransform.localScale.x < 0)
            {
                _gunTransform.localScale = new Vector3(1, 1, 1);
            }
            else if (_input.Move().x < 0 && _gunTransform.localScale.x > 0)
            {
                _gunTransform.localScale = new Vector3(-1, 1, 1);
            }
        }

        private void CheckDash()
        {
            if (_time - _dashTime > _profile.DashDuration)
            {
                _isDashing = false;
            }
        }

        private void ApplyForce()=> _rigidbody.velocity = _velocity;
        
        #region Collisions
       
        private void DetectGround()
        {
            Physics2D.queriesStartInColliders = false; // is not hit itself

            // Ground and Ceiling
            bool groundHit = Physics2D.CapsuleCast(_colider.bounds.center, _colider.size, _colider.direction, 0,
                Vector2.down, _profile.GrounderDistance, ~_profile.PlayerLayer);
            bool ceilingHit = Physics2D.CapsuleCast(_colider.bounds.center, _colider.size, _colider.direction, 0,
                Vector2.up, _profile.GrounderDistance, ~_profile.PlayerLayer);

            // Hit a Ceiling
            if (ceilingHit) _velocity.y = Mathf.Min(0, _velocity.y);

            // Landed on the Ground
            if (!_isGrounded && groundHit)
            {
                _isGrounded = true;
                _bufferedJumpUsable = true;
                _coyoteUsable = true;
            }
            else if (_isGrounded && !groundHit)  // Left the Ground
            {
                _isGrounded = false;
                _inAirTime = _time;
            }
            
            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        #endregion


        #region Jumping
        private void HandleJump()
        {
            if (!_isReadyToJump && !HasBufferedJump) return;
            if (_isGrounded || CanUseCoyote) ExecuteJump();
            _isReadyToJump = false;
        }

        private void ExecuteJump()
        {
            _velocity.y = _profile.JumpPower;
            _timeJumpStart = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
        }

        #endregion

        #region Movement_Horizontal
        private void HandleMovement()
        {
            if (_input.Move().x == 0)
            {
                var deceleration = _isGrounded ? _profile.GroundDeceleration : _profile.AirDeceleration;
                _velocity.x = Mathf.MoveTowards(_velocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _velocity.x = Mathf.MoveTowards(_velocity.x, _input.Move().x *  (_isInSprint? _profile.SprintSpeed : _profile.MaxSpeed ), _profile.Acceleration * Time.fixedDeltaTime);
            }
        }

        #endregion

        #region Gravity

        private void HandleGravity()
        {
            if (_isGrounded && _velocity.y <= 0f)
            {
                _velocity.y = _profile.GroundingForce;
            }
            else
            {
                var airGravity = _profile.FallAcceleration;

                _velocity.y = Mathf.MoveTowards(_velocity.y, -_profile.MaxFallSpeed,
                    airGravity * Time.fixedDeltaTime);
            }
        }

        #endregion

        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpStart + _profile.JumpBuffer;
        private bool CanUseCoyote => _coyoteUsable && !_isGrounded && _time < _inAirTime + _profile.CoyoteTime;
    }
}