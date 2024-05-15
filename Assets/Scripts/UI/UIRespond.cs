using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIRespond : MonoBehaviour
{
    public GameObject player; //���
    private Player playerCs; //player�ű�
    private Vector3 playerStartVec;
    public GameObject[] itemObj; //��������
    public GameObject musicPanel;

    private ItemWolf wolf;
    private MusicData musicdata;

    private void Start()
    {
        playerStartVec = player.transform.position;
        playerCs = player.GetComponent<Player>();
    }

    //��Ϸ���� �������˵�
    public void Game_back()
    {
        string str = File.ReadAllText(Application.streamingAssetsPath + "/JBT_musicData.json");
        musicdata = JsonUtility.FromJson<MusicData>(str);

        if (musicdata.isSoundOpen)
            MusicMgr.Instance.PlaySound("Button_Click", musicdata.soundValue, false);
        SceneManager.LoadScene("Start");
    }

    //��Ϸ���� ��������
    public void Game_musicSetting()
    {
        string str = File.ReadAllText(Application.streamingAssetsPath + "/JBT_musicData.json");
        musicdata = JsonUtility.FromJson<MusicData>(str);

        if (musicdata.isSoundOpen)
            MusicMgr.Instance.PlaySound("Button_Click", musicdata.soundValue, false);
        musicPanel.SetActive(true);
    }

    //��Ϸ���� ���¿�ʼ
    public void Game_refresh()
    {
        PlayerMgr.isShowPanel = false;

        string str = File.ReadAllText(Application.streamingAssetsPath + "/JBT_musicData.json");
        musicdata = JsonUtility.FromJson<MusicData>(str);

        if (musicdata.isSoundOpen)
            MusicMgr.Instance.PlaySound("Button_Click", musicdata.soundValue, false);

        //��һص�ԭ��
        PlayerMgr.isMove = false;
        player.transform.position = playerStartVec;
        player.GetComponent<Animator>().SetBool("up", false);
        player.GetComponent<Animator>().SetBool("down", false);
        player.GetComponent<Animator>().SetBool("right", false);
        player.GetComponent<Animator>().SetFloat("Horizontal", 1);
        player.GetComponent<Animator>().SetFloat("Vertical", 0);
        playerCs.playerState = E_PlayerState.Noting;
        playerCs.nowGrid = 11;
        playerCs.nextGrid = 0;
        playerCs.lastGrid = 0;

        //�����ӡ
        foreach(var index in playerCs.gridData.Keys)
            playerCs.gridData[index].obj = null;
        Destroy(GameObject.Find("FootPrint"));
        PlayerMgr.footprintCount = 0;

        //����item
        for (int i = 0; i < itemObj.Length; i++)
        {
            if (itemObj[i].TryGetComponent<ItemWolf>(out wolf))
            {
                wolf.isShowPunch = false;
            }
            itemObj[i].SetActive(true);
        }
    }
}
