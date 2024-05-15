using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMgr
{
    //单例
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

    //取物体
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


    //消除脚印
    public void PushObj(GameObject obj)
    {
        obj.SetActive(false);

        if (!poolDic.ContainsKey(obj.name))
            poolDic.Add(obj.name, new Stack<GameObject>());

        poolDic[obj.name].Push(obj);
    }

    //清空场景上的脚印
    public void ClearPool()
    {
        poolDic.Clear();
    }
}
