using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    private Image image;
    private Button button;
    private RectTransform rectTransform;
    private TextMeshProUGUI textMeshPro;
    [SerializeField] private GameObject bulletPrefab;

    private Canvas uiCanvas;
    private Camera worldCamera;

    public BlockColor gunColor;
    private Board board;

    [SerializeField] private Transform gunSlots;
    [SerializeField] private AllGuns allguns;
    [SerializeField] private GunSlot currentGunSlot;

    private int UIammo = 10;
    private float targetScale = 0.6f;
    private float moveDuration = 0.3f;

    private bool isActivated = false;
    private bool isDestroyed = false;

    private void Start()
    {
        uiCanvas = GetComponentInParent<Canvas>();
        worldCamera = Camera.main;
        board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();

        if ((int)gunColor >= 0)
        {
            image.sprite = GamePalette.Instance.GetSprite(gunColor);
        }

        textMeshPro.SetText(UIammo.ToString());
    }

    public void OnAmmoClicked()
    {
        var gunSlot = allguns.GetEmptyGunSlot(); 
        Sequence seq = DOTween.Sequence(); 
        Vector3 targetWorldPos = gunSlot.transform.position; 
        seq.Join(transform.DOMove(targetWorldPos, moveDuration).SetEase(Ease.OutCubic)); 
        seq.OnComplete(() => 
        { 
            transform.SetParent(gunSlot.transform, false); 
            transform.localPosition = Vector2.zero; 
            rectTransform.sizeDelta = new(100f, 100f); 
            button.transition = Selectable.Transition.None; 
            button.interactable = false; isActivated = true; 
        });

        currentGunSlot = gunSlot;
    }

    private bool finishShooting = true;
    public void Shoot()
    {
        finishShooting = false;
        var canDestroyBlock = board.GetFirstRow();
        Sequence seq = DOTween.Sequence();
        foreach (var block in canDestroyBlock)
        {
            if (block.TheTile.isShooting) continue;
            var blockColor = (BlockColor)block.TheTile.GetColor();
            if (blockColor == gunColor)
            {
                // Shoot animation
                var blockTransform = block.TheTile.transform;

                float targetZ = 0;
                Vector3 startWorld = UIToWorldHelpers.RectTransformToWorldPosition(rectTransform, uiCanvas, worldCamera, targetZ);

                Bullet bullet = BlockPool.Instance.GetBullet();
                bullet.transform.position = startWorld;

                block.TheTile.isShooting = true;

                Vector3 targetPos = blockTransform.position + new Vector3(0.5f, 0f, 0f);
                bullet.FireTo(targetPos, seq, () =>
                {
                    UIammo--;
                    textMeshPro.SetText(UIammo.ToString());
                    seq.Append(block.TheTile.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() =>
                    {
                        block.Clear();
                        board.boardHelper.ImmGravitySolver(block, seq);
                        block.TheTile.isShooting = false;
                    }));
                    //board.PopTiles();

                    if (UIammo == 0)
                    {
                        isDestroyed = true;
                        Destroy(gameObject);
                    }
                });
            }
        }

        
        seq.OnComplete(() =>
        {
            finishShooting = true;
        });
        seq.Play();
    }

    float time = 0;
    public void Update()
    {
        if (gameObject.activeSelf == false)
        {
            if (!isDestroyed)
            {
                Destroy(gameObject);
                isDestroyed = true;
            }
            return;
        }
        if (isActivated)
        {
            if (finishShooting)
            {
                Shoot();
            }
        }
    }


}
