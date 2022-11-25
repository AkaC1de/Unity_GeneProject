using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitCollection : MonoBehaviour
{
    public GameObject initCollection1;
    public GameObject initCollection2;
    public GameObject initCollection3;
    public static bool off = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        offInitCollection();
    }
    public void offInitCollection()
    {
        if(off == true)
        {
            Destroy(initCollection1);
            Destroy(initCollection2);
            Destroy(initCollection3);
        }
    }
}
