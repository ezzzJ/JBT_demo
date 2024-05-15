using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum E_PlayerState
{
    Noting,
    hotSpring,
    wolf,
    barrier
}

public partial class Player
{
    [NonSerialized]
    public E_PlayerState playerState = E_PlayerState.Noting;

    [NonSerialized]
    public MusicData musicdata;

    private void Start()
    {
        string str = File.ReadAllText(Application.streamingAssetsPath + "/JBT_musicData.json");
        musicdata = JsonUtility.FromJson<MusicData>(str);

        if (musicdata.isMusicOpen)
            MusicMgr.Instance.PlayBKMusic("Game_BGM", musicdata.musicValue, false);
    }
}
