using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScript : MonoBehaviour
{
    [SerializeField] Transform player;

    public float MoveSpeed;

    private void Update()
    {
        transform.position = 
            Vector3.Lerp(transform.position, new Vector3(0, 10, -7) + player.transform.position, MoveSpeed * Time.deltaTime);
    }
}
