using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TypewriterText : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float charactersPerSecond = 40f;
    [SerializeField] private float punctuationPause = 0.2f;
    [SerializeField] private float commaPause = 0.08f;

    [Header("Sound")]
    [Tooltip("Clips played as characters are revealed. A random clip is chosen each time to avoid repetition.")]
    [SerializeField] private AudioClip[] typingClips;
    [Tooltip("Play a sound every N revealed characters. 1 = every character, 2 = every other, etc.")]
    [Min(1)]
    [SerializeField] private int playEveryNCharacters = 2;
    [Tooltip("Skip sounds for whitespace and punctuation.")]
    [SerializeField] private bool skipNonLetters = true;
    [SerializeField] private float waitToStartSound = 0.1f;

    private TextMeshProUGUI text;
    private Coroutine running;
    private bool skipRequested;

    public bool IsRevealing { get; private set; }
    public event System.Action RevealCompleted;

    private void OnEnable()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void Play(string sentence)
    {
        Stop();
        text.text = string.Empty;
        running = StartCoroutine(Reveal(sentence));
    }

    public void EmptyText() { text.text = string.Empty; }

    public void Stop()
    {
        if (running != null) StopCoroutine(running);
        running = null;
        IsRevealing = false;
        skipRequested = false;
    }

    public void SkipToEnd()
    {
        if (IsRevealing) skipRequested = true;
    }

    public void SetColor(Color color)
    {
        if (text == null) text = GetComponent<TextMeshProUGUI>();
        text.color = color;
    }

    public void SetFont(TMP_FontAsset fontAsset)
    {
        if (fontAsset) text.font = fontAsset;
    }

    private IEnumerator Reveal(string sentence)
    {
        if (text == null) text = GetComponent<TextMeshProUGUI>();

        IsRevealing = true;
        skipRequested = false;

        text.text = sentence;
        text.maxVisibleCharacters = 0;
        text.ForceMeshUpdate();

        int totalChars = text.textInfo.characterCount;
        float delay = 1f / Mathf.Max(charactersPerSecond, 0.0001f);
        yield return new WaitForSecondsRealtime(waitToStartSound);
        for (int i = 0; i < totalChars; i++)
        {
            if (skipRequested)
            {
                text.maxVisibleCharacters = totalChars;
                break;
            }

            text.maxVisibleCharacters = i + 1;
            char c = text.textInfo.characterInfo[i].character;

            PlayTypingSound(c, i);

            yield return new WaitForSecondsRealtime(delay + GetPause(c));
        }

        text.maxVisibleCharacters = totalChars;
        IsRevealing = false;
        running = null;
        RevealCompleted?.Invoke();
    }

    private void PlayTypingSound(char c, int index)
    {
        if (typingClips == null || typingClips.Length == 0) return;
        if (index % playEveryNCharacters != 0) return;
        if (skipNonLetters && !char.IsLetterOrDigit(c)) return;

        var clip = typingClips[Random.Range(0, typingClips.Length)];
        if (clip == null) return;

        if(clip) AudioManager.Instance.PlaySFX(clip);
    }

    private float GetPause(char c)
    {
        switch (c)
        {
            case '.':
            case '!':
            case '?':
            case ':':
            case ';':
                return punctuationPause;
            case ',':
            case '—':
            case '–':
                return commaPause;
            default:
                return 0f;
        }
    }


}