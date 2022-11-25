using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShell : MonoBehaviour
{
    public float speed;
    public float stopTime = .5f;
    public float fadeSpeed = .01f;
    private Rigidbody2D rb;
    private SpriteRenderer SR;
    public float angelNum;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
       
    }
    private void OnEnable()
    {
        float angel = Random.Range(-angelNum, angelNum);
        rb.velocity = Quaternion.AngleAxis(angel, Vector3.forward) * Vector3.up * speed;
        StartCoroutine(Stop());
        SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, 1);
        rb.gravityScale = 3;
    }
    IEnumerator Stop()
    {
        yield return new WaitForSeconds(stopTime);
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        while (SR.color.a > 0)
        {
            SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, SR.color.a - fadeSpeed);
            yield return new WaitForFixedUpdate();
        }
        //Destroy(gameObject);
        ObjectPool.Instance.PushObject(gameObject);
    }
}