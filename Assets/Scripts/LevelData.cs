using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public List<TileEntry> tiles = new List<TileEntry>();
    public int rows;
    public int cols;
}

[Serializable]
public class TileEntry
{
    public int row;
    public int col;
    public int color; // -1 = empty
}
