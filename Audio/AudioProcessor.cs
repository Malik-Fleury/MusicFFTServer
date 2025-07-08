using MusicFFTServer.Tools;
using NAudio.Dsp;

namespace MusicFFTServer.Audio;

public class AudioProcessor
{
    private const int SampleRate = 44100;
    private const int SampleSize = 38400;
    private const int Channels = 2;
    private const int FloatSize = 4;

    private readonly int fftM;
    private readonly float baseFrequency;

    private DoubleBuffer<float> leftBuffer;
    private DoubleBuffer<float> rightBuffer;
    
    private DoubleBuffer<Complex> leftComplexBuffer;
    private DoubleBuffer<Complex> rightComplexBuffer;

    private DoubleBuffer<AudioAnalysis> leftAnalysisBuffer;
    private DoubleBuffer<AudioAnalysis> rightAnalysisBuffer;
    
    public AudioProcessor()
    {
        int bufferSize = SampleSize / (Channels * FloatSize);
        
        leftBuffer = new DoubleBuffer<float>(bufferSize);
        rightBuffer = new DoubleBuffer<float>(bufferSize);
        
        leftComplexBuffer = new DoubleBuffer<Complex>(bufferSize);
        rightComplexBuffer = new DoubleBuffer<Complex>(bufferSize);

        leftAnalysisBuffer = new DoubleBuffer<AudioAnalysis>(bufferSize);
        rightAnalysisBuffer = new DoubleBuffer<AudioAnalysis>(bufferSize);

        fftM = (int)MathF.Log2(bufferSize);
        baseFrequency = (float)SampleRate / bufferSize;
    }

    public AudioStereoAnalysis Process(byte[] buffer)
    {
        float[] backLeftBuffer = leftBuffer.GetBackBuffer();
        float[] backRightBuffer = rightBuffer.GetBackBuffer();
        
        Complex[] backLeftComplexBuffer = leftComplexBuffer.GetBackBuffer();
        Complex[] backRightComplexBuffer = rightComplexBuffer.GetBackBuffer();

        AudioAnalysis[] backLeftAnalysisBuffer = leftAnalysisBuffer.GetBackBuffer();
        AudioAnalysis[] backRightAnalysisBuffer = rightAnalysisBuffer.GetBackBuffer();
        
        for (int i = 0; i < leftBuffer.GetSize(); i++)
        {
            int index = i * Channels * FloatSize;
            backLeftBuffer[i] = BitConverter.ToSingle(buffer, index);
            backRightBuffer[i] = BitConverter.ToSingle(buffer, index + FloatSize);
        }

        for (int i = 0; i < backLeftComplexBuffer.Length; i++)
        {
            int index = i * Channels;
            float window = 1.0f; // Future, compute Hann's window ?

            Complex leftComplex = new Complex();
            leftComplex.X = backLeftBuffer[i] * window;
            leftComplex.Y = 0;
            
            Complex rightComplex = new Complex();
            rightComplex.X = backRightBuffer[i] * window;
            rightComplex.Y = 0;
            
            backLeftComplexBuffer[i] = leftComplex;
            backRightComplexBuffer[i] = rightComplex;
        }
        
        FastFourierTransform.FFT(true, fftM, backLeftComplexBuffer);
        FastFourierTransform.FFT(true, fftM, backRightComplexBuffer);

        IEnumerable<Complex> leftComplexes = backLeftComplexBuffer.Take(backLeftComplexBuffer.Length / 2);
        IEnumerable<Complex> rightComplexes = backRightComplexBuffer.Take(backRightComplexBuffer.Length / 2);
        
        int frequencyIndex = 0;
        foreach (Complex complex in leftComplexes)
        {
            backLeftAnalysisBuffer[frequencyIndex] = CreateAudioAnalysis(complex, frequencyIndex);
            frequencyIndex += 1;
        }

        frequencyIndex = 0;
        foreach (Complex complex in rightComplexes)
        {
            backRightAnalysisBuffer[frequencyIndex] = CreateAudioAnalysis(complex, frequencyIndex);
            frequencyIndex += 1;
        }
        
        leftBuffer.SwapBuffer();
        rightBuffer.SwapBuffer();
        
        leftComplexBuffer.SwapBuffer();
        rightComplexBuffer.SwapBuffer();
        
        leftAnalysisBuffer.SwapBuffer();
        rightAnalysisBuffer.SwapBuffer();

        return new AudioStereoAnalysis(
            leftAnalysisBuffer.GetFrontBuffer(),
            rightAnalysisBuffer.GetFrontBuffer(),
            baseFrequency);
    }

    private AudioAnalysis CreateAudioAnalysis(Complex complex, int frequencyIndex)
    {
        float frequency = frequencyIndex * baseFrequency;
        float magnitude = (float) Math.Sqrt(complex.X * complex.X + complex.Y * complex.Y);
        return new AudioAnalysis(frequency, magnitude);
    }
}