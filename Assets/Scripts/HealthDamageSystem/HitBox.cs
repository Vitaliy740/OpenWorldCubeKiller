using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class HitBox : BaseObjectScene, ISetDamage
{
    [SerializeField]
    private ParticleSystem _applyDamageEffect;
    [SerializeField]
    private ETeam _team;
    [SerializeField]
    private DamageType _applyDamageType=DamageType.All;
    [SerializeField]
    private UnityEvent _onApplyDamageEvent;

    public ETeam Team { get => _team; set => _team = value; }

    public event ApplyDamageDelegate ApplyDamageEvent;
    public event ApplyDamagePosDelegate DirectionDamageEvent;
    public void ApplyDamage(DamageDTO damageData)
    {
        if (damageData.Damage <= 0f || _applyDamageType != DamageType.All && damageData.DealDamageType != _applyDamageType || damageData.Team == Team)
        {
            //Debug.Log("NOTDamageApplyed"+(damageData.Damage <= 0f)+);
            return;
        }
        else 
        {
            Debug.Log("DamageApplyed");
        }
        _onApplyDamageEvent?.Invoke();
        ApplyDamageEvent?.Invoke(damageData.Damage);
        DirectionDamageEvent?.Invoke(damageData.Position);
        if (_applyDamageEffect) 
        {
            Instantiate(_applyDamageEffect, damageData.Position, damageData.Rotation).gameObject.SetActive(true);
        }
        //throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
}
public delegate void ApplyDamageDelegate(float damage);
public delegate void ApplyDamagePosDelegate(Vector3 pos);
