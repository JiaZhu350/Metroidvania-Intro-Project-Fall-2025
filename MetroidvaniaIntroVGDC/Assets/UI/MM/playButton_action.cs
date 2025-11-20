using UnityEngine;
using UnityEngine.SceneManagement;

public class playButton_action : MonoBehaviour
{
    public string targetSceneName;
    private void LoadSpecificScene(string sceneName)
    {
        //Debug.Log("PlayButton did action.");
        SceneManager.LoadScene(sceneName);
    }

    // What happens when button is clicked.
     public void action()
    {
        LoadSpecificScene(targetSceneName); 
    }

}
