using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;
    public float gravityScale;
    [SerializeField] Animator anim;
    CharacterController cc;

    private float yVelocity;
    [SerializeField] float RotateSpeed;

    public WeaponBase WeaponScript;
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }
    void Update()
    {
        if (!WeaponScript.isAttack) Movement();
        Attack();
    }
    void Movement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        var velocity = new Vector3(h, 0, v).normalized * MoveSpeed;

        if (!cc.isGrounded) yVelocity -= 9.8f * gravityScale * Time.deltaTime;
        else yVelocity = 0;

        if (Vector3.Magnitude(velocity) > 1)
        {
            anim.SetBool("IsWalk",true);
            var y = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation,
                Quaternion.Euler(new Vector3(0, y, 0)), RotateSpeed * 10 * Time.deltaTime);
        }
        else
        {
            anim.SetBool("IsWalk", false);
        }
        velocity.y = yVelocity;

        cc.Move(velocity * Time.deltaTime);
    }
    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!WeaponScript.isAttack)
            {
                anim.SetTrigger("Attack");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // ���콺 Ŭ�� ��ġ���� ���� ���������� ���� ���͸� ���մϴ�.
                    Vector3 direction = hit.point - transform.position;
                    direction.y = 0; // ���� ������ y ������ 0���� ����ϴ�.

                    // ���� ���͸� �������� ĳ���͸� ȸ����ŵ�ϴ�.
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = targetRotation;
                }
                WeaponScript.Use();
            }
        }
    }
}
