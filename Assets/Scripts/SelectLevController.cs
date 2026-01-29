using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Linq;

public class SelectLevController : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement root;

    void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        root = _uiDocument.rootVisualElement;
    }

    void Start()
    {
        // Find the ScrollView
        var scrollView = root.Q<ScrollView>(); // Locate the ScrollView container

        if (scrollView != null)
        {
            // Query the content container of the ScrollView for buttons
            var contentContainer = scrollView.contentContainer;
            if (contentContainer != null)
            {
                // Now query for all Button elements inside the content container
                var buttons = contentContainer.Query<Button>().ToList();

                foreach (Button button in buttons)
                {
                    string levelName = button.name; // Assuming button names correspond to level names
                    button.RegisterCallback<ClickEvent>(evt => 
                    {
                        GameManagerController.Instance.curLevIndex =
                            System.Array.FindIndex(GameManagerController.Instance.levels, level =>
                            level.levelName == levelName);
                        StartCoroutine(GameManagerController.Instance.SceneTransStart(levelName));
                    });
                }
            }
        }
    }
}
