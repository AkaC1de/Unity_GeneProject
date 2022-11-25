using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float time;
    public Text timerText;
    public static bool isTimeOut= false;
    public GameObject Level1;
    private bool startTime = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartTimer();
    }
    void StartTimer()
    {
        timerText.text = time.ToString("F2");
        if (isTimeOut == false && startTime == true)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                isTimeOut = true;
                startTime = false;
                timerText.text = "00:00";
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //��ײ��ʼ��ʱ
        if(collision.CompareTag("OtherGround"))
        {
            startTime = true;
        }
        //�ռ���Ʒ
        if (collision.CompareTag("TimeUp"))
        {
            time += 10;
            Destroy(collision.gameObject);
        }
        //�ռ���ʼ��Ʒ
        if (collision.CompareTag("InitTimeUp"))
        {
            time += 15;
            InitCollection.off = true;
        }
    }
    
}
