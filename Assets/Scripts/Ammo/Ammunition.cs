using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ammunition : BaseObjectScene
{
    protected ETeam _team;
    [SerializeField] protected float _damage = 1f;
    [SerializeField] protected ParticleSystem _hitParticle;
    public void SetTeam(ETeam newTeam) 
    {
        _team = newTeam;
    }
    public void SetDamage(float newDamage)
    {
        _damage = newDamage;
    }
}
