using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereDamager : MonoBehaviour
{
    [SerializeField] private float _standartDamage;
    private float _currentDamage=0f;
    private SphereCollider _damageCollider;
    // Start is called before the first frame update
    void Start()
    {
        _damageCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    public void StartDamage(float magnitude) 
    {
        StopCoroutine(DamageCooldown());
        _damageCollider.enabled = true;
        _damageCollider.radius = magnitude;
        _currentDamage = magnitude * _standartDamage;
        StartCoroutine(DamageCooldown());
    }
    private void OnTriggerEnter(Collider other)
    {
        var hitb = other.GetComponent<HitBox>();
        if (hitb) 
        {
            DamageDTO damageDTO = new DamageDTO { Damage = _currentDamage, DealDamageType = DamageType.Ball, Team = ETeam.Player, Position = other.transform.position };
            hitb.ApplyDamage(damageDTO);
        }
    }
    void Update()
    {
        
    }
    private IEnumerator DamageCooldown() 
    {
        yield return new WaitForSeconds(0.2f);
        _currentDamage = 0;
        
    }
}
