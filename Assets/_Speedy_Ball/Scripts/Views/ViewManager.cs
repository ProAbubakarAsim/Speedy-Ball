using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace OnefallGames
{
    public class ViewManager : MonoBehaviour
    {
        public static ViewManager Instance { get; private set; }

        [SerializeField] private HomeViewController homeViewController = null;
        [SerializeField] private IngameViewController ingameViewController = null;
        [SerializeField] private CharacterViewController characterViewController = null;
        public GameObject settings, exit, loading, modeSelection, ballSelection;
        public Image fillBar;

        #region Character Selection Variables
        public GameObject[] Balls;
        public Button nextBtn, previousBtn;
        public GameObject buyBtn, selectBtn, lockIcon;
        public TextMeshProUGUI unlockCoins, totalCoins;
        private int selectedBall;
        #endregion


        public HomeViewController HomeViewController { get { return homeViewController; } }
        public IngameViewController IngameViewController { get { return ingameViewController; } }
        public CharacterViewController CharacterViewController { get { return characterViewController; } }
        public void OpenSettings()
        {
            settings.SetActive(true);
            homeViewController.gameObject.SetActive(false);
            PlayClickButtonSound();
        }
        public void OpenBallSelection()
        {
            homeViewController.gameObject.SetActive(false);
            for (int i = 1; i < Balls.Length; i++)
            {
                if (PlayerPrefs.GetString("UnlockedBalls" + i) == "Unlocked")
                {
                    Balls[i].GetComponent<BallLocked>().locked = false;
                }
            }
            Balls[0].SetActive(true);
            selectedBall = 0;
            previousBtn.interactable = false;
            nextBtn.interactable = true;
            selectBtn.SetActive(true);
            buyBtn.SetActive(false);
            ballSelection.SetActive(true);
        }
        public void CloseBallSelection()
        {
            homeViewController.gameObject.SetActive(true);
            for (int i = 0; i < Balls.Length; i++)
            {
                Balls[i].SetActive(false);
            }
            lockIcon.SetActive(false);
            ballSelection.SetActive(false);
        }

        public void SelectBall()
        {
            OpenModeSelection();
            ballSelection.SetActive(false);
            PlayerPrefs.SetInt("BallNumber", selectedBall);
        }

        #region Character Selection
        public void NextCharacter()
        {
            PlayClickButtonSound();
            if (selectedBall < Balls.Length)
            {
                selectedBall++;
                BallLocked();

                previousBtn.interactable = true;
                if (selectedBall >= Balls.Length - 1)
                {
                    nextBtn.interactable = false;
                }
            }
        }
        public void PreviousCharacter()
        {
            PlayClickButtonSound();
            if (selectedBall > 0)
            {
                selectedBall--;
                BallLocked();
                nextBtn.interactable = true;
                if (selectedBall == 0)
                {
                    previousBtn.interactable = false;
                }
            }
        }
        void BallLocked()
        {
            for (int i = 0; i < Balls.Length; i++)
            {
                Balls[i].SetActive(false);
            }
            Balls[selectedBall].SetActive(true);
            if (Balls[selectedBall].GetComponent<BallLocked>().locked == true)
            {
                unlockCoins.text = ((1000 * selectedBall) + " Coins").ToString();
                selectBtn.SetActive(false);
                lockIcon.SetActive(true);
                buyBtn.SetActive(true);
            }
            else
            {
                selectBtn.gameObject.SetActive(true);
                lockIcon.SetActive(false);
                buyBtn.SetActive(false);
            }
            if(HomeManager.Instance)
                HomeManager.Instance.insufficientAmount.SetActive(false);
            PlayerPrefs.SetInt("selectedBall", selectedBall);
        }
        public void BuyCharacter()
        {
            PlayClickButtonSound();
            if (PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_TOTAL_COINS) >= (1000 * selectedBall))
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_TOTAL_COINS, PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_TOTAL_COINS) - (1000 * selectedBall));
                PlayerPrefs.SetString(("UnlockedBalls" + selectedBall), "Unlocked");
                Balls[selectedBall].GetComponent<BallLocked>().locked = false;
                buyBtn.SetActive(false);
                selectBtn.SetActive(true);
                lockIcon.SetActive(false);
                UpdateCoins();
            }
            else
            {
                StartCoroutine(InsufficientAmount());
            }
        }
        IEnumerator InsufficientAmount()
        {
            if(HomeManager.Instance)
            {
                HomeManager.Instance.insufficientAmount.SetActive(true);
                yield return new WaitForSeconds(2f);
                HomeManager.Instance.insufficientAmount.SetActive(false);
            }
        }
        public void AddCoins()
        {
            PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_TOTAL_COINS, PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_TOTAL_COINS) + 500);
            UpdateCoins();
        }
        void UpdateCoins()
        {
            totalCoins.text = PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_TOTAL_COINS).ToString();
        }
        #endregion

        public void CloseSettings()
        {
            settings.SetActive(false);
            PlayClickButtonSound();
            homeViewController.gameObject.SetActive(true);
        }
        public void OpenModeSelection()
        {
            PlayClickButtonSound();
            modeSelection.SetActive(true);
            homeViewController.gameObject.SetActive(false);
        }
        public void SnowMode()
        {
            PlayClickButtonSound();
            LoadScene("Snow", 3f);
        }
        public void CityMode()
        {
            PlayClickButtonSound();
            LoadScene("City", 3f);
        }
        public void OpenExit()
        {
            exit.SetActive(true);
            homeViewController.gameObject.SetActive(false);
            PlayClickButtonSound();
        }
        public void CloseExit()
        {
            exit.SetActive(false);
            PlayClickButtonSound();
            homeViewController.gameObject.SetActive(true);
        }
        public void QuitGame()
        {
            PlayClickButtonSound();
            Application.Quit();
        }

        public ViewType ActiveViewType { private set; get; }

        private void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

            }
        }
        private void Start()
        {
            Sound_Vibrate_Music();
            LoadScene("MainMenu", 3f);
        }
        private void Update()
        {
            if(fillBar.fillAmount < 1f)
            {
                fillBar.fillAmount += 0.005f;
            }
        }
        public void LoadScene(string sceneName, float delay)
        {
            StartCoroutine(CRLoadingScene(sceneName, delay));
        }
        private IEnumerator CRMovingRect(RectTransform rect, Vector2 startPos, Vector2 endPos, float movingTime)
        {
            Vector2 currentPos = new Vector2(Mathf.RoundToInt(rect.anchoredPosition.x), Mathf.RoundToInt(rect.anchoredPosition.y));
            if (!currentPos.Equals(endPos))
            {
                rect.anchoredPosition = startPos;
                float t = 0;
                while (t < movingTime)
                {
                    t += Time.deltaTime;
                    float factor = EasyType.MatchedLerpType(LerpType.EaseInOutQuart, t / movingTime);
                    rect.anchoredPosition = Vector2.Lerp(startPos, endPos, factor);
                    yield return null;
                }
            }
        }
        private IEnumerator CRScalingRect(RectTransform rect, Vector2 startScale, Vector2 endScale, float scalingTime)
        {
            rect.localScale = startScale;
            float t = 0;
            while (t < scalingTime)
            {
                t += Time.deltaTime;
                float factor = EasyType.MatchedLerpType(LerpType.EaseInOutQuart, t / scalingTime);
                rect.localScale = Vector2.Lerp(startScale, endScale, factor);
                yield return null;
            }
        }
        private IEnumerator CRLoadingScene(string sceneName, float delay)
        {
            loading.SetActive(true);
            ingameViewController.gameObject.SetActive(false);
            modeSelection.SetActive(false);
            fillBar.fillAmount = 0f;
            yield return new WaitForSeconds(delay - 0.5f);
            SceneManager.LoadScene(sceneName);
            yield return new WaitForSeconds(0.5f);
            if(sceneName == "MainMenu")
            {
                homeViewController.gameObject.SetActive(true);
                OnShowView(ViewType.HOME_VIEW);
                GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
                GetComponent<Canvas>().worldCamera = Camera.main;
            }
            else
            {
                ingameViewController.gameObject.SetActive(true);
                OnShowView(ViewType.INGAME_VIEW);
                GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            }
        }

        public void PlayClickButtonSound()
        {
            //ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.button);
        }


        #region Button Sound_Music_Vibrate Variables
        private SoundsManager sounds;
        public Sprite on, off;
        public Image vibrate, sound, music;
        private string vibratePref = "Vibrate", soundPref = "Sound", musicPref = "Music";
        private bool isVibrate, isSound, isMusic;

        #region Sound & Music & Vibrate Functions
        public void Sound_Vibrate_Music()
        {
            if (PlayerPrefs.GetInt(vibratePref) == 1)
            {
                vibrate.sprite = off;
                isVibrate = false;
            }
            else
            {
                vibrate.sprite = on;
                isVibrate = true;
            }

            if (PlayerPrefs.GetInt(soundPref) == 1)
            {
                sound.sprite = off;
                isSound = false;
            }
            else
            {
                sound.sprite = on;
                isSound = true;
            }

            if (PlayerPrefs.GetInt(musicPref) == 1)
            {
                music.sprite = off;
                isMusic = false;
            }
            else
            {
                music.sprite = on;
                isMusic = true;
            }
        }
        public void Vibrate()
        {
            PlayClickButtonSound();
            if (isVibrate == true)
            {
                isVibrate = false;
                PlayerPrefs.SetInt(vibratePref, 1);
                vibrate.sprite = off;
            }
            else
            {
                isVibrate = true;
                PlayerPrefs.SetInt(vibratePref, 0);
                vibrate.sprite = on;
            }
        }
        public void Sound()
        {
            PlayClickButtonSound();
            if (isSound == true)
            {
                AudioListener.volume = 0f;
                PlayerPrefs.SetInt(soundPref, 1);
                isSound = false;
                sound.sprite = off;
            }
            else
            {
                AudioListener.volume = 1f;
                PlayerPrefs.SetInt(soundPref, 0);
                isSound = true;
                sound.sprite = on;
            }
        }
       
        public void Music()
        {
            PlayClickButtonSound();
            if (isMusic == true)
            {
                PlayerPrefs.SetInt(musicPref, 1);
                isMusic = false;
                music.sprite = off;
            }
            else
            {
                PlayerPrefs.SetInt(musicPref, 0);
                isMusic = true;
                music.sprite = on;
            }
            FindObjectOfType<HomeManager>().Sound();
        }
        #endregion

        #endregion

        public void MoveRect(RectTransform rect, Vector2 startPos, Vector2 endPos, float movingTime)
        {
            StartCoroutine(CRMovingRect(rect, startPos, endPos, movingTime));
        }

        public void ScaleRect(RectTransform rect, Vector2 startScale, Vector2 endScale, float scalingTime)
        {
            StartCoroutine(CRScalingRect(rect, startScale, endScale, scalingTime));
        }

        public void OnShowView(ViewType viewType)
        {
            if (viewType == ViewType.HOME_VIEW)
            {
                homeViewController.gameObject.SetActive(true);
                homeViewController.OnShow();
                ActiveViewType = ViewType.HOME_VIEW;

                ////Hide all other views
                ingameViewController.gameObject.SetActive(false);
                characterViewController.gameObject.SetActive(false);
                loading.SetActive(false);
            }
            else if (viewType == ViewType.INGAME_VIEW)
            {
                ingameViewController.gameObject.SetActive(true);
                ingameViewController.OnShow();
                ActiveViewType = ViewType.INGAME_VIEW;

                ////Hide all other views
                homeViewController.gameObject.SetActive(false);
                characterViewController.gameObject.SetActive(false);
                loading.gameObject.SetActive(false);
            }
            else if (viewType == ViewType.CHARACTER_VIEW)
            {
                characterViewController.gameObject.SetActive(true);
                characterViewController.OnShow();
                ActiveViewType = ViewType.CHARACTER_VIEW;

                ////Hide all other views
                ingameViewController.gameObject.SetActive(false);
                homeViewController.gameObject.SetActive(false);
                loading.gameObject.SetActive(false);
            }
            else if (viewType == ViewType.LOADING_VIEW)
            {
                loading.SetActive(true);
                fillBar.fillAmount = 0f;
                ActiveViewType = ViewType.LOADING_VIEW;

                ////Hide all other views
                homeViewController.gameObject.SetActive(false);
                ingameViewController.gameObject.SetActive(false);
                characterViewController.gameObject.SetActive(false);
            }
        }
    }
}
