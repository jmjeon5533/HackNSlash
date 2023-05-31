using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] List<GameObject> EnemyList = new List<GameObject>();

    public float SpawnTime;
    private float curTime;

    [SerializeField] Vector3 SummonPos;

    void Update()
    {
        curTime += Time.deltaTime;
        if(curTime >= SpawnTime){
            curTime -= SpawnTime;
            Instantiate(EnemyList[Random.Range(0,EnemyList.Count)],SummonPos,Quaternion.identity);
        }
    }
}
