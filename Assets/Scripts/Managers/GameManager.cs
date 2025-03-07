using UnityEngine;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    public bool isMatchMode = false;
    public int initialPoints = 10;
    public int maxListeners = 5;
    public float timeLimit = 30f;

    [Header("Topics")]
    public List<TopicData> topics = new List<TopicData>();

    // Game State
    private int currentPoints;
    private int clearCount;
    private int matchScore;
    private int currentQuestion;
    private string currentTopic;
    private string hostAnswer;
    private List<ListenerAnswer> listenerAnswers = new List<ListenerAnswer>();
    private float remainingTime;
    private bool isGameActive;

    // Events
    public event Action<string> OnTopicChanged;
    public event Action<int> OnPointsChanged;
    public event Action<int> OnScoreChanged;
    public event Action<float> OnTimeChanged;
    public event Action<List<ListenerAnswer>> OnListenerAnswersUpdated;
    public event Action OnGameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDefaultTopics();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeDefaultTopics()
    {
        string[] defaultTopics = new string[]
        {
            "好きな食べ物は？",
            "休日の過ごし方は？",
            "最近ハマっていることは？",
            "子供の頃の夢は？",
            "行ってみたい国は？",
            "好きな映画のジャンルは？",
            "今日の朝ごはんは？",
            "最近買った物は？",
            "好きな季節は？",
            "特技は？"
        };

        foreach (string topic in defaultTopics)
        {
            topics.Add(new TopicData(topic));
        }
    }

    public void StartGame()
    {
        currentPoints = initialPoints;
        clearCount = 0;
        matchScore = 0;
        currentQuestion = 1;
        isGameActive = true;

        StartNewQuestion();
    }

    public void StartNewQuestion()
    {
        if (topics.Count == 0) return;

        int randomIndex = UnityEngine.Random.Range(0, topics.Count);
        currentTopic = topics[randomIndex].question;
        hostAnswer = "";
        listenerAnswers.Clear();
        remainingTime = timeLimit;

        OnTopicChanged?.Invoke(currentTopic);
        OnTimeChanged?.Invoke(remainingTime);
    }

    public void SubmitHostAnswer(string answer)
    {
        hostAnswer = answer;
        StartListenerPhase();
    }

    public void ProcessListenerAnswer(string uniqueId, string username, string nickname, string profilePictureUrl, string answer)
    {
        if (!isGameActive || listenerAnswers.Count >= maxListeners) return;

        int existingIndex = listenerAnswers.FindIndex(a => a.uniqueId == uniqueId);
        if (existingIndex != -1)
        {
            listenerAnswers[existingIndex] = new ListenerAnswer(uniqueId, username, nickname, profilePictureUrl, answer);
        }
        else
        {
            listenerAnswers.Add(new ListenerAnswer(uniqueId, username, nickname, profilePictureUrl, answer));
        }

        OnListenerAnswersUpdated?.Invoke(listenerAnswers);

        if (listenerAnswers.Count >= maxListeners)
        {
            FinishListenerPhase();
        }
    }

    private void StartListenerPhase()
    {
        remainingTime = timeLimit;
    }

    public void FinishListenerPhase()
    {
        int matchCount = 0;
        foreach (var answer in listenerAnswers)
        {
            if (answer.answer.ToLower() == hostAnswer.ToLower())
            {
                matchCount++;
            }
        }

        if (isMatchMode)
        {
            matchScore += matchCount;
            OnScoreChanged?.Invoke(matchScore);
        }
        else
        {
            if (matchCount == 0)
            {
                clearCount++;
            }
            else
            {
                currentPoints -= matchCount;
                if (currentPoints <= 0)
                {
                    GameOver();
                    return;
                }
            }
            OnPointsChanged?.Invoke(currentPoints);
        }

        currentQuestion++;
    }

    private void GameOver()
    {
        isGameActive = false;
        OnGameOver?.Invoke();
    }

    private void Update()
    {
        if (isGameActive && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            OnTimeChanged?.Invoke(remainingTime);

            if (remainingTime <= 0)
            {
                FinishListenerPhase();
            }
        }
    }
}
