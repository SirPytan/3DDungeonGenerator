using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class RoomOpening : MonoBehaviour
{
    [SerializeField] private bool m_Connected = false;

    private Room m_Room = null;
    private DungeonGenerator m_DungeonGenerator = null;
    private List<GameObject> m_RoomPrefabs = null;

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
    public void SpawnAdjacentRoom(in GameObject parent)
    {
        StartCoroutine(SpawnAdjacentRoom(parent));
        //SpawnRoom(parent);
    }

    IEnumerator SpawnAdjacentRoom(GameObject parent)
    {
        //yield return new WaitForFixedUpdate();
        yield return null;
        SpawnRoom(parent);
    }

    private void SpawnRoom(in GameObject parent)
    {
        int counter = 0;
        bool fittingRoomFound = false;

        while (!fittingRoomFound)
        {
            //Step 1: Get Random room
            GameObject randomRoomPrefab = m_RoomPrefabs[0];//-----------------------------------------------------change
            //GameObject randomRoomPrefab = m_RoomPrefabs[Random.Range(0, m_RoomPrefabs.Count)];
            Debug.Log("Try to spawn room: " + randomRoomPrefab.gameObject.name);
            Room randomRoom = randomRoomPrefab.GetComponent<Room>();
            BoxCollider roomBoxCollider = randomRoom.GetBoxCollider();

            //Step 2: Get Random room opening
            List<RoomOpening> roomOpenings = randomRoom.GetUnconnectedRoomOpenings();
            RoomOpening roomOpening = roomOpenings[Random.Range(0, roomOpenings.Count)];

            //Step 3: Calculate localPos of the roomOpening of the not spawned room
            Vector3 localPos = roomOpening.gameObject.transform.localPosition;
            //Step 4: Calculate the needed rotation, between the current room opening and the other room opening
            float angleBetweenBothOpenings = Vector3.SignedAngle(gameObject.transform.forward, roomOpening.gameObject.transform.forward * -1, Vector3.up) * -1;
            //Debug.Log("Angle between both vectors: " + angleBetweenBothOpenings);
            Quaternion rotation = Quaternion.Euler(new Vector3(0, angleBetweenBothOpenings, 0));

            //Step 5: Calculate the position of the box for check overlap
            Vector3 roomOverlapBoxCenter = gameObject.transform.position + (localPos * -1) + roomBoxCollider.gameObject.transform.localPosition + roomBoxCollider.center;

            //Step 6: Rotate the roomOverlapBox around this room opening position, to align the room correctly to this entrance 
            Vector3 rotatedRoomPivot = RotatePointAroundPivot(roomOverlapBoxCenter, gameObject.transform.position,
                new Vector3(0, angleBetweenBothOpenings, 0));

            //Optional: Draw Box that checks for overlap
            Bounds roomBoxBounds = new Bounds(rotatedRoomPivot, roomBoxCollider.size);
            DebugExtension.DebugBounds(roomBoxBounds, Color.blue, 30, false);

            //Step 7: Check if collider are in the way, to spawn this room
            bool overlapsWithColliders = Physics.CheckBox(rotatedRoomPivot, roomBoxCollider.bounds.extents, Quaternion.identity, LayerMask.GetMask("RoomBoundingBox"));

            //Step 8: Spawn the room, if no colliders are in the way 
            if (!overlapsWithColliders)
            {
                GameObject spawnedRoomPrefab = Instantiate(randomRoomPrefab, rotatedRoomPivot - roomBoxCollider.gameObject.transform.localPosition, rotation, parent.transform);
                Debug.Log("Spawned room: " + spawnedRoomPrefab.gameObject.name);

                //Step 6: Check which room openings from the spawned room are now connected with other room openings and mark them as connected

                fittingRoomFound = true;
            }
            else
            {
                Debug.Log("Could not spawn room");
            }

            counter++;
            if (counter > 24)
            {
                Debug.Log("Could not spawn a room, broke up after " + counter + " attemps.");
                break;
            }
        }

        //(Step 7: Notify original room, that you are done)
    }






    public bool IsConnected()
    {
        return m_Connected;
    }
}
