using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OnefallGames
{
    public class CharacterViewController : MonoBehaviour
    {

        [SerializeField] private RectTransform topBarTrans = null;
        [SerializeField] private RectTransform bottomBarTrans = null;
        //[SerializeField] private Text totalCoinsTxt = null;
        [SerializeField] private Text characterPriceTxt = null;
        [SerializeField] private Button selectBtn = null;
        [SerializeField] private Button unlockBtn = null;

        private CharacterInforController currentCharacterInforController = null;
        public void OnShow()
        {
            ViewManager.Instance.MoveRect(topBarTrans, topBarTrans.anchoredPosition, new Vector2(topBarTrans.anchoredPosition.x, 0), 0.5f);
            ViewManager.Instance.MoveRect(bottomBarTrans, bottomBarTrans.anchoredPosition, new Vector2(bottomBarTrans.anchoredPosition.x, 0), 0.5f);
        }

        private void OnDisable()
        {
            topBarTrans.anchoredPosition = new Vector2(topBarTrans.anchoredPosition.x, 150);
            bottomBarTrans.anchoredPosition = new Vector2(topBarTrans.anchoredPosition.x, -250);
        }

        private void Update()
        {
        }


        public void UpdateUI(CharacterInforController characterInfor)
        {
            currentCharacterInforController = characterInfor;
            if (!characterInfor.IsUnlocked) //The character is not unlocked yet
            {
                selectBtn.gameObject.SetActive(false);
                unlockBtn.gameObject.SetActive(true);
                characterPriceTxt.text = characterInfor.CharacterPrice.ToString();
                
                
                {
                    //Not enough coins -> dont allow user buy this character
                    unlockBtn.interactable = false;
                }
            }
            else//The character is already unlocked
            {
                unlockBtn.gameObject.SetActive(false);
                selectBtn.gameObject.SetActive(true);
            }
        }

        public void UnlockBtn()
        {
            currentCharacterInforController.Unlock();
            UpdateUI(currentCharacterInforController);
        }

        public void SelectBtn()
        {
            BackBtn();
        }

        public void BackBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            ViewManager.Instance.LoadScene("MainMenu", 3f);
        }
    }
}
