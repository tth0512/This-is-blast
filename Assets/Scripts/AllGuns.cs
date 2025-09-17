using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllGuns : MonoBehaviour
{
    [SerializeField] private Transform gunTransform;
    [SerializeField] private Board board;

    public List<GunSlot> gunSlots;

    public GunSlot GetEmptyGunSlot()
    {
        for (int i = 0; i < gunSlots.Count; ++i)
        {
            if (gunSlots[i].transform.childCount == 0)
            {
                return gunSlots[i];
            }
        }

        return null;
    }

    private void Update()
    {
    }
}
