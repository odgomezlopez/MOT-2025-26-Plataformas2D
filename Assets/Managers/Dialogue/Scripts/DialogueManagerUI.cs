using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManagerUI : ScreenController
{
    private enum State { Idle, Revealing, AwaitingNext }

    [Header("Input")]
    [SerializeField] private InputActionReference nextSentence;

    [Header("Designs")]
    [SerializeField] private DialogueDesign leftDesign;
    [SerializeField] private DialogueDesign rightDesign;

    private readonly Queue<DialogueSO.DialogueLine> sentences = new();
    private DialogueDesign activeDesign;
    private State state = State.Idle;

    protected override void Awake()
    {
        base.Awake();
        newActionMap = "Dialogue";
    }

    public override void ShowScreen()
    {
        base.ShowScreen();
        SubscribeInput();
        SubscribeReveal(leftDesign);
        SubscribeReveal(rightDesign);
    }

    public override void HideScreen()
    {
        UnsubscribeInput();
        UnsubscribeReveal(leftDesign);
        UnsubscribeReveal(rightDesign);

        leftDesign?.StopReveal();
        rightDesign?.StopReveal();
        leftDesign?.Hide();
        rightDesign?.Hide();

        activeDesign = null;
        state = State.Idle;
        base.HideScreen();
    }

    public void ShowDialogue(DialogueSO dialogue)
    {
        sentences.Clear();
        foreach (var line in dialogue.lines)
            sentences.Enqueue(line);

        ShowScreen();
        AdvanceToNextSentence();
    }

    // --- Input ---------------------------------------------------------------

    private void SubscribeInput()
    {
        if (nextSentence != null)
            nextSentence.action.performed += OnNextPressed;
    }

    private void UnsubscribeInput()
    {
        if (nextSentence != null)
            nextSentence.action.performed -= OnNextPressed;
    }

    private void OnNextPressed(InputAction.CallbackContext _)
    {
        if (activeDesign == null) return;

        switch (state)
        {
            case State.Revealing:
                activeDesign.DialogueText.SkipToEnd();
                break;
            case State.AwaitingNext:
                AdvanceToNextSentence();
                break;
        }
    }

    // --- Reveal events -------------------------------------------------------

    private void SubscribeReveal(DialogueDesign design)
    {
        if (design != null && design.DialogueText != null)
            design.DialogueText.RevealCompleted += OnRevealCompleted;
    }

    private void UnsubscribeReveal(DialogueDesign design)
    {
        if (design != null && design.DialogueText != null)
            design.DialogueText.RevealCompleted -= OnRevealCompleted;
    }

    private void OnRevealCompleted()
    {
        // Only react if the completed reveal was from the active design.
        // (Both are subscribed; the inactive one shouldn't be revealing,
        // but this guards against stray events.)
        if (state != State.Revealing) return;
        state = State.AwaitingNext;
        activeDesign?.ShowAdvice();
    }

    // --- Flow ----------------------------------------------------------------

    private void AdvanceToNextSentence()
    {
        if (sentences.Count == 0)
        {
            HideScreen();
            return;
        }

        var line = sentences.Dequeue();
        var character = line.SpeakerData;
        var target = ResolveDesign(character);

        if (target == null)
        {
            Debug.LogWarning($"[DialogueManagerUI] No design assigned for side '{character?.screenSide}'. Skipping line.");
            AdvanceToNextSentence();
            return;
        }

        SwitchActiveDesign(target);
        target.ApplySpeaker(character);
        target.HideAdvice();

        state = State.Revealing;
        target.DialogueText.Play(line.Sentence.GetLocalizedString());
    }

    private DialogueDesign ResolveDesign(CharacterDataSO character)
    {
        if (character == null) return activeDesign;
        return character.screenSide switch
        {
            CharacterDataSO.ScreenSide.Left => leftDesign,
            CharacterDataSO.ScreenSide.Right => rightDesign,
            _ => activeDesign
        };
    }

    private void SwitchActiveDesign(DialogueDesign target)
    {
        if (activeDesign == target)
        {
            // Same side speaking again — make sure it's shown (no-op if already).
            target.Show();
            return;
        }

        // Hide the previous side, show the new one.
        if (activeDesign != null)
        {
            activeDesign.HideAdvice();
            activeDesign.Hide();
        }

        activeDesign = target;
        activeDesign.Show();
    }
}