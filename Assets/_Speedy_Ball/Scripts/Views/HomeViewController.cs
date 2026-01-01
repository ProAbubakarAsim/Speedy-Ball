using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace OnefallGames
{
    public class HomeViewController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currentLevelTxt , coinsText;
        [SerializeField] private DailyRewardViewController dailyRewardViewController;



        public DailyRewardViewController DailyRewardViewController { get { return dailyRewardViewController; } }


        public void OnShow()
        {
            dailyRewardViewController.gameObject.SetActive(false);
            currentLevelTxt.text = PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_SAVED_LEVEL, 1).ToString();
            coinsText.text = PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_TOTAL_COINS).ToString();
            ViewManager.Instance.loading.SetActive(false);

        }
        public void CoinsAdd()
        {
            if (AdmobMediation.instance)
            {
                AdmobMediation.instance.rewardedAD_Show();
                PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_TOTAL_COINS, PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_TOTAL_COINS) + 100);
                UpdateCoinText();
            }
        }
        public void UpdateCoinText()
        {
            coinsText.text = PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_TOTAL_COINS).ToString();

        }
        public void PlayBtn()
        {
            //if (AdmobMediation.instance)
            //{
            //    AdmobMediation.instance.DEV_ShowInterstitalAD();
            //}
            ViewManager.Instance.PlayClickButtonSound();
            ViewManager.Instance.OpenModeSelection();
        }

        public void OpenBallSelection()
        {
            PlayerPrefs.SetInt("ShowAd", PlayerPrefs.GetInt("ShowAd") + 1);
            if(PlayerPrefs.GetInt("ShowAd") %2 == 0)
            {
                if (AdmobMediation.instance)
                {
                    AdmobMediation.instance.DEV_ShowInterstitalAD();
                }
            }
            ViewManager.Instance.PlayClickButtonSound();
            ViewManager.Instance.OpenBallSelection();
        }

        public void RewardBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            StartCoroutine(CRHandleRewardBtn());
        }
        private IEnumerator CRHandleRewardBtn()
        {
            yield return new WaitForSeconds(0.5f);
            dailyRewardViewController.gameObject.SetActive(true);
            dailyRewardViewController.OnShow();
        }


        public void CharacterBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            ViewManager.Instance.LoadScene("Character", 0.25f);
        }
        public void ShareBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
        }
        private IEnumerator CRHandleUpgradeBtn()
        {
            yield return new WaitForSeconds(0.5f);
            ViewManager.Instance.LoadScene("Upgrade", 0.25f);
        }

        public void RateAppBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
        }

        public void LeaderboardBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
        }
        public void RemoveAdsBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
        }
    }
}
