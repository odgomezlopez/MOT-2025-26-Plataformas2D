using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Dialogue/Dialogue Asset")]
public class DialogueSO : ScriptableObject
{
    [System.Serializable]
    public class DialogueLine
    {
        [SerializeField] private CharacterDataSO speakerData;
        [SerializeField] private LocalizedString sentence;

        public CharacterDataSO SpeakerData => speakerData;
        public LocalizedString Sentence => sentence;
    }

    [SerializeField] public List<DialogueLine> lines = new();

    public void StartDialogue()
    {
        if (DialogueManager.Instance != null)
            DialogueManager.Instance.StartDialogue(this);
    }
}