using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    
    void Update()
    {
        agent.SetDestination(target.position);
    }
}
