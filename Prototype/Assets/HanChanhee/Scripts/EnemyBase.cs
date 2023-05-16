using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle, Follow, Attack, Dead, Damaged, Catch, Move
}

public abstract class EnemyBase : MonoBehaviour, IEnemyBase
{
    public float atkDamage;

    public float hp;

    public float maxHp;

    protected Vector3 targetPos;

    public Transform target;

    public EnemyState enemyState;

    public LayerMask whatIsPlayer;

    public float range = 6;

    public float attackRange = 3;
        
    

    // Start is called before the first frame update
    public void Start()
    {
       
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Initilize()
    {
        whatIsPlayer = LayerMask.GetMask("Player");
        enemyState = EnemyState.Idle;
        maxHp = hp;
    }

    public virtual void ChangeState(EnemyState state)
    {
        if (enemyState == state) return;
        enemyState = state;
    }

    public abstract void Attack();

    public abstract void Damaged(float value);

    public abstract void Death();

    public abstract void Movement();
    
}
