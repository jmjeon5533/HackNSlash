using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public float MoveSpeed = 5;
    public int Damage;
    private void Start()
    {
        Destroy(gameObject,5);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
    protected virtual void Update()
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, 0.3f, LayerMask.GetMask("Enemy"));
        if (hit.Length > 0)
        {
            hit[0].GetComponent<EnemyBase>().Damaged(Damage);
            Destroy(gameObject);
        }
        else
        {
            transform.Translate(transform.forward * MoveSpeed * Time.deltaTime);
        }
    }
}
