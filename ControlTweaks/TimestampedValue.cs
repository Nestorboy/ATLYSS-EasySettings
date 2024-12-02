using UnityEngine;

namespace Nessie.ATLYSS.ControlTweaks;

public struct TimestampedValue<T> where T : struct
{
    private float _timeStamp;
    private float _unscaledTimeStamp;

    public T Value { get; private set; }

    public float TimeSinceChanged => Time.time - _timeStamp;
    public float UnscaledTimeSinceChanged => Time.unscaledTime - _unscaledTimeStamp;

    public bool ChangedWithin(float interval) => TimeSinceChanged <= interval;
    public bool ChangedWithinUnscaled(float interval) => UnscaledTimeSinceChanged <= interval;

    public void UpdateState(T state)
    {
        if (state.Equals(Value)) return;

        OnChanged(state);
    }

    private void OnChanged(T newValue)
    {
        Value = newValue;
        _timeStamp = Time.time;
        _unscaledTimeStamp = Time.unscaledTime;
    }
}