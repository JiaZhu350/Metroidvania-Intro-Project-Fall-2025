using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance;
    [SerializeField] private AudioSource soundFXPObject;
    [SerializeField] private AudioSource MusicSource;
    public AudioClip backgroundMusicClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        MusicSource.clip = backgroundMusicClip;
        MusicSource.loop = true;
        MusicSource.Play();
    }

    public void PlaySoundFXClip(AudioClip clip, Transform spawnTransform, float volume = 1.0f)
    {
        AudioSource audioSource = Instantiate(soundFXPObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        float clipLength = clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayRandomSoundFXClip(AudioClip[] clip, Transform spawnTransform, float volume = 1.0f)
    {
        int Rand = Random.Range(0, clip.Length);

        AudioSource audioSource = Instantiate(soundFXPObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = clip[Rand];
        audioSource.volume = volume;
        audioSource.Play();

        float clipLength = clip[Rand].length;
        Destroy(audioSource.gameObject, clipLength);
    }
}
