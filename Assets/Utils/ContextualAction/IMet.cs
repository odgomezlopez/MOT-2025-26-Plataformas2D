using System;

public interface IMet
{
    /// <summary>Current value of the check.</summary>
    bool IsMet { get; }

    /// <summary>Fired whenever IsMet changes.</summary>
    event Action<bool> OnMetChanged;
}

public interface ICheck: IMet { }
public interface IRequirement : IMet { 
    string FailRequiermentMsg { get; }
}
