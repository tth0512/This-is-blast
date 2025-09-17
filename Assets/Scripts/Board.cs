using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Board : MonoBehaviour
{
    public BoardHelper boardHelper;

    public const int Size = 10;

    public const float cellSize = 0.68f;
    public const float scaleSize = 1.41f;

    [SerializeField] private BlockPool blockPool;
    [SerializeField] private Tile cellPrefab;
    [SerializeField] private Transform cellTransform;

    private Slot[,] blocks;
    private readonly int[,] data = new int[Size, Size];
    private int[] colHeight = new int[Size];
    private Queue<int>[] columnBuffers = new Queue<int>[Size];

    private void Start()
    {
        CreateBoard();
    }

    float currentTime = 0;
    private void Update()
    {
        //if (Time.time - currentTime > 2f)
        //{
        //    PopTiles();
        //    currentTime = Time.time;
        //}
    }

    public List<Slot> GetFirstRow()
    {
        List<Slot> res = new List<Slot>();
        for (var c = 0; c < Size; ++c)
        {
            if (!blocks[0, c].IsEmpty)
                res.Add(blocks[0, c]);
        }

        return res;
    }

    private void CreateBoard()
    {
        blocks = new Slot[Size, Size];
        boardHelper = new BoardHelper(blocks, colHeight);

        for (var c = 0; c < Size; c++)
        {
            for (var r = 0; r < Size; r++)
            {
                blocks[r, c] = new Slot(r, c, GetCellWorldPos(c, r));
            }
        }


        for (var c = 0; c < Size; ++c)
        {
            columnBuffers[c] = new Queue<int>();

            //if (UnityEngine.Random.Range(0, 10) < 5)
            //{
            //    colHeight[c] = 0;
            //    continue;
            //}

            int height = UnityEngine.Random.Range(0, Size * 3);
            height = Size - 1;
            colHeight[c] = height;

            for (var r = 0; r <= Mathf.Min(height, Size - 1); ++r)
            {
                //int colorIndex = GamePalette.Instance.GetRandomColor();
                int colorIndex = 0;
                var tile = Instantiate(cellPrefab, cellTransform);
                tile.Init(colorIndex);
                blocks[r, c].SetTile(tile, true);
                data[r, c] = colorIndex;
            }

            // fill buffer for rows above visible area
            for (var r = Size; r <= height; ++r)
            {
                int colorIndex = GamePalette.Instance.GetRandomColor();
                columnBuffers[c].Enqueue(colorIndex);
            }
        }
    }

    public void PopTiles()
    {
        AfterPop();
    }

    private void AfterPop()
    {
        DropExistingTiles();
    }

    private void DropExistingTiles()
    {
        int loop = 0;

        while(true)
        {
            if (boardHelper.GravitySolver())
            {
                break;
            }

            if (loop > 100)
            {
                Debug.LogError("Critical Error: Gravity solver ended up in a dead loop!");
                break;
            }
        }

        boardHelper.ApplyTileMove();
    }

    private bool Valid(int col, int row)
    {
        return col >= 0 && col < Size && row >= 0 && row < Size;
    }

    public Vector2 GetCellWorldPos(int col, int row)
    {
        return new Vector2(col * cellSize, row * cellSize);
    }


}
