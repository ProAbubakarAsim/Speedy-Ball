using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OnefallGames
{
    public class CollectedCoinsViewController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup bottomButtonsCanvasGroup = null;
        [SerializeField] private RectTransform sunbrustImageTrans = null;
        [SerializeField] private RectTransform doubleCoinsBtnTrans = null;
        //[SerializeField] private Text collectedCoinsTxt = null;

        public void OnShow()
        {
            bottomButtonsCanvasGroup.alpha = 0f;
            StartCoroutine(CRFadingInBottonButtons());
            
            
        }

        private void Update()
        {
            sunbrustImageTrans.localEulerAngles += Vector3.forward * 100f * Time.deltaTime;
        }

        private void OnDisable()
        {
            sunbrustImageTrans.localEulerAngles = Vector3.zero;
            doubleCoinsBtnTrans.gameObject.SetActive(true);
            bottomButtonsCanvasGroup.alpha = 0f;
        }


        private IEnumerator CRFadingInBottonButtons()
        {
            yield return new WaitForSeconds(0.5f);
            float fadingTime = 1f;
            float t = 0;
            while (t < fadingTime)
            {
                t += Time.deltaTime;
                float factor = t / fadingTime;
                bottomButtonsCanvasGroup.alpha = Mathf.Lerp(0f, 1f, factor);
                yield return null;
            }
        }



        public void DoubleCoinsBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            doubleCoinsBtnTrans.gameObject.SetActive(false);
        }


        public void ClaimBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            ViewManager.Instance.IngameViewController.EndGameViewController.OnCollectedCoinsViewClose();
            gameObject.SetActive(false);
        }
    }
}
