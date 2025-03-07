using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Screens")]
    public GameObject settingsScreen;
    public GameObject gameScreen;
    public GameObject gameOverScreen;

    [Header("Settings UI")]
    public TMP_Dropdown gameModeDropdown;
    public TMP_InputField initialPointsInput;
    public TMP_InputField maxListenersInput;
    public TMP_InputField timeLimitInput;
    public TMP_InputField topicInput;
    public Button addTopicButton;
    public Transform topicListContent;
    public Button startGameButton;
    public TextMeshProUGUI serverStatusText;
    public Button toggleServerButton;

    [Header("Game UI")]
    public TextMeshProUGUI currentQuestionText;
    public TextMeshProUGUI clearCountText;
    public TextMeshProUGUI currentPointsText;
    public TextMeshProUGUI matchScoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI currentTopicText;
    public TMP_InputField hostAnswerInput;
    public Button submitHostAnswerButton;
    public GameObject hostAnswerPhase;
    public GameObject listenerAnswerPhase;
    public TextMeshProUGUI listenerCountText;
    public Image listenerProgressBar;
    public Button finishCollectingButton;
    public GameObject resultsPhase;
    public Transform listenersAnswersContent;
    public TextMeshProUGUI matchResultText;
    public Button nextQuestionButton;

    [Header("Game Over UI")]
    public TextMeshProUGUI finalScoreText;
    public Button returnToSettingsButton;

    [Header("Effect Prefabs")]
    public GameObject likeEffectPrefab;
    public GameObject giftEffectPrefab;
    public GameObject followEffectPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetupUI()
    {
        // Settings UI Setup
        gameModeDropdown.onValueChanged.AddListener(OnGameModeChanged);
        addTopicButton.onClick.AddListener(OnAddTopicClicked);
        startGameButton.onClick.AddListener(OnStartGameClicked);
        toggleServerButton.onClick.AddListener(OnToggleServerClicked);

        // Game UI Setup
        submitHostAnswerButton.onClick.AddListener(OnSubmitHostAnswerClicked);
        finishCollectingButton.onClick.AddListener(OnFinishCollectingClicked);
        nextQuestionButton.onClick.AddListener(OnNextQuestionClicked);

        // Game Over UI Setup
        returnToSettingsButton.onClick.AddListener(OnReturnToSettingsClicked);

        // Event Subscriptions
        GameManager.Instance.OnTopicChanged += UpdateCurrentTopic;
        GameManager.Instance.OnPointsChanged += UpdatePoints;
        GameManager.Instance.OnScoreChanged += UpdateScore;
        GameManager.Instance.OnTimeChanged += UpdateTimer;
        GameManager.Instance.OnListenerAnswersUpdated += UpdateListenerAnswers;
        GameManager.Instance.OnGameOver += ShowGameOver;

        TikTokManager.Instance.OnConnectionStatusChanged += UpdateServerStatus;
    }

    private void OnGameModeChanged(int value)
    {
        bool isMismatchMode = value == 0;
        initialPointsInput.gameObject.SetActive(isMismatchMode);
    }

    private void OnAddTopicClicked()
    {
        string topic = topicInput.text.Trim();
        if (!string.IsNullOrEmpty(topic))
        {
            GameManager.Instance.topics.Add(new TopicData(topic));
            AddTopicToList(topic);
            topicInput.text = "";
        }
    }

    private void AddTopicToList(string topic)
    {
        // トピックリストのUIを実装
    }

    private void OnStartGameClicked()
    {
        GameManager.Instance.StartGame();
        ShowGameScreen();
    }

    private void OnToggleServerClicked()
    {
        if (TikTokManager.Instance.IsConnected())
        {
            TikTokManager.Instance.StopServer();
        }
        else
        {
            TikTokManager.Instance.StartServer();
        }
    }

    private void OnSubmitHostAnswerClicked()
    {
        string answer = hostAnswerInput.text.Trim();
        if (!string.IsNullOrEmpty(answer))
        {
            GameManager.Instance.SubmitHostAnswer(answer);
            ShowListenerAnswerPhase();
        }
    }

    private void OnFinishCollectingClicked()
    {
        GameManager.Instance.FinishListenerPhase();
        ShowResultsPhase();
    }

    private void OnNextQuestionClicked()
    {
        GameManager.Instance.StartNewQuestion();
        ShowHostAnswerPhase();
    }

    private void OnReturnToSettingsClicked()
    {
        ShowSettingsScreen();
    }

    private void UpdateCurrentTopic(string topic)
    {
        currentTopicText.text = topic;
    }

    private void UpdatePoints(int points)
    {
        currentPointsText.text = points.ToString();
    }

    private void UpdateScore(int score)
    {
        matchScoreText.text = score.ToString();
    }

    private void UpdateTimer(float time)
    {
        timerText.text = Mathf.CeilToInt(time).ToString();
    }

    private void UpdateListenerAnswers(List<ListenerAnswer> answers)
    {
        listenerCountText.text = $"{answers.Count}/{GameManager.Instance.maxListeners}";
        listenerProgressBar.fillAmount = (float)answers.Count / GameManager.Instance.maxListeners;
    }

    private void UpdateServerStatus(bool isConnected)
    {
        serverStatusText.text = isConnected ? "オンライン" : "オフライン";
        serverStatusText.color = isConnected ? Color.green : Color.red;
        toggleServerButton.GetComponentInChildren<TextMeshProUGUI>().text = 
            isConnected ? "サーバー停止" : "サーバー開始";
    }

    private void ShowSettingsScreen()
    {
        settingsScreen.SetActive(true);
        gameScreen.SetActive(false);
        gameOverScreen.SetActive(false);
    }

    private void ShowGameScreen()
    {
        settingsScreen.SetActive(false);
        gameScreen.SetActive(true);
        gameOverScreen.SetActive(false);
    }

    private void ShowGameOver()
    {
        settingsScreen.SetActive(false);
        gameScreen.SetActive(false);
        gameOverScreen.SetActive(true);

        bool isMismatchMode = !GameManager.Instance.isMatchMode;
        if (isMismatchMode)
        {
            finalScoreText.text = $"クリア数: {GameManager.Instance.clearCount}問";
        }
        else
        {
            finalScoreText.text = $"最終スコア: {GameManager.Instance.matchScore}点";
        }
    }

    private void ShowHostAnswerPhase()
    {
        hostAnswerPhase.SetActive(true);
        listenerAnswerPhase.SetActive(false);
        resultsPhase.SetActive(false);
        hostAnswerInput.text = "";
        hostAnswerInput.interactable = true;
    }

    private void ShowListenerAnswerPhase()
    {
        hostAnswerPhase.SetActive(false);
        listenerAnswerPhase.SetActive(true);
        resultsPhase.SetActive(false);
        hostAnswerInput.interactable = false;
    }

    private void ShowResultsPhase()
    {
        hostAnswerPhase.SetActive(false);
        listenerAnswerPhase.SetActive(false);
        resultsPhase.SetActive(true);
    }

    public void ShowLikeEffect(LikeData data)
    {
        // いいねエフェクトの実装
    }

    public void ShowGiftEffect(GiftData data)
    {
        // ギフトエフェクトの実装
    }

    public void ShowFollowEffect(FollowData data)
    {
        // フォローエフェクトの実装
    }
}
