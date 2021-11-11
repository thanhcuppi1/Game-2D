using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

namespace Game2D
{
    public class GameplayPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_TxtHP;
        [SerializeField] private Image m_ImgHpBar;

        private void OnHpChanged(int curHp, int maxHp)
        {
            m_TxtHP.text = $"{curHp}/{maxHp}";
            m_ImgHpBar.fillAmount = curHp * 1f / maxHp;
        }

        public void BtnPause_Pressed()
        {
            GamePlayManager.Instance.Pause();
        }
    }
}