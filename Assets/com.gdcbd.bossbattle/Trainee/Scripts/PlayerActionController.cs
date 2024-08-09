using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionController : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Jumping")]
    public float jumpSpeed;
    public InputActionReference jump;
    int maxJump = 2;
    int jumpRemaining;

    [Header("GroundCheck")]
    public Transform groundPos;
    public Vector2 groundCheckSize = new Vector2 (0.5f, 0.5f);
    public LayerMask groundLayerMask;

    [Header("Shoot")]
    public InputActionReference shoot;
    public GameObject playerbulletPrefab;
    public Transform playerfirePoint;
    public float playerbulletSpeed = 10f;
    public Transform boss;

    bool isParryTouched = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
         
    }

    void Update()
    {
        GroundedCheck();
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpRemaining > 0)
        {
            if (context.performed)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                jumpRemaining--;
            }
            else if (context.canceled)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                jumpRemaining--;
            }
        }
    }
    private void GroundedCheck()
    {
        if (Physics2D.OverlapBox(groundPos.position, groundCheckSize, 0, groundLayerMask))
        {
            jumpRemaining = maxJump;
        }
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundPos.position, groundCheckSize);
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (boss == null) return;
        GameObject firebullet = Instantiate(playerbulletPrefab, playerfirePoint.position, playerfirePoint.rotation);
        Rigidbody2D firebulletRb = firebullet.GetComponent<Rigidbody2D>();

        // Calculate the direction to the boss
        Vector2 direction = (boss.position - playerfirePoint.position).normalized;
        firebulletRb.velocity = direction * playerbulletSpeed;

        //parry
        if(isParryTouched && jumpRemaining == 0)
        {
            firebullet.transform.DOScale(new Vector2(2, 2), 0.5f);
            isParryTouched = false;
        }
    }

    public void TouchedParry()
    {
        isParryTouched = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactable.interact(gameObject);
        }
    }
}
