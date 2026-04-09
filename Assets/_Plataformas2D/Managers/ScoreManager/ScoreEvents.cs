using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreEvents : MonoBehaviour
{
    //Este script se encarga de manejar eventos relacionados con la puntuación.
    //Permite configurar eventos que se activarán al alcanzar ciertos valores de puntuación.
    [System.Serializable]
    public class ScoreEvent { 
        public int scoreAmount;
        public UnityEvent evento;
        public bool triggered = false;
    }    

    [SerializeField] List<ScoreEvent> events;

    //Me suscribo/desuscribo a los cambios de puntuación
    private void OnEnable()
    {
        ScoreManager.Instance.Score.OnValueChanged += CheckEvents;
    }

    private void OnDisable()
    {
        ScoreManager.Instance.Score.OnValueChanged -= CheckEvents;
    }

    //Compruebo si se han alcanzado los valores de puntuación para activar los eventos correspondientes

    private void CheckEvents(int obj)
    {
        foreach (var scoreEvent in events)
        {
            if (!scoreEvent.triggered && obj >= scoreEvent.scoreAmount)
            {
                scoreEvent.evento.Invoke();
                scoreEvent.triggered = true;
            }
        }
    }

}
