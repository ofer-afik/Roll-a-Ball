using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement root;
    private Button startButton;

    void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        root = _uiDocument.rootVisualElement;
        startButton = root.Q<Button>("StartButton");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startButton.RegisterCallback<ClickEvent>(OnStartButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnStartButtonClicked(ClickEvent evt)
    {
        StartCoroutine(GameManagerController.Instance.SceneTransStart("SelectLevelScene"));
    }
}