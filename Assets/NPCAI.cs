using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NPCAI : MonoBehaviour, Damagable
{
    // Start is called before the first frame update
    public Rigidbody body;
    public float mySpeed;
    public GameObject Target;
    public NPCAI myScript;
    public float healthvalue;
    public ParticleSystem gethiteffect;
    public float health { get; set; }
    public float maxHealth { get ; set; }
    public void gethit(float damage, Vector3 dir)
    {

        if (health <= maxHealth/2)
        {
            int LayerIgnoreRaycast = LayerMask.NameToLayer("Grabable");
            transform.gameObject.layer = LayerIgnoreRaycast;
            
            Target = GameObject.FindGameObjectWithTag("Player");

            body.freezeRotation = false;
            GetComponent<Renderer>().material.color = Color.white;
            body.useGravity = true;
            body.AddTorque(30,0,0);
        }
        health -= damage;
        gethiteffect.Play();
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        body.AddForce(dir * 2, ForceMode.Impulse);
    }

    void Start()
    {
        maxHealth = healthvalue;
        health = healthvalue;
        body = GetComponent<Rigidbody>();
        Target = GameObject.FindWithTag("Player");
        myScript = GetComponent<NPCAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(health >= maxHealth / 2)
        {
            if (Vector3.Distance(transform.position, Target.transform.position) <= 18|| health!=maxHealth)
            {
                body.MovePosition(Vector3.MoveTowards(transform.position, Target.transform.position, mySpeed));
                float rotationSpeed = 2.0f;
                Vector3 direction = (Target.transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            }

        }

        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (health >= maxHealth / 2)
            if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward, ForceMode.Impulse);
        }
    }
}
