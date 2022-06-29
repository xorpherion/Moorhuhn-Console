using System.Diagnostics;
using System.Text;

namespace Moorhuhn_Console;

public class Renderer
{
    public char[][] Pixels { get; }

    public Renderer(int width, int height)
    {
        height--;
        height--;
        width--;

        Pixels = new char[height][];
        for (int i = 0; i < Pixels.Length; i++)
        {
            Pixels[i] = new char[width];
        }
    }

    public void Draw(int score, Stopwatch playTimer, TimeSpan maxPlayTime)
    {
        Console.SetCursorPosition(0, 0);
        // Console.Clear();
        var timeLeft = maxPlayTime - playTimer.Elapsed;
        if (timeLeft.TotalMilliseconds < 0)
            timeLeft = TimeSpan.Zero;
        Console.WriteLine(String.Join("", Field(), $"Punktestand: {score}",
            $"| Zeit übrig: {timeLeft.TotalMilliseconds}"));
    }

    private String Field()
    {
        StringBuilder sb = new();
        foreach (var row in Pixels)
        {
            foreach (var pixel in row)
                sb.Append(pixel);

            sb.Append('\n');
        }

        return sb.ToString();
    }

    private String Clear()
    {
        return String.Join("", Pixels.Select(row => "\n").ToList());
    }
}