using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNode : WorldTile
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BaseIsHit(int i)
    {
        ((PlayerStats)FindObjectOfType(typeof(PlayerStats))).RemoveLife(i);
    }
}
