using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MusicMgr
{
    private static MusicMgr instance = null;
    public static MusicMgr Instance
    {
        get
        {
            if (instance == null)
                instance = new MusicMgr();
            return instance;
        }
    }
    private MusicMgr() { }

    //背景音乐播放组件
    private AudioSource bkMusic = null;

    //用于音效组件依附的对象
    private GameObject soundObj = null;
    //管理正在播放的音效
    private List<AudioSource> soundList = new List<AudioSource>();
    //音效是否在播放
    private bool soundIsPlay = true;

    public void Update()
    {
        if (!soundIsPlay)
            return;

        //不停的遍历容器 检测有没有音效播放完毕 播放完了 就移除销毁它
        //为了避免边遍历边移除出问题 我们采用逆向遍历
        for (int i = soundList.Count - 1; i >= 0; --i)
        {
            if (!soundList[i].isPlaying)
            {
                GameObject.Destroy(soundList[i]);
                soundList.RemoveAt(i);
            }
        }
    }


    //播放背景音乐
    public void PlayBKMusic(string name, float value, bool isDontDes)
    {
        //动态创建播放背景音乐的组件 并且 不会过场景移除 
        //保证背景音乐在过场景时也能播放
        if (bkMusic == null)
        {
            GameObject obj = new GameObject();
            obj.name = "BKMusic";
            if (isDontDes) GameObject.DontDestroyOnLoad(obj);
            bkMusic = obj.AddComponent<AudioSource>();
        }

        AudioClip clip = Resources.Load<AudioClip>("AudioRES/" + name);

        bkMusic.clip = clip;
        bkMusic.loop = true;
        bkMusic.volume = value;
        bkMusic.Play();
    }

    //停止背景音乐
    public void StopBKMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Stop();
    }

    //暂停背景音乐
    public void PauseBKMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Pause();
    }

    //设置背景音乐大小
    public void ChangeBKMusicValue(float v)
    {
        if (bkMusic == null)
            return;
        bkMusic.volume = v;
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name">音效名字</param>
    /// <param name="isLoop">是否循环</param>
    public void PlaySound(string name, float v, bool isLoop = false)
    {
        if (soundObj == null)
        {
            //音效依附的对象 一般过场景音效都需要停止 所以我们可以不处理它过场景不移除
            soundObj = new GameObject("soundObj");
        }

        AudioClip clip = Resources.Load<AudioClip>("AudioRES/" + name);

        //加载音效资源 进行播放
        AudioSource source = soundObj.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = isLoop;
        source.volume = v;
        source.Play();
        //存储容器 用于记录 方便之后判断是否停止
        soundList.Add(source);
    }

    /// <summary>
    /// 停止播放音效
    /// </summary>
    /// <param name="source">音效组件对象</param>
    public void StopSound(AudioSource source)
    {
        if (soundList.Contains(source))
        {
            //停止播放
            source.Stop();
            //从容器中移除
            soundList.Remove(source);
            //从依附对象上移除
            GameObject.Destroy(source);
        }
    }

    /// <summary>
    /// 改变音效大小
    /// </summary>
    /// <param name="v"></param>
    public void ChangeSoundValue(float v)
    {
        for (int i = 0; i < soundList.Count; i++)
        {
            if (soundList[i])
                soundList[i].volume = v;
        }
    }

    /// <summary>
    /// 继续播放或者暂停所有音效
    /// </summary>
    /// <param name="isPlay">是否是继续播放 true为播放 false为暂停</param>
    public void PlayOrPauseSound(bool isPlay)
    {
        if (isPlay)
        {
            soundIsPlay = true;
            for (int i = 0; i < soundList.Count; i++)
            {
                if (soundList[i])
                    soundList[i].Play();
            }
        }
        else
        {
            soundIsPlay = false;
            for (int i = 0; i < soundList.Count; i++)
            {
                if (soundList[i])
                    soundList[i].Pause();
            }
        }
    }
}
