using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//使用该对象池时，要先传入生成物体的参数（调用GetObject函数），并且在用完该物体后，将其放回对象池（调用PushObject函数）。
//如果对生成的物体有位置要求，则在生成物体的代码后面为其添加设置位置的代码。
public class ObjectPool
{
    private static ObjectPool instance;
    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();
    private GameObject pool;
    
    //单例模式
    public static ObjectPool Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ObjectPool();
            }
            return instance;
        }
    }

    //开启对象池，传入参数（传入需要生成的物体，返回所生成的物体）
    public GameObject GetObject(GameObject prefab)
    {
        GameObject _object;//用于返回的值
        if(!objectPool.ContainsKey(prefab.name)|| objectPool[prefab.name].Count <=0)
            //判断对象池中有没有要生成的物体，或者数量够不够用
            //如果不够，就生成一个，并且调用PushObject函数，将生成物体放入对象池中
        {
            _object = GameObject.Instantiate(prefab);//生成物体
            PushObject(_object);//调用该函数，将生成的物体放入对象池
            if (pool == null)//判断有没有“总对象池”这个空物体用于放置下面生成的物体（使界面看起来简洁）
            {
                pool = new GameObject("ObjectPool");//生成一个用于放置的空物体“总对象池”
            }
            GameObject childPool = GameObject.Find(prefab.name + "Pool");//判断下面的有没有用于放置的“子对象池”
            if (!childPool)
            {
                childPool = new GameObject(prefab.name + "Pool");//生成该子物体的对象池
                childPool.transform.SetParent(pool.transform);//让该子对象池位于总对象池下面
            }
                _object.transform.SetParent(childPool.transform);//将生成的子物体放置于子对象池下面
        }
        //如果对象池中有物体，且够用，则拿出来一个现在要用的物体，让他显示
        _object = objectPool[prefab.name].Dequeue();//拿出物体
        _object.SetActive(true);//显示
        return _object;//返回值
    }
    public void PushObject(GameObject prefab)//将物体放回对象池
    {
        string _name = prefab.name.Replace("(Clone)", string.Empty);//查找生成的物体
        if (!objectPool.ContainsKey(_name))//如果对象池（字典）中没有位置，则给他开辟一个位置
        {
            objectPool.Add(_name, new Queue<GameObject>());//开辟位置
        }
        //如果有他的位置，则将他放回，关闭显示
        objectPool[_name].Enqueue(prefab);//放回
        prefab.SetActive(false);//关闭显示
    }
}
