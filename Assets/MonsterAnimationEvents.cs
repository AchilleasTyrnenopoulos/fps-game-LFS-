using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAnimationEvents : MonoBehaviour
{
    public void AgentStop()
    {
        GetComponentInParent<NavMeshAgent>().isStopped = true;
        GetComponentInParent<MonsterController>().attacking = true;
    }

    public void AgentResume()
    {
        GetComponentInParent<NavMeshAgent>().isStopped = false;
        GetComponentInParent<MonsterController>().attacking = false;
    }
}
