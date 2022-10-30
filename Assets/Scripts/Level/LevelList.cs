using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class LevelList
{
    private uint x = 0;
    private uint y = 0;
    private float roomSpacing = 0.0f;
    private Vector2Int startRoom = new Vector2Int(0, 0);
    public LevelList(uint dim)
    {
        this.x = dim;
        this.y = dim;
    }
    public LevelList(uint dim, float roomSpacing)
    {
        this.x = dim;
        this.y = dim;
        this.roomSpacing = roomSpacing;
    }
    public LevelList(uint x, uint y)
    {
        this.x = x;
        this.y = y;
    }
}

