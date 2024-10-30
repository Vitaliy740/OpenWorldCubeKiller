using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapons
{
    [SerializeField] protected PistolBullet _projectile;
    protected override void Awake()
    {
        base.Awake();
        //_projectile.SetTeam(_team);
    }
    public override void Fire()
    {
        if (_fire && _currentAmmo>0) 
        {
            if (!_projectile) return;
            var Projectile=Instantiate(_projectile, _firePoint.transform.position, _firePoint.transform.rotation);
            if (Projectile != null)
            {
                Projectile.GetRigidbody.velocity = Vector3.zero;
                Projectile.GetRigidbody.angularVelocity = Vector3.zero;
                Projectile.GetRigidbody.rotation = Quaternion.Euler(Vector3.zero);
                Projectile.GetRigidbody.AddForce(_firePoint.forward * _force, ForceMode.VelocityChange);
                Projectile.SetDamage(_damage);
                Projectile.SetTeam(_team);
                if (_animator) 
                {
                    _animator.Play("Shoot",0,0f);
                }
                if (_muzzleFlashEffect) 
                {
                    _muzzleFlashEffect.Play();
                }
                if (_shotSound) 
                {
                    _shotSound.Play();
                }
                _fire = false;
                StartCoroutine(StartCooldown());
                if (!HasInfiniteAmmo)
                {
                    _currentAmmo -= 1;
                }
            }
        }
    }
   /* private void Update()
    {
        if (FindObjectOfType<PlayerInputs>().shoot)
        {
            _animator.Play("Shoot", 0, 0f);
        }
    }*/


}
