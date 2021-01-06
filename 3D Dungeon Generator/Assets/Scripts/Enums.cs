using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EOrientation : int
{
    ZPositiv = 0,
    XPositiv = 1,
    ZNegativ = 2,
    XNegativ = 3
}

enum ETileType : int
{
    Nothing = 0,
    Floor = 1,
    Ceiling = 2,
    Wall = 3,
    Stair = 4,
    Door = 5,
    InnerCorner = 6,
    OuterCorner = 7,
    Bridge = 8,
    Pillar = 9
}

enum ESide : int
{
    FrontZPositiv = 0,
    RightXPositiv = 1,
    BackZNegativ = 2,
    LeftXNegativ = 3,
    TopYPositiv = 4,
    BottomYNegativ = 5
}

enum ERoomType : int
{
    Hallway = 0,
    Corner = 1,
    Room = 2,
    Staircase = 3
}
