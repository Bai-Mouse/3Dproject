using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public void gethit(float damage)
    {
        health-=damage;
        gethiteffect.Play();
        if (health <= 0)
        {
            Destroy(gameObject);
        }
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
        
    }
}
