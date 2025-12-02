using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MMUI_handler : MonoBehaviour
{
    public UIDocument uiDoc; // Assign this in the Inspector

    private Button playButton;
    private Button quitButton;

    public playButton_action playButton_script;

    void OnEnable()
    {
        if (uiDoc != null)
        {
            // Query for the button by name
            playButton = uiDoc.rootVisualElement.Q<Button>("playButton"); 
            quitButton = uiDoc.rootVisualElement.Q<Button>("quitButton");

            // Attach the action
            if (playButton != null)
            {
                playButton.clicked += OnPlayButtonClicked;
            }
            if (quitButton != null)
            {
                quitButton.clicked += OnQuitButtonClicked;
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
    public void OnQuitButtonClicked()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }
}