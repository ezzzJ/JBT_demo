using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PunchPanel : MonoBehaviour
{
    private SpriteAtlas punch; //����ͼ��
    private Sprite playerAns;
    private Sprite WolfAns;
    private Sprite[] WolfAnsArray = new Sprite[3];
    private MusicData musicdata;
    private bool isEnd = false;
    private bool isWin = false;

    //���ƹ�ϵ
    private Dictionary<Sprite, string> playerShitouKill;
    private Dictionary<Sprite, string> playerJiandaoKill;
    private Dictionary<Sprite, string> playerBuKill;

    public Text txtWolf;
    public Text txtPlayer;
    public Image imgPlayerAns;
    public Image imgWolfAns;
    public Image imgRes;
    public GameObject btnPlayer;

    public float outSpeed = 0.1f; //�����ٶ�
    public float wolfRandomSpeed = 0.07f; //�������ʾ�����ļ��ʱ��
    
    void Start()
    {
        punch = Resources.Load<SpriteAtlas>("Punch");
        WolfAnsArray[0] = punch.GetSprite("WolfShitou");
        WolfAnsArray[1] = punch.GetSprite("WolfJiandao");
        WolfAnsArray[2] = punch.GetSprite("WolfBu");

        playerShitouKill = new Dictionary<Sprite, string>() { { WolfAnsArray[0], "��" }, { WolfAnsArray[1], "Ӯ" }, { WolfAnsArray[2], "��" } };
        playerJiandaoKill = new Dictionary<Sprite, string>() { { WolfAnsArray[0], "��" }, { WolfAnsArray[1], "��" }, { WolfAnsArray[2], "Ӯ" } };
        playerBuKill = new Dictionary<Sprite, string>() { { WolfAnsArray[0], "Ӯ" }, { WolfAnsArray[1], "��" }, { WolfAnsArray[2], "��" } };
    }

    void Update()
    {
        //�ж�ʤ�� ���Ҷ��򲻹���Ѱʲô��?
        if (isEnd)
        {
            string result = "";
            switch (playerAns.name)
            {
                case "PlayerShitou(Clone)":
                    result = playerShitouKill[WolfAns];
                    break;
                case "PlayerJiandao(Clone)":
                    result = playerJiandaoKill[WolfAns];
                    break;
                case "PlayerBu(Clone)":
                    result = playerBuKill[WolfAns];
                    break;
            }

            if (result == "Ӯ")
            {
                imgRes.sprite = Resources.Load<Sprite>("Win");
                imgRes.gameObject.SetActive(true);
                isWin = true;
                txtPlayer.text = "��� ������Ϸ ����ð�հ�!";
                txtWolf.text = "�ðɣ���Ӯ��";
                GameObject.Find("Player").GetComponent<Player>().playerState = E_PlayerState.Noting;
            }
            else if (result == "��")
            {
                imgRes.sprite = Resources.Load<Sprite>("Lose");
                imgRes.gameObject.SetActive(true);
                txtWolf.text = "���Ҷ��򲻹���Ѱʲô��?";
                txtPlayer.text = "......";
                isWin = false;
            }
        }
    }

    //�������
    IEnumerator WolfTalk(string txt)
    {
        txtWolf.text = "";
        for (int i = 0; i < txt.Length; i++)
        {
            txtWolf.text += txt[i];
            yield return new WaitForSeconds(outSpeed);
        }
    }

    //�ǵ��������������
    IEnumerator WolfChoose()
    {
        int index = 0;
        for (int i = 0; i < Random.Range(20, 25); i++) //�����
        {
            imgWolfAns.sprite = WolfAnsArray[index];
            index = index == 2 ? 0 : index + 1;
            yield return new WaitForSeconds(wolfRandomSpeed);
        }
        WolfAns = imgWolfAns.sprite;
        isEnd = true;
    }

    //���ʯͷ
    public void ClickShitou()
    {
        string str = File.ReadAllText(Application.streamingAssetsPath + "/JBT_musicData.json");
        musicdata = JsonUtility.FromJson<MusicData>(str);

        if (musicdata.isSoundOpen)
            MusicMgr.Instance.PlaySound("Button_Click", musicdata.soundValue, false);
        playerAns = punch.GetSprite("PlayerShitou");
        PanelChanged();
    }
    //�������
    public void ClickJiandao()
    {
        string str = File.ReadAllText(Application.streamingAssetsPath + "/JBT_musicData.json");
        musicdata = JsonUtility.FromJson<MusicData>(str);

        if (musicdata.isSoundOpen)
            MusicMgr.Instance.PlaySound("Button_Click", musicdata.soundValue, false);
        playerAns = punch.GetSprite("PlayerJiandao");
        PanelChanged();
    }
    //�����
    public void ClickBu()
    {
        string str = File.ReadAllText(Application.streamingAssetsPath + "/JBT_musicData.json");
        musicdata = JsonUtility.FromJson<MusicData>(str);

        if (musicdata.isSoundOpen)
            MusicMgr.Instance.PlaySound("Button_Click", musicdata.soundValue, false);
        playerAns = punch.GetSprite("PlayerBu");
        PanelChanged();
    }

    //���������Ϸ
    public void ClickContinue()
    {
        string str = File.ReadAllText(Application.streamingAssetsPath + "/JBT_musicData.json");
        musicdata = JsonUtility.FromJson<MusicData>(str);

        if (musicdata.isSoundOpen)
            MusicMgr.Instance.PlaySound("Button_Click", musicdata.soundValue, false);
        if (isEnd)
        {
            if (!isWin)
            {
                btnPlayer.SetActive(true);
                txtPlayer.gameObject.SetActive(false);
                imgPlayerAns.gameObject.SetActive(false);
                imgWolfAns.gameObject.SetActive(false);
                imgRes.gameObject.SetActive(false);
                txtWolf.text = "���ڵȴ��Է�����";
            }
            else if (isWin)
            {
                PlayerMgr.canMove = true;
                Destroy(gameObject);
            }
        }
    }

    //��Ұ��갴ť�� ���ı任
    public void PanelChanged()
    {
        isEnd = false;
        imgPlayerAns.sprite = playerAns;
        imgPlayerAns.gameObject.SetActive(true); //��ʾ��ɫ����ͼƬ
        btnPlayer.SetActive(false); //���ؽ�ɫ������ť
        txtPlayer.gameObject.SetActive(true); //��ʾ��ɫ�Ի���Ϣ

        //�ǳ���
        StartCoroutine(WolfTalk("���ڳ���..."));
        imgWolfAns.gameObject.SetActive(true);
        StartCoroutine(WolfChoose());
    }
}
