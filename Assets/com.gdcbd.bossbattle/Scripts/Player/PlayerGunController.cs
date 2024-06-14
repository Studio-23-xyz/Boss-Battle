using System;
using UnityEngine;

public class PlayerGunController : MonoBehaviour
{
    [SerializeField] private ProjectileController _projectileController;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform _body;
    
    
    public void Fire()
    {
        if (_projectileController == null || firePoint == null) return;
        ProjectileController newProjectile = Instantiate(_projectileController.gameObject, firePoint.position, firePoint.rotation).GetComponent<ProjectileController>();
        newProjectile.Throw( firePoint.right * _body.localScale.x);
    }
}
