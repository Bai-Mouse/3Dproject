using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 2f;
    public float JumpForce = 10f;
    public bool onGround;
    public float gravityScale = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        
        Vector3 aimDir=transform.TransformDirection(GetMovement());

        rb.MovePosition(transform.position+ aimDir * speed*Time.deltaTime);
        rb.AddForce(Jump() * JumpForce);
        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);
        onGround = false;
    }
    Vector3 GetMovement()
    {
        float xmove = Input.GetAxis("Horizontal");
        float zmove = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(xmove, 0, zmove);
        
        Debug.DrawRay(transform.position, rb.velocity, Color.yellow);
        Debug.DrawRay(transform.position, transform.TransformDirection(dir * 2), Color.red);
        return dir;
    }
    Vector3 Jump()
    {
        if (onGround)
            return Vector3.up * JumpForce * Input.GetAxisRaw("Jump");
        else
            return Vector3.zero;
        
    }
    private void OnCollisionStay(Collision collision)
    {
        if(rb.velocity.y<=0)
        onGround = true;
    }

}
