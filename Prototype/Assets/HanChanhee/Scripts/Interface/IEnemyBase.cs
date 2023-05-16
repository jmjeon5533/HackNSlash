using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyBase
{
    void Initilize();
    
    void Death();

    void Damaged(float value);
    
    void Movement();
    
    void Attack();
}
