using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] JoyStick LjoyStick;
    [SerializeField] JoyStick RjoyStick;

    [SerializeField] float MoveSpeed;
    [SerializeField] int Damage;
    Rigidbody rigid;
    float CamSpeed = 5f;
    [SerializeField] float AttackCoolTime;
    [SerializeField] Transform FirePos;
    [SerializeField] GameObject BulletPrefab;
    float CurAttackTime;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        camPos();
        Attack();
    }
    void camPos()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, transform.position + new Vector3(0, 10, -3.5f), Time.deltaTime * CamSpeed);
    }
    void Movement()
    {
        var joyStick = LjoyStick;
        rigid.velocity = new Vector3(joyStick.Value.x, 0, joyStick.Value.y) * (MoveSpeed * 100) * Time.deltaTime;

        if (Vector3.Magnitude(RjoyStick.Value) > 0.3f)
        {
            var y = Mathf.Atan2(RjoyStick.Value.y, RjoyStick.Value.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(0, -y, 0);
        }
    }
    void Attack()
    {
        if (Vector3.Magnitude(RjoyStick.Value) > 0.3f)
        {
            CurAttackTime += Time.deltaTime;
            if (CurAttackTime >= AttackCoolTime)
            {
                CurAttackTime -= AttackCoolTime;
                var bullet = Instantiate(BulletPrefab, FirePos.position, Quaternion.Euler(new Vector3(0,transform.eulerAngles.y,0)/2));
                bullet.GetComponent<BulletBase>().Damage = Damage;
            }
        }
    }
}
