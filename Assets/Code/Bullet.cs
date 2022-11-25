using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public GameObject explosion;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();  
    }
    public void SetSpeed(Vector2 direction)
    {
        rb.velocity = direction * speed;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Instantiate(explosion, transform.position, Quaternion.identity);
        GameObject exp = ObjectPool.Instance.GetObject(explosion);
        exp.transform.position = transform.position;
        Destroy(gameObject);
        //ObjectPool.Instance.PushObject(gameObject);
    }
}
