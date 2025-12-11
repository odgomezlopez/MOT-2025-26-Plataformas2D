using System;

public interface IMet
{
    /// <summary>Current value of the check.</summary>
    ObservableValue<bool> IsMet { get; }
}

public interface ICheck: IMet { }
public interface IRequirement : IMet { 
    string FailRequiermentMsg { get; }
}
