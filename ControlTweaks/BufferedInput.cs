namespace Nessie.ATLYSS.ControlTweaks;

public struct BufferedInput<T>  where T : struct
{
    private TimestampedValue<T> _input;

    public T Value => _input.Value;

    public bool Consumed { get; private set; }

    public float TimeSinceChanged => _input.TimeSinceChanged;

    private bool IsDefaultInputValue => _input.Value.Equals(default(T));

    public bool PressedWithin(float interval) => !IsDefaultInputValue && !Consumed && _input.ChangedWithin(interval);
    public bool PressedWithinUnscaled(float interval) => !IsDefaultInputValue && !Consumed && _input.ChangedWithinUnscaled(interval);

    public void UpdateState(T state)
    {
        if (state.Equals(Value)) return;

        _input.UpdateState(state);
        Consumed = false;
    }

    public void Consume()
    {
        Consumed = true;
    }
}