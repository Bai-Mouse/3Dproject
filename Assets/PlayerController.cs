using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 2f;
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
        rb.velocity = GetMovement()* speed;
        
    }
    Vector3 GetMovement()
    {
        float xmove = Input.GetAxis("Horizontal");
        float zmove = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(xmove, 0, zmove);
        Debug.DrawRay(transform.position, rb.velocity, Color.yellow);
        Debug.DrawRay(transform.position, move*2, Color.red);
        return move;
    }
}
