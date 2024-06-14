using System;
using DG.Tweening;
using UnityEngine;

namespace BossBattle.Utility
{
    public class ParryObjectController2 : MonoBehaviour
    {
        private Collider2D objectCollider;
        private Rigidbody2D rigidbody2D;

        private void Start()
        {
            objectCollider = GetComponent<Collider2D>();
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player") )
            {
                Vector2 collisionDirection = GetCollisionDirection(other);
                Debug.Log($"Collision Direction: {collisionDirection}");

                if (collisionDirection == Vector2.up)
                {
                    DOVirtual.DelayedCall(0.1f, () =>
                    {
                        objectCollider.enabled = false;
                        transform.DOScale(Vector3.zero, 1.0f).OnComplete(() =>
                        {
                            Destroy(gameObject);
                        });
                    });
                    other.gameObject.GetComponent<PlayerVisualController>().PowerUP();
                }
                else
                {
                    rigidbody2D.isKinematic = false;
                    other.gameObject.GetComponent<PlayerVisualController>().Dead();
                }
            }else if (other.gameObject.CompareTag("Projectile"))
            {
                rigidbody2D.isKinematic = false;
            }
        }

        private Vector2 GetCollisionDirection(Collision2D collision)
        {
            ContactPoint2D contactPoint = collision.contacts[0];
            Vector2 collisionNormal = contactPoint.normal;

            return collisionNormal.y < 0 ? Vector2.up : Vector2.down;
        }
    }
}