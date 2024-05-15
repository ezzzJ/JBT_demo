using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPanel : MonoBehaviour
{
    public GameObject menuPanel;
    private MusicData musicdata;

    private void Start()
    {
        string str = File.ReadAllText(Application.streamingAssetsPath + "/JBT_musicData.json");
        musicdata = JsonUtility.FromJson<MusicData>(str);

        if (musicdata.isMusicOpen)
            MusicMgr.Instance.PlayBKMusic("Start_BGM", musicdata.musicValue, false);
    }

    public void Click_Start(string sceneName)
    {
        string str = File.ReadAllText(Application.streamingAssetsPath + "/JBT_musicData.json");
        musicdata = JsonUtility.FromJson<MusicData>(str);

        if (musicdata.isSoundOpen)
            MusicMgr.Instance.PlaySound("Button_Click", musicdata.soundValue, false);
        SceneManager.LoadScene(sceneName);
    }

    public void Click_menu()
    {
        string str = File.ReadAllText(Application.streamingAssetsPath + "/JBT_musicData.json");
        musicdata = JsonUtility.FromJson<MusicData>(str);

        if (musicdata.isSoundOpen)
            MusicMgr.Instance.PlaySound("Button_Click", musicdata.soundValue, false);
        menuPanel.SetActive(true);
    }

    public void Click_exit()
    {
        string str = File.ReadAllText(Application.streamingAssetsPath + "/JBT_musicData.json");
        musicdata = JsonUtility.FromJson<MusicData>(str);

        if (musicdata.isSoundOpen)
            MusicMgr.Instance.PlaySound("Button_Click", musicdata.soundValue, false);
        Application.Quit();
    }
}
