using System.Collections.Generic;
using UnityEngine;

public class BlockPool : MonoBehaviour
{
    public static BlockPool Instance;

    [SerializeField] private Tile prefab;
    [SerializeField] private Transform cellTransform;

    [SerializeField] private Bullet bulletPrefab;

    [SerializeField] private int pollSize = 20;
    [SerializeField] private int bulletSize = 10;

    private Queue<Tile> poll = new Queue<Tile>();
    private Queue<Bullet> bulletPool = new Queue<Bullet>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }

        FillBulletPoll();

    }

    public void FillBulletPoll()
    {
        while(bulletPool.Count < bulletSize)
        {
            var bullet = Instantiate(bulletPrefab);
            bullet.gameObject.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }

    public void ReturnBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bulletPool.Enqueue(bullet);
    }

    public Bullet GetBullet()
    {
        if (bulletPool.Count == 0)
        {
            FillBulletPoll();
        }

        var obj = bulletPool.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    //public Tile GetCell()
    //{
    //    if (poll.Count == 0)
    //    {
    //        FillPoll();
    //    }
        
    //    var obj = poll.Dequeue();
    //    obj.gameObject.SetActive(true);
    //    return obj;
    //}

    public void ReturnBlock(Tile tile)
    {
        tile.gameObject.SetActive(false);
        poll.Enqueue(tile);
    }
}
