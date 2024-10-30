using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathTest : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private NavMeshPath _path;
    private float _elapsed = 0.0f;
    private void Start()
    {
        _path = new NavMeshPath();
        _elapsed = 0.0f;
        var agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(Vector3.zero);
    }
    private void Update()
    {
        _elapsed += Time.deltaTime;
        if (_elapsed > 1.0f)
        {
            _elapsed -= 1.0f;
            NavMesh.CalculatePath(transform.position, Vector3.zero, NavMesh.AllAreas, _path);
        }
        for (int i = 0; i < _path.corners.Length - 1; i++)
            Debug.DrawLine(_path.corners[i], _path.corners[i + 1], Color.red);
        if (_path.corners.Length >= 2)
        {
            // path.corners[1] Ближайшая точка к нам
        }
    }
}
