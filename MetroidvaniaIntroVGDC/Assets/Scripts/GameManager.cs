using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    [SerializeField] public Vector2 respawnPoint;
    [HideInInspector] public string respawnScene;
    [SerializeField] private GameObject player;

    FadeInOut fade;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (string.IsNullOrEmpty(respawnScene))
        {
            respawnScene = SceneManager.GetActiveScene().name;
        }
            

        if (respawnPoint == Vector2.zero && player != null)
        {
            respawnPoint = player.transform.position;
        }
        fade = Object.FindAnyObjectByType<FadeInOut>();

    }
    public void SetRespawn(RespawnPoint rp)
    {
        respawnPoint = rp.transform.position;
        respawnScene = SceneManager.GetActiveScene().name;
        player.GetComponent<PlayerHealth>().UnfreezeMovement();
        //Debug.Log($"Respawn set in scene {respawnScene} at {respawnPoint}");
    }
    public void RespawnPlayer()
    {
        Debug.Log($"Respawning player in scene {respawnScene} at {respawnPoint}");
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (SceneManager.GetActiveScene().name != respawnScene)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(respawnScene);
        }
        else
        {
            MovePlayerToRespawn();
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == respawnScene)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            player = GameObject.FindGameObjectWithTag("Player");
            MovePlayerToRespawn();
            
        }
    }

    private void MovePlayerToRespawn()
    {
        if (player != null)
        {
            player.transform.position = respawnPoint;
            player.GetComponent<PlayerHealth>().Respawn();

        }
        else
        {
            Debug.LogWarning("Player object not found for respawn.");
        }
    }


    public IEnumerator FadeAndRespawn(float fadeTime)
    {
        if (fade != null)
        {
            fade.TimeToFade = fadeTime;
            fade.FadeIn();
            yield return new WaitForSeconds(fadeTime + 1);
            RespawnPlayer();
            fade.FadeOut();
        }
        else
        {
            Debug.LogWarning("FadeInOut component not found. Respawning without fade.");
            RespawnPlayer();
        }
    }

    public void StartFadeAndRespawn(float fadeTime)
    {
        StartCoroutine(FadeAndRespawn(fadeTime));
    }
}
