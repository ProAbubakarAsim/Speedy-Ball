using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace OnefallGames
{
    public class PlayerController : MonoBehaviour
    {

        public static PlayerController Instance { private set; get; }
        public static event System.Action<PlayerState> PlayerStateChanged = delegate { };
        public MeshRenderer mesh;
        //public Material[] newBalls;

        public PlayerState PlayerState
        {
            get
            {
                return playerState;
            }

            private set
            {
                if (value != playerState)
                {
                    value = playerState;
                    PlayerStateChanged(playerState);
                }
            }
        }


        [Header("Player Configuration")]
        [SerializeField] private AudioSource PlayerFallDownSound;
        [SerializeField] private AudioSource BoostSFX;

        [SerializeField] private float jumpThreshold = 0.1f;
        [SerializeField] private float minJumpVelocity = 80f;
        [SerializeField] private float maxJumpVelocity = 140f;
        [SerializeField] private float fallingSpeed = -50f;
        [SerializeField] private float switchingLaneSpeed = 15f;
        [SerializeField] private float BallSpeed = 1.5f;



        [Header("Player References")]
        //[SerializeField] private MeshFilter meshFilter = null;
        [SerializeField] private MeshRenderer meshRender = null;
        [SerializeField] private GameObject trailObject = null;
        [SerializeField] private GameObject magnetObject = null;
        [SerializeField] private GameObject laserObject = null;
        [SerializeField] private GameObject Boosterobject = null;

        [SerializeField] private AudioSource laserSound;
        [SerializeField] private AudioSource MagnetSound;
        [SerializeField] private LayerMask platformLayerMask = new LayerMask();
        [SerializeField] private LayerMask obstacleLayerMask = new LayerMask();
        [SerializeField] private LayerMask finishLineLayerMask = new LayerMask();


        public bool IsActiveMagnetMode { private set; get; }


        private PlayerState playerState = PlayerState.Died;
        private GameObject detectedPlatform = null;
        private Vector2 mouseDownPos = Vector2.zero;
        private Vector3 playerAngles = Vector3.zero;
        private float movingSpeed = 0f;
        private float currentJumpVelocity = 0;
        private bool isActiveLaserMode = false;
        private bool isActiveBoosterMode = false;


        private bool isStopControl = false;
        private bool isSwitchingLane = false;
        private bool isFalling = false;
        GameObject Player;



        private void OnEnable()
        {
            IngameManager.IngameStateChanged += IngameManager_IngameStateChanged;
        }
        private void OnDisable()
        {
            IngameManager.IngameStateChanged -= IngameManager_IngameStateChanged;
        }
        private void IngameManager_IngameStateChanged(IngameState obj)
        {
            if (obj == IngameState.Playing)
            {
                PlayerLiving();
            }
            else if (obj == IngameState.CompletedLevel)
            {
                PlayerCompleteLevel();
            }
        }
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
            PlayerState = PlayerState.Prepare;
            playerState = PlayerState.Prepare;
            //mesh.material = newBalls[Random.Range(0, newBalls.Length)];
            //print(mesh.material.name);
            //Update character
            //CharacterInforController charInfor = ServicesManager.Instance.CharacterContainer.CharacterInforControllers[ServicesManager.Instance.CharacterContainer.SelectedCharacterIndex];
            //meshFilter.sharedMesh = charInfor.MeshFilter.sharedMesh;
            //meshRender.sharedMaterial = charInfor.MeshRenderer.sharedMaterial;

            //Add another actions here
            IsActiveMagnetMode = false;
            trailObject.SetActive(false);
            magnetObject.SetActive(false);
            laserObject.SetActive(false);
            mouseDownPos = Vector2.zero;
            playerAngles = Vector3.zero;
            currentJumpVelocity = 0;
            isStopControl = true;
            isSwitchingLane = false;
            isFalling = false;
            Player = IngameManager.Instance.Balls[PlayerPrefs.GetInt("BallNumber")];
            meshRender = Player.GetComponent<MeshRenderer>();
        }
        private void Update()
        {
            if (playerState == PlayerState.Living && !isStopControl)
            {

                if (Input.GetMouseButtonDown(0))
                {
                    mouseDownPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                }

                if (Input.GetMouseButtonUp(0))
                {
                    Vector2 mouseUpPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                    float swipeDistance = mouseUpPos.y - mouseDownPos.y;

                    if (swipeDistance >= jumpThreshold)
                    {
                        if (!isFalling)
                        {
                            //ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.playerJump);
                            isFalling = true;
                            float yRange = mouseUpPos.y - mouseDownPos.y;
                            currentJumpVelocity = yRange * minJumpVelocity;
                            currentJumpVelocity = (currentJumpVelocity > maxJumpVelocity) ? maxJumpVelocity : currentJumpVelocity;
                        }
                    }


                    else
                    {
                        if ((float)System.Math.Round(swipeDistance, 2) < 0.1f)
                        {
                            if (Camera.main.ScreenToViewportPoint(Input.mousePosition).x <= 0.5f)
                            {
                                //Switch to left lane
                                float currenX = (float)System.Math.Round(transform.position.x, 1);
                                if (currenX > -2f && !isSwitchingLane)
                                {
                                    isSwitchingLane = true;
                                    StartCoroutine(CRSwitchingLane(-1));
                                }
                            }
                            else
                            {
                                //Switch to right lane
                                float currenX = (float)System.Math.Round(transform.position.x, 1);
                                if (currenX < 2f && !isSwitchingLane)
                                {
                                    isSwitchingLane = true;
                                    StartCoroutine(CRSwitchingLane(1));
                                }
                            }
                        }
                    }
                }


                //Move on y and z axis 
                if (currentJumpVelocity <= fallingSpeed)
                {
                    currentJumpVelocity = fallingSpeed;

                }
                else
                {
                    currentJumpVelocity = currentJumpVelocity + fallingSpeed * Time.deltaTime;
                }
                float newY = (transform.position + Vector3.up * (currentJumpVelocity * Time.deltaTime + fallingSpeed * Time.deltaTime * Time.deltaTime / 2)).y;
                if (newY < 0)
                {
                    if (isFalling)
                    {
                        //ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.playerLanded);
                        isFalling = false;
                    }
                    newY = 0;
                }
                Vector3 currentPlayerPos = transform.position;
                currentPlayerPos.y = newY;
                currentPlayerPos.z += movingSpeed * Time.deltaTime * BallSpeed;
                transform.position = currentPlayerPos;

                //Rotate the model
                playerAngles += new Vector3(movingSpeed * 40f * Time.deltaTime, 0, 0);
                //Player.transform.eulerAngles += new Vector3(100 * Time.deltaTime, 0, 0);
                meshRender.transform.localEulerAngles = playerAngles;


                //Check collide with a platform bellow
                Vector3 centerPoint = transform.position + Vector3.up;
                Vector3 forwardPoint = centerPoint + Vector3.forward * meshRender.bounds.extents.z;
                Vector3 backPoint = centerPoint + Vector3.back * meshRender.bounds.extents.z;
                RaycastHit raycastHit;
                bool hitPlatform = Physics.Raycast(centerPoint, Vector3.down, out raycastHit, 1000f, platformLayerMask);
                if (!hitPlatform)
                    hitPlatform = Physics.Raycast(forwardPoint, Vector3.down, out raycastHit, 1000f, platformLayerMask);
                if (!hitPlatform)
                    hitPlatform = Physics.Raycast(backPoint, Vector3.down, out raycastHit, 1000f, platformLayerMask);

                if (!hitPlatform && !isFalling) //Player reached the ground and stil not hit any platform -> fall down
                {
                    //ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.playerFalled);
                    PlayerDied();
                    IngameManager.Instance.HandlePlayerDied();
                    if (gameObject.activeInHierarchy)
                    {
                        StartCoroutine(CRFallingDown());

                    }
                }
                else if (hitPlatform)
                {
                    detectedPlatform = (detectedPlatform != raycastHit.collider.gameObject) ? raycastHit.collider.gameObject : detectedPlatform;
                }


                //Check collide with obstacle
                Collider[] obstacleColliders = Physics.OverlapSphere(meshRender.bounds.center, meshRender.bounds.extents.x, obstacleLayerMask);
                if (obstacleColliders.Length > 0)
                {
                    //ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.playerExploded);
                    EffectManager.Instance.CreatePlayerExplodeEffect(transform.position);
                    PlayerDied();
                    IngameManager.Instance.HandlePlayerDied();
                    meshRender.enabled = false;
                    this.
                    trailObject.SetActive(false);
                }


                //Check collide with finish line
                Collider[] finishLineColliders = Physics.OverlapSphere(meshRender.bounds.center, meshRender.bounds.extents.x, finishLineLayerMask);
                if (finishLineColliders.Length > 0)
                {
                    IngameManager.Instance.CountPassedPlatform();
                    IngameManager.Instance.CompleteLevel();
                }
            }
        }
        private void PlayerLiving()
        {
            //Fire event
            PlayerState = PlayerState.Living;
            playerState = PlayerState.Living;

            //Add another actions here
            if (IngameManager.Instance.IsRevived)
            {
                StartCoroutine(CRHandlingActionsAfterRevived());
            }
            else
            {
                trailObject.SetActive(true);
                isStopControl = false;
            }
        }
        private void PlayerDied()
        {
            //Fire event
            PlayerState = PlayerState.Died;
            playerState = PlayerState.Died;
            this.gameObject.SetActive(false);
            //if (AdmobMediation.instance)
            //{
            //    AdmobMediation.instance.rewardedAD_Show();
            //}

            //Add another actions here
            //ServicesManager.Instance.ShareManager.CreateScreenshot();
            isStopControl = true;
        }
        private void PlayerCompleteLevel()
        {
            //if (AdmobMediation.instance)
            //{
            //    AdmobMediation.instance.DEV_ShowInterstitalAD();
            //}
            //Fire event
            PlayerState = PlayerState.CompletedLevel;
            playerState = PlayerState.CompletedLevel;
            //Add another actions here
            //ServicesManager.Instance.ShareManager.CreateScreenshot();
            StartCoroutine(CRMovingFoward());
        }
        private IEnumerator CRHandlingActionsAfterRevived()
        {
            //Reset parameters
            trailObject.SetActive(true);
            mouseDownPos = Vector2.zero;
            playerAngles = Vector3.zero;
            currentJumpVelocity = 0;
            isSwitchingLane = false;
            isFalling = false;
            detectedPlatform.GetComponent<PlatformController>().DisableAllObstacles();

            //Reset position and angles
            transform.position = new Vector3(0f, 0f, detectedPlatform.transform.position.z);
            meshRender.transform.localEulerAngles = Vector3.zero;
            meshRender.enabled = true;
            yield return new WaitForSeconds(0.75f);
            isStopControl = false;

            //Increase movingSpeed
            float movingSpeedTemp = movingSpeed;
            movingSpeed = 0;
            float t = 0;
            float lerpTime = 1f;
            while (t < lerpTime)
            {
                t += Time.deltaTime;
                float factor = t / lerpTime;
                movingSpeed = Mathf.Lerp(0f, movingSpeedTemp, factor);
                yield return null;
            }
        }
        private IEnumerator CRSwitchingLane(int turn)
        {
            float t = 0;
            float startX = transform.position.x;
            float endX = (turn == -1) ? startX - 2f : startX + 2f;
            float switchingTime = 2f / switchingLaneSpeed;
            while (t < switchingTime)
            {
                t += Time.deltaTime;
                float factor = t / switchingTime;
                Vector3 pos = transform.position;
                pos.x = Mathf.Lerp(startX, endX, factor);
                transform.position = pos;
                yield return null;
            }

            isSwitchingLane = false;
        }
        private IEnumerator CRMovingFoward()
        {
            while (true)
            {
                if (currentJumpVelocity <= fallingSpeed)
                    currentJumpVelocity = fallingSpeed;
                else
                    currentJumpVelocity = currentJumpVelocity + fallingSpeed * Time.deltaTime;
                float newY = (transform.position + Vector3.up * (currentJumpVelocity * Time.deltaTime + fallingSpeed * Time.deltaTime * Time.deltaTime / 2)).y;
                newY = (newY < 0) ? 0 : newY;
                Vector3 currentPlayerPos = transform.position;
                currentPlayerPos.y = newY;
                currentPlayerPos.z += movingSpeed * Time.deltaTime;
                transform.position = currentPlayerPos;

                //Rotate the model
                playerAngles += new Vector3(movingSpeed * 20f * Time.deltaTime, 0, 0);
                meshRender.transform.localEulerAngles = playerAngles;

                yield return null;
            }
        }
        private IEnumerator CRFallingDown()
        {

            float fallingTime = 1f;
            while (fallingTime > 0)
            {
                if (currentJumpVelocity <= fallingSpeed)
                    currentJumpVelocity = fallingSpeed;
                else
                    currentJumpVelocity = currentJumpVelocity + fallingSpeed * Time.deltaTime;
                float newY = (transform.position + Vector3.up * (currentJumpVelocity * Time.deltaTime + fallingSpeed * Time.deltaTime * Time.deltaTime / 2)).y;
                Vector3 currentPlayerPos = transform.position;
                currentPlayerPos.y = newY;
                transform.position = currentPlayerPos;
                yield return null;
                fallingTime -= Time.deltaTime;
            }
            meshRender.enabled = false;
            trailObject.SetActive(false);
        }
        private IEnumerator CRCoutingMagnetMode()
        {
            float magnetModeTime = IngameManager.Instance.GetMagnetModeTime();
            while (magnetModeTime > 0)
            {
                yield return null;
                magnetModeTime -= Time.deltaTime;
                if (playerState != PlayerState.Living)
                {
                    IsActiveMagnetMode = false;
                    magnetObject.SetActive(false);
                    yield break;
                }
            }

            magnetObject.SetActive(false);
            IsActiveMagnetMode = false;
        }
        private IEnumerator CRCoutingLaserMode()
        {
            float laserModeTime = IngameManager.Instance.GetLaserModeTime();
            while (laserModeTime > 0)
            {
                yield return null;
                laserModeTime -= Time.deltaTime;
                if (playerState != PlayerState.Living)
                {
                    isActiveLaserMode = false;
                    laserObject.SetActive(false);
                    yield break;
                }

                //Draw raycast to detect the obstacle
                Vector3 centerPoint = transform.position + Vector3.up * meshRender.bounds.extents.y;
                RaycastHit raycastHit;
                bool hitObstacle = Physics.Raycast(centerPoint, Vector3.forward, out raycastHit, 20f, obstacleLayerMask);
                if (hitObstacle)
                {
                    raycastHit.transform.root.GetComponent<ObstacleController>().OnExplode();
                }
            }

            laserObject.SetActive(false);
            isActiveLaserMode = false;
        }

        private IEnumerator CRCoutingBoosterMode()
        {
            float BoosterModeTime = IngameManager.Instance.GetBoosterTime();
            while (BoosterModeTime > 0)
            {
                yield return null;
                BoosterModeTime -= Time.deltaTime;
                if (playerState != PlayerState.Living)
                {
                    isActiveBoosterMode = false;
                    Boosterobject.SetActive(false);
                    yield break;
                }

                //Draw raycast to detect the obstacle
                //Vector3 centerPoint = transform.position + Vector3.up * meshRender.bounds.extents.y;
                //RaycastHit raycastHit;
                //bool hitObstacle = Physics.Raycast(centerPoint, Vector3.forward, out raycastHit, 20f, obstacleLayerMask);
                //if (hitObstacle)
                //{
                //    raycastHit.transform.root.GetComponent<ObstacleController>().OnExplode();
                //}
            }

            Boosterobject.SetActive(false);


            BallSpeed = BallSpeed / 2;

            isActiveBoosterMode = false;
        }

        private IEnumerator CRCoutingAdMode()
        {
            float AdModeTime = IngameManager.Instance.GetAdTime();
            while (AdModeTime > 0)
            {
                yield return null;
                AdModeTime -= Time.deltaTime;
                if (playerState != PlayerState.Living)
                {
                    isActiveBoosterMode = false;
                    Boosterobject.SetActive(false);
                    //BallSpeed = BallSpeed / 2;
                    Debug.Log("Coroutine Stops");
                    yield break;
                }

                //Draw raycast to detect the obstacle
                //Vector3 centerPoint = transform.position + Vector3.up * meshRender.bounds.extents.y;
                //RaycastHit raycastHit;
                //bool hitObstacle = Physics.Raycast(centerPoint, Vector3.forward, out raycastHit, 20f, obstacleLayerMask);
                //if (hitObstacle)
                //{
                //    raycastHit.transform.root.GetComponent<ObstacleController>().OnExplode();
                //}
            }

            Boosterobject.SetActive(false);


            //BallSpeed = BallSpeed / 2;

            isActiveLaserMode = false;
        }
        public void SetMovingSpeed(float speed)
        {
            movingSpeed = speed;
        }
        public void ActiveMagnetMode()
        {
            if (!IsActiveMagnetMode)
            {
                IsActiveMagnetMode = true;
                magnetObject.SetActive(true);
                MagnetSound.Play();
                StartCoroutine(CRCoutingMagnetMode());
            }
        }
        public void ActiveLaserMode()
        {
            if (!isActiveLaserMode)
            {
                isActiveLaserMode = true;
                laserObject.SetActive(true);
                laserSound.Play();
                StartCoroutine(CRCoutingLaserMode());
            }
        }

        public void ActiveBoosterMode()
        {
            if (!isActiveBoosterMode)
            {
                BoostSFX.Play();
                isActiveBoosterMode = true;
                Boosterobject.SetActive(true);
                BallSpeed = BallSpeed * 2;
                StartCoroutine(CRCoutingBoosterMode());

            }
        }

        public void ActiveAD()
        {

            Debug.Log("AD SHow Here");
            if (AdmobMediation.instance)
                AdmobMediation.instance.rewardedAD_Show();


        }
    }
}
