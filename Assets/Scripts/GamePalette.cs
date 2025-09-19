using UnityEngine;

public class GamePalette : MonoBehaviour 
{ 
    public static GamePalette Instance { get; private set; }

    [SerializeField]
    private Sprite[] palette;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        palette = Resources.LoadAll<Sprite>("Sprites/Basic Metal");
        if (palette.Length == 0)
        {
            Debug.LogError("Cant find Blocks Sprites");  
        }
    }

    public Sprite GetSprite(int color)
    {
        if (palette == null || palette.Length == 0) return null;
        return palette[(int)color % palette.Length];
    }
    public Sprite GetSprite(BlockColor color)
    {
        if (palette == null || palette.Length == 0) return null;
        return palette[(int)color % palette.Length];
    }

    public int GetRandomColor()
    {
        if (palette == null || palette.Length == 0) return 0;
        return Random.Range(0, palette.Length - 1);
    }

    public int Length => palette.Length;
}

public enum BlockColor
{
    Grey,
    White,
    Black,
    Red,
    Orange,
    Yellow,
    Green,
    Cyan,
    Blue,
    Pink,
    Reds
}