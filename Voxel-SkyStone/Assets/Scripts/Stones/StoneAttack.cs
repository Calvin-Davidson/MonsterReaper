using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class StoneAttack : MonoBehaviour
{
    [SerializeField] private SkystoneGrid skystoneGrid;

    private void Start()
    {
        foreach (var skystoneGridStone in skystoneGrid.Stones)
        {
            skystoneGridStone.onStonePlace.AddListener(() => Attack(skystoneGridStone));
        }
    }

    private void Attack(Stone stone)
    {
        Stone above = skystoneGrid.GetStoneAbove(stone);
        Stone right = skystoneGrid.GetStoneRight(stone);
        Stone bottom = skystoneGrid.GetStoneUnder(stone);
        Stone left = skystoneGrid.GetStoneLeft(stone);
        if (above != null && IsStronger(stone, above, Side.Top)) above.TeamSide = stone.TeamSide;
        if (right != null && IsStronger(stone, right, Side.Top)) right.TeamSide = stone.TeamSide;
        if (bottom != null && IsStronger(stone, bottom, Side.Top)) bottom.TeamSide = stone.TeamSide;
        if (left != null && IsStronger(stone, left, Side.Top)) left.TeamSide = stone.TeamSide;
    }

    private bool IsStronger(Stone attacker, Stone defender, Side side)
    {
        if (defender.StoneData == null) return false;
        switch (side)
        {
            case Side.Top:
                return attacker.StoneData.TopDamage > defender.StoneData.BottomDamage;
            case Side.Right:
                return attacker.StoneData.RightDamage > defender.StoneData.LeftDamage;
            case Side.Bottom:
                return attacker.StoneData.BottomDamage > defender.StoneData.TopDamage;
            case Side.Left:
                return attacker.StoneData.LeftDamage > defender.StoneData.RightDamage;
            default:
                return false;
        }
    }
}