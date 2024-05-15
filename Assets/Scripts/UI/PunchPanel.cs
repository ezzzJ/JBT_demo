using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PunchPanel : MonoBehaviour
{
    private SpriteAtlas punch; //招数图集
    private Sprite playerAns;
    private Sprite WolfAns;
    private Sprite[] WolfAnsArray = new Sprite[3];
    private MusicData musicdata;
    private bool isEnd = false;
    private bool isWin = false;

    //克制关系
    private Dictionary<Sprite, string> playerShitouKill;
    private Dictionary<Sprite, string> playerJiandaoKill;
    private Dictionary<Sprite, string> playerBuKill;

    public Text txtWolf;
    public Text txtPlayer;
    public Image imgPlayerAns;
    public Image imgWolfAns;
    public Image imgRes;
    public GameObject btnPlayer;

    public float outSpeed = 0.1f; //吐字速度
    public float wolfRandomSpeed = 0.07f; //狼随机显示招数的间隔时间
    
    void Start()
    {
        punch = Resources.Load<SpriteAtlas>("Punch");
        WolfAnsArray[0] = punch.GetSprite("WolfShitou");
        WolfAnsArray[1] = punch.GetSprite("WolfJiandao");
        WolfAnsArray[2] = punch.GetSprite("WolfBu");

        playerShitouKill = new Dictionary<Sprite, string>() { { WolfAnsArray[0], "输" }, { WolfAnsArray[1], "赢" }, { WolfAnsArray[2], "输" } };
        playerJiandaoKill = new Dictionary<Sprite, string>() { { WolfAnsArray[0], "输" }, { WolfAnsArray[1], "输" }, { WolfAnsArray[2], "赢" } };
        playerBuKill = new Dictionary<Sprite, string>() { { WolfAnsArray[0], "赢" }, { WolfAnsArray[1], "输" }, { WolfAnsArray[2], "输" } };
    }

    void Update()
    {
        //判断胜负 连我都打不过还寻什么宝?
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

            if (result == "赢")
            {
                imgRes.sprite = Resources.Load<Sprite>("Win");
                imgRes.gameObject.SetActive(true);
                isWin = true;
                txtPlayer.text = "点击 继续游戏 继续冒险吧!";
                txtWolf.text = "好吧，你赢了";
                GameObject.Find("Player").GetComponent<Player>().playerState = E_PlayerState.Noting;
            }
            else if (result == "输")
            {
                imgRes.sprite = Resources.Load<Sprite>("Lose");
                imgRes.gameObject.SetActive(true);
                txtWolf.text = "连我都打不过还寻什么宝?";
                txtPlayer.text = "......";
                isWin = false;
            }
        }
    }

    //逐字输出
    IEnumerator WolfTalk(string txt)
    {
        txtWolf.text = "";
        for (int i = 0; i < txt.Length; i++)
        {
            txtWolf.text += txt[i];
            yield return new WaitForSeconds(outSpeed);
        }
    }

    //狼的招数是随机出的
    IEnumerator WolfChoose()
    {
        int index = 0;
        for (int i = 0; i < Random.Range(20, 25); i++) //随机次
        {
            imgWolfAns.sprite = WolfAnsArray[index];
            index = index == 2 ? 0 : index + 1;
            yield return new WaitForSeconds(wolfRandomSpeed);
        }
        WolfAns = imgWolfAns.sprite;
        isEnd = true;
    }

    //点击石头
    public void ClickShitou()
    {
        string str = File.ReadAllText(Application.streamingAssetsPath + "/JBT_musicData.json");
        musicdata = JsonUtility.FromJson<MusicData>(str);

        if (musicdata.isSoundOpen)
            MusicMgr.Instance.PlaySound("Button_Click", musicdata.soundValue, false);
        playerAns = punch.GetSprite("PlayerShitou");
        PanelChanged();
    }
    //点击剪刀
    public void ClickJiandao()
    {
        string str = File.ReadAllText(Application.streamingAssetsPath + "/JBT_musicData.json");
        musicdata = JsonUtility.FromJson<MusicData>(str);

        if (musicdata.isSoundOpen)
            MusicMgr.Instance.PlaySound("Button_Click", musicdata.soundValue, false);
        playerAns = punch.GetSprite("PlayerJiandao");
        PanelChanged();
    }
    //点击布
    public void ClickBu()
    {
        string str = File.ReadAllText(Application.streamingAssetsPath + "/JBT_musicData.json");
        musicdata = JsonUtility.FromJson<MusicData>(str);

        if (musicdata.isSoundOpen)
            MusicMgr.Instance.PlaySound("Button_Click", musicdata.soundValue, false);
        playerAns = punch.GetSprite("PlayerBu");
        PanelChanged();
    }

    //点击继续游戏
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
                txtWolf.text = "正在等待对方出招";
            }
            else if (isWin)
            {
                PlayerMgr.canMove = true;
                Destroy(gameObject);
            }
        }
    }

    //玩家按完按钮后 面板的变换
    public void PanelChanged()
    {
        isEnd = false;
        imgPlayerAns.sprite = playerAns;
        imgPlayerAns.gameObject.SetActive(true); //显示角色招数图片
        btnPlayer.SetActive(false); //隐藏角色招数按钮
        txtPlayer.gameObject.SetActive(true); //显示角色对话信息

        //狼出招
        StartCoroutine(WolfTalk("正在出招..."));
        imgWolfAns.gameObject.SetActive(true);
        StartCoroutine(WolfChoose());
    }
}
