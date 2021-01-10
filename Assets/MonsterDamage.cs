using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterDamage : MonoBehaviour
{
    public int rightSlapDamage;
    public int leftPunchDamage;

    public float attackRange;

    NavMeshAgent agent;
    GameObject player;
    private void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void RightSlap()
    {
        if(Vector3.Distance(agent.transform.position, player.transform.position) < attackRange)
            player.GetComponent<PlayerHealthController>().DamagePlayer(rightSlapDamage);
    }

    public void LeftPunch()
    {
        if (Vector3.Distance(agent.transform.position, player.transform.position) < attackRange)
            player.GetComponent<PlayerHealthController>().DamagePlayer(leftPunchDamage);
    }

}
