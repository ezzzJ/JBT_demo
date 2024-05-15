using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footprintDes : MonoBehaviour
{
    private Player player;
    private bool isDes = false;
    private int desGrid;
    private void OnMouseDown()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if (player.playerState == E_PlayerState.hotSpring && player.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            if (player.gridData.ContainsKey(player.nowGrid + 10) && player.gridData[player.nowGrid + 10].data == transform.position)
            {
                isDes = true;
                desGrid = player.nowGrid + 10;
            }
            if (player.gridData.ContainsKey(player.nowGrid - 10) && player.gridData[player.nowGrid - 10].data == transform.position)
            {
                isDes = true;
                desGrid = player.nowGrid - 10;
            }
            if (player.gridData.ContainsKey(player.nowGrid - 1) && player.gridData[player.nowGrid - 1].data == transform.position)
            {
                isDes = true;
                desGrid = player.nowGrid - 1;
            }
            if (player.gridData.ContainsKey(player.nowGrid + 1) && player.gridData[player.nowGrid + 1].data == transform.position)
            {
                isDes = true;
                desGrid = player.nowGrid + 1;
            }
        }
        if (isDes)
        {
            player.playerState = E_PlayerState.Noting;
            player.gridData[desGrid].obj = null;
            PlayerMgr.footprintCount--;
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        player = collision.gameObject.GetComponent<Player>();
        if (collision.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            collision.transform.position = player.gridData[player.lastGrid].data;
            player.nowGrid = player.lastGrid;
        }
    }
}
