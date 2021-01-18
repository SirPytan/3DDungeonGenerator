using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region RoomGenerator
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
#endregion

#region DungeonGenerator
public enum ERoomType : int
{
    None = -1,
    Hallway = 0,
    Room3x3 = 1,
    Staircase = 2,
    BigRoom = 3
}

public enum ERoomEntranceType : int
{
    None = -1,
    End = 0,
    Straight2Way = 1,
    Corner2Way = 2,
    TCross3Way = 3,
    Cross4Way = 4
}
/*
    HallwayEnd = 0,
    Hallway2WayStraight = 1,
    Hallway2WayCorner = 2,
    Hallway3Way = 3,
    Hallway4Way = 4,
    RoomEnd = 5,
    Room2WayStraight = 6,
    Room2WayCorner = 7,
    Room3Way = 7,
    Room4Way = 8,
    Staircase2Way = 9,
    MultilayerRoom = 10,
    BigRoom = 11,
 */
#endregion

