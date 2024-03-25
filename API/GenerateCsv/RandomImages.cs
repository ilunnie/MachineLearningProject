using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public static class RandomImages
{
    private static HttpClient httpClient = new HttpClient();

    public static void HttpClientHandler(HttpClientHandler httpClientHandler)
        => httpClient = new HttpClient(httpClientHandler);

    public static async Task<List<Image<Rgba32>>> RandomImage(int count)
    {
        List<Task<Image<Rgba32>>> downloadTasks = new List<Task<Image<Rgba32>>>();
        for (int i = 0; i < count; i++)
        {
            downloadTasks.Add(RandomImages.RandomImage());
        }
        await Task.WhenAll(downloadTasks);

        List<Image<Rgba32>> images = new List<Image<Rgba32>>();
        foreach (var task in downloadTasks)
        {
            images.Add(await task);
        }
        return images;
    }

    public static async Task<Image<Rgba32>> RandomImage()
    {
        string imageUrl;
        do imageUrl = await ImageUrlAsync();
        while (imageUrl == null);

        using (HttpResponseMessage response = await httpClient.GetAsync(imageUrl))
        {
            if (response.IsSuccessStatusCode)
            {
                using (Stream stream = await response.Content.ReadAsStreamAsync())
                {
                    return Image.Load<Rgba32>(stream);
                }
            }
            else
            {
                Console.WriteLine($"Falha ao baixar imagem: {response.StatusCode}");
                return null;
            }
        }
    }

    public static async Task<string> ImageUrlAsync()
    {
        HttpResponseMessage response = await httpClient.GetAsync("https://picsum.photos/0");
        response.EnsureSuccessStatusCode();

        return response.RequestMessage?.RequestUri?.ToString();
    }
}