using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassOnePoint : MonoBehaviour
{
    public int needFootprint = 0;
    public GameObject passPanel;
    public UIRespond ui;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!PlayerMgr.isShowPanel && collision.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            PlayerMgr.canMove = false;
            PlayerMgr.isShowPanel = true;
            if (PlayerMgr.footprintCount == needFootprint)
            {
                passPanel.SetActive(true);
            }
            else if (PlayerMgr.footprintCount != needFootprint)
            {
                ui.Game_refresh();
                PlayerMgr.canMove = true;
            }
        }
    }

    public void Click_next(string scene)
    {
        PlayerMgr.canMove = true;
        PlayerMgr.footprintCount = 0;
        PlayerMgr.isShowPanel = false;
        SceneManager.LoadScene(scene);
    }

    public void Click_back()
    {
        PlayerMgr.canMove = true;
        PlayerMgr.footprintCount = 0;
        PlayerMgr.isShowPanel = false;
        SceneManager.LoadScene("Start");
    }
}
