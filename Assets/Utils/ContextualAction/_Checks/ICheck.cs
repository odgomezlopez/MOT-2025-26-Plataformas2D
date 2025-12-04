using System;

public interface ICheck
{
    /// <summary>Current value of the check.</summary>
    bool IsMet { get; }

    /// <summary>Fired whenever IsMet changes.</summary>
    event Action<bool> OnCheckValueChanged;
}


//Marca los elementos que al activarse se activa la UI
public interface ICheckUI : ICheck { }
