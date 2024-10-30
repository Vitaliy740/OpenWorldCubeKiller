using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : Ammunition
{
    [SerializeField]private float _timeToDestruct = 10f;

    [SerializeField] private float _mass = 0.01f;
    private float _currentDamage;

    protected override void Awake() 
    {
        base.Awake();

        Destroy(InstanceObject, _timeToDestruct);
        _currentDamage = _damage;
        GetRigidbody.mass = _mass;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision " + collision.gameObject.name);
        if (collision.collider.tag == "Bullet") return;
        var DamageSetter=collision.transform.GetComponent<ISetDamage>();
        if (DamageSetter != null)
        {
            DamageSetter.ApplyDamage(new DamageDTO
            {
                Damage = _currentDamage,
                Team = _team,
                DealDamageType = DamageType.Bullet,
                Position = transform.position,
                Rotation = transform.rotation * Quaternion.Euler(180, 180, 180)
            });
        }
        else
        {
            if (_hitParticle)
            {
                Instantiate(_hitParticle, transform.position, transform.rotation * Quaternion.Euler(180, 180, 180));
            }
        }
        Destroy(InstanceObject);
    }
    
}
