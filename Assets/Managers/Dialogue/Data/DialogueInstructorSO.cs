using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;


[CreateAssetMenu(menuName = "Dialogue/Dialogue Instructor Asset")]
public class DialogueInstructorSO : DialogueSO
{
	[SerializeField] private CharacterDataSO maleSpeakerData;
	[SerializeField] private CharacterDataSO femaleSpeakerData;

	public CharacterDataSO GetSpeakerData()
	{
		if (PlayerPrefs.GetString("instructorGender", "Male") == "Male") return maleSpeakerData;
		else return femaleSpeakerData;
	}
}
