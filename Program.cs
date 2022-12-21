global using Iot.Device.Max7219;
using Iot.Device.SenseHat;
using System.Drawing;

Console.WriteLine("Hello, World!");

SenseHat sh = new();
SenseHatLedMatrix matrix = sh.LedMatrix;
matrix.Fill();

VirtualLedMatrix vmatrix = new(8, 8);
for (int i = 'a'; i < 'j' + 10; i++)
{
    char letter = (char)i;
    vmatrix.WriteLetter(letter, Color.DarkBlue);
    matrix.Write(vmatrix.Pixels);
    Thread.Sleep(1500);   
}

matrix.Fill();

for (int i = 0; i < 8; i++)
{
    matrix.SetPixel(i, i, Color.BlueViolet);
    matrix.SetPixel(7 - i, i, Color.BlueViolet);
}

Thread.Sleep(1000);
matrix.Fill();

for (int i = 0; i < 8; i++)
{
    matrix.SetPixel(i, 0, Color.DarkRed);
    Thread.Sleep(20);
}

for (int i = 0; i < 8; i++)
{
    matrix.SetPixel(7, i, Color.Red);
    Thread.Sleep(20);
}

for (int i = 7; i >= 0; i--)
{
    matrix.SetPixel(i, 7, Color.OrangeRed);
    Thread.Sleep(20);
}

for (int i = 7; i >= 0; i--)
{
    matrix.SetPixel(0, i, Color.MediumVioletRed);
    Thread.Sleep(20);
}

Thread.Sleep(1000);
matrix.Fill();

Spiral();

Thread.Sleep(1000);
matrix.Fill();

// write a smiley to devices buffer
var smiley = new byte[]
{
    0b00111100,
    0b01000010,
    0b10100101,
    0b10000001,
    0b10100101,
    0b10011001,
    0b01000010,
    0b00111100
};

vmatrix.AddBytesWithColor(smiley, Color.DarkBlue);
matrix.Write(vmatrix.Pixels);
Thread.Sleep(1000);

for (int i = 0; i < 8; i++)
{
    vmatrix.ScrollRight();
    matrix.Write(vmatrix.Pixels);
    Thread.Sleep(200);
}

void Spiral()
{
    for (int j = 0; j < 4; j++)
    {
        for (int i = 0; i < 8; i++)
        {
            matrix.SetPixel(Math.Abs(i - j), 0 + j, Color.Aquamarine);
            Thread.Sleep(15);
        }

        for (int i = 0; i < 8; i++)
        {
            matrix.SetPixel(7 - j, Math.Abs(i - j), Color.Azure);
            Thread.Sleep(15);
        }

        for (int i = 7; i >= 0; i--)
        {
            matrix.SetPixel(Math.Abs(i - j), 7 - j, Color.Aqua);
            Thread.Sleep(15);
        }

        for (int i = 7; i >= 0; i--)
        {
            matrix.SetPixel(0 + j, Math.Abs(i - j), Color.AliceBlue);
            Thread.Sleep(15);
        }
    }
}
