using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct Variation
{
    [Tooltip("Probability between 0 = 0% and 1 = 100%"), Range(0.0f,1.0f)]
    public float Probability;
    public GameObject TileGameObject;
}

[Serializable]
public struct AdjacentTileStruct
{
    bool m_ZeroDegreeToParent;
    bool m_Minus90DegreeToParent;
    bool m_Plus90DegreeToParent;
    bool m_Plus180DegreeToParent;
}
