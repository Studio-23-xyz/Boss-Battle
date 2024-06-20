using System;
using com.gdcbd.bossbattle.Scripts.Components;
using UnityEngine;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class PlayerGunManager : MonoBehaviour
{
    [SerializeField] private Transform _playerGunContainer;
    [SerializeField] private GunController _sampleGun1;
    private Transform _turretTransform;

    private void Start()
    {
        SetupGun();
        // TODO : Object pool, gun reload time, bullet collision and damage
    }

    public void SetupGun()
    {
          var visibleGun = Instantiate(_sampleGun1._gunPrefab, _playerGunContainer);
          _turretTransform = visibleGun.transform.Find("Turret");
         
    }
    public void Fire()
    {
        if (_sampleGun1 != null) _sampleGun1.Shoot(new ShootInfo(_turretTransform, _turretTransform.right * _playerGunContainer.localScale.x));
    }

    public void Reload()
    {
        _sampleGun1.Reload();
    }
}
