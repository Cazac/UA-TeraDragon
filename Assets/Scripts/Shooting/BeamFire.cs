using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamFire : MonoBehaviour
{

    [Header("Who to chase")]
    public GameObject enemy;

    [Header("Beam Data")]
    public bool isBeam;
    public float beamDamage;
    public float beamSlowdown;
    public int beamChainTargets;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
