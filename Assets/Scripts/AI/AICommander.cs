using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICommander : BaseObjectScene
{
    [SerializeField]
    protected GameObject _player;
    [SerializeField]
    protected List<CubeEnemyAi> _squadMembers=new List<CubeEnemyAi>();
    protected bool _isPlayerSpotted = false;
    protected bool _battleNotified = false;
    protected int _targetLost = 0;
    
    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < _squadMembers.Count; i++)
        {
            _squadMembers[i].PlayerSpottedEvent += GoIntoBattle;
            _squadMembers[i].PlayerLostEvent += TargetLost;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger Enter: tag="+other.tag+" name"+other.name);
        //if (other.tag == _player.tag) 
        //{
        //    //Debug.Log("Working");
        //    SetupAITarget();
        //}
    }
    protected void SetupAITarget()
    {

        if (_isPlayerSpotted) return;
        for (int i = 0; i < _squadMembers.Count; i++)
        {
            _squadMembers[i].SetTarget(_player);
        }
        _isPlayerSpotted = true;
    }
    protected void TargetLost() 
    {
        _targetLost += 1;
        if (_targetLost >= CheckIsDeadAmmount()) 
        {
            _battleNotified = false;
            //Debug.Log("BattleCanStartAgain");
        }
    }
    protected int CheckIsDeadAmmount() 
    {
        int result = 0;
        for (int i = 0; i < _squadMembers.Count; i++)
        {
            if (!_squadMembers[i].IsDead) 
            {
                result += 1;
            }
        }
        return result;
    }
    protected void GoIntoBattle() 
    {
        if (_battleNotified) return;
        for (int i = 0; i < _squadMembers.Count; i++)
        {
              _squadMembers[i].GoIntoBattle();

        }
        _battleNotified = true;
    }
}
public delegate void OnBeenSpotted();