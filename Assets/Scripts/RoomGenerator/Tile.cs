using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private ETileType m_TileTypeM = ETileType.Nothing;
    
    
    //[SerializeField] private EOrientation m_Orientation = EOrientation.ZPositiv;
    private Vector3Int m_ID = Vector3Int.zero;

    public AdjacentTile m_Tile;
    
    public void SetID(Vector3Int id)
    {
        m_ID = id;
    }

    public Vector3Int GetID()
    {
        return m_ID;
    }
    
    
    
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
