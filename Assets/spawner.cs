using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemy;
    float timer;
    void Start()
    {
        timer = 100;
    }

    // Update is called once per frame
    void Update()
    {
        timer+= Time.deltaTime;
        if (timer >= 20)
        {
            Instantiate(enemy).transform.position=transform.position;
            timer = 0;
        }
    }
}
