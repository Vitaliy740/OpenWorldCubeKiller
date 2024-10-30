using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : BaseObjectScene
{
    [SerializeField]
    private float _normalShapeHealth;
    [SerializeField]
    private float _normalShapeMaxHealth;
    [SerializeField]
    private HitBox _normalHitBox;
    private float _sphereShapeHealth;
    private float _sphereShapeMaxHealth;
    private HitBox _sphereHitBox;
    public UnityEvent DeathEvent;

    public event HeathChangeDelegate HealthChangeEvent;

    public float GetNormalHealthPercent() 
    {
        return _normalShapeHealth / _normalShapeMaxHealth;
    }
    public float GetSpherelHealthPercent()
    {
        return _sphereShapeHealth / _sphereShapeMaxHealth;
    }

    public void AddMaxNormalHealth(float delta) 
    {
        float currentHealthPercent = GetNormalHealthPercent();
        _normalShapeMaxHealth += delta;
        SetNormalHealth( _normalShapeMaxHealth * currentHealthPercent);
    }
    private void SetNormalHealth(float newHealth) 
    {
        _normalShapeHealth = Mathf.Clamp(newHealth, 0f, _normalShapeMaxHealth);
        HealthChangeEvent?.Invoke();
        if (_normalShapeHealth == 0) 
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            DeathEvent?.Invoke();
        }
    }
    private void SetSpherelHealth(float newHealth)
    {
        _sphereShapeHealth = Mathf.Clamp(newHealth, 0f, _sphereShapeMaxHealth);
    }
    private void RecieveDamage(float damage) 
    {
        SetNormalHealth(_normalShapeHealth - damage);
    }
    public void AddMaxSpherelHealth(float delta)
    {
        float currentHealthPercent = GetSpherelHealthPercent();
        _sphereShapeMaxHealth += delta;
        SetSpherelHealth(_normalShapeMaxHealth * currentHealthPercent);
    }
    protected override void Awake()
    {
        base.Awake();
        _normalHitBox.ApplyDamageEvent += RecieveDamage;
    }
    public void AddNormalHealth(float delta) 
    {
        SetNormalHealth(_normalShapeHealth + delta);
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public delegate void HeathChangeDelegate();
