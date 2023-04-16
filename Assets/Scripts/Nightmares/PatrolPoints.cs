using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PatrolPoints : MonoBehaviour
{
    private List<Transform> _points;
    private int _currentIndex;

    private void Awake()
    {
        _points = GameObject.FindGameObjectsWithTag("Patrol Point").Select(o => o.transform).ToList();
        _currentIndex = Random.Range(0, _points.Count);
    }

    public bool HasReached(NavMeshAgent agent)
    {
        return Vector3.Distance(agent.transform.position, _points[_currentIndex].position) <= .5f;
    }

    public Transform GetNext()
    {
        var random = Random.Range(0, _points.Count);
        if (random >= _currentIndex) random = (random + 1) % _points.Count;
        _currentIndex = random;
        
        return _points[_currentIndex];
    }
}
