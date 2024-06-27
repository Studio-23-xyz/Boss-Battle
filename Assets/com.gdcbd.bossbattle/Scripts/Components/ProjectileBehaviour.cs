using UnityEngine;
using UnityEngine.Serialization;

namespace com.gdcbd.bossbattle.components
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ProjectileBehaviour : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        public float lifeSpan = 2f;
        private float _time => TimeManager.Instance.TimeCount();
        private float _timer;
        
        void OnEnable()
        {
            _timer = TimeManager.Instance.TimeCount();
        }

        void Update()
        {
            if (_timer + lifeSpan <= _time)
            {
                DisableProjectile();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                DisableProjectile();
                Debug.Log("Bullet hit player!");
            }
        }

        public void AddThrust(Vector2 velocity)
        {
            if (rb != null)
            {
                //  rb.linearVelocity = Vector2.forward * _speed;
                rb.velocity = velocity;
            }
        }
        private void DisableProjectile()
        {
            AddThrust(Vector2.zero);
            gameObject.SetActive(false);  
           
            
        }
    }
}