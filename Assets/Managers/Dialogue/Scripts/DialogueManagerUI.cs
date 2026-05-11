using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class DialogueManagerUI : ScreenController
{
    #region Inspector Fields

    [Header("Animation")]
    [SerializeField] private bool animateOnFirstShow = true;
    [SerializeField] private bool animateOnHide = true;
    [SerializeField] private bool animateOnSpeakerChange = true;

    [Header("Input")]
    [SerializeField] private InputActionReference nextSentence;
    [SerializeField] private AudioResource nextSentenceSFX;

    [SerializeField] private DialogueDesignRegistry designs = new();

    #endregion

    #region State

    private DialogueDesignManager activeDesign;
    private bool isRevealing;
    private readonly Queue<DialogueSO.DialogueLine> sentences = new();

    #endregion

    #region Unity Lifecycle

    protected override void Awake()
    {
        base.Awake();
        newActionMap = "Dialogue"; //Todo: adapt to use GameManager states instead of action maps, or make it flexible to support both approaches.

        designs.Build(this);
    }

    #endregion

    #region Screen Show / Hide

    public override void ShowScreen()
    {
        base.ShowScreen();
        nextSentence.action.performed += OnNextPressed;
    }

    public override void HideScreen()
    {
        nextSentence.action.performed -= OnNextPressed;
        StartCoroutine(HideRoutine());
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        nextSentence.action.performed -= OnNextPressed;
    }

    private IEnumerator HideRoutine()
    {
        var hiding = activeDesign;
        SetActiveDesign(null);
        isRevealing = false;

        if (hiding != null)
        {
            hiding.DialogueText.Stop();
            yield return hiding.Hide(animateOnHide);
        }
        DialogueManager.Instance.EndDialogue();
        base.HideScreen();
    }

    #endregion

    #region Dialogue Flow

    public void ShowDialogue(DialogueSO dialogue)
    {
        sentences.Clear();
        foreach (var line in dialogue.lines)
            sentences.Enqueue(line);

        ShowScreen();
        AdvanceToNextSentence();
    }

    private void AdvanceToNextSentence()
    {

        if (sentences.Count == 0)
        {
            HideScreen();
            return;
        }

        var line = sentences.Dequeue();
        var target = designs.Resolve(line.SpeakerData?.dialogueDesign);

        if (target == null)
        {
            Debug.LogError($"[{nameof(DialogueManagerUI)}] No design available to render this line.", this);
            AdvanceToNextSentence();
            return;
        }

        StartCoroutine(SwitchAndPlay(target, line));
    }

    private IEnumerator SwitchAndPlay(DialogueDesignManager target, DialogueSO.DialogueLine line)
    {
        // Prepare the design's visuals before it appears so it never pops mid-animation.
        target.ApplySpeaker(line.SpeakerData,line.Status);
        target.HideAdvice();

        if (activeDesign != target)
        {
            bool animate = activeDesign == null ? animateOnFirstShow : animateOnSpeakerChange;

            if (activeDesign != null)
            {
                activeDesign.HideAdvice();
                yield return activeDesign.Hide(animate);
            }

            SetActiveDesign(target);
            yield return activeDesign.Show(animate);
        }

        isRevealing = true;
        activeDesign.DialogueText.Play(line.Sentence.GetLocalizedString());
    }

    /// <summary>
    /// Swaps the active design and moves the RevealCompleted subscription with it.
    /// Pass null to clear.
    /// </summary>
    private void SetActiveDesign(DialogueDesignManager next)
    {
        if (activeDesign == next) return;

        if (activeDesign != null)
            activeDesign.DialogueText.RevealCompleted -= OnRevealCompleted;

        activeDesign = next;

        if (activeDesign != null)
            activeDesign.DialogueText.RevealCompleted += OnRevealCompleted;
    }

    #endregion

    #region Input & Reveal Callbacks

    private void OnNextPressed(InputAction.CallbackContext _)
    {
        if (activeDesign == null) return;

        if (isRevealing)
            activeDesign.DialogueText.SkipToEnd();
        else
        {
            AdvanceToNextSentence();
            if(nextSentenceSFX) AudioManager.Instance.PlaySFX(nextSentenceSFX);
        }
    }

    private void OnRevealCompleted()
    {
        if (!isRevealing) return;
        isRevealing = false;
        activeDesign?.ShowAdvice();
    }

    #endregion
}