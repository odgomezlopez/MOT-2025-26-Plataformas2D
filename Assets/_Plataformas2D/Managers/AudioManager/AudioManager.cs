using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager I;
    void Awake() => I = this;

    //Sources
    [SerializeField] AudioSource backgroundSource;

    //Cambiar el sonido de background
    public void PlayBackground(AudioResource res)
    {
        if (backgroundSource == null) return;
        backgroundSource.resource = res;
        backgroundSource.Play();
    }

    public void PauseBackround()
    {
        if (backgroundSource == null) return;
        backgroundSource.Pause();
    }

    // AudioResource: al menos public AudioClip Clip;
    public static void Play(
        AudioResource res,
        Vector3 position,
        float volume = 1f,
        float pitch  = 1f,
        bool is3D    = false,              // false = 2D (defecto), true = 3D
        AudioMixerGroup mixer = null)
    {
        var go = new GameObject("Audio_" + res.name);
        go.transform.position = position;

        var src = go.AddComponent<AudioSource>();
        src.resource   = res;
        src.volume = volume;
        src.pitch  = pitch;
        src.spatialBlend = is3D ? 1f : 0f;  // 2D -> 0, 3D -> 1
        if (mixer != null) src.outputAudioMixerGroup = mixer;

        src.Play();
        Object.Destroy(go, src.clip.length / Mathf.Max(pitch, 0.01f));
    }
}
