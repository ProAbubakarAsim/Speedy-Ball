using System.Collections;
using UnityEngine;


namespace OnefallGames
{
    public class ItemController : MonoBehaviour
    {

        [Header("Item Configuration")]
        [SerializeField] private ItemType itemType = ItemType.COIN;
        [SerializeField] private float minRotatingSpeed = 150f;
        [SerializeField] private float maxRotatingSpeed = 350f;

        [Header("Item References")]
        [SerializeField] private MeshRenderer meshRenderer = null;
        [SerializeField] private LayerMask playerLayerMask = new LayerMask();

        public ItemType ItemType { get { return itemType; } }
        public MeshRenderer MeshRenderer { get { return meshRenderer; } }

        private Vector3 originalPos = Vector3.zero;
        private float rotatingSpeed = 0;

        private void OnEnable()
        {
            originalPos = transform.position;
            if (itemType == ItemType.COIN)
            {
                StartCoroutine(CRMovingToPlayer());
            }
        }

        public void Start()
        {
            rotatingSpeed = Random.Range(minRotatingSpeed, maxRotatingSpeed);
        }

        private void Update()
        {
            if (itemType == ItemType.COIN || itemType == ItemType.MAGNET)
            {
                transform.localEulerAngles += Vector3.up * rotatingSpeed * Time.deltaTime;
            }

            //Check collide with player
            Collider[] colliders = Physics.OverlapBox(meshRenderer.bounds.center, meshRenderer.bounds.extents, transform.rotation, playerLayerMask);
            if (colliders.Length > 0)
            {
                if (itemType == ItemType.COIN)
                {
                    //ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.collectCoin);
                    //ServicesManager.Instance.CoinManager.AddCollectedCoins(1);
                    EffectManager.Instance.CreateCollectCoinEffect(meshRenderer.bounds.center);
                    gameObject.SetActive(false);
                }
                else if (itemType == ItemType.MAGNET)
                {
                    //ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.collectMagnet);
                    EffectManager.Instance.CreateCollectMagnetEffect(transform.position + Vector3.up * 0.1f);
                    PlayerController.Instance.ActiveMagnetMode();
                    gameObject.SetActive(false);
                }
                else if (itemType == ItemType.LASER)
                {
                    //ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.collectLaser);
                    EffectManager.Instance.CreateCollectLaserEffect(transform.position + Vector3.up * 0.1f);
                    PlayerController.Instance.ActiveLaserMode();
                    gameObject.SetActive(false);
                }
                else if(itemType == ItemType.SpeedBost)
                {
                    PlayerController.Instance.ActiveBoosterMode();
                    gameObject.SetActive(false);
                }
                else if(itemType == ItemType.AdHurdle)
                {
                    //if (AdmobMediation.instance)
                    //{
                    //    AdmobMediation.instance.rewardedAD_Show();
                    //}
                    PlayerController.Instance.ActiveAD();
                    gameObject.SetActive(false);
                }
            }

            //Check and disable object
            if (transform.position.z <= PlayerController.Instance.transform.position.z - 10f && IngameManager.Instance.IngameState == IngameState.Playing)
            {
                gameObject.SetActive(false);
            }
        }




        /// <summary>
        /// Moving this coin to player when magnet mode of the player is active.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRMovingToPlayer()
        {
            yield return null;
            while (true)
            {
                if (PlayerController.Instance.IsActiveMagnetMode)
                {
                    float zDistance = transform.position.z - PlayerController.Instance.transform.position.z;
                    if (zDistance > 0 && zDistance <= 7)
                    {
                        break;
                    }
                }
                yield return null;
            }

            while (gameObject.activeInHierarchy)
            {
                if (IngameManager.Instance.IngameState != IngameState.Playing)
                {
                    transform.position = originalPos;
                    yield break;
                }
                Vector3 direction = (PlayerController.Instance.transform.position - transform.position).normalized;
                transform.position += direction * 50f * Time.deltaTime;
                yield return null;
            }
        }

    }
}
