using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 100f;
    [SerializeField] private float lifeTime = 2f;

    [SerializeField] private Rigidbody2D _rigidbody; 

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void Throw(Vector2 direction)
    {
        if (_rigidbody != null)
        {
            _rigidbody.velocity = direction * projectileSpeed;
        }
    }
}