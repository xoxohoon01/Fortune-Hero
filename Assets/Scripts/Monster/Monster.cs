using GoogleSheet.Type;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster
{
    public int ID; // HeroData ID
    public int level; // ���� ����

    public Monster(int id = 0, int level = 1)
    {
        ID = id;
        this.level = level;
    }
}
