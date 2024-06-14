using UnityEngine;

namespace BossBattle
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerProfile _profile;
        private Rigidbody2D _rigidbody;
        private CapsuleCollider2D _colider;
        private InputManager _input;

        private Vector2 _velocity;
        private bool _isGrounded;
        private bool _isReadyToJump;  
        private bool _isInSprint;
        private bool _isInFire;
        
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
            }
        }
        private void OnJumpPressed() =>_isReadyToJump = true;
        private void OnSprintPressed() => _isInSprint = true;

        private void OnSprintReleased() => _isInSprint = false;

        private void OnFirePressed() => _isInFire = true;
        private void OnFireReleased()  => _isInFire = false;
        

        #endregion

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _colider = GetComponent<CapsuleCollider2D>();
        }

        private void FixedUpdate()
        {
            DetectGround();
            HandleJump();
            HandleMovement();
            HandleGravity();
            ApplyForce();
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
                _isGrounded = true;
            // Left the Ground
            else if (_isGrounded && !groundHit) _isGrounded = false;
        }

        #endregion


        #region Jumping
        private void HandleJump()
        {
            if (!_isReadyToJump) return;
            if (_isGrounded) ExecuteJump();
            _isReadyToJump = false;
        }

        private void ExecuteJump()
        {
            _velocity.y = _profile.JumpPower;
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
                _velocity.x = Mathf.MoveTowards(_velocity.x, _input.Move().x * _profile.MaxSpeed * (_isInSprint? 2: 1), _profile.Acceleration * Time.fixedDeltaTime);
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

      
    }
}