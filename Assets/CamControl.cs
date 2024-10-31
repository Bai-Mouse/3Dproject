using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CamControl : MonoBehaviour
{
    Vector3 myLook;
    float lookSpeed = 1000f;
    public Camera myCam;
    public float camlook = 90f;
    PlayerController playerController;
    float onstarttimer;
    bool dead;
    public bool Lock;
    // Start is called before the first frame update

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        myLook = transform.localEulerAngles;
        playerController = GetComponent<PlayerController>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.Dead)
        {
            if (dead == false)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                dead = true;
                myCam.transform.parent = null;
                myCam.transform.position += transform.forward * -4;
            }

            
            myCam.transform.LookAt(transform.position);
            return;
        }
        if (Lock)
        {
            return;
        }
        onstarttimer += Time.deltaTime;
        myLook += DeltaLook()*Time.deltaTime* lookSpeed;
        myLook.y = Mathf.Clamp(myLook.y,-camlook,camlook);
        transform.rotation = Quaternion.Euler(0f,myLook.x,0f);
        myCam.transform.rotation = Quaternion.Euler(-myLook.y, myLook.x, 0f);
        
    }
    //here were going to clculate the difference in mouse position on screen relative to the previous frame
    Vector3 DeltaLook()
    {
        Vector3 dlook;
        float rotY = Input.GetAxisRaw("Mouse Y");
        float rotX = Input.GetAxisRaw("Mouse X");
        dlook = new Vector3(rotX, rotY, 0);
        if (onstarttimer < 0.5f)
        {
            dlook = Vector3.ClampMagnitude(dlook, 0);
        }
        return dlook;
    }
}
