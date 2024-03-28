using System;
using System.Collections.Generic;
using System.IO;
using ColorMine.ColorSpaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

// Number of records that will have in the generated csv
long records = (long)1e7;

List<object[]> data = new() {
    new string[] { "L", "A", "B" }
};

string folder = Path.Combine(Environment.CurrentDirectory, "img");
string[] files = Directory.GetFiles(folder, "*.jpg");
int filesLength = files.Length;

Image<Rgba32>[] images = new Image<Rgba32>[filesLength];
for (int i = 0; i < filesLength; i++)
{
    Console.Clear();
    Console.WriteLine($"Carregando imagens...");
    Console.WriteLine($"> {(((i + 1) / (double)filesLength) * 100).ToString("0.##")}% Concluido");
    using (FileStream stream = new FileStream(files[i], FileMode.Open, FileAccess.Read))
        images[i] = Image.Load<Rgba32>(stream);
}

var refs = records / filesLength;
for (int i = 0; i < filesLength; i++)
{
    Console.Clear();
    Console.WriteLine("Imagens Carregadas!");
    Console.WriteLine();
    Console.WriteLine("Obtendo dados...");
    Console.WriteLine($"> {(((i + 1) / (double)filesLength) * 100).ToString("0.##")}% Concluido");
    for (int j = 0; j < refs; j++)
    {
        int x = Random.Shared.Next(images[i].Width);
        int y = Random.Shared.Next(images[i].Height);
        Rgba32 pixel = images[i][x, y];

        byte r = pixel.R;
        byte g = pixel.G;
        byte b = pixel.B;

        var color = new Rgb { R = r, G = g, B = b };
        var lab = color.To<Lab>();

        data.Add(new object[] { lab.L, lab.A, lab.B });
    }
}

Console.Clear();
Console.WriteLine("Imagens Carregadas!");
Console.WriteLine("Dados Obtidos!");
Console.WriteLine();
Console.WriteLine("Escrevendo CSV...");
string filePath = "colorScale.csv";
Csv.WriteCsv(filePath, data.ToArray());