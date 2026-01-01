using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace OnefallGames
{
    public class SceneLoader : MonoBehaviour
    {
        private static string targetScene = string.Empty;

        private void Start()
        {
            ViewManager.Instance.OnShowView(ViewType.LOADING_VIEW);
            StartCoroutine(LoadingScene());
        }

        private IEnumerator LoadingScene()
        {
            AsyncOperation asyn = SceneManager.LoadSceneAsync(targetScene);
            while (!asyn.isDone)
            {
                yield return null;
            }
        }

        /// <summary>
        /// Set target scene.
        /// </summary>
        /// <param name="sceneName"></param>
        public static void SetTargetScene(string sceneName)
        {
            targetScene = sceneName;
        }
    }
}
