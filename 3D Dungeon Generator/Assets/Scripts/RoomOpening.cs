using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class RoomOpening : MonoBehaviour
{
    [SerializeField] private bool m_IsConnected = false;
    [SerializeField] private BoxCollider m_Trigger = null;

    [SerializeField] private RoomProbabilityScriptableObject m_AllowedRooms = null;

    private Room m_Room = null;
    private DungeonGenerator m_DungeonGenerator = null;
    private List<RoomTypeList> m_RoomPrefabs = null;

    private bool m_IsCoroutineDone = false;
    private bool m_FittingRoomFound = false;

    public bool IsCoroutineDone()
    {
        return m_IsCoroutineDone;
    }

    void OnDrawGizmos()
    {
        DebugExtension.DrawArrow(gameObject.transform.position, gameObject.transform.forward, Color.yellow);
    }

    private void Awake()
    {
        m_DungeonGenerator = GameObject.Find("DungeonGenerator").GetComponent<DungeonGenerator>();
        if (m_DungeonGenerator != null)
        {
            m_RoomPrefabs = m_DungeonGenerator.GetRoomPrefabList();
        }
    }

    //https://answers.unity.com/questions/532297/rotate-a-vector-around-a-certain-point.html
    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }

    public void SetRoom(in Room room)
    {
        m_Room = room;
    }

    private const string m_Tagname = "opening";
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == m_Tagname)
        {
            //Debug.Log("Trigger: " + other.gameObject.name);
            other.GetComponent<RoomOpening>().SetConnected(true);
            //Debug.Log("Set other to connected.");
            m_Trigger.enabled = false;
            //if (other.gameObject.GetComponent<RoomOpening>().IsConnected())
            //{
            //    Debug.Log("Trigger other is connected");
            //}
        }
    }

    private GameObject GetAllowedRandomRoom()
    {
        List<RoomTypeProbability> roomTypeProbabilities = m_AllowedRooms.GetRoomProbabilityList();
        float probability = Random.Range(0.0f, 1.0f);

        ERoomType type = ERoomType.None;

        float max = 0;
        for(int i = 0; i < roomTypeProbabilities.Count; i++)
        {
            max += roomTypeProbabilities[i].Probability;
            if (probability <= max)
            {
                type = roomTypeProbabilities[i].RoomType;
                break;
            }
        }

        List<GameObject> rooms = m_RoomPrefabs.Find(room => room.RoomType == type).m_Rooms;
        GameObject randomRoomPrefab = rooms[Random.Range(0, rooms.Count)];

        return randomRoomPrefab;
    }
    
    public IEnumerator SpawnAdjacentRoomEnumerator(GameObject parent)
    {
        m_IsCoroutineDone = false;
        
        yield return null;

        #region SpawnRoom
        int counter = 0;
        m_FittingRoomFound = false;

        //Todo: For the future: To speed up the process of finding a fitting room, I could put the different rooms in a list and each room that got tested gets removed, same for the possible openings
        while (!m_FittingRoomFound)
        {
            //Step 1: Get Random room
            //List<GameObject> rooms = m_RoomPrefabs.Find(room => room.RoomType == ERoomType.Room3x3).m_Rooms;
            //GameObject randomRoomPrefab = rooms[Random.Range(0, rooms.Count)];

            yield return StartCoroutine(SpawnRoom(parent, GetAllowedRandomRoom()));

            if (counter > 49)
            {
                Debug.Log("Could not spawn a room, broke up after " + counter + " attemps. And spawned BlockedEntrance Prefab");
                yield return StartCoroutine(SpawnRoom(parent, m_DungeonGenerator.GetBlockedEntrancePrefab(), true));

                break;
            }
            counter++;
        }

        //Debug.Log("Coroutine done");
        m_IsCoroutineDone = true;
        #endregion
    }

    private IEnumerator SpawnRoom(GameObject parent, GameObject roomPrefab, bool ignoreBoundingBox = false)
    {
        //Debug.Log("Try to spawn room: " + randomRoomPrefab.gameObject.name);
        //Get random room and boxCollider of that room
        Room randomRoom = roomPrefab.GetComponent<Room>();
        BoxCollider roomBoxCollider = null;
        if (!ignoreBoundingBox)
        {
            roomBoxCollider = randomRoom.GetBoxCollider();
        }

        //Step 2: Get Random room opening
        List<RoomOpening> roomOpenings = randomRoom.GetRoomOpenings();
        RoomOpening roomOpening = roomOpenings[Random.Range(0, roomOpenings.Count)];

        //Step 3: Calculate localPos of the roomOpening of the not spawned room
        Vector3 localPos = roomOpening.gameObject.transform.localPosition;
        //Step 4: Calculate the needed rotation, between the current room opening and the other room opening
        float angleBetweenBothOpenings = Vector3.SignedAngle(gameObject.transform.forward, roomOpening.gameObject.transform.forward * -1, Vector3.up) * -1;
        //Debug.Log("Angle between both vectors: " + angleBetweenBothOpenings);
        Quaternion rotation = Quaternion.Euler(new Vector3(0, angleBetweenBothOpenings, 0));

        //Step 5: Calculate the room position
        Vector3 roomCenter = gameObject.transform.position + (localPos * -1);
        
        //Step 6: Rotate the room around this room opening position, to align the room correctly to this entrance 
        Vector3 rotatedRoomPivot = RotatePointAroundPivot(roomCenter, gameObject.transform.position,
            new Vector3(0, angleBetweenBothOpenings, 0));

        bool overlapsWithColliders = false;

        if (!ignoreBoundingBox)
        {
            //Calculate center of the collider box at the final position
            Vector3 checkBoxCenter = rotatedRoomPivot + roomBoxCollider.gameObject.transform.localPosition +
                                     roomBoxCollider.center;
            
            Bounds roomBoxBounds = new Bounds(checkBoxCenter, roomBoxCollider.size);
            //Optional: Draw Box that checks for overlap
            DebugExtension.DebugBounds(roomBoxBounds, Color.blue, 15, false); //Can't be rotated, so I can't display it correctly


            //Step 7: Check if collider are in the way, to spawn this room
            overlapsWithColliders = Physics.CheckBox(checkBoxCenter, roomBoxBounds.extents, Quaternion.identity, LayerMask.GetMask("RoomBoundingBox"));//rotation was Quaternion.identity
        }

        //Step 8: Spawn the room, if no colliders are in the way 
        if (!overlapsWithColliders)
        {
            GameObject spawnedRoomPrefab = Instantiate(roomPrefab, rotatedRoomPivot, rotation, parent.transform);
            Debug.Log("Spawned room: " + spawnedRoomPrefab.gameObject.name);


            m_IsConnected = true;
            yield return null; //Important: It gives the trigger a chance to update and marks the room openings as connected, if the collider is in the triggerbox of another opening

            m_DungeonGenerator.GetNewGeneratedRoomsList().Add(spawnedRoomPrefab.GetComponent<Room>());
            m_FittingRoomFound = true;
        }
        else
        {
            Debug.Log("Could not spawn room");
        }
    }

    public bool IsConnected()
    {
        return m_IsConnected;
    }

    public void SetConnected(bool isConnected)
    {
        m_IsConnected = isConnected;
    }
}
