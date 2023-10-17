using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EditMode
{
    Place,
    Loop
}
public enum LoopType
{
    Move,
    Rotate,
    Fire
}
public enum Element
{
    Fire,
    Ice,
    None,
}

public enum ObjectType
{
    BulletTower,
    RayTower,
    Bullet,
    Ray,
    Wall,
    Glass,
    PlayerMark,
    Star,
    SmallWall,
    Thumpher,
}

public enum StageType
{
    Official,
    Custom,
    Editing
}


public class Defines : MonoBehaviour
{
    public static Defines instance
    {
        get
        {
            if (defines == null)
            {
                defines = FindObjectOfType<Defines>();
            }
            return defines;
        }
    }

    private static Defines defines;

    public Color FireColor;
    public Color IceColor;
    public Color DangerColor;

    private void Awake()
    {
        DefineColor();
    }

    public void DefineColor()
    {
        FireColor = new Color(1, 0.6570f, 0, 1);
        IceColor = new Color(0, 0.8062f, 1, 1);
        DangerColor = Color.red;
    }

    public bool isHaveElement(int code)
    {
        return code == 0 || code == 1 || code == 2 || code == 3;
    }
}
