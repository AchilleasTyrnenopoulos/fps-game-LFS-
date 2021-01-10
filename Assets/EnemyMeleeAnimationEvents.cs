using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeAnimationEvents : MonoBehaviour
{

    public void AgentStop()
    {
        GetComponentInParent<NavMeshAgent>().isStopped = true;
        GetComponentInParent<EnemyMeleeController>().attacking = true;
    }

    public void AgentResume()
    {
        GetComponentInParent<NavMeshAgent>().isStopped = false;
        GetComponentInParent<EnemyMeleeController>().attacking = false;
    }
}
