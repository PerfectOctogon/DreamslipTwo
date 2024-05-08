using UnityEngine.Timeline;

public class ParameterizedEmitter<T> : SignalEmitter
{
    public T parameter;
}

public class SignalEmitterWithString : ParameterizedEmitter<string>
{
    
}
