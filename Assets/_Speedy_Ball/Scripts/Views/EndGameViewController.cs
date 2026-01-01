using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace OnefallGames
{
    public class EndGameViewController : MonoBehaviour
    {
        [SerializeField] private RectTransform replayBtnTrans = null;
        [SerializeField] private Text[] currentLevelTxt = null;
        [SerializeField] private GameObject completeBox, failBox;

        [SerializeField] private CollectedCoinsViewController collectedCoinsViewController = null;

        public void OnShow()
        {
            ViewManager.Instance.ScaleRect(replayBtnTrans, Vector2.zero, Vector2.one, 0.75f);

            for (int i = 0; i < currentLevelTxt.Length; i++)
            {
                currentLevelTxt[i].text = "Level: " + PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_SAVED_LEVEL).ToString();
            }
           
            {
                collectedCoinsViewController.gameObject.SetActive(false);
                if (PlayerController.Instance.PlayerState == PlayerState.CompletedLevel)
                {
                    completeBox.SetActive(true);
                    failBox.SetActive(false);
                    PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_TOTAL_COINS, PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_TOTAL_COINS) + 500);
                }
                else
                {
                    completeBox.SetActive(false);
                    failBox.SetActive(true);
                }
            }
        }
        
        private void OnDisable()
        {
            replayBtnTrans.localScale = Vector2.zero;
        }

        /// <summary>
        /// Wait for an amount of time then show the collectedCoinsViewController.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRWaitAndShowCollectedCoinsView()
        {
            yield return new WaitForSeconds(0.75f);

            //Show CollectedCoinsView
            collectedCoinsViewController.gameObject.SetActive(true);
            collectedCoinsViewController.OnShow();
        }



        /// <summary>
        /// handle actions when collected coins view closed.
        /// </summary>
        public void OnCollectedCoinsViewClose()
        {
            if (IngameManager.Instance.IngameState == IngameState.CompletedLevel)
            {
                completeBox.SetActive(true);
            }
            else
            {
                failBox.SetActive(true);
            }
        }
        public void ReplayBtn()
        {
            PlayerPrefs.SetInt("ReplayAd", PlayerPrefs.GetInt("ReplayAd") + 1);
            if(PlayerPrefs.GetInt("ReplayAd") % 2 == 0)
            {
                if (AdmobMediation.instance)
                {
                    AdmobMediation.instance.DEV_ShowInterstitalAD();
                }
            }
            ViewManager.Instance.PlayClickButtonSound();
            SoundsManager.instance.ClickButton();
            ViewManager.Instance.LoadScene(SceneManager.GetActiveScene().name, 0.25f);
            if (PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_SAVED_LEVEL) == 11)
            {
                PlayerPrefs.DeleteAll();
                ViewManager.Instance.LoadScene("MainMenu", 3f);
            }
        }
        public void ShareBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
        }
        public void CharacterBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            ViewManager.Instance.LoadScene("Character", 0.25f);
        }
        public void HomeBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            ViewManager.Instance.LoadScene("MainMenu", 3f);
        }
    }
}
