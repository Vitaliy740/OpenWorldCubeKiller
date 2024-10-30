using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHelicopterAI : CubeEnemyAi
{
    [SerializeField]
    protected Weapons _rocketLauncher;

    public GameObject Loot;
    protected override void Awake()
    {
        base.Awake();
        _targetNoticed = true;
        _currentNoticeLvl = 100f;
        _lastFaceState = AIFaceState.Fighting;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    protected override void CalculateTheBehavior()
    {
        //Debug.Log("CalculateStart");
        //base.CalculateTheBehavior();
        if (Vector3.Distance(_wayPoints[_wayCount].position, transform.position) < 0.1f) 
        {
            _currPatrolTimeout += Time.deltaTime;
            if (_currPatrolTimeout > _patrolTimeOut)
            {
                _currPatrolTimeout = 0f;
                GenerateNextWayPoint();
            }
        }
        transform.position= Vector3.MoveTowards(transform.position, _wayPoints[_wayCount].position, _moveSpeed * Time.deltaTime);
        if (Stagger()) return;
        //Debug.Log("NotStaggered");
        Vector3 targetPos = new Vector3(_target.transform.position.x,
            _cubeView.transform.position.y,
            _target.transform.position.z);
        Vector3 shootRandPos = new Vector3(
            _target.transform.position.x + Random.Range(-_bulletSpread, _bulletSpread),
            _target.transform.position.y,
            _target.transform.position.z + Random.Range(-_bulletSpread, _bulletSpread));
        transform.LookAt(_target.transform.position);
        _weapon.FirePoint.LookAt(shootRandPos);
        //_rocketLauncher.FirePoint.LookAt(shootRandPos);
        if (!CheckIfBlocked())
        {
            //Debug.Log("Not Blocked");
            _weapon.Fire();
           // _rocketLauncher.Fire();
        }
    }
    // Update is called once per frame
    protected override void Update()
    {
        _viewRenderer.material.SetColor("_EmissionColor", Color.Lerp(_startColor, new Color(2.828f, 0f, 0f, 1f), _currentNoticeLvl / 100f));
        CalculateTheBehavior();
    }
    public override void RecieveDamage(float damage)
    {
        base.RecieveDamage(damage);
        if (_isDead) 
        {
            Loot.SetActive(true);
            StartCoroutine(DeathAnim());
        }
    }
    private IEnumerator DeathAnim() 
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        
    }
}
