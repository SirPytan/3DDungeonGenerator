using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AdjacentTile
{
    //Possible Orientations
    public bool m_ZeroDegreeToParent;
    public bool m_Minus90DegreeToParent;
    public bool m_Plus90DegreeToParent;
    public bool m_Plus180DegreeToParent;
}
