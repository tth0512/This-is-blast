using NUnit.Framework.Constraints;
using UnityEngine;

public class MakeLevel : MonoBehaviour
{
    private const float cellSize = 0.68f;
    private const int Size = 10;

    private Grid currentGrid;
    [SerializeField] private Transform cellTransform;

    [SerializeField] private Tile cellPrefab;
    public InputManager inputManager;

    private Slot[,] blocks;

    private int currentColor;

    private void Start()
    {
        currentGrid = GetComponentInParent<Grid>();
        blocks = new Slot[Size, Size];

        for (var c = 0; c < Size; c++)
        {
            for (var r = 0; r < Size; r++)
            {
                blocks[r, c] = new Slot(r, c, GetCellWorldPos(c, r));
            }
        }

        StopPlacement();
        StartPlacement();
    }

    private void Update()
    {

    }

    private Vector2 GetCellPosition()
    {
        var mousePosition = Input.mousePosition;

        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        //Debug.Log(mouseWorldPosition);
        Debug.Log(currentGrid.WorldToCell(mouseWorldPosition));

        var toCell = currentGrid.WorldToCell(mouseWorldPosition);

        return new(toCell.x, toCell.y - 5);
    }

    public void StartPlacement()
    {
        inputManager.OnClickedLeft -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;

        inputManager.OnClickedLeft += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StopPlacement()
    {
        inputManager.OnClickedLeft -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }

        int colorIndex = currentColor;
        var tile = Instantiate(cellPrefab, cellTransform);
        tile.Init(colorIndex);

        var cellPosition = GetCellPosition();

        Debug.Log(cellPosition);

        int c = (int)cellPosition.x;
        int r = (int)cellPosition.y;

        blocks[r, c].SetTile(tile, true);
        //data[r, c] = colorIndex;

    }

    public Vector2 GetCellWorldPos(int col, int row)
    {
        return new Vector2(col * cellSize, row * cellSize);
    }
}
