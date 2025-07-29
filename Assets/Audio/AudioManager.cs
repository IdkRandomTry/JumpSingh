using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip bg;
    public AudioClip jump;
    public AudioClip walk;

    private void Start()
    {
        MusicSource.clip = bg;
        MusicSource.Play();
    }
}
