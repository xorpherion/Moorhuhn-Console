using System.Collections.ObjectModel;
using System.Text;

namespace Moorhuhn_Console;

public class Renderer
{
    public int ColumnSize { get; }
    public char[][] Pixels { get; }

    public Renderer(int width, int height, int columnSize)
    {
        height--;
        height--;
        width--;
        
        ColumnSize = columnSize;
        Pixels = new char[height][];
        for (int i = 0; i < Pixels.Length; i++)
        {
            Pixels[i] = new char[width];
        }

        for (int y = 0; y < Math.Min(2,height); y++)
        {
            for (int x = 0; x < Math.Min(5,width); x++)
            {
                Pixels[y][x] = (char) (65+x);
            }
        }
    }

    public void Draw()
    {
        Console.SetCursorPosition(0,0);
        Console.WriteLine(String.Join("",Field()));
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
        return String.Join("",Pixels.Select(row => "\n").ToList());
    }
}