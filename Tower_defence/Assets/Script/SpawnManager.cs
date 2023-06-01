using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] List<GameObject> EnemyList = new List<GameObject>();

    public float SpawnTime;
    private float curTime;
    public float SpawnStartTime;
    bool start = false;

    public UnityEngine.UI.Text WaitSecond;

    [SerializeField] Vector3 SummonPos;

    void Update()
    {
        curTime += Time.deltaTime;
        WaitSecond.enabled = !start;
        if (start)
        {
            if (curTime >= SpawnTime)
            {
                curTime -= SpawnTime;
                Instantiate(EnemyList[Random.Range(0, EnemyList.Count)], SummonPos, Quaternion.identity);
            }
        }
        else
        {
            WaitSecond.text = $"남은 시간 : {SpawnStartTime -(int)curTime}초";
            if(curTime >= SpawnStartTime)
            {
                start = true;
                curTime = 0;
            }
        }

    }
}
