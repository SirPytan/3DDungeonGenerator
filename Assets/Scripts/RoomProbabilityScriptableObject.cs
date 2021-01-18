using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "RoomProbability", menuName = "ScriptableObjects/RoomProbability", order = 2)]
public class RoomProbabilityScriptableObject : ScriptableObject
{
    [SerializeField] private List<RoomTypeProbability> m_AllowedRoomTypes = new List<RoomTypeProbability>();

    public ref List<RoomTypeProbability> GetRoomProbabilityList()
    {
        return ref m_AllowedRoomTypes;
    }
}
