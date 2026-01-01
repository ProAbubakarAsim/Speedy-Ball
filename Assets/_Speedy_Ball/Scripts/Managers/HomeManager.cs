using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OnefallGames
{
    public class HomeManager : MonoBehaviour
    {
        public static HomeManager Instance { private set; get; }

        public GameObject insufficientAmount;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(Instance.gameObject);
                Instance = this;
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void Start()
        {
            Sound();
            Application.targetFrameRate = 60;
            //ViewManager.Instance.OnShowView(ViewType.HOME_VIEW);

            //Update character
            //CharacterInforController charInfor = ServicesManager.Instance.CharacterContainer.CharacterInforControllers[ServicesManager.Instance.CharacterContainer.SelectedCharacterIndex];
            //characterMeshFilter.sharedMesh = charInfor.MeshFilter.sharedMesh;
            //characterMeshRenderer.sharedMaterial = charInfor.MeshRenderer.sharedMaterial;

            //Report level to leaderboard
            string username = PlayerPrefs.GetString(PlayerPrefsKeys.PPK_SAVED_USER_NAME);
            if (!string.IsNullOrEmpty(username))
            {
                //ServicesManager.Instance.LeaderboardManager.SetPlayerLeaderboardData();
            }
            //ServicesManager.Instance.ShareManager.CreateScreenshot();
        }
        public void Sound()
        {
            if (PlayerPrefs.GetInt("Music") == 1)
            {
                GetComponent<AudioSource>().volume = 0f;
            }
            else
            {
                GetComponent<AudioSource>().volume = 1f;
            }
        }
    }
}