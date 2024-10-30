using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthStationInteraction : Interaction
{

    [SerializeField]
    private float _healthContainer=100f;
    [SerializeField]
    private float _recieveHealthPerSecond = 25f;

    private PlayerHealth _normalHealth;
    private PlayerHealth _ballHealth;


    public override void Interact()
    {
        if (_healthContainer > 0f && (_normalHealth.GetNormalHealthPercent()<1f||_ballHealth.GetNormalHealthPercent()<1f))
        {
            _normalHealth.AddNormalHealth(_recieveHealthPerSecond * Time.deltaTime);
            _ballHealth.AddNormalHealth(_recieveHealthPerSecond * Time.deltaTime);
            _healthContainer -= _recieveHealthPerSecond * Time.deltaTime;
        }
    }
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        _normalHealth = GameObject.Find("PlayerCharacter").GetComponent<PlayerHealth>();
        _ballHealth = GameObject.Find("PlayerCharacter").GetComponentInChildren<Ball>(true).GetComponent<PlayerHealth>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
