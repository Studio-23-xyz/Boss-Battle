using System;
using DG.Tweening;
using UnityEngine;

namespace com.gdcbd.bossbattle.Scripts.Components
{
    [CreateAssetMenu(fileName = "NewProjectileController", menuName = "BossBattle/ProjectileController")]
    public class ProjectileController : AbstractProjectileController
    {
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private float _speed = 200f;
        [SerializeField] private float _damage;
        [SerializeField] private float _lifeSpan = 2f;
       
        
       
        public override void Launch(ShootInfo shootInfo)
        {
            if (_projectilePrefab != null)
            {
                GameObject projectileInstance = Instantiate(_projectilePrefab,  shootInfo.startTransform);
                projectileInstance.transform.parent = null;
                Rigidbody2D rb = projectileInstance.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                  //  rb.linearVelocity = Vector2.forward * _speed;
                  rb.velocity = shootInfo.directon * _speed;
                }
                
            }
        }
        
        
    }

    
   
    
}