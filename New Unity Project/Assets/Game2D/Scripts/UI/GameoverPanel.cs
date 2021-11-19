using System.Collections;
using System.Collections.Generic;
using UnityEngine;  
using TMPro;
using UnityEngine.UI;

namespace Game2D
{
    public class GameoverPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_TxtResult;

        public void DisplayResult(bool isWin)
        {
            if (isWin)
                m_TxtResult.text = "YOU WIN";
            else
                m_TxtResult.text = "YOU LOSE";
        }

        public void BtnNextLevel_Pressed()
        {
            GamePlayManager.Instance.NextLevel();
        }

        public void BtnRestart_Pressed()
        {
            GamePlayManager.Instance.Restart();
        }
    }
}