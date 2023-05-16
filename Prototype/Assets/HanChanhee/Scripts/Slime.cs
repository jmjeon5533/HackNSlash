using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Slime : EnemyBase
{
    Animator animator;
     NavMeshAgent agent;

    float updatePathTime = 0.2f;
    float curPathTime = 0;
    float updateCatchTime = 3f;
    float curCatchTime = 0;
    float updateAttackTime = 1.5f;
    float curAttackTime = 0;

    [SerializeField] List<AnimationClip> attackClip = new List<AnimationClip>();

    
    public override void Attack()
    {
        if (Vector3.Distance(transform.position, target.position) > attackRange)
        {
            ChangeState(EnemyState.Move);
            curAttackTime = 0;
            return;
        }
        curAttackTime += Time.deltaTime;
        agent.SetDestination(transform.position);
            transform.LookAt(target);
        if(curAttackTime > updateAttackTime)
        {
            curAttackTime = 0;
            animator.SetInteger("Attack", Random.Range(1, 2));

            
        }
    }

    public override void Damaged(float value)
    {
        hp -= value;
        ChangeState(EnemyState.Damaged);
        
        if (hp < 0)
        {
            hp = 0;
        }

    }

    void UpdateState()
    {
        switch(enemyState)
        {
            case EnemyState.Idle:
                animator.SetInteger("State", 0);
                agent.SetDestination(transform.position);
                
                CheckPlayer();
                break; 
            case EnemyState.Move:
                animator.SetInteger("State", 2);
                CheckAttackRange();
                Movement();
                
                break;
            case EnemyState.Catch:
                animator.SetInteger("State", 4);
                animator.SetInteger("Sense", 1);
                Catch();
                break;
            case EnemyState.Attack:
                animator.SetInteger("State", 1);
                Attack();
                break;
            case EnemyState.Damaged:
                animator.SetInteger("State", 3);
                break;
            default:
                break;
        }
    }

    public override void Death()
    {

    }

    public void CheckPlayer()
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, range, whatIsPlayer);
        if(hit != null)
        {
            foreach(Collider c in hit)
            {
                target = c.transform;
                ChangeState(EnemyState.Move);
            }
        }
    }

    public override void ChangeState(EnemyState state)
    {
        base.ChangeState(state);
        
    }

    public override void Initilize()
    {
        base.Initilize();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void CheckAttackRange()
    {
       if(Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            ChangeState(EnemyState.Attack);
        }
    }

    void Catch()
    {
        curCatchTime += Time.deltaTime;
        agent.SetDestination(transform.position);
        CheckPlayer();
        if(curCatchTime >= updateCatchTime)
        {
            curCatchTime = 0;
            animator.SetInteger("Sense", 0);
            ChangeState(EnemyState.Idle);
            
        }
    }

    

    public override void Movement()
    {
        if(Vector3.Distance(transform.position, target.position) > range * 1.5f)
        {
            ChangeState(EnemyState.Catch);
            curPathTime = 0;
            return;
        }
        curPathTime += Time.deltaTime;
        if(curPathTime > updatePathTime)
        {
            curPathTime = 0;
            agent.SetDestination(target.position);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Initilize();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }
}