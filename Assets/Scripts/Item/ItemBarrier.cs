using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemBarrier : MonoBehaviour
{
    private RaycastHit2D hitUp;
    private RaycastHit2D hitRight;
    private RaycastHit2D hitLeft;
    private RaycastHit2D hitDown;
    private RaycastHit2D hitPlayer;

    public float distance;
    public LayerMask objLayer;
    public Player player;

    //是否是引导性关卡
    public bool isOne;

    //障碍的新手教程
    public GameObject Barrier;
    //是否已经引导过
    private bool isGet = false;

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
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            player = collision.gameObject.GetComponent<Player>();
            collision.gameObject.transform.position = player.gridData[player.lastGrid].data;
        }
    }

    IEnumerator ShowTeach(GameObject player)
    {
        yield return new WaitUntil(() => player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        PlayerMgr.canMove = false;

        Barrier.SetActive(true);
    }
}
