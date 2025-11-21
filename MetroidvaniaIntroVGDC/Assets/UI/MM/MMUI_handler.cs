using UnityEngine;
using UnityEngine.UIElements;

public class MMUI_handler : MonoBehaviour
{
    public UIDocument uiDoc; // Assign this in the Inspector

    private Button playButton;

    public playButton_action playButton_script;

    void OnEnable()
    {
        if (uiDoc != null)
        {
            // Query for the button by name
            playButton = uiDoc.rootVisualElement.Q<Button>("playButton"); 

            // Attach the action
            if (playButton != null)
            {
                playButton.clicked += OnPlayButtonClicked;
            }
        }
    }

    void OnDisable()
    {
        // Detach the action to prevent memory leaks
        if (playButton != null)
        {
            playButton.clicked -= OnPlayButtonClicked;
        }
    }

    // Define the action to be performed when the button is clicked
    public void OnPlayButtonClicked()
    {
        Debug.Log("Button 'playButton' was clicked!");
        playButton_script.action();
        // Add your desired logic here
    }
}