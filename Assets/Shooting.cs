using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Shooting : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Bullet;
    public CamControl CamControl;
    public PlayerController PlayerController;
    public GameObject gun;
    public float shootcd=0.2f,currentcd;
    public LineRenderer line;
    
    public LayerMask building, grabable;
    
    private float currentRotationX = 0f;
    public GameObject GrabObject;
    public float zoom;
    bool isRotating;
    public float rotationSpeed = 100f;
    private void Start()
    {
        zoom = 4;
        currentcd = 0;
        CamControl = GetComponent<CamControl>();
        PlayerController = GetComponent<PlayerController>();
    }
    private void FixedUpdate()
    {
        line.enabled = false;
        if (PlayerController.Dead)
        {
            gun.SetActive(false);
            return;
        }
        if (GrabObject)
        {
            
            RaycastHit Target;
            if (Physics.Raycast(transform.position, CamControl.myCam.transform.forward, out Target, zoom, building))
            {
                if (Vector3.Distance(Target.point, transform.position) > 2f)
                
                GrabObject.GetComponent<Rigidbody>().MovePosition((Target.point - GrabObject.transform.position) / 2 + GrabObject.transform.position);
                
                
            
            }
            else
            {
                GrabObject.GetComponent<Rigidbody>().MovePosition((transform.position + CamControl.myCam.transform.forward * zoom- GrabObject.transform.position)/2+ GrabObject.transform.position);
            
            }
            GrabObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            
        }
    }
    private void Update()
    {
        if (PlayerController.Dead)
        {

            return;
        }
        if (GrabObject) {
            if (Input.GetMouseButtonDown(1))
            {
                GrabObject.GetComponent<Rigidbody>().freezeRotation = true;
                isRotating = true;
                CamControl.Lock = true;
            }

            // 检测鼠标右键是否松开
            if (Input.GetMouseButtonUp(1))
            {
                GrabObject.GetComponent<Rigidbody>().freezeRotation = false;
                isRotating = false;
                CamControl.Lock = false;
            }

            // 如果正在旋转，更新物体旋转
            if (isRotating)
            {

                float mouseX = Input.GetAxis("Mouse X"); // 鼠标在X轴上的移动量
                float mouseY = Input.GetAxis("Mouse Y"); // 鼠标在Y轴上的移动量

                // 根据鼠标X轴移动旋转物体的Y轴（水平旋转）
                GrabObject.transform.Rotate(Vector3.up, -mouseX * rotationSpeed * Time.deltaTime, Space.World);

                // 根据鼠标Y轴移动旋转物体的X轴（垂直旋转）
                GrabObject.transform.Rotate(Vector3.right, mouseY * rotationSpeed * Time.deltaTime, Space.World);
            }
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            zoom += Time.deltaTime*60;
            if (zoom >= 7)
            {
                zoom=7;
            }
        }
        else if (scroll < 0f)
        {
            zoom -= Time.deltaTime*60;
            if (zoom <= 2)
            {
                zoom = 2;
            }
        }
        if (Input.GetKey(KeyCode.Mouse0)&& currentcd <= 0)
        {
            currentcd = shootcd;
            GameObject bullet= Instantiate(Bullet);
            bullet.transform.position= gun.transform.position;
            bullet.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1.0f,1f),2, Random.Range(-1f, 1f)),ForceMode.Impulse);
            gun.transform.localEulerAngles = new Vector3(-10, gun.transform.localEulerAngles.y, gun.transform.localEulerAngles.z);
            currentRotationX = 10f;
            RaycastHit Target; 
            if(Physics.Raycast(transform.position, CamControl.myCam.transform.forward, out Target, 1000))
            {
                line.enabled = true;
                line.transform.position = new Vector3(gun.transform.position.x, gun.transform.position.y+0.1f, gun.transform.position.z) + CamControl.myCam.transform.forward* 0.5f;
                line.SetPosition(0, Vector3.zero);
                line.SetPosition(1, Target.point- transform.position);
                if (Target.transform.GetComponent<Damagable>() != null)
                {
                    Target.transform.GetComponent<Damagable>().gethit(40, CamControl.myCam.transform.forward);
                    
                }
            }
            else
            {
                line.enabled = true;
                line.transform.position = new Vector3(gun.transform.position.x, gun.transform.position.y + 0.1f, gun.transform.position.z) + CamControl.myCam.transform.forward * 0.5f;
                line.SetPosition(0, Vector3.zero);
                line.SetPosition(1, CamControl.myCam.transform.forward*200f);
            }
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(GrabObject == null)
            {

                RaycastHit Target;
                if (Physics.Raycast(transform.position, CamControl.myCam.transform.forward, out Target, 6, grabable))
                {

                    
                        line.enabled = true;
                        line.transform.position = new Vector3(gun.transform.position.x, gun.transform.position.y + 0.1f, gun.transform.position.z) + CamControl.myCam.transform.forward * 0.5f;
                        line.SetPosition(0, Vector3.zero);
                        line.SetPosition(1, Target.point - transform.position);
                        GrabObject = Target.transform.gameObject;
                        
                    if (!GrabObject.GetComponent<Rigidbody>())
                    {
                        GrabObject=null;
                    }
                    else
                    {
                        int LayerIgnoreRaycast = LayerMask.NameToLayer("Player");
                        GrabObject.transform.gameObject.layer = LayerIgnoreRaycast;
                        GrabObject.GetComponent<Rigidbody>().mass += 100;
                        GrabObject.GetComponent<Rigidbody>().constraints &= ~(RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ); ;
                    }
                    
                }
            }
            else
            {
                int LayerIgnoreRaycast = LayerMask.NameToLayer("Grabable");
                GrabObject.transform.gameObject.layer = LayerIgnoreRaycast;
                GrabObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                GrabObject = null;
            }
        }
        if (currentRotationX > 0)
        {
            currentRotationX += (0- currentRotationX)/20; // Decrease the rotation
            if (currentRotationX < 0) currentRotationX = 0; // Clamp to target
            gun.transform.localEulerAngles = new Vector3(-currentRotationX, gun.transform.localEulerAngles.y, gun.transform.localEulerAngles.z);
        }
        currentcd -=Time.deltaTime;
    }
}
