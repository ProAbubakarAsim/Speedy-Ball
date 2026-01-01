using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OnefallGames
{
    public class PlayingViewController : MonoBehaviour
    {
        [SerializeField] private RectTransform topBarTrans = null;
        [SerializeField] private Image levelProgressImg = null;
        [SerializeField] private Text currentLevelTxt = null;
        [SerializeField] private Text nextLevelTxt = null;


        public void OnShow()
        {
            ViewManager.Instance.MoveRect(topBarTrans, topBarTrans.anchoredPosition, new Vector2(topBarTrans.anchoredPosition.x, -275f), 1f);
            //ViewManager.Instance.MoveRect(bottomBarTrans, bottomBarTrans.anchoredPosition, new Vector2(bottomBarTrans.anchoredPosition.x, 150f), 1f);
            currentLevelTxt.text = IngameManager.Instance.CurrentLevel.ToString();
            nextLevelTxt.text = (IngameManager.Instance.CurrentLevel + 1).ToString();
            if (!IngameManager.Instance.IsRevived)
            {
                levelProgressImg.fillAmount = 0;
            }
        }

        private void OnDisable()
        {
            topBarTrans.anchoredPosition = new Vector2(topBarTrans.anchoredPosition.x, 100f);
            //bottomBarTrans.anchoredPosition = new Vector2(bottomBarTrans.anchoredPosition.x, -100f);
        }



        /// <summary>
        /// Update the fill amount of levelProgressImg.
        /// </summary>
        /// <param name="currentHealth"></param>
        /// <param name="maxHealth"></param>
        public void UpdateLevelProgressImg(int passedPlatformAmount, int totalPlatformAmount)
        {
            levelProgressImg.fillAmount = passedPlatformAmount / (float)totalPlatformAmount;
        }
    }
}
