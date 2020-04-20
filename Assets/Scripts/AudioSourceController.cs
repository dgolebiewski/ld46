using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
    private const float FADE_THRESHOLD = 0.05f;

    [SerializeField]
    private float fadeSpeed = 0.5f;
    [SerializeField]
    private float defaultVolume;

    private float targetVolume;

    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        targetVolume = source.volume;
    }

    void Update()
    {
        if(source.volume == targetVolume)
            return;

        source.volume = Mathf.Lerp(source.volume, targetVolume, fadeSpeed * Time.deltaTime);

        if(Mathf.Abs(source.volume - targetVolume) < FADE_THRESHOLD)
        {
            source.volume = targetVolume;

            if(source.volume == 0)
                source.Stop();
        }
    }

    public void FadeIn()
    {
        source.volume = 0;
        targetVolume = defaultVolume;
        source.Play();
    }

    public void FadeOut()
    {
        targetVolume = 0;
    }
}
