using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TimeControler : MonoBehaviour
{
    public struct RecordData
    {
        public Vector2 pos;
        public Vector2 vel;
    }
    int recordMax = 10000000;
    int recordCount;
    int recordIndex;
    public bool wasSteppingBack = false;
    RecordData[,] recordData;
    TimeControlled[] timeObjects;
    public GameObject Level1;
    public float conTime;//回溯持续时间
    public float timer;
    public GameObject wall;
    public void Awake()
    {
        timeObjects = GameObject.FindObjectsOfType<TimeControlled>();
        recordData = new RecordData[timeObjects.Length, recordMax];
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        wall = GameObject.FindWithTag("1");
        //bool pause = Input.GetKey(KeyCode.UpArrow);
        //bool stepBack = Input.GetKey(KeyCode.Space);
        if (Timer.isTimeOut == true)
        {
            wasSteppingBack = true;
            Destroy(wall);
            if (recordIndex > 0)
            {
                recordIndex--;

                for (int objectIndex = 0; objectIndex < timeObjects.Length; objectIndex++)
                {
                    TimeControlled timeObject = timeObjects[objectIndex];
                    RecordData data = recordData[objectIndex, recordIndex];
                    timeObject.transform.position = data.pos;
                    timeObject.velocity = data.vel;
                }
            }

            TimeOut();
        }
        if (Timer.isTimeOut == false)
        {

            for (int objectIndex = 0; objectIndex < timeObjects.Length; objectIndex++)
            {
                TimeControlled timeObject = timeObjects[objectIndex];
                RecordData data = new RecordData();
                data.pos = timeObject.transform.position;
                data.vel = timeObject.velocity;
                recordData[objectIndex, recordCount] = data;
            }
            recordCount++;
            recordIndex = recordCount;
            foreach (TimeControlled timeObject in timeObjects)
            {
                timeObject.TimeUpdate();
            }
        }
        //ActTimeBack1();
    }
    public void Init()
    {
        if (TimeSilder.init == true)
        {
            Array.Clear(timeObjects, 0, timeObjects.Length);
            Array.Clear(recordData, 0, recordData.Length);
            TimeSilder.init = false;
        }
    }
    public void TimeOut()
    {
        if (Timer.isTimeOut == true && recordIndex == 0)
        {
            Level1.SetActive(true);
        }
    }
    public void ActTimeBack1()
    {

        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        { 
            if (timer == 0)
            {
                ActTimeBack2();
                timer = conTime;
                recordCount = recordIndex;
            }
            wasSteppingBack = false;
        }
    }

    public void ActTimeBack2()
    {
        wasSteppingBack = true;
        if (recordIndex > 0)
        {
            recordIndex--;

            for (int objectIndex = 0; objectIndex < timeObjects.Length; objectIndex++)
            {
                TimeControlled timeObject = timeObjects[objectIndex];
                RecordData data = recordData[objectIndex, recordIndex];
                timeObject.transform.position = data.pos;
                timeObject.velocity = data.vel;
            }
        }
    
    }
}
        
 

