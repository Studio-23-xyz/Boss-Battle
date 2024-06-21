using UnityEngine;
using UnityEngine.Serialization;

namespace com.gdcbd.bossbattle.player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerHealth Health;

        [SerializeField]  private PlayerGunManager playerGunManager;
        [SerializeField] private PlayerVisualController _playerVisualController;
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
        private bool _isStartedMoving; // use to player flip toward player moving direction

        private bool _cachedQueryStartInColliders; // ignore itself collision

        private float _time => TimeManager.Instance.TimeCount();
        private float _timeJumpStart;
        private int _jumpCount;

        private float _inAirTimeStamp = float.MinValue;
        private float _dashTime;
        private int _dashCount;
        private bool _bufferedJumpUsable;
        private bool _coyoteUsable;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Health == null) Debug.LogWarning("Player profile not found!", this);
        }
#endif

        #region InputBinding

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
        private void OnJumpPressed()
        {
            _isReadyToJump = true;
            _timeJumpStart = _time;
        }

        private void OnSprintPressed()
        {
            _isInSprint = true;
        }

        private void OnSprintReleased()
        {
            _isInSprint = false;
        }

        private void OnFirePressed()
        {
            playerGunManager.Fire();
            _isInFire = true;
        }

        private void OnFireReleased()
        {
            _isInFire = false;
        }

        private void OnDashPressed()
        {
            if (_dashCount >= Health.MaxDashInAir) return;
            
            _isDashing = true;
            _dashTime = _time;
            var _dashDirection = Mathf.Sign(_input.Move().x); // TODO: dash will be in player forward
            _velocity.x = _dashDirection * Health.DashSpeed;
            _dashCount++;    
            
        }

        private void OnMovePressed()
        {
            _isStartedMoving = true;
            FlipSprite();
        }

        private void OnMoveReleased()
        {
            _isStartedMoving = false;
        }


        private void OnCrouchPressed()
        {
            if (_isGrounded || CanUseCoyote) PerformCrouch();
        }
        private void OnCrouchReleased()
        {
            StandUp();
        }
        #endregion
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _colider = GetComponent<CapsuleCollider2D>();
            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
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
            }
            ApplyForce();
        }

        private void CheckDash()
        {
            if (_time - _dashTime > Health.DashDuration) _isDashing = false;
        }

        private void FlipSprite()
        {
            // TODO: flip sprite x
            if (_input.Move().x > 0 && _gunTransform.localScale.x < 0)
                _gunTransform.localScale = new Vector3(1, 1, 1);
            else if (_input.Move().x < 0 && _gunTransform.localScale.x > 0)
                _gunTransform.localScale = new Vector3(-1, 1, 1);
            Debug.Log($" Character flip!");
        }

        private void ApplyForce()
        {
            _rigidbody.velocity = _velocity;
        }

        #region Collisions

        private void DetectGround()
        {
            Physics2D.queriesStartInColliders = false; // is not hit itself

            // Ground and Ceiling
            bool groundHit = Physics2D.CapsuleCast(_colider.bounds.center, _colider.size, _colider.direction, 0,
                Vector2.down, Health.GrounderDistance, ~Health.PlayerLayer);
            bool ceilingHit = Physics2D.CapsuleCast(_colider.bounds.center, _colider.size, _colider.direction, 0,
                Vector2.up, Health.GrounderDistance, ~Health.PlayerLayer);

            // Hit a Ceiling
            if (ceilingHit) _velocity.y = Mathf.Min(0, _velocity.y);

            // Landed on the Ground
            if (!_isGrounded && groundHit)
            {
                _isGrounded = true;
                _jumpCount = 0;
                _dashCount = 0;
                _bufferedJumpUsable = true;
                _coyoteUsable = true;
            }
            else if (_isGrounded && !groundHit) // Left the Ground
            {
                _isGrounded = false;
                _inAirTimeStamp = _time;
            }

            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        #endregion


        #region Jumping

        private void HandleJump()
        {
            if (!_isReadyToJump && !HasBufferedJump) return;
            if (_isGrounded || CanUseCoyote || _jumpCount < Health.MaxJumpInAir) ExecuteJump();
            _isReadyToJump = false;
        }

        private void ExecuteJump()
        {
            _velocity.y = Health.JumpPower;
            _timeJumpStart = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            _jumpCount++;
        }

        #endregion

        #region Movement_Horizontal

        private void HandleMovement()
        {
            if (_input.Move().x == 0)
            {
                var deceleration = _isGrounded ? Health.GroundDeceleration : Health.AirDeceleration;
                _velocity.x = Mathf.MoveTowards(_velocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _velocity.x = Mathf.MoveTowards(
                    _velocity.x,
                    _input.Move().x * (_isInCrouch ? Health.CrouchSpeed :
                        _isInSprint ? Health.SprintSpeed : Health.Speed),
                    Health.Acceleration * Time.fixedDeltaTime
                );
            }
        }

        #endregion

        #region Gravity

        private void HandleGravity()
        {
            if (_isGrounded && _velocity.y <= 0f)
            {
                _velocity.y = Health.GroundingForce;
            }
            else
            {
                var airGravity = Health.FallAcceleration;

                _velocity.y = Mathf.MoveTowards(_velocity.y, -Health.MaxFallSpeed,
                    airGravity * Time.fixedDeltaTime);
            }
        }

        #endregion

        #region Crouch
        private void PerformCrouch()
        {
            _isInCrouch = true;
            _playerVisualController.Crouch();
        }
        private void StandUp()
        {
            _isInCrouch = false;
            _playerVisualController.StandUp();
        }
        #endregion
        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpStart + Health.JumpBuffer;
        private bool CanUseCoyote => _coyoteUsable && !_isGrounded && _time < _inAirTimeStamp + Health.CoyoteTime;
    }
}