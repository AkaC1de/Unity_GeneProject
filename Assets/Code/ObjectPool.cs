using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ʹ�øö����ʱ��Ҫ�ȴ�����������Ĳ���������GetObject�����������������������󣬽���Żض���أ�����PushObject��������
//��������ɵ�������λ��Ҫ��������������Ĵ������Ϊ���������λ�õĴ��롣
public class ObjectPool
{
    private static ObjectPool instance;
    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();
    private GameObject pool;
    
    //����ģʽ
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

    //��������أ����������������Ҫ���ɵ����壬���������ɵ����壩
    public GameObject GetObject(GameObject prefab)
    {
        GameObject _object;//���ڷ��ص�ֵ
        if(!objectPool.ContainsKey(prefab.name)|| objectPool[prefab.name].Count <=0)
            //�ж϶��������û��Ҫ���ɵ����壬����������������
            //���������������һ�������ҵ���PushObject�������������������������
        {
            _object = GameObject.Instantiate(prefab);//��������
            PushObject(_object);//���øú����������ɵ������������
            if (pool == null)//�ж���û�С��ܶ���ء�������������ڷ����������ɵ����壨ʹ���濴������ࣩ
            {
                pool = new GameObject("ObjectPool");//����һ�����ڷ��õĿ����塰�ܶ���ء�
            }
            GameObject childPool = GameObject.Find(prefab.name + "Pool");//�ж��������û�����ڷ��õġ��Ӷ���ء�
            if (!childPool)
            {
                childPool = new GameObject(prefab.name + "Pool");//���ɸ�������Ķ����
                childPool.transform.SetParent(pool.transform);//�ø��Ӷ����λ���ܶ��������
            }
                _object.transform.SetParent(childPool.transform);//�����ɵ�������������Ӷ��������
        }
        //���������������壬�ҹ��ã����ó���һ������Ҫ�õ����壬������ʾ
        _object = objectPool[prefab.name].Dequeue();//�ó�����
        _object.SetActive(true);//��ʾ
        return _object;//����ֵ
    }
    public void PushObject(GameObject prefab)//������Żض����
    {
        string _name = prefab.name.Replace("(Clone)", string.Empty);//�������ɵ�����
        if (!objectPool.ContainsKey(_name))//�������أ��ֵ䣩��û��λ�ã����������һ��λ��
        {
            objectPool.Add(_name, new Queue<GameObject>());//����λ��
        }
        //���������λ�ã������Żأ��ر���ʾ
        objectPool[_name].Enqueue(prefab);//�Ż�
        prefab.SetActive(false);//�ر���ʾ
    }
}
