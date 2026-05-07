using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TypewriterText : MonoBehaviour
{
    [SerializeField] private float charactersPerSecond = 40f;
    [SerializeField] private float punctuationPause = 0.2f;
    [SerializeField] private float commaPause = 0.08f;

    private TextMeshProUGUI text;
    private Coroutine running;
    private bool skipRequested;

    public bool IsRevealing { get; private set; }
    public event System.Action RevealCompleted;

    private void OnEnable() => text = GetComponent<TextMeshProUGUI>();

    public void Play(string sentence)
    {
        Stop();
        running = StartCoroutine(Reveal(sentence));
    }

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
        if(text == null) text = GetComponent<TextMeshProUGUI>();
        text.color = color;
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

        for (int i = 0; i < totalChars; i++)
        {
            if (skipRequested)
            {
                text.maxVisibleCharacters = totalChars;
                break;
            }

            text.maxVisibleCharacters = i + 1;
            char c = text.textInfo.characterInfo[i].character;
            yield return new WaitForSecondsRealtime(delay + GetPause(c));
        }

        text.maxVisibleCharacters = totalChars;
        IsRevealing = false;
        running = null;
        RevealCompleted?.Invoke();
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