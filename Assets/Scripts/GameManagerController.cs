using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;
using System.Collections;

public class GameManagerController : MonoBehaviour
{
    // Singleton instance
    public static GameManagerController Instance { get; private set; }

    // Levels data
    public Level[] levels = new Level[]
    {
        new Level("Level1", -5f, 15f, false),
    };

    // Game-wide variables
    public SceneTransData sceneTransData;
    public Scene curScene;
    public int curLevIndex = 0;
    public InputActionAsset InputActions;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SceneTransEnd());
        curScene = scene;
        InputActions.FindActionMap("Player").Enable();
    }

    private void Awake()
    {
        // Ensure only one instance of GameManagerController exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
        }

        sceneTransData.Scale = new StyleScale(new Vector2(0f, 0f));
    }

    public IEnumerator SceneTransStart(string sceneName)
    {
        Time.timeScale = 0f;

        while (sceneTransData.Scale.value.value.x <= 2.25f && sceneTransData.Scale.value.value.y <= 2.25f)
        {
            float ScaleX = sceneTransData.Scale.value.value.x;
            float ScaleY = sceneTransData.Scale.value.value.y;
            float SpeedX = sceneTransData.AnimSpeed.value.value.x;
            float SpeedY = sceneTransData.AnimSpeed.value.value.y;

            sceneTransData.Scale = new StyleScale(new Vector2(ScaleX + SpeedX, ScaleY + SpeedY));
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator SceneTransEnd()
    {
        yield return null; // Frames do not run when loading, so this will stop until ready

        while (sceneTransData.Scale.value.value.x >= 0f && sceneTransData.Scale.value.value.y >= 0f)
        {
            float ScaleX = sceneTransData.Scale.value.value.x;
            float ScaleY = sceneTransData.Scale.value.value.y;

            float SpeedX = sceneTransData.AnimSpeed.value.value.x;
            float SpeedY = sceneTransData.AnimSpeed.value.value.y;

            sceneTransData.Scale = new StyleScale(new Vector2(ScaleX - SpeedX, ScaleY - SpeedY));
            yield return null;
        }

        sceneTransData.Scale = new StyleScale(new Vector2(0f, 0f));

        Time.timeScale = 1f;
    }
}

public class Level
{
    public bool isComplete { get; set; }
    public string levelName { get; private set; }
    public float upperBound { get; private set; }
    public float lowerBound { get; private set; }
    public Level(string name, float lower, float upper, bool complete)
    {
        levelName = name;
        lowerBound = lower;
        upperBound = upper;
        isComplete = complete;
    }
}