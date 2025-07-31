using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    public AudioClip BGmusic;

    private void Start()
    {
        musicSource.clip = BGmusic;
        musicSource.Play();
    }
}
