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
    bool isAttack; //°ø°ÝÁß
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }
    void Update()
    {
        if (!isAttack) Movement();
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
            anim.SetInteger("Move", 1);
            var y = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;
            anim.gameObject.transform.rotation =
                Quaternion.RotateTowards(anim.gameObject.transform.rotation,
                Quaternion.Euler(new Vector3(0, y, 0)), RotateSpeed * 10 * Time.deltaTime);
        }
        else
        {
            anim.SetInteger("Move", 0);
        }
        velocity.y = yVelocity;

        cc.Move(velocity * Time.deltaTime);
    }
    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isAttack)
            {
                isAttack = true;
                StartCoroutine(EndAttack());
            }
        }
    }
    IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(1.267f);
        isAttack = false;
        anim.SetBool("Attack", isAttack);
    }
}
