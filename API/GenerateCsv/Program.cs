using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ColorMine.ColorSpaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

HttpClient.DefaultProxy.Credentials = System.Net.CredentialCache.DefaultCredentials;

int references = 100;
long lines = (long)1e6;

List<object[]> data = new() {
    new string[] { "L", "A", "B" }
};

List<Image<Rgba32>> images = await RandomImages.RandomImage(references);
Console.WriteLine("Requisições Finalizadas");

var quantLines = lines / references;
foreach (var image in images)
{
    for (int j = 0; j < quantLines; j++)
    {
        await Task.Run(() =>
        {
            Rgba32 pixelColor = image[
                Random.Shared.Next(image.Width),
                Random.Shared.Next(image.Height)
            ];
            byte r = pixelColor.R;
            byte g = pixelColor.G;
            byte b = pixelColor.B;

            var color = new Rgb { R = r, G = g, B = b };
            var lab = color.To<Lab>();

            data.Add(new object[] { lab.L, lab.A, lab.B });
        });
    }
};


// if (image != null)
// {
//     Console.WriteLine($"Dimensões da imagem: {image.Width} x {image.Height}");

//     for (int y = 0; y < image.Height; y++)
//     {
//         for (int x = 0; x < image.Width; x++)
//         {
//             Rgba32 pixelColor = image[x, y];
//             byte r = pixelColor.R;
//             byte g = pixelColor.G;
//             byte b = pixelColor.B;

//             var L = 0.2126 * r + 0.7152 * g + 0.0722 * b;
//             byte gray = (byte)Math.Round(L * 255);

//             data.Add(new object[]{r, g, b, gray});
//         }
//     }
// }

Console.WriteLine("Escrevendo o csv...");
string filePath = "colorScale.csv";
Csv.WriteCsv(filePath, data.ToArray());