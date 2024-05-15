using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemWolf : MonoBehaviour
{
    private RaycastHit2D hitUp;
    private RaycastHit2D hitRight;
    private RaycastHit2D hitLeft;
    private RaycastHit2D hitDown;
    private RaycastHit2D hitPlayer;

    public float distance;
    public LayerMask objLayer;
    public Player player;
    public GameObject canvas;

    //是否是引导性关卡
    public bool isOne;

    //怪物的新手教程
    public GameObject Wolf;
    //是否已经引导过
    private bool isGet = false;

    [NonSerialized]
    public bool isShowPunch = false;

    private MusicData musicdata;

    void Update()
    {
        if (isOne && !isGet && player.playerState == E_PlayerState.Noting)
        {
            hitUp = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.up, distance, objLayer);
            hitDown = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.down, distance, objLayer);
            hitLeft = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.left, distance, objLayer);
            hitRight = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.right, distance, objLayer);

            if (hitUp) hitPlayer = hitUp;
            else if (hitDown) hitPlayer = hitDown;
            else if (hitLeft) hitPlayer = hitLeft;
            else if (hitRight) hitPlayer = hitRight;

            if (hitPlayer)
            {
                StartCoroutine(ShowTeach(hitPlayer.collider.gameObject));
                isGet = true;
            }
        }
        if (isShowPunch && player.playerState == E_PlayerState.Noting)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            player = collision.gameObject.GetComponent<Player>();
            player.playerState = E_PlayerState.wolf;
            string str = File.ReadAllText(Application.streamingAssetsPath + "/JBT_musicData.json");
            musicdata = JsonUtility.FromJson<MusicData>(str);

            if (musicdata.isSoundOpen)
                MusicMgr.Instance.PlaySound("Wolf", musicdata.soundValue, false);
            //显示猜拳界面
            if (!isShowPunch)
            {
                PlayerMgr.canMove = false;
                Instantiate(Resources.Load<GameObject>("WolfFightPanel"), canvas.transform);
                isShowPunch = true;
            }
        }
    }

    IEnumerator ShowTeach(GameObject player)
    {
        yield return new WaitUntil(() => player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        PlayerMgr.canMove = false;

        Wolf.SetActive(true);
    }
}
