using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

public static class Requester
{
    private static string[] options = { "DecisionTree", "ElasticNet" };
    public static int Option { get; set; }
    public static string Algorithm
    {
        get => options[Option % options.Length];
    }

    public static async Task<Bitmap> toColorAsync(this Bitmap bitmap)
    {
        var L = bitmap.Luminosity();
        string json = JsonConvert.SerializeObject(new { L });

        using (HttpClient client = new HttpClient())
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("http://127.0.0.1:5000/" + Algorithm, content);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                double[][] array = JsonConvert.DeserializeObject<double[][]>(responseBody);

                Bitmap result = new Bitmap(bitmap.Width, bitmap.Height);

                for (int x = 0; x < bitmap.Width; x++)
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        int coord = bitmap.Width * y + x;

                        Color rgb = Utils.LabToRgb(L[coord], array[coord][0], array[coord][1]);
                        result.SetPixel(x, y, rgb);
                    }

                MessageBox.Show(json);
                return result;
            }
            else throw new HttpRequestException("Unanswered request!");
        }
    }
}