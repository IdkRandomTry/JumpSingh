using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip birdsChirping;
    public AudioClip middleWind;
    public AudioClip highWind;
    public AudioClip jump;
    public AudioClip walk;


    public GameObject camTarget; // Assign CamTarget in the Inspector
    private camsnap camSnapScript;
    private int currentLevel = -1;

    public float fadeTime = 1f;

    private void Start()
    {
        if (camTarget == null)
        {
            Debug.LogError("CamTarget not assigned in AudioManager!");
            return;
        }

        camSnapScript = camTarget.GetComponent<camsnap>();
        if (camSnapScript == null)
        {
            Debug.LogError("No camsnap script found on CamTarget!");
        }
    }

    void Update()
    {
        if (camSnapScript == null) return;

        int level = camSnapScript.currentLevel;

        if (level != currentLevel)
        {
            currentLevel = level;
            AudioClip selectedClip = GetClipForLevel(level);

            if (MusicSource.clip != selectedClip)
            {
                MusicSource.clip = selectedClip;
                MusicSource.Play();
                // StartCoroutine(FadeToNewTrack(selectedClip));
            }
        }

    }

    private AudioClip GetClipForLevel(int level)
    {
        if (level <= 1) return birdsChirping;
        else if (level <= 3) return middleWind;
        else return highWind;
    }

    // private IEnumerator FadeToNewTrack(AudioClip newClip)
    // {
    //     // Fade out
    //     float startVolume = MusicSource.volume;
    //     for (float t = 0; t < fadeTime; t += Time.deltaTime)
    //     {
    //         MusicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
    //         yield return null;
    //     }

    //     MusicSource.volume = 0;
    //     MusicSource.Stop();
    //     MusicSource.clip = newClip;
    //     MusicSource.Play();

    //     // Fade in
    //     for (float t = 0; t < fadeTime; t += Time.deltaTime)
    //     {
    //         MusicSource.volume = Mathf.Lerp(0, startVolume, t / fadeTime);
    //         yield return null;
    //     }

    //     MusicSource.volume = startVolume;
    // }

}
