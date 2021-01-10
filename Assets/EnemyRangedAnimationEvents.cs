using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRangedAnimationEvents : MonoBehaviour
{

    public void AgentStop()
    {
        //GetComponentInParent<NavMeshAgent>().isStopped = true;
        GetComponentInParent<EnemyRangedController>().attacking = true;
    }

    public void AgentResume()
    {
        //GetComponentInParent<NavMeshAgent>().isStopped = false;
        GetComponentInParent<EnemyRangedController>().attacking = false;
    }
}
