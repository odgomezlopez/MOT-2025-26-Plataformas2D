using System;
using UnityEngine;

[System.Serializable]
public class ObservableRangedFloat
{
    [SerializeField] private float _Value = 3;
    [SerializeField] private float _MaxValue= 3;
    
    //Getter/Setter
    public float Value { get => _Value; set => SetValue(value);}
    public float MaxValue { get => _MaxValue;set => SetMaxValue(value);}

    //Eventos
    public event Action<float,float,float> OnValueChanged;//Evento que se dispara cuando el valor cambia

    //Funciones
    private void SetValue(float newValue)
    {
        float oldValue= _Value;
        _Value = Mathf.Clamp(newValue, 0, _MaxValue);

        if(oldValue != _Value) 
            OnValueChanged?.Invoke(_Value, _MaxValue, oldValue);
    }

    private void SetMaxValue(float newValue)
    {
        float oldValue = _MaxValue;
        _MaxValue = Mathf.Max(newValue, 1);

        if (oldValue != _MaxValue) 
            OnValueChanged?.Invoke(_Value, _MaxValue, oldValue);
    }

    public void ForceNotify()
    {
        OnValueChanged?.Invoke(_Value, _MaxValue,0);
    }

    public void Reset()
    {
        Value = MaxValue;
    }
}

