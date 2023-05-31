using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private int HP;
    NavMeshAgent agent;
    Transform target;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        target = BuildingSystem.current.player.gameObject.transform;
    }
    private void Update()
    {
        agent.SetDestination(target.position);
    }
    public void Damaged(int Damage)
    {
        HP -= Damage;
        if(HP <= 0) Destroy(gameObject);
    }
}
