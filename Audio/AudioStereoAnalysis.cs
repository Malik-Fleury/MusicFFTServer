namespace MusicFFTServer.Audio;

public struct AudioStereoAnalysis
{
    private readonly AudioAnalysis[] leftBuffer;

    private readonly AudioAnalysis[] rightBuffer;
    private readonly float baseFrequency;

    public AudioStereoAnalysis(AudioAnalysis[] leftBuffer, AudioAnalysis[] rightBuffer, float baseFrequency)
    {
        this.leftBuffer = leftBuffer;
        this.rightBuffer = rightBuffer;
        this.baseFrequency = baseFrequency;
    }
    
    public AudioAnalysis[] LeftBuffer => leftBuffer;

    public AudioAnalysis[] RightBuffer => rightBuffer;
    
    public float BaseFrequency => baseFrequency;
}