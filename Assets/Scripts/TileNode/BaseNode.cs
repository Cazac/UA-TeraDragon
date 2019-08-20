using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNode : WorldTile
{

    public void BaseIsHit(int i)
    {
        Debug.Log("AFASGFAQGAgf");
        ((PlayerStats)FindObjectOfType(typeof(PlayerStats))).RemoveLife(i);
    }
}
