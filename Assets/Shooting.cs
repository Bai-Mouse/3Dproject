using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Bullet;
    public CamControl CamControl;
    public PlayerController PlayerController;
    public GameObject gun;
    public float shootcd=0.2f,currentcd;
    private void Start()
    {
        currentcd = 0;
        CamControl = GetComponent<CamControl>();
        PlayerController = GetComponent<PlayerController>();
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0)&& currentcd <= 0)
        {
            currentcd = shootcd;
            GameObject bullet= Instantiate(Bullet);
            bullet.transform.position= gun.transform.position;
            bullet.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1.0f,1f),2, Random.Range(-1f, 1f)),ForceMode.Impulse);
            gun.transform.eulerAngles = new Vector3(-10, gun.transform.rotation.y, gun.transform.rotation.z);

        }
        gun.transform.Rotate((0-gun.transform.rotation.x)/2, 0, 0);
        currentcd -=Time.deltaTime;
    }
}
