using UnityEngine;

public class Slot : MonoBehaviour
{
    public Tile TheTile { get; private set; }
    public int R { get; private set; }
    public int C { get; private set; }

    public Vector2 Position;
    public bool IsEmpty => TheTile == null;

    public Slot(int r, int c, Vector2 position)
    {
        R = r;
        C = c;
        Position = position;
    }

    public Slot(Tile tile, int r, int c, Vector2 position)
    {
        R = r;
        C = c;
        TheTile = tile;
        Position = position;

        SetTile(TheTile, true);
    }

    public void SetTile(Tile tile, bool setPosition)
    {
        TheTile = tile;
        TheTile.SetCoord(R, C);
        if (setPosition)
        {
            TheTile.SetPosition(Position);
        }
    }

    private void Update()
    {
        if (TheTile != null)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void Clear() { TheTile = null; }
    
}
