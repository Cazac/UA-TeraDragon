﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class WorldTile : MonoBehaviour
{
    public int gvalue;
    public int hValue;

    public int gridY;
    public int gridX;

    public int posX;
    public int posY;

    public bool walkable;

    public List<WorldTile> myNeighbours;
}
