using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSilder : TimeControlled
{
    public float recordTime;
    public float timer;
    static public bool init = false;
    private Transform initTransform;
    private void Start()
    {
        initTransform = gameObject.transform;
    }
    public override void TimeUpdate()
    {
        Vector2 pos = transform.position;
        pos.x += 1* Time.deltaTime;
        transform.position = pos;
    }
    public void Update()
    {
        //Timer();
    }
    public void Timer()
    {
        if (timer < recordTime)
        {
            timer += Time.deltaTime;
        }
        if (timer >= recordTime)
        {
            gameObject.transform.position = initTransform.position;
            timer = 0;
            init = true;
            
        }
        
    }
}
