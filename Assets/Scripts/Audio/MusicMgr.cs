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

    //�������ֲ������
    private AudioSource bkMusic = null;

    //������Ч��������Ķ���
    private GameObject soundObj = null;
    //�������ڲ��ŵ���Ч
    private List<AudioSource> soundList = new List<AudioSource>();
    //��Ч�Ƿ��ڲ���
    private bool soundIsPlay = true;

    public void Update()
    {
        if (!soundIsPlay)
            return;

        //��ͣ�ı������� �����û����Ч������� �������� ���Ƴ�������
        //Ϊ�˱���߱������Ƴ������� ���ǲ����������
        for (int i = soundList.Count - 1; i >= 0; --i)
        {
            if (!soundList[i].isPlaying)
            {
                GameObject.Destroy(soundList[i]);
                soundList.RemoveAt(i);
            }
        }
    }


    //���ű�������
    public void PlayBKMusic(string name, float value, bool isDontDes)
    {
        //��̬�������ű������ֵ���� ���� ����������Ƴ� 
        //��֤���������ڹ�����ʱҲ�ܲ���
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

    //ֹͣ��������
    public void StopBKMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Stop();
    }

    //��ͣ��������
    public void PauseBKMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Pause();
    }

    //���ñ������ִ�С
    public void ChangeBKMusicValue(float v)
    {
        if (bkMusic == null)
            return;
        bkMusic.volume = v;
    }

    /// <summary>
    /// ������Ч
    /// </summary>
    /// <param name="name">��Ч����</param>
    /// <param name="isLoop">�Ƿ�ѭ��</param>
    public void PlaySound(string name, float v, bool isLoop = false)
    {
        if (soundObj == null)
        {
            //��Ч�����Ķ��� һ���������Ч����Ҫֹͣ �������ǿ��Բ����������������Ƴ�
            soundObj = new GameObject("soundObj");
        }

        AudioClip clip = Resources.Load<AudioClip>("AudioRES/" + name);

        //������Ч��Դ ���в���
        AudioSource source = soundObj.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = isLoop;
        source.volume = v;
        source.Play();
        //�洢���� ���ڼ�¼ ����֮���ж��Ƿ�ֹͣ
        soundList.Add(source);
    }

    /// <summary>
    /// ֹͣ������Ч
    /// </summary>
    /// <param name="source">��Ч�������</param>
    public void StopSound(AudioSource source)
    {
        if (soundList.Contains(source))
        {
            //ֹͣ����
            source.Stop();
            //���������Ƴ�
            soundList.Remove(source);
            //�������������Ƴ�
            GameObject.Destroy(source);
        }
    }

    /// <summary>
    /// �ı���Ч��С
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
    /// �������Ż�����ͣ������Ч
    /// </summary>
    /// <param name="isPlay">�Ƿ��Ǽ������� trueΪ���� falseΪ��ͣ</param>
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
