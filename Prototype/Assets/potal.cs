using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potal : MonoBehaviour
{
    public Transform targeting;
    void Start()
    {
        
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, targeting.position - transform.position,Color.red);
    }

    private void Update()
    {
        var hits = Physics.OverlapBox(transform.position, transform.lossyScale * 1.2f, transform.rotation, LayerMask.GetMask("Player"));
        if (hits.Length > 0)
        {
            hits[0].GetComponent<CharacterController>().enabled = false;
            hits[0].transform.position = targeting.position + targeting.up * 2;
            hits[0].GetComponent<CharacterController>().enabled = true;
        }
    }
}
