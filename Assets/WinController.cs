using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinController : MonoBehaviour
{
    [SerializeField]
    private GameObject WinScreenLeftPlayer, WinScreenRightPlayer;

    public static void Win(bool leftPlayer)
    {
        if (leftPlayer) 
        {
            FindObjectOfType<WinController>().WinScreenLeftPlayer.SetActive(true);
        }
        else
        {
            FindObjectOfType<WinController>().WinScreenRightPlayer.SetActive(true);
        }
    }
}
