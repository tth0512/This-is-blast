using DG.Tweening;
using System.Drawing;
using System.Threading.Tasks;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int R {  get; private set; }
    public int C { get; private set; }
    private Vector2 targetMove;

    public const float cellSize = 0.68f;
    public const float scaleSize = 1.41f;
    private float moveDuration = 0.5f;
    private int colorID;

    private SpriteRenderer sprite;

    public bool isShooting;
    public bool isMoving;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Init(int colorID)
    {
        isMoving = false;
        this.colorID = colorID;
        var color = (GamePalette.BlockColor)colorID;
        sprite.sprite = GamePalette.Instance.GetSprite(color);
        gameObject.SetActive(true);
        SetScale(scaleSize);
    }
    public int GetColor() { return colorID; }

    public void ChangeColor(int colorID)
    {
        sprite.sprite = GamePalette.Instance.GetSprite(colorID);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetCoord(int r, int c)
    {
        R = r;
        C = c;
    }

    public void SetScale(float scaleSize)
    {
        transform.localScale = new(scaleSize, scaleSize);
    }
    public void SetPosition(Vector2 position)
    {
        transform.localPosition = position;
    }

    public void RecordMoveTo(Vector2 position) { 

        targetMove = position;
        isMoving = true;
    }

    public void ApplyMove(Sequence seq)
    {
        if (isMoving)
        {
            transform.DOKill();
            seq.Append(transform.DOLocalMove(targetMove, moveDuration).SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    isMoving = false;
                }));
        }
    }

    public void Pop()
    {
        transform.DOKill();
        //BlockPool.Instance.ReturnBlock(this);
    }

    public Vector2 GetCellWorldPos(int col, int row)
    {
        return new Vector2(col * cellSize, row * cellSize);
    }

    
}
