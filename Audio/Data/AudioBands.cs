using System.Text.Json.Serialization;

namespace MusicFFTServer.Audio;

public class AudioBands
{
    private const float MultiplyFactor = 1000;
    
    private const int MinBassFrequency = 2;
    private const int MaxBassFrequency = 27;
    private const int MinMediumFrequency = 27;
    private const int MaxMediumFrequency = 435;
    private const int MinHighFrequency = 435;
    private const int MaxHighFrequency = 2200;
    
    private AudioBand bassBand = new AudioBand(0,2,27,0);
    private AudioBand mediumBand = new AudioBand(0,27,435,0);
    private AudioBand highBand = new AudioBand(0,435,2200,0);

    public AudioBands(AudioStereoAnalysis audioStereoAnalysis)
    {
        Update(audioStereoAnalysis);
    }

    public void Update(AudioStereoAnalysis audioStereoAnalysis)
    {
        float totalBassMagnitude = 0.0f;
        float totalMediumMagnitude = 0.0f;
        float totalHighMagnitude = 0.0f;
        
        for (int i = 0; i < audioStereoAnalysis.LeftBuffer.Length; i++)
        {
            AudioAnalysis leftAnalysis = audioStereoAnalysis.LeftBuffer[i];
            AudioAnalysis rightAnalysis = audioStereoAnalysis.RightBuffer[i];

            float averageMagnitude = (leftAnalysis.GetMagnitude() + rightAnalysis.GetMagnitude()) / 2.0f;
            
            if (i is >= MinBassFrequency and < MaxBassFrequency)
            {
                totalBassMagnitude += averageMagnitude;
            }
            
            if (i is >= MinMediumFrequency and < MaxMediumFrequency)
            {
                totalMediumMagnitude += averageMagnitude;
            }
            
            if (i is >= MinHighFrequency and < MaxHighFrequency)
            {
                totalHighMagnitude += averageMagnitude;
            }
        }

        bassBand = new AudioBand(
            audioStereoAnalysis.BaseFrequency,
            MinBassFrequency,
            MaxBassFrequency,
            totalBassMagnitude / (MaxBassFrequency - MinBassFrequency)
        );
        
        mediumBand = new AudioBand(
            audioStereoAnalysis.BaseFrequency,
            MinMediumFrequency,
            MaxMediumFrequency,
            totalMediumMagnitude / (MaxMediumFrequency - MinMediumFrequency)
        );
        
        highBand = new AudioBand(
            audioStereoAnalysis.BaseFrequency,
            MinHighFrequency,
            MaxHighFrequency,
            totalHighMagnitude / (MaxHighFrequency - MinHighFrequency)
        );
        
        bassBand.Magnitude *= MultiplyFactor;
        mediumBand.Magnitude *= MultiplyFactor;
        highBand.Magnitude *= MultiplyFactor;
    }
    
    [JsonPropertyName("B")]
    public AudioBand BassBand
    {
        get => bassBand;
        set => bassBand = value;
    }

    [JsonPropertyName("M")]
    public AudioBand MediumBand
    {
        get => mediumBand;
        set => mediumBand = value;
    }

    [JsonPropertyName("H")]
    public AudioBand HighBand
    {
        get => highBand;
        set => highBand = value;
    }
}