using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomCreater : MonoBehaviour
{
    public enum Direction
    {
        up,down,left,right
    }
    public Direction direction;
    [Header("房间信息")]
    public GameObject roomPrefab;
    public int roomNumber;
    public Color startColor, endColor;
    [Header("位置控制")]
    public Transform createrPoint;
    public float xOffest;
    public float yOffest;
    public LayerMask roomLayer;
    public GameObject endRoom;
    public int maxStep;
    public List<Room> rooms = new List<Room>();
    List<GameObject> farRooms = new List<GameObject>();
    List<GameObject> lessFarRooms = new List<GameObject>();
    List<GameObject> oneWayRooms = new List<GameObject>();
    public WallType wallType;
    public GameObject[] roomPre;
    private int roomPreNum;
    public int preNum;
    public GameObject bossRoom;

    // Start is called before the first frame update
    void RoomCreate()
    {
        roomPreNum = Random.Range(0, preNum);
        rooms.Add(Instantiate(roomPre[roomPreNum], createrPoint.position, Quaternion.identity).GetComponent<Room>());

    }
    void Start()
    {
        rooms.Add(Instantiate(roomPrefab, createrPoint.position, Quaternion.identity).GetComponent<Room>());
        ChangePoint();
        for (int i = 0; i< roomNumber; i++)
        {
            //rooms.Add(Instantiate(roomPrefab, createrPoint.position, Quaternion.identity).GetComponent<Room>());
            RoomCreate();
            ChangePoint();
        }
        rooms[0].GetComponent<SpriteRenderer>().color = startColor;
        foreach (var room in rooms)
        {
            //if (room.transform.position.sqrMagnitude>endRoom.transform.position.magnitude)
            //{
            //    endRoom = room.gameObject;
            //}
            SetRoom(room, room.transform.position);
        }
        FindEndRoom();
        endRoom.GetComponent<SpriteRenderer>().color = endColor;
        rooms.Add(Instantiate(bossRoom, endRoom.transform.position, Quaternion.identity).GetComponent<Room>());
        endRoom.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
       // if (Input.anyKeyDown)
       // {
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       // }
    }
    public void ChangePoint()
    {
        do
        {
            direction = (Direction)Random.Range(0, 4);
            switch (direction)
            {
                case Direction.up:
                    createrPoint.position += new Vector3(0, yOffest, 0);
                    break;
                case Direction.down:
                    createrPoint.position += new Vector3(0, -yOffest, 0);
                    break;
                case Direction.left:
                    createrPoint.position += new Vector3(-xOffest, 0, 0);
                    break;
                case Direction.right:
                    createrPoint.position += new Vector3(xOffest, 0, 0);
                    break;
            }
        }
        while(Physics2D.OverlapCircle(createrPoint.position,0.2f,roomLayer));
        
    }
    public void SetRoom(Room newRoom,Vector3 roomPosition)
    {
        newRoom.roomUp = Physics2D.OverlapCircle(roomPosition + new Vector3(0, yOffest, 0), 0.2f, roomLayer);
        newRoom.roomDown = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -yOffest, 0), 0.2f, roomLayer);
        newRoom.roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffest, 0, 0), 0.2f, roomLayer);
        newRoom.roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffest, 0, 0), 0.2f, roomLayer);
        newRoom.UpdateRoom();
        switch (newRoom.doorNum)
        {
           case 1:
                if (newRoom.roomUp)
                    Instantiate(wallType.wallUp, roomPosition, Quaternion.identity);
                if (newRoom.roomDown)
                    Instantiate(wallType.wallDown, roomPosition, Quaternion.identity);
                if (newRoom.roomLeft)
                    Instantiate(wallType.wallLeft, roomPosition, Quaternion.identity);
                if (newRoom.roomRight)
                    Instantiate(wallType.wallRight, roomPosition, Quaternion.identity);
                break;
            case 2:
                if (newRoom.roomUp && newRoom.roomDown)
                    Instantiate(wallType.wallUpDown, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomLeft)
                    Instantiate(wallType.wallUpLeft, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomRight)
                    Instantiate(wallType.wallUpRight, roomPosition, Quaternion.identity);
                if (newRoom.roomDown && newRoom.roomLeft)
                    Instantiate(wallType.wallDownLeft, roomPosition, Quaternion.identity);
                if (newRoom.roomDown && newRoom.roomRight)
                    Instantiate(wallType.wallDownRight, roomPosition, Quaternion.identity);
                if (newRoom.roomLeft && newRoom.roomRight)
                    Instantiate(wallType.wallLeftRight, roomPosition, Quaternion.identity);
                break;
            case 3:
                if (newRoom.roomUp && newRoom.roomDown && newRoom.roomLeft)
                    Instantiate(wallType.wallUpDownLeft, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomDown && newRoom.roomRight)
                    Instantiate(wallType.wallUpDownRight, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomRight && newRoom.roomLeft)
                    Instantiate(wallType.wallUpLeftRight, roomPosition, Quaternion.identity);
                if (newRoom.roomDown && newRoom.roomRight && newRoom.roomLeft)
                    Instantiate(wallType.wallDownLeftRight, roomPosition, Quaternion.identity);
                break;
            case 4:
                if (newRoom.roomUp && newRoom.roomDown && newRoom.roomRight && newRoom.roomLeft)
                    Instantiate(wallType.wallAll, roomPosition, Quaternion.identity);
                break;
        }
    }
    public void FindEndRoom()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].stepToStart > maxStep)
            {
                maxStep = rooms[i].stepToStart;
            }
        }
            foreach (var room in rooms)
            {
                if(room.stepToStart == maxStep)
                {
                    farRooms.Add(room.gameObject);
                }
                if(room.stepToStart == maxStep - 1)
                {
                    lessFarRooms.Add(room.gameObject);
                }
            }
        for (int i = 0; i < farRooms.Count; i++)
        {
                if(farRooms[i].GetComponent<Room>().doorNum == 1)
            {
                oneWayRooms.Add(farRooms[i]);
            }
        }
        for (int i = 0; i < lessFarRooms.Count; i++)
        {
            if (lessFarRooms[i].GetComponent<Room>().doorNum == 1)
            {
                oneWayRooms.Add(lessFarRooms[i]);
            }
        }
        if (oneWayRooms.Count != 0)
        {
            endRoom = oneWayRooms[Random.Range(0, oneWayRooms.Count)];
        }
        else
        {
            endRoom = farRooms[Random.Range(0, farRooms.Count)];
        }
    }
    [System.Serializable]
    public class WallType
    {
        public GameObject wallLeft, wallRight, wallUp, wallDown,
                          wallUpDown, wallUpLeft, wallUpRight, wallDownLeft, wallDownRight,wallLeftRight,
                          wallUpDownLeft, wallUpDownRight, wallUpLeftRight, wallDownLeftRight,
                          wallAll;
    }
}
