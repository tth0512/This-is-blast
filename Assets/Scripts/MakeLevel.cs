using NUnit.Framework.Constraints;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using UnityEngine.XR;

public class MakeLevel : MonoBehaviour
{
    public string currentLevel;
    private const float cellSize = 0.68f;
    private const int Size = 10;

    private Grid currentGrid;
    [SerializeField] private Transform cellTransform;

    [SerializeField] private Tile cellPrefab;
    public InputManager inputManager;

    private Slot[,] blocks;

    public int currentColor = 5;

    [SerializeField] private float placeInterval = 0.12f;
    [SerializeField] private float deleteInterval = 0.12f;

    private Vector2Int? lastPlacedCell = null;
    private float lastPlaceTime = -999f;

    private Vector2Int? lastDeletedCell = null;
    private float lastDeleteTime = -999f;

    private void Start()
    {
        currentGrid = GetComponentInParent<Grid>();
        currentLevel = UIManager.Instance.GetCurrentLevel;
        if (currentGrid == null)
        {
            enabled = false;
            return;
        }

        blocks = new Slot[Size, Size];

        for (var c = 0; c < Size; c++)
        {
            for (var r = 0; r < Size; r++)
            {
                blocks[r, c] = new Slot(r, c, GetCellWorldPos(c, r));
            }
        }

        ImportCurrentLevel();

    }

    private void OnEnable()
    {

        if (inputManager == null)
        {
            Debug.LogError("MakeLevel: InputManager không được gán và không tìm thấy trong scene!");
            return;
        }

        inputManager.OnClickedLeft -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        inputManager.OnClickedRight -= DeleteStructure;

        inputManager.OnClickedLeft += PlaceStructure;
        inputManager.OnExit += StopPlacement;
        inputManager.OnClickedRight += DeleteStructure;
    }

    private void OnDisable()
    {
        if (inputManager == null) return;
        inputManager.OnClickedLeft -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        inputManager.OnClickedRight -= DeleteStructure;
    }

    public void StartPlacement()
    {
        lastPlacedCell = null;
        lastPlaceTime = -999f;
        //Debug.Log("StartPlacement called");
    }

    public void StopPlacement()
    {
        lastPlacedCell = null;
        lastPlaceTime = -999f;
        //Debug.Log("StopPlacement called");
    }

    private void DeleteStructure()
    {
        if (!Input.GetMouseButton(1))
        {
            lastDeletedCell = null;
            return;
        }

        // pointer over UI -> ignore
        if (inputManager != null && inputManager.IsPointerOverUI()) return;

        // compute cell under mouse
        Vector2 cellPos = GetCellPosition();
        int c = Mathf.FloorToInt(cellPos.x);
        int r = Mathf.FloorToInt(cellPos.y);

        if (c < 0 || c >= Size || r < 0 || r >= Size)
        {
            lastDeletedCell = null;
            return;
        }

        var currentCell = new Vector2Int(c, r);
        float now = Time.time;

        // if moved to a new cell, delete immediately
        if (!lastDeletedCell.HasValue || lastDeletedCell.Value != currentCell)
        {
            DoDeleteAt(c, r);
            lastDeletedCell = currentCell;
            lastDeleteTime = now;
            return;
        }

        // otherwise, respect interval
        if (deleteInterval <= 0f)
        {
            DoDeleteAt(c, r);
            lastDeleteTime = now;
            return;
        }

        if (now - lastDeleteTime >= deleteInterval)
        {
            DoDeleteAt(c, r);
            lastDeleteTime = now;
        }
    }
    private void PlaceStructure()
    {
        if (!Input.GetMouseButton(0))
        {
            lastPlacedCell = null;
            return;
        }

        if (inputManager.IsPointerOverUI())
            return;

        if (cellPrefab == null || cellTransform == null)
        {
            Debug.LogError("PlaceStructure: cellPrefab hoặc cellTransform chưa gán!");
            return;
        }

        var cellPosition = GetCellPosition();

        int c = Mathf.FloorToInt(cellPosition.x);
        int r = Mathf.FloorToInt(cellPosition.y);

        if (c < 0 || c >= Size || r < 0 || r >= Size) return;

        var currentCell = new Vector2Int(c, r);
        float now = Time.time;

        if (!lastPlacedCell.HasValue || lastPlacedCell.Value != currentCell)
        {
            DoPlaceAt(c, r);
            lastPlacedCell = currentCell;
            lastPlaceTime = now;
            return;
        }

        if (placeInterval <= 0f)
        {
            DoPlaceAt(c, r);
            lastPlaceTime = now;
            return;
        }

        if (now - lastPlaceTime >= placeInterval)
        {
            DoPlaceAt(c, r);
            lastPlaceTime = now;
        }
    }

    private void DoPlaceAt(int c, int r)
    {
        if (blocks[r, c].TheTile != null)
        {
            return;
        }
        int colorIndex = currentColor;
        var tile = Instantiate(cellPrefab, cellTransform);
        tile.Init(colorIndex);

        blocks[r, c].SetTile(tile, true);
        //Debug.Log(blocks[r, c] != null);
        //Debug.Log($"color at {r}, {c}: " + blocks[r, c].TheTile.GetColor());
    }

    private void DoDeleteAt(int c, int r)
    {
        var slot = blocks[r, c];

        if (slot.TheTile == null)
        {
            // nothing to delete
            return;
        }

        // destroy tile GameObject and clear slot
        Destroy(slot.TheTile.gameObject);
        //Debug.Log($"Deleted tile at [{r}, {c}]");
    }

    private Vector2 GetCellPosition()
    {
        var mousePosition = Input.mousePosition;

        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        var toCell = currentGrid.WorldToCell(mouseWorldPosition);

        return new(toCell.x, toCell.y - 5);
    }

    public Vector2 GetCellWorldPos(int col, int row)
    {
        return new Vector2(col * cellSize, row * cellSize);
    }

    public LevelData ExportLevel()
    {
        var data = new LevelData();
        data.rows = Size;
        data.cols = Size;

        for (int r = 0; r < Size; r++)
        {
            for (int c = 0; c < Size; c++)
            {
                var slot = blocks[r, c];
                var te = new TileEntry { row = r, col = c };

                te.color = (slot.TheTile != null) ? slot.TheTile.GetColor() : -1;
                data.tiles.Add(te);
            }
        }
        return data;
    }

    public void ImportLevel(LevelData data)
    {
        if (data == null) return;

        // clear cũ
        for (int r = 0; r < Size; r++)
            for (int c = 0; c < Size; c++)
                if (blocks[r, c].TheTile != null)
                {
                    Destroy(blocks[r, c].TheTile.gameObject);
                    blocks[r, c].Clear();
                }

        // load mới
        foreach (var te in data.tiles)
        {
            if (te.color < 0) continue;

            var tile = Instantiate(cellPrefab, cellTransform);
            tile.Init(te.color);
            tile.SetPosition(GetCellWorldPos(te.col, te.row));
            blocks[te.row, te.col].SetTile(tile, false);
        }
    }

    public void ImportCurrentLevel()
    {
        var data = LevelManager.LoadLevelFromFile(currentLevel);
        ImportLevel(data);
    }

    public void ExportCurrentLevel()
    {
        var data = ExportLevel();
        LevelManager.SaveLevelToFile(data, currentLevel);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            ExportCurrentLevel();
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            ImportCurrentLevel();
        }

    }


}
