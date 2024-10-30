using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
//[RequireComponent(typeof(NavMeshAgent))]
public class CubeEnemyAi : BaseObjectScene
{
    [Header("Stats")]
    [SerializeField]
    protected float _health=3f;
    [SerializeField]
    protected float _moveSpeed;
    [Header("Perception")]
    [SerializeField]
    protected EnemyAIState _startState;
    [SerializeField]
    protected float _seePerceptionDistance=10f;
    [SerializeField]
    [Range(0f,180f)]
    protected float _fOV=75f;
    [SerializeField]
    protected float _noticeTimeout;
    [SerializeField]
    protected float _noticeAmmount;
    [SerializeField]
    protected GameObject _target;
    [Header("Patrol")]
    [SerializeField]
    protected float _patrolTimeOut;
    [SerializeField]
    protected List<Transform> _wayPoints=new List<Transform>();
    [Header("BattleParameters")]
    [SerializeField]
    protected LayerMask ignoreLayers;
    [SerializeField]
    protected GameObject _cubeView;
    [SerializeField]
    protected Weapons _weapon;
    [SerializeField]
    protected float _bulletSpread = 0f;
    [SerializeField]
    protected float _stoppingDistance=10f;
    [Range(0,100)]
    [SerializeField]
    protected int _staggerChance = 80;
    [SerializeField]
    protected float _staggerTime = 0.4f;
    [Header("Presentation")]
    [SerializeField]
    protected AIFaceState _startFaceState=AIFaceState.Patroling;
    [SerializeField]
    protected Renderer _faceQuid;
    [SerializeField]
    protected List<EnemyFaceView> _faceViews = new List<EnemyFaceView>();
    [SerializeField]
    protected Animator _anim;

    public UnityEvent DeathEvent;
    [SerializeField]
    protected List<GameObject> _dropItems=new List<GameObject>();
    protected EnemyAIState _currentState;
    protected AIFaceState _currentFaceState;
    protected bool _targetNoticed = false;
    protected NavMeshAgent _agent;
    [SerializeField]
    protected int _wayCount = 0;
    protected float _currentNoticeLvl = 0f;
    protected float _currPatrolTimeout = 0f;
    protected float _currNoticeTimeout = 0f;
    protected float _currentStaggerTime = 0f;
    protected AIFaceState _lastFaceState;
    protected Color _startColor;
    protected Renderer _viewRenderer;
    protected bool _spotedInvoked = false;
    protected Dictionary<AIFaceState, Material> _faceMatDict = new Dictionary<AIFaceState, Material>();
    protected bool _isDead=false;
    public bool TargetNoticed { get => _targetNoticed; private set { _targetNoticed = value; } }
    public bool IsDead { get => _isDead; private set { _isDead = value; } }
    public event OnBeenSpotted PlayerSpottedEvent;
    public event OnBeenSpotted PlayerLostEvent;
    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
        if (_agent)
            _agent.speed = _moveSpeed;
        _currentState = _startState;
        _viewRenderer = _cubeView.GetComponent<Renderer>();
        _viewRenderer.material.EnableKeyword("_EMISSION");
        _startColor = _viewRenderer.material.GetColor("_EmissionColor");
        _currentFaceState = _startFaceState;
        foreach (var item in _faceViews)
        {
            _faceMatDict.Add(item.FaceState, item.FaceMaterial);
        }
        _cubeView.GetComponent<HitBox>().ApplyDamageEvent += RecieveDamage;

        //Debug.Log(_startColor);

    }
    protected virtual void Update()
    {
        _viewRenderer.material.SetColor("_EmissionColor", Color.Lerp(_startColor, new Color(2.828f, 0f, 0f, 1f), _currentNoticeLvl / 100f));
        if (Stagger()) return;
        PerceptTheEnviroment();
        CalculateTheBehavior();
    }
    protected virtual void CalculateTheBehavior() 
    {
        if (_currentState == EnemyAIState.Patrol && _wayPoints.Count >= 2 && !TargetNoticed)
        {

            _agent.stoppingDistance = 0;
            if (_wayPoints != null)
            {
                _agent.SetDestination(_wayPoints[_wayCount].position);
            }
            if (!_agent.hasPath)
            {
                _currPatrolTimeout += Time.deltaTime;
                if (_currPatrolTimeout > _patrolTimeOut)
                {
                    _currPatrolTimeout = 0;
                    GenerateNextWayPoint();
                }
            }
        }
        else if (TargetNoticed)
        {
            if (_target)
            {
                _currentFaceState = AIFaceState.Fighting;
                _agent.stoppingDistance = _stoppingDistance;
                _agent.SetDestination(_target.transform.position);
                Vector3 targetPos = new Vector3(_target.transform.position.x, _cubeView.transform.position.y, _target.transform.position.z);
                Vector3 shootRandPos = new Vector3(
                    _target.transform.position.x + Random.Range(-_bulletSpread, _bulletSpread),
                    _target.transform.position.y,
                    _target.transform.position.z + Random.Range(-_bulletSpread, _bulletSpread));
                transform.LookAt(targetPos);
                _weapon.FirePoint.LookAt(shootRandPos);
                if (!CheckIfBlocked())
                {

                    _weapon.Fire();
                }
                else
                {
                    if (!_agent.hasPath)
                    {
                        MoveToRandomPoint();
                    }
                }
                SetFace();
            }

        }
    }
    protected virtual void PerceptTheEnviroment() 
    {
        if (_target != null)
        {
            var dis = Vector3.Distance(Position, _target.transform.position);
            if (dis < _seePerceptionDistance)
            {
                if (Vector3.Angle(_cubeView.transform.forward, _target.transform.position - Position) <= _fOV)
                {
                    if (!CheckIfBlocked())
                    {
                        if (_currentNoticeLvl < 100f)
                        {
                            _currentNoticeLvl += _noticeAmmount * Time.deltaTime;
                        }
                        _currNoticeTimeout = _noticeTimeout;
                        //_targetNoticed = true;

                        //_currentState = EnemyAIState.Battle;
                    }
                }

            }
            if (_currNoticeTimeout > 0)
            {
                _currNoticeTimeout -= Time.deltaTime;
            }
            else if (_currentNoticeLvl > 0f)
            {
                _currentNoticeLvl -= _noticeAmmount * Time.deltaTime;
            }
            if (_currentNoticeLvl >= 100f)
            {
                TargetNoticed = true;
                _currentFaceState = AIFaceState.Fighting;
                _currentState = EnemyAIState.Battle;
                SetFace();
                //Debug.Log("Target Noticed");
                PlayerSpottedEvent?.Invoke();
            }
            else if (_currentNoticeLvl <= 0f)
            {
                //Debug.Log("Target Lost");
                TargetNoticed = false;

                _currentState = _startState;
                _currentFaceState = AIFaceState.Patroling;
                SetFace();
                _cubeView.transform.rotation = transform.rotation;
                PlayerLostEvent?.Invoke();
            }
            //Debug.Log(_currentNoticeLvl);

        }
    }
    protected virtual bool Stagger() 
    {
        if (_currentFaceState == AIFaceState.Staggered)
        {
            _currentStaggerTime += Time.deltaTime;
            if (_currentStaggerTime >= _staggerTime)
            {
                _currentFaceState = _lastFaceState;
                if(_agent)
                    _agent.isStopped = false;
                SetFace();
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }
    private void MoveToRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere;
        //randomDirection += transform.position;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, 10f, NavMesh.AllAreas);
        Debug.DrawRay(transform.position, randomDirection*10,Color.green,7f);
        _agent.SetDestination(navHit.position);
    }
    private void SetFace() 
    {
        //Debug.Log("SettingFace: " + _currentFaceState);
        
        _faceQuid.material = _faceMatDict[_currentFaceState];
    }
    protected bool CheckIfBlocked()
    {
        RaycastHit hit;
        //Debug.DrawLine(Position, _target.transform.position, Color.red,7f);

        if(Physics.Linecast(Position,_target.transform.position,out hit,~ignoreLayers)) 
        {
            if (hit.transform.tag == _target.transform.tag) 
            {
                return false;
            }
        }
        return true;
    }
    protected void GenerateNextWayPoint() 
    {
        if (_wayCount < _wayPoints.Count - 1) 
        {
            _wayCount += 1;
        }
        else 
        {
            _wayCount = 0;
        }
    }
    public virtual void RecieveDamage(float damage)
    {
        if (_isDead) return;
        _health -= damage;
        if (Random.Range(0, 100) <= _staggerChance)
        {
            //Debug.Log("Staggered");
            _currentStaggerTime = 0f;
            _lastFaceState = _currentFaceState;
            _currentFaceState = AIFaceState.Staggered;
            if (_agent)
            {
                _agent.isStopped = true;
            }
            SetFace();
        }

        if (_health <= 0f) 
        {
            _isDead = true;

            if (_anim != null)
            {
                _anim?.Play("Death", 0, 0f);
            }
            _currentFaceState = AIFaceState.Dead;
            SetFace();
            this.enabled = false;
            DeathEvent?.Invoke();
            for (int i = 0; i < _dropItems.Count; i++)
            {
                Instantiate(_dropItems[i], transform.position, transform.rotation);
            }
            //Debug.Log(this.enabled);

        }
        if (_currentState != EnemyAIState.Battle) 
        {
            GoIntoBattle();
            //PlayerSpottedEvent?.Invoke();
        }

    }
    public void GoIntoBattle() 
    {
        Debug.Log("ToBattle");
        //if (_currentState != EnemyAIState.Battle)
        //{
            _currentNoticeLvl = 100f;
            _currNoticeTimeout = _noticeTimeout;
        
        //}
    }
    public void SetTarget(GameObject newTarget) 
    {
        Debug.Log("Target been setted");
        _target = newTarget;

    }
}
[System.Serializable]
public class EnemyFaceView 
{
    public AIFaceState FaceState;
    public Material FaceMaterial;
}
public enum AIFaceState 
{
    Patroling,
    Fighting,
    Staggered,
    Dead
}
public enum EnemyAIState 
{
    Idle,
    Patrol,
    Curious,
    Battle
}
