using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMgr
{
    //����
    private static PoolMgr instance = null;
    public static PoolMgr Instance
    {
        get
        {
            if (instance == null)
                instance = new PoolMgr();
            return instance;
        }
    }
    private PoolMgr() { }

    private Dictionary<string, Stack<GameObject>> poolDic = new Dictionary<string, Stack<GameObject>>();

    //ȡ����
    public GameObject GetObj(string name)
    {
        GameObject obj;
        if (poolDic.ContainsKey(name) && poolDic[name].Count > 0)
        {
            obj = poolDic[name].Pop();
            obj.SetActive(true);
        }
        else
        {
            obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
            obj.name = name;
        }

        return obj;
    }


    //������ӡ
    public void PushObj(GameObject obj)
    {
        obj.SetActive(false);

        if (!poolDic.ContainsKey(obj.name))
            poolDic.Add(obj.name, new Stack<GameObject>());

        poolDic[obj.name].Push(obj);
    }

    //��ճ����ϵĽ�ӡ
    public void ClearPool()
    {
        poolDic.Clear();
    }
}
