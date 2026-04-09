using UnityEditor;
using UnityEngine;

public class DevManager : MonoBehaviour
{
#if UNITY_EDITOR
    [CustomEditor(typeof(DevManager)), CanEditMultipleObjects]
    public class DevManagerEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //ZONA MODIFICABLE, SOLO PARA DESARROLLADORES
            if (GUILayout.Button("Reset Player Position"))
            {
                var player = FindAnyObjectByType<PlayerController>();
                if (player) player.transform.position = Vector3.zero;
            }

            if (GUILayout.Button("Game Over"))
            {
                GameManager.Instance.GameOver();
            }

            if (GUILayout.Button("Win"))
            {
                GameManager.Instance.Win();
            }
        }
    }
#endif
}
