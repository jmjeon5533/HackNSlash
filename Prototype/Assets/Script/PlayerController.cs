using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;
    public float gravityScale;
    Animator anim;
    CharacterController cc;

    private float yVelocity;
    void Start()
    {
        //anim = transform.GetChild(0).GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }
    void Update()
    {
        Movement();
    }
    void Movement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        var velocity = new Vector3(h, 0, v).normalized * MoveSpeed;

        if (!cc.isGrounded) yVelocity -= 9.8f * gravityScale * Time.deltaTime;
        else yVelocity = 0;

        velocity.y = yVelocity;

        cc.Move(velocity * Time.deltaTime);
    }
}
