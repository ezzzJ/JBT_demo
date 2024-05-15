using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    [NonSerialized]
    //地图格子数据
    public Dictionary<int, GridData> gridData = new Dictionary<int, GridData> {
        { 11, new GridData{ data = new Vector3(-5.15f, -4f, 0)} },
        { 12, new GridData{ data = new Vector3(-3.5f, -4f, 0)} },
        { 13, new GridData{ data = new Vector3(-1.7f, -4f, 0)} },
        { 14, new GridData{ data = new Vector3(0, -4f, 0)} },
        { 15, new GridData{ data = new Vector3(1.75f, -4f, 0)} },
        { 16, new GridData{ data = new Vector3(3.47f, -4f, 0)} },
        { 17, new GridData{ data = new Vector3(5.2f, -4f, 0)} },

        { 21, new GridData{ data = new Vector3(-5.15f, -2.3f, 0)} },
        { 22, new GridData{ data = new Vector3(-3.5f, -2.3f, 0)} },
        { 23, new GridData{ data = new Vector3(-1.7f, -2.3f, 0)} },
        { 24, new GridData{ data = new Vector3(0, -2.3f, 0)} },
        { 25, new GridData{ data = new Vector3(1.75f, -2.3f, 0)} },
        { 26, new GridData{ data = new Vector3(3.47f, -2.3f, 0)} },
        { 27, new GridData{ data = new Vector3(5.2f, -2.3f, 0)} },

        { 31, new GridData{ data = new Vector3(-5.15f, -0.5f, 0)} },
        { 32, new GridData{ data = new Vector3(-3.5f, -0.5f, 0)} },
        { 33, new GridData{ data = new Vector3(-1.7f, -0.5f, 0)} },
        { 34, new GridData{ data = new Vector3(0, -0.5f, 0)} },
        { 35, new GridData{ data = new Vector3(1.75f, -0.5f, 0)} },
        { 36, new GridData{ data = new Vector3(3.47f, -0.5f, 0)} },
        { 37, new GridData{ data = new Vector3(5.2f, -0.5f, 0)} },

        { 41, new GridData{ data = new Vector3(-5.15f, 1.2f, 0)} },
        { 42, new GridData{ data = new Vector3(-3.5f, 1.2f, 0)} },
        { 43, new GridData{ data = new Vector3(-1.7f, 1.2f, 0)} },
        { 44, new GridData{ data = new Vector3(0, 1.2f, 0)} },
        { 45, new GridData{ data = new Vector3(1.75f, 1.2f, 0)} },
        { 46, new GridData{ data = new Vector3(3.47f, 1.2f, 0)} },
        { 47, new GridData{ data = new Vector3(5.2f, 1.2f, 0)} },

        { 51, new GridData{ data = new Vector3(-5.15f, 3f, 0)} },
        { 52, new GridData{ data = new Vector3(-3.5f, 3f, 0)} },
        { 53, new GridData{ data = new Vector3(-1.7f, 3f, 0)} },
        { 54, new GridData{ data = new Vector3(0, 3f, 0)} },
        { 55, new GridData{ data = new Vector3(1.75f, 3f, 0)} },
        { 56, new GridData{ data = new Vector3(3.47f, 3f, 0)} },
        { 57, new GridData{ data = new Vector3(5.2f, 3f, 0)} },
    };

    private List<int> lockArea = new List<int>() { 51, 52, 53, 54, 55, 56, 57, 43, 44, 45, 46, 47};

    [NonSerialized]
    public int nowGrid = 11;

    [NonSerialized]
    public int lastGrid;

    [NonSerialized]
    public int nextGrid;

    private char faceToward;

    private Animator animator;
    private AnimatorStateInfo nowAnimation;
    private SpriteRenderer spRenderer;
    private SpriteRenderer fpRenderer;
    public bool isOne = true;
    private GameObject footPrint;

    [SerializeField]
    private float moveSpeed = 1f;

    void Awake()
    {
        spRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        footPrint = GameObject.Find("FootPrint");
    }

    void Update()
    {
        Debug.Log(PlayerMgr.footprintCount);
        nowAnimation = animator.GetCurrentAnimatorStateInfo(0);
        if (!PlayerMgr.canMove)
            return;
        //处于待机状态
        if (nowAnimation.IsName("Idle"))
        {
            MoveAction();
        }

        //处于移动状态
        else
        {
            if (PlayerMgr.isMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, gridData[nextGrid].data, moveSpeed * Time.deltaTime);
            }
            if (!gridData[nowGrid].obj)
            {
                //留脚印
                GameObject footprint = new GameObject();
                footprint.name = "footprint";
                if (!footPrint) footPrint = new GameObject{ name = "FootPrint"};
                footprint.transform.parent = footPrint.transform;
                BoxCollider2D collider = footprint.AddComponent<BoxCollider2D>();
                footprint.AddComponent<footprintDes>();
                fpRenderer = footprint.AddComponent<SpriteRenderer>();

                collider.offset = new Vector2(0, 2);
                collider.size = new Vector2(11, 12);
                fpRenderer.sprite = Resources.Load<Sprite>("footprint");
                fpRenderer.sortingLayerName = "Item";

                footprint.transform.position = gridData[nowGrid].data;
                footprint.transform.localScale = new Vector3(0.1f, 0.1f, 0);
                footprint.layer = LayerMask.NameToLayer("Item");

                gridData[nowGrid].obj = footprint;
                PlayerMgr.footprintCount++;
            }
        }

        //移动完毕
        if (gridData.ContainsKey(nextGrid) && transform.position == gridData[nextGrid].data)
        {
            animator.SetBool("up", false);
            animator.SetBool("right", false);
            animator.SetBool("down", false);

            nowGrid = nextGrid;
            switch (faceToward)
            {
                case 'W':
                    animator.SetFloat("Horizontal", 0f);
                    animator.SetFloat("Vertical", 1f);
                    break;
                case 'A':
                    animator.SetFloat("Horizontal", -1f);
                    animator.SetFloat("Vertical", 0f);
                    break;
                case 'S':
                    animator.SetFloat("Horizontal", 0f);
                    animator.SetFloat("Vertical", -1f);
                    break;
                case 'D':
                    animator.SetFloat("Horizontal", 1f);
                    animator.SetFloat("Vertical", 0f);
                    break;
            }
        }
    }

    private void MoveAction()
    {
        //WASD移动
        if (Input.GetKeyDown(KeyCode.W))
        {
            nextGrid = nowGrid + 10;
            if (!gridData.ContainsKey(nextGrid))
                return;
            if (isOne && lockArea.Contains(nextGrid))
                return;
            animator.SetBool("up", true);
            faceToward = 'W';
            lastGrid = nowGrid;
            PlayerMgr.isMove = true;

            string str = File.ReadAllText(Application.streamingAssetsPath + "/JBT_musicData.json");
            musicdata = JsonUtility.FromJson<MusicData>(str);
            if (musicdata.isSoundOpen)
                MusicMgr.Instance.PlaySound("Walk", musicdata.soundValue, false);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            nextGrid = nowGrid - 1;
            if (!gridData.ContainsKey(nextGrid))
                return;
            if (isOne && lockArea.Contains(nextGrid))
                return;
            spRenderer.flipX = true;
            animator.SetBool("right", true);
            faceToward = 'A';
            lastGrid = nowGrid;
            PlayerMgr.isMove = true;

            string str = File.ReadAllText(Application.streamingAssetsPath + "/JBT_musicData.json");
            musicdata = JsonUtility.FromJson<MusicData>(str);
            if (musicdata.isSoundOpen)
                MusicMgr.Instance.PlaySound("Walk", musicdata.soundValue, false);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            nextGrid = nowGrid - 10;
            if (!gridData.ContainsKey(nextGrid))
                return;
            if (isOne && lockArea.Contains(nextGrid))
                return;
            animator.SetBool("down", true);
            faceToward = 'S';
            lastGrid = nowGrid;
            PlayerMgr.isMove = true;

            string str = File.ReadAllText(Application.streamingAssetsPath + "/JBT_musicData.json");
            musicdata = JsonUtility.FromJson<MusicData>(str);
            if (musicdata.isSoundOpen)
                MusicMgr.Instance.PlaySound("Walk", musicdata.soundValue, false);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            nextGrid = nowGrid + 1;
            if (!gridData.ContainsKey(nextGrid))
                return;
            if (isOne && lockArea.Contains(nextGrid))
                return;
            spRenderer.flipX = false;
            animator.SetBool("right", true);
            faceToward = 'D';
            lastGrid = nowGrid;
            PlayerMgr.isMove = true;

            string str = File.ReadAllText(Application.streamingAssetsPath + "/JBT_musicData.json");
            musicdata = JsonUtility.FromJson<MusicData>(str);
            if (musicdata.isSoundOpen)
                MusicMgr.Instance.PlaySound("Walk", musicdata.soundValue, false);
        }
    }
}
