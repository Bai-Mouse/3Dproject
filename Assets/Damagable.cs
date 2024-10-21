using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Damagable
{
    // Start is called before the first frame update
    public float health { get; set; }
    public float maxHealth { get; set; }

    
    public void gethit(float damage)
    {
    }
}
