using NAudio.Wave;

namespace MusicFFTServer.Audio;

public class AudioManager
{
    public delegate void AnalysisDoneEventHandler(AudioStereoAnalysis audioStereoAnalysis);
    public event AnalysisDoneEventHandler OnAnalysisDone;
    
    private WasapiLoopbackCapture loopbackCapture;
    private AudioProcessor audioProcessor;
    
    public AudioManager(int sampleRate = 44100, int fftSize = 1024)
    {
        loopbackCapture = new WasapiLoopbackCapture();
        loopbackCapture.DataAvailable += OnDataAvailable;

        audioProcessor = new AudioProcessor();
    }

    public void StartListening()
    {
        loopbackCapture.StartRecording();
    }

    public void StopListening()
    {
        loopbackCapture.StopRecording();
    }

    private void OnDataAvailable(object? sender, WaveInEventArgs waveInEventArgs)
    {
        AudioStereoAnalysis audioStereoAnalysis = audioProcessor.Process(waveInEventArgs.Buffer);
        OnAnalysisDone?.Invoke(audioStereoAnalysis);
    }
}