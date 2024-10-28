using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
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
    
    public LayerMask building;
    
    private float currentRotationX = 0f;
    public GameObject GrabObject;
    private void Start()
    {
        currentcd = 0;
        CamControl = GetComponent<CamControl>();
        PlayerController = GetComponent<PlayerController>();
    }
    private void FixedUpdate()
    {
        line.enabled = false;
        
        if (GrabObject)
        {
            RaycastHit Target;
            if (Physics.Raycast(transform.position, CamControl.myCam.transform.forward, out Target,100, building))
            {

                GrabObject.transform.position = Target.point;
            }
            
        }
    }
    private void Update()
    {
        
        if(Input.GetKey(KeyCode.Mouse0)&& currentcd <= 0)
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
                    int LayerIgnoreRaycast = LayerMask.NameToLayer("Building");
                    Target.transform.gameObject.layer = LayerIgnoreRaycast;
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
                if (Physics.Raycast(transform.position, CamControl.myCam.transform.forward, out Target, 1000, building))
                {
                    line.enabled = true;
                    line.transform.position = new Vector3(gun.transform.position.x, gun.transform.position.y + 0.1f, gun.transform.position.z) + CamControl.myCam.transform.forward * 0.5f;
                    line.SetPosition(0, Vector3.zero);
                    line.SetPosition(1, Target.point - transform.position);
                    GrabObject = Target.transform.gameObject;
                    int LayerIgnoreRaycast = LayerMask.NameToLayer("Player");
                    GrabObject.transform.gameObject.layer = LayerIgnoreRaycast;
                }
            }
            else
            {
                int LayerIgnoreRaycast = LayerMask.NameToLayer("Building");
                GrabObject.transform.gameObject.layer = LayerIgnoreRaycast;
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
