using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public int damage;
    public float rate;
    private BoxCollider meleeArea;
    [SerializeField] private TrailRenderer Trail;
    public bool isAttack;
    public bool hit = false;

    private void Start()
    {
        meleeArea = GetComponent<BoxCollider>();
    }

    public void Use()
    {
        isAttack = true;
        hit = false;
        StartCoroutine("Swing");
    }
    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        Trail.enabled = true;
        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;
        Trail.enabled = false;
        isAttack = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !hit && meleeArea.enabled)
        {
            other.GetComponent<IEnemyBase>().Damaged(damage);
            hit = true;
        }
    }
}
