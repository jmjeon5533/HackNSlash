using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigScript : MonoBehaviour
{
    public Transform Target;
    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(Target.position + new Vector3(0,0,-2));
    }
}
