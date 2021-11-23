using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Game2D
{
    public class GameoverPanel : MonoBehaviour
    {
        [SerializeField] private Button m_BtnNextLevel;
        [SerializeField] private TextMeshProUGUI m_TxtResult;

        public void DisplayResult(bool isWin)
        {
            if (isWin)
            {
                m_TxtResult.text = "YOU WIN";
                string nextLevel = GamePlayManager.Instance.LevelsData.GetNextLevel();
                if (string.IsNullOrEmpty(nextLevel))
                    m_BtnNextLevel.gameObject.SetActive(false);
            }
            else
            {
                m_TxtResult.text = "YOU LOSE";
                m_BtnNextLevel.gameObject.SetActive(false);
            }
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