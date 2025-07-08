namespace MusicFFTServer.Tools;

public class DoubleBuffer<T>
{
    private T[] frontBuffer;
    private T[] backBuffer;

    public DoubleBuffer(int size)
    {
        frontBuffer = new T[size];
        backBuffer = new T[size];
    }
    
    public void SwapBuffer()
    {
        (frontBuffer, backBuffer) = (backBuffer, frontBuffer);
    }

    public int GetSize()
    {
        return frontBuffer.Length;
    }
    
    public T[] GetFrontBuffer()
    {
        return frontBuffer;
    }

    public T[] GetBackBuffer()
    {
        return backBuffer;
    }
}