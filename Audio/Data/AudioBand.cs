using System.Text.Json.Serialization;

namespace MusicFFTServer.Audio;

public class AudioBand
{
    private float baseFrequency;
    private float min;
    private float max;
    private float magnitude;

    public AudioBand(float baseFrequency, float min, float max, float magnitude)
    {
        this.baseFrequency = MathF.Round(baseFrequency, 2);
        this.min = min;
        this.max = max;
        this.magnitude = MathF.Round(magnitude, 4);
    }
    
    [JsonPropertyName("bf")]
    public float BaseFrequency
    {
        get => baseFrequency;
        set => baseFrequency = value;
    }

    [JsonPropertyName("mn")]
    public float Min
    {
        get => min;
        set => min = value;
    }

    [JsonPropertyName("mx")]
    public float Max
    {
        get => max;
        set => max = value;
    }

    [JsonPropertyName("mgn")]
    public float Magnitude
    {
        get => magnitude;
        set => magnitude = value;
    }

    public float GetMinFrequency()
    {
        return min * baseFrequency;
    }
    
    public float GetMaxFrequency()
    {
        return max * baseFrequency;
    }
}