using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BarrierData : MonoBehaviour
{
    public int health;

    [Header("DEBUG ONLY")]
    [SerializeField]
    private bool isDestroyed = false;



    public int Health { get => health; set => health = value; }
    public bool IsDestroyed { get => isDestroyed; set => isDestroyed = value; }
}
