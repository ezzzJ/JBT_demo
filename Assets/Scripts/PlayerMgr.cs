using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMgr
{
    //µ¥Àý
    private static PlayerMgr instance = null;
    public static PlayerMgr Instance
    {
        get
        {
            if (instance == null)
                instance = new PlayerMgr();
            return instance;
        }
    }
    private PlayerMgr() { }

    public static int footprintCount = 0;
    public static bool canMove = true;

    public static bool isMove = false;
    public static bool isShowPanel = false;
}
