using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

namespace Unity.LEGO.Game
{
    // The Root component for the game.
    // It sets the game state and broadcasts events to notify the different systems of a game state change.

    public class GameFlowManager : MonoBehaviour
    {
       

        [Header("Win")]
        [SerializeField, Tooltip("The name of the scene you want to load when the game is won.")]
        string m_WinScene = "Menu Win";
        [SerializeField, Tooltip("The delay in seconds between the game is won and the win scene is loaded.")]
        float m_WinSceneDelay = 5.0f;
        [Tooltip("Sound played on win")]
        public AudioClip victorySound;
        [Tooltip("Prefab for the win game message")]
        public GameObject WinGameMessagePrefab;


        [Tooltip("Duration of delay before the fade-to-black, if winning")]
        public float delayBeforeFadeToBlack = 4f;
        [Tooltip("Duration of delay before the win message")]
        public float delayBeforeWinMessage = 2f;

        [Header("Lose")]
        [SerializeField, Tooltip("The name of the scene you want to load when the game is lost.")]
        string m_LoseScene = "Menu Lose";
        [SerializeField, Tooltip("The delay in seconds between the game is lost and the lose scene is loaded.")]
        float m_LoseSceneDelay = 3.0f;
        [Tooltip("Sound played on lose")]
        public AudioClip loseSound;

        [Header("Parameters")]
        [Tooltip("Duration of the fade-to-black at the end of the game")]
        public float endSceneLoadDelay = 3f;
        [Tooltip("The canvas group of the fade-to-black screen")]
        public CanvasGroup endGameFadeCanvasGroup;
        
        [Tooltip("Delay before displaying the message")]
        public float delayBeforeShowing = 2f;


        [SerializeField, HideInInspector, Tooltip("The delay in seconds until we activate the controller look inputs.")]
        float m_StartGameLockedControllerTimer = 0.3f;

        public static string PreviousScene { get; private set; }

        public bool GameIsEnding { get; private set; }
        
        float m_GameOverSceneTime;

       [SerializeField, HideInInspector, Tooltip("Next Level")]
       string m_GameOverSceneToLoad;

        CinemachineFreeLook m_FreeLookCamera;

        string m_ControllerAxisXName;
        string m_ControllerAxisYName;

        float m_TimeLoadEndGameScene;
        string m_SceneToLoad;


        void Awake()
        {
            EventManager.AddListener<GameOverEvent>(OnGameOver);

            m_FreeLookCamera = FindObjectOfType<CinemachineFreeLook>();
#if !UNITY_EDITOR
            Cursor.lockState = CursorLockMode.Locked;
#endif

            // Enable camera depth texture to ensure fog works even without shadows.
            Camera.main.depthTextureMode = DepthTextureMode.Depth;

            // Backup and lock look rotation
            if (m_FreeLookCamera)
            {
                m_ControllerAxisXName = m_FreeLookCamera.m_XAxis.m_InputAxisName;
                m_ControllerAxisYName = m_FreeLookCamera.m_YAxis.m_InputAxisName;
                m_FreeLookCamera.m_XAxis.m_InputAxisName = "";
                m_FreeLookCamera.m_YAxis.m_InputAxisName = "";
            }
        }

        void Start()
        {
            StartCoroutine(StartGameLockLookRotation());

            //AudioUtility.SetMasterVolume(1);
        }

        IEnumerator StartGameLockLookRotation()
        {
            while (m_StartGameLockedControllerTimer > 0.0f)
            {
                m_StartGameLockedControllerTimer -= Time.deltaTime;
                if (m_StartGameLockedControllerTimer < 0.0f)
                {
                    if (m_FreeLookCamera)
                    {
                        m_FreeLookCamera.m_XAxis.m_InputAxisName = m_ControllerAxisXName;
                        m_FreeLookCamera.m_YAxis.m_InputAxisName = m_ControllerAxisYName;
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }

        void Update()
        {
            float timeRatio = 1 - (m_TimeLoadEndGameScene - Time.time) / endSceneLoadDelay;
            endGameFadeCanvasGroup.alpha = timeRatio;
            //AudioUtility.SetMasterVolume(1 - timeRatio);

            if (GameIsEnding)
            {
                if (Time.time >= m_GameOverSceneTime)
                {
#if !UNITY_EDITOR
            Cursor.lockState = CursorLockMode.None;
#endif
                    PreviousScene = SceneManager.GetActiveScene().name;
                    SceneManager.LoadScene(m_GameOverSceneToLoad);
                }
            }
        }

        void OnGameOver(GameOverEvent evt)
        {
            if (!GameIsEnding)
            {
                GameIsEnding = true;

                // Remember the scene to load and handle the camera accordingly.
                if (evt.Win)
                {
                    m_GameOverSceneToLoad = m_WinScene;
                    m_GameOverSceneTime = Time.time + m_WinSceneDelay;

                    // play a sound on win
                    var audioSource = gameObject.AddComponent<AudioSource>();
                    audioSource.clip = victorySound;
                    audioSource.playOnAwake = false;

                    audioSource.outputAudioMixerGroup = AudioUtility.GetAudioGroup(AudioUtility.AudioGroups.HUDVictory);
                    audioSource.PlayScheduled(AudioSettings.dspTime + delayBeforeWinMessage);
                    

                    // create a game message

 

                    var message = Instantiate(WinGameMessagePrefab).GetComponent<DisplayMessage>();
                    if (message)
                    {
                        message.delayBeforeShowing = delayBeforeWinMessage;
                        message.GetComponent<Transform>().SetAsLastSibling();
                    }

                    // Zoom in on the player.
                    StartCoroutine(ZoomInOnPlayer());
                }
                else
                {
                    m_GameOverSceneToLoad = m_LoseScene;
                    m_GameOverSceneTime = Time.time + m_LoseSceneDelay;

                     // play a sound on win
                    var audioSource = gameObject.AddComponent<AudioSource>();
                    audioSource.clip = loseSound;
                    audioSource.playOnAwake = false;

                    audioSource.outputAudioMixerGroup = AudioUtility.GetAudioGroup(AudioUtility.AudioGroups.HUDVictory);
                    audioSource.PlayScheduled(AudioSettings.dspTime + delayBeforeWinMessage);
 

                    // Stop following the player.
                    if (m_FreeLookCamera)
                    {
                        m_FreeLookCamera.Follow = null;
                    }
                }
            }
        }

        void OnDestroy()
        {
            EventManager.RemoveListener<GameOverEvent>(OnGameOver);
        }

        IEnumerator ZoomInOnPlayer()
        {
            // Disable controller look rotation
            if (m_FreeLookCamera)
            {
                m_FreeLookCamera.m_XAxis.m_InputAxisValue = 0.0f;
                m_FreeLookCamera.m_YAxis.m_InputAxisValue = 0.0f;
                m_FreeLookCamera.m_XAxis.m_InputAxisName = "";
                m_FreeLookCamera.m_YAxis.m_InputAxisName = "";

                // Backup Middle Rig Zoom Factor 
                var zoomFactor = 1.0f;
                float middleRigZoomFactor = m_FreeLookCamera.m_Orbits[1].m_Radius;

                while (zoomFactor > 0.3f)
                {
                    m_FreeLookCamera.m_YAxis.Value = Mathf.Lerp(m_FreeLookCamera.m_YAxis.Value, 0.6f, 3.0f * Time.deltaTime);    // Ensure the vertical axis reset to a reasonable value (0.6 is the default prefab value) with a simple lerp

                    zoomFactor -= 0.1f * Time.deltaTime;
                    m_FreeLookCamera.m_Orbits[1].m_Radius = middleRigZoomFactor * zoomFactor;

                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }
}
