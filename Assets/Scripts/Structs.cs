using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region RoomGenerator
[Serializable]
struct Variation
{
    [Tooltip("Probability between 0 = 0% and 1 = 100%"), Range(0.0f, 1.0f)]
    public float Probability;
    public GameObject TileGameObject;
}
#endregion

#region DungeonGenerator
[Serializable]
public struct RoomTypeProbability
{
    public ERoomType RoomType;
    [Tooltip("Probability between 0 = 0% and 1 = 100%"), Range(0.0f, 1.0f)]
    public float Probability;
}

[Serializable]
public struct RoomTypeList
{
    public ERoomType RoomType;
    public List<GameObject> m_Rooms;
}
#endregion
