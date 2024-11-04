using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float HeatLimit=100f;
    public float speed = 2f;
    public float JumpForce = 10f;
    public bool onGround;
    public float gravityScale = 5;
    public int LavaLayer;
    public float Temp,TempA;
    public Slider Slider;
    public bool Dead;
    public GameObject RestartButton;
    public GameObject Winning;
    bool touchinglava;
    // Start is called before the first frame update
    void Start()
    {
        RestartButton.SetActive(false);
        rb = GetComponent<Rigidbody>();
        Slider.maxValue = HeatLimit;
        Slider.value = 0;
        Slider.gameObject.SetActive(false);
        LavaLayer = LayerMask.NameToLayer("Lava");

    }
    public void Restart()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        if (!Dead)
        {
            Vector3 aimDir = transform.TransformDirection(GetMovement());

            rb.MovePosition(transform.position + aimDir * speed * Time.deltaTime);
            rb.AddForce(Jump() * JumpForce);
            rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);
        }
        
        onGround = false;
            
        Slider.value = Temp;
        if(touchinglava)
        Temp += TempA;
        if (TempA <= 0)
        {
            Temp -= Time.fixedDeltaTime * 5;
        }
        else
        {
            if (!touchinglava)
            {
                TempA -=Time.fixedDeltaTime;
            }
        }
            if (Temp <= 0)
            {
                Slider.gameObject.SetActive(false);
                TempA = 0;
                Temp=0;
            }
        
        if (Temp >= HeatLimit)
        {
            Temp = HeatLimit;
            Dead = true;
            rb.freezeRotation = false;
            rb.AddTorque(3, 0, 0);
            RestartButton.SetActive(true);
        }
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
    private void OnTriggerStay(Collider other)
    {

        if(other.gameObject.layer== LavaLayer)
        {
            Slider.gameObject.SetActive(true);
            if (Temp == 0)
            {
                Temp = 0.01f;
            }
            TempA += Time.fixedDeltaTime*2;
            touchinglava = true;
        }
        if(other.gameObject.tag == "Wining")
        {
            Winning.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LavaLayer)
        {
            touchinglava=false;
        }
    }
}
