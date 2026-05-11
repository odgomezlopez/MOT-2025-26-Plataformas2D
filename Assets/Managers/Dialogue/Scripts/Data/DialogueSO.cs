using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using static CharacterDataSO;

[CreateAssetMenu(menuName = "Dialogue/Dialogue Asset")]
public class DialogueSO : ScriptableObject
{
    [System.Serializable]
    public class DialogueLine
    {
        [SerializeField] private CharacterDataSO speakerData;
        [SerializeField] private LocalizedString sentence;
        [SerializeField] private CharacterStatus status;

        public CharacterDataSO SpeakerData => speakerData;
        public LocalizedString Sentence => sentence;
        public CharacterStatus Status => status;
    }

    [SerializeField] public List<DialogueLine> lines = new();

    [SerializeField] public UnityEngine.Events.UnityEvent OnEndDialogue;

    public void StartDialogue()
    {
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.StartDialogue(this);
    }
}