using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var hb = other.GetComponent<HitBox>();

        hb.ApplyDamage(new DamageDTO { Damage = 100000f, Team = ETeam.Cubes,DealDamageType=DamageType.Bullet,Position=transform.position,Rotation=transform.rotation});
    }
}
