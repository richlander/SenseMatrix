using System.Drawing;
using  Iot.Device.Graphics;

public class VirtualLedMatrix
{
    private readonly int _x;
    private readonly int _y;
    private readonly int _pixels;
    private readonly Color[] _matrix;

    public VirtualLedMatrix(int x, int y)
    {
        _x = x;
        _y = y;
        _pixels = x * y;
        _matrix = new Color[_pixels];
    }

    public ReadOnlySpan<Color> Pixels => _matrix;

    public IFont? Font { get; set; }

    public Color this[int x, int y]
    {
        get => _matrix[GetPosition(x, y)];
        set
        {
            _matrix[GetPosition(x, y)] = value;
        }
    }

    public Color this[int index]
    {
        get => _matrix[index];
        set
        {
            _matrix[index] = value;
        }
    }

    public void Fill(Color color)
    {
        _matrix.AsSpan().Fill(color);
    }

    public void Clear() => Fill(Color.Black);

    public void AddBytesWithColor(ReadOnlySpan<byte> pixel, Color color)
    {
        for (int i = 0; i < 8; i++)
        {
            AddByteWithColor(pixel[i], color, i);
        }        
    }

    private void AddBytesWithColor(IReadOnlyList<byte> pixel, Color color)
    {
        for (int i = 0; i < 8; i++)
        {
            AddByteWithColor(pixel[i], color, i);
        }        
    }

    private void AddByteWithColor(byte data, Color color, int index)
    {
        for (int j = 0; j < 8; j++)
        {
            var mask = 1 << j;
            var b = data & mask;
            var c = b > 1 ? color : Color.Black;
            var address = index * _x + j;
            _matrix[address] = c;
        }
    }

    public void Write(ReadOnlySpan<Color> pixels) => pixels.CopyTo(_matrix);

    public void ScrollRight()
    {
        for (int i = _pixels - 2; i >= 0; i --)
        {
            if (i % 8 is not 7)
            {
                _matrix[i + 1] = _matrix[i];
                _matrix[i] = Color.Black;
            }
        }
    }

    /// <summary>
    /// Writes a char to the given device with the specified font.
    /// </summary>
    public void WriteLetter(char letter, Color color)
    {
        var Height = 8;
        var Width = 8;
        var x = 0;
        var y = 0;
        BdfFont font = BdfFont.Load(@"font.bdf");
        int heightToDraw = Math.Min(Height - y, font.Height);
        int firstColumnToDraw = x < 0 ? Math.Abs(x) : 0;
        int lastColumnToDraw = x + font.Width > Width ? Width - x : font.Width;        
        font.GetCharData(letter, out ReadOnlySpan<ushort> charData);

        //int b = 8 * (sizeof(ushort) - (int)Math.Ceiling(((double)font.Width) / 8)) + firstColumnToDraw;

        for (int i = 0; i < heightToDraw; i++)
        {
            for (int j = firstColumnToDraw; j < lastColumnToDraw; j++)
            {
                //int value = charData[i] << (b + j - firstColumnToDraw)  ;

                if ((charData[i] & 1 << Width - j - 1) != 0)
                {
                    this[x+i, y+j] = Color.Red;
                    // SetPixel(x + j, y + i, textR, textG, textB, buffer);
                }
                else
                {
                    this[x+i, y+j] = Color.Black;
                    // SetPixel(x + j, y + i, bkR, bkG, bkB, buffer);
                }
            }
        }
    }

    private int GetPosition(int x, int y) => x * _x + y;

    private (int x, int y) GetAddressFromPosition(int position) => (position / _x, position % _y);
}
