namespace MusicFFTServer.Audio;

public struct AudioAnalysis
{
    private float frequency;
    private float magnitude;

    public AudioAnalysis(float frequency, float magnitude)
    {
        this.frequency = frequency;
        this.magnitude = magnitude;
    }

    public float GetFrequency()
    {
        return frequency;
    }

    public float GetMagnitude()
    {
        return magnitude;
    }
}