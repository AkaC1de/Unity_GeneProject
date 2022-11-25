using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public static float interval = 0.6f;
    public GameObject bulletPrefab;
    public GameObject bulletShellPrefab;
    public Transform muzzlePos;
    public Transform shellPos;
    protected Vector2 mousePos;
    protected Vector2 direction;
    protected float timer;
    public Animator anim;
    protected float flipY;
    public float angelNum;
    public Transform gunPos;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        flipY = transform.localScale.y;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        gameObject.transform.position = gunPos.position;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < transform.position.x)
        {
            transform.localScale = new Vector3(flipY, -flipY, 1);
        }
        else
        {
            transform.localScale = new Vector3(flipY, flipY, 1);
        }
        Shoot();
    }
    public  virtual void Shoot()
    {
        direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        transform.right = direction;

        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
            }
        }
        if (Input.GetButton("Fire1"))
        {
            if (timer == 0)
            {
                timer = interval;
                Fire();
            }
        }
    }
    protected virtual void Fire()
    {
        anim.SetTrigger("Shoot");
        //GameObject bullet = Instantiate(bulletPrefab, muzzlePos.position, Quaternion.identity);
        GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
        bullet.transform.position = muzzlePos.position;

        float angel = Random.Range(-angelNum, angelNum);
        bullet.GetComponent<Bullet>().SetSpeed(Quaternion.AngleAxis(angel, Vector3.forward) * direction);

        //Instantiate(bulletShellPrefab, shellPos.position, shellPos.rotation);
        GameObject bulletShell = ObjectPool.Instance.GetObject(bulletShellPrefab);
        bulletShell.transform.position = shellPos.position;
        bulletShell.transform.rotation = shellPos.rotation;
    }
}
