using UnityEngine;
using UnityEngine.UIElements;

namespace InContextLevelEditor.UI
{
    public class MainMenuUIController : MonoBehaviour
    {
        public Button StartButton;
        public Button MessageButton;
        public Label MessageText;

        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            StartButton = root.Q<Button>("StartButton");
            MessageButton = root.Q<Button>("MessageButton");
            MessageText = root.Q<Label>("MessageText");

            StartButton.clicked += OnStartButtonPressed;
            MessageButton.clicked += OnMessageButonPressed;
        }

        void OnDestroy()
        {
            StartButton.clicked -= OnStartButtonPressed;
            MessageButton.clicked -= OnMessageButonPressed;
        }

        void OnStartButtonPressed()
        {
            //SceneManager.LoadScene("Game");
        }

        void OnMessageButonPressed()
        {
            MessageText.text = "Hello there!";
            MessageText.style.display = DisplayStyle.Flex; //unhides
        }
    }
}