using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    // Start is called before the first frame update
    float time;
    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        if (time >= 5)
        {
            Destroy(gameObject);
        }
    }
}
