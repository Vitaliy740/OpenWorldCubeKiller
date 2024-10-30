using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapons : BaseObjectScene
{

    #region Serialize Variable 
    [SerializeField]
    protected int _id;
    [SerializeField]
    protected float _damage=1f;
    [SerializeField]
    protected int _startAmmo = 5;
    [SerializeField]
    protected int _maxAmmo = 10;
    // Позиция, из которой будут вылетать снаряды
    [SerializeField] protected Transform _firePoint;
    // Сила выстрела                
    [SerializeField] protected float _force = 500f;
    // Время задержки между выстрелами           
    [SerializeField] protected float _rechargeTime = 0.2f;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected ETeam _team;
    [SerializeField] protected ParticleSystem _muzzleFlashEffect;
    [SerializeField] protected AudioSource _shotSound;
    #endregion
    #region Protected Variable
    protected bool _fire = true;
    protected int _currentAmmo;
    // Флаг, разрешающий выстрел                                               
    #endregion
    #region Virtual Function
    // Функция для вызова выстрела, обязательна во всех дочерних классах
    public bool HasInfiniteAmmo = false;
    public event ShotDelegate ShootEvent;
    public bool IsPicked = false;
    public bool IsMaxAmmo { get { return _currentAmmo >= _maxAmmo; } }
    public int Id { get { return _id; } }
    public int CurrentAmmo { get { return _currentAmmo; } }
    public int MaxAmmp { get { return _maxAmmo; } }
    public abstract void Fire();

    public Transform FirePoint { get { return _firePoint; } }
    protected virtual void OnEnable() 
    {
        if (_animator) 
        {
            _animator.enabled = true;
            _animator?.Play("Switch", 0, 0f);
        }
    }
    #endregion
    protected IEnumerator StartCooldown() 
    {
        yield return new WaitForSeconds(_rechargeTime);
        _fire = true;
    }
    protected override void Awake()
    {
        base.Awake();
        _currentAmmo = _startAmmo;
        if (!_animator) 
        {
            _animator=GetComponent<Animator>();
        }
        if (_animator) 
        {
            _animator.SetFloat("ShootSpeed", 1f / _rechargeTime);
        }
    }
    public void ResetWeaponState()
    {
        _fire = true;
        if (_animator)
        {
            _animator.enabled = false;
        }
    }
    public void AddAmmo(int ammo) 
    {
        _currentAmmo =Mathf.Clamp(_currentAmmo+ ammo,0,_maxAmmo);
    }
    public void AddMaxAmmo(int ammo) 
    {
        _maxAmmo += ammo;
    }
}
public delegate void ShotDelegate();
public interface ISetDamage
{
    void ApplyDamage(DamageDTO damageData);
}
public class DamageDTO 
{
    public float Damage;
    public ETeam Team;
    public DamageType DealDamageType;
    public Vector3 Position;
    public Quaternion Rotation;
}
public enum ETeam 
{
    Player,
    Cubes
}
public enum DamageType 
{
    Bullet,
    Ball,
    All
}
