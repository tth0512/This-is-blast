using DG.Tweening;
using UnityEngine;

public class BoardHelper
{
    private Slot[,] blocks;
    private const int Size = 10;
    private int[] colHeight;

    private Sequence seq = DOTween.Sequence();
    public BoardHelper(Slot[,] blocks, int[] colHeight)
    {
        this.blocks = blocks;
        this.colHeight = colHeight;
    }

    public void ImmGravitySolver(Slot belowSlot, Sequence seq)
    {
        for (var r = belowSlot.R + 1; r < Size; ++r)
        {
            var c = belowSlot.C;
            var currentSlot = blocks[r, c];

            if (!currentSlot.IsEmpty)
            {
                colHeight[c]--;
                belowSlot.SetTile(currentSlot.TheTile, false);
                currentSlot.Clear();
                belowSlot.TheTile.RecordMoveTo(belowSlot.Position);
                belowSlot.TheTile.ApplyMove(seq);

                belowSlot = blocks[belowSlot.R + 1, c];
            }
        }
    }
    public bool GravitySolver()
    {
        bool solved = true;

        for (var c = 0; c < Size; ++c)
        {
            for (var r = 0; r < Size; ++r) {
                var currentSlot = blocks[r, c];

                if (!currentSlot.IsEmpty)
                {
                    colHeight[c]--;
                    int nextRow = r - 1;

                    if (nextRow >= 0 && blocks[nextRow, c].IsEmpty)
                    {

                        var belowSlot = blocks[nextRow, c];
                        belowSlot.SetTile(currentSlot.TheTile, false);
                        currentSlot.Clear();
                        belowSlot.TheTile.RecordMoveTo(belowSlot.Position);
                        solved = false;
                        
                    }
                    else
                    {

                    }
                }
            }
        }

        return solved;
    }

    public void ApplyTileMove()
    {
        for (var r = 0; r < Size; ++r)
        {
            Sequence seq = DOTween.Sequence();
            for (var c = 0; c < Size; ++c)
            {
                var slot = blocks[r, c];

                if (!slot.IsEmpty)
                {
                    slot.TheTile.ApplyMove(seq);
                }
            }

            seq.Play();
        }
    }
}
