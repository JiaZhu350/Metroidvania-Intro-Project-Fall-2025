using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class playButton_action : MonoBehaviour
{
    public string targetSceneName;
    public GameObject playerPrefab;
    
    private void LoadSpecificScene(string sceneName)
    {
        //Debug.Log("PlayButton did action.");
        SceneManager.LoadScene(sceneName);
    }

    // What happens when button is clicked.
    public void action()
    {
        StartCoroutine(LoadLevel(targetSceneName)); 
    }

    public Animator transition;
    public float transitionTime = 1f;

    IEnumerator LoadLevel(string targetSceneName)
    {
        //start transition animation
        transition.SetTrigger("Start");

        //wait
        yield return new WaitForSeconds(transitionTime);

        var player = Instantiate(playerPrefab, new Vector3(0f, 8f, 0f), Quaternion.identity);
        DontDestroyOnLoad(player);
        
        //load new scene.
        LoadSpecificScene(targetSceneName);
    }

}
