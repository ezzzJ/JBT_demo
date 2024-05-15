using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Barrier_DiolagueOutput : MonoBehaviour
{
    public List<string> txtString;
    public float outSpeed = 0.1f;
    public Text txtDialogue;
    private bool isPress = false;

    void Start()
    {
        StartCoroutine(OutPut());
    }

    IEnumerator OutPut()
    {
        for (int index = 0; index < txtString.Count; index++)
        {
            txtDialogue.text = "";
            for (int i = 0; i < txtString[index].Length; i++)
            {
                txtDialogue.text += txtString[index][i];
                yield return new WaitForSeconds(outSpeed);
            }
            if (index == 0)
            {
                transform.Find("barrier_btnPlayer").gameObject.SetActive(true);
                yield return new WaitUntil(() => isPress == true);
                transform.Find("barrier_btnPlayer").gameObject.SetActive(false);
                isPress = false;
            }
            else
            {
                yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
                if (index == txtString.Count - 1)
                {
                    PlayerMgr.canMove = true;
                    gameObject.SetActive(false);
                }
            }
        }
    }

    public void PressAnwser()
    {
        isPress = true;
    }
}
