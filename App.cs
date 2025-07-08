using System.Text.Json;
using MusicFFTServer.Audio;
using MusicFFTServer.Server;

namespace MusicFFTServer;

public class App
{
    private UdpServer udpServer = new UdpServer(8987, true);
    private AudioManager audioManager = new AudioManager();

    private void Configure()
    {
        audioManager.OnAnalysisDone += AudioManagerOnOnAnalysisDone;
    }
    
    private void AudioManagerOnOnAnalysisDone(AudioStereoAnalysis audiostereoanalysis)
    {
        AudioBands audioBands = new AudioBands(audiostereoanalysis);

        String json = JsonSerializer.Serialize(audioBands);
        
        udpServer.SendMessage(json);
    }

    public void Run()
    {
        Configure();
        audioManager.StartListening();
        
        Console.ReadKey();
        audioManager.StopListening();
    }
}