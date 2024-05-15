using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class ItemHotSpring : MonoBehaviour
{
    private RaycastHit2D hitUp;
    private RaycastHit2D hitRight;
    private RaycastHit2D hitLeft;
    private RaycastHit2D hitDown;
    private RaycastHit2D hitPlayer;

    public float distance;
    public LayerMask objLayer;

    //�Ƿ��������Թؿ�
    public bool isOne;

    //��Ȫ�����ֽ̳�
    public GameObject HotSpring;
    //�Ƿ��Ѿ�������
    private bool isGet = false;

    private MusicData musicdata;

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //������״̬��Ϊhotspring
        collider.gameObject.GetComponent<Player>().playerState = E_PlayerState.hotSpring;
        string str = File.ReadAllText(Application.streamingAssetsPath + "/JBT_musicData.json");
        musicdata = JsonUtility.FromJson<MusicData>(str);

        if (musicdata.isSoundOpen)
            MusicMgr.Instance.PlaySound("HotPot", musicdata.soundValue, true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isOne && !isGet && collision.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            PlayerMgr.canMove = false;
            HotSpring.SetActive(true);
            isGet = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        MusicMgr.Instance.PlayOrPauseSound(false);
        collision.gameObject.GetComponent<Player>().playerState = E_PlayerState.Noting;
        gameObject.SetActive(false);
    }
}