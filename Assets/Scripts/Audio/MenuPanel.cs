using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    public Toggle togMusic;
    public Toggle togSound;
    public Slider sliderMusic;
    public Slider sliderSound;
    public Button btnClose;

    [NonSerialized]
    public MusicData musicdata = new();

    private void Awake()
    {
        ReadMusicData(Application.streamingAssetsPath + "/JBT_musicData.json");
    }

    void Start()
    {
        togMusic.isOn = musicdata.isMusicOpen;
        togSound.isOn = musicdata.isSoundOpen;
        sliderMusic.value = musicdata.musicValue;
        sliderSound.value = musicdata.soundValue;

        togMusic.onValueChanged.AddListener((bool isOpen) =>
        {
            musicdata.isMusicOpen = isOpen;
            if (isOpen)
            {
                MusicMgr.Instance.PlayBKMusic("Start_BGM", musicdata.musicValue, false);
            }
            else if (!isOpen && GameObject.Find("BKMusic"))
                MusicMgr.Instance.StopBKMusic();
        });
        togSound.onValueChanged.AddListener((bool isOpen) =>
        {
            musicdata.isSoundOpen = isOpen;
            if (!isOpen && GameObject.Find("soundObj"))
                MusicMgr.Instance.PlayOrPauseSound(false);
        });

        sliderMusic.onValueChanged.AddListener((float value) =>
        {
            musicdata.musicValue = value;
            if (GameObject.Find("BKMusic"))
                MusicMgr.Instance.ChangeBKMusicValue(value);
        });
        sliderSound.onValueChanged.AddListener((float value) =>
        {
            musicdata.soundValue = value;
            if (GameObject.Find("soundObj"))
                MusicMgr.Instance.ChangeSoundValue(value);
        });
    }

    public void Clidk_close()
    {
        WriteMusicData(Application.streamingAssetsPath + "/JBT_musicData.json");
        string str = File.ReadAllText(Application.streamingAssetsPath + "/JBT_musicData.json");
        musicdata = JsonUtility.FromJson<MusicData>(str);

        if (musicdata.isSoundOpen)
            MusicMgr.Instance.PlaySound("Button_Click", musicdata.soundValue, false);
        gameObject.SetActive(false);
    }

    //把数据读取出来
    public void ReadMusicData(string path)
    {
        string strContent = File.ReadAllText(path);
        musicdata = JsonUtility.FromJson<MusicData>(strContent);
    }

    //把数据存到硬盘中
    public void WriteMusicData(string path)
    {
        string strContent = JsonUtility.ToJson(musicdata);
        File.WriteAllText(path, strContent);
    }
}
