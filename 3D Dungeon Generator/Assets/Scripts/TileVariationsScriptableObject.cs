using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "TileVariation", menuName = "ScriptableObjects/TileVariation", order = 1)]
public class TileVariationsScriptableObject : ScriptableObject
{
    [SerializeField] private ETileType m_Type = ETileType.Nothing;
    [SerializeField] private List<Variation> m_Variations = new List<Variation>();
}
