using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomOpening : MonoBehaviour
{
    [SerializeField] private bool m_Connected = false;


    private DungeonGenerator m_DungeonGenerator = null; 
    
    private void Awake()
    {
        m_DungeonGenerator = GameObject.Find("DungeonGenerator").GetComponent<DungeonGenerator>();
        if (m_DungeonGenerator != null)
        {

        }
    }

    public bool IsConnected()
    {
        return m_Connected;
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
