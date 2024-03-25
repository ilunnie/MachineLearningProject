using System;
using System.IO;
using System.Text;


public static class Csv
{
    public static void WriteCsv(string filePath, object[][] data)
    {
        using (var streamWriter = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            foreach (var line in data)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    string valor = Convert.ToString(line[i]);
                    double num;
                    if (!double.TryParse(line[i].ToString(), out num))
                        streamWriter.Write($"\"{valor.Replace("\"", "\"\"")}\"");
                    else
                        streamWriter.Write(line[i].ToString().Replace(",", "."));

                    if (i < line.Length - 1)
                    {
                        streamWriter.Write(",");
                    }
                }
                streamWriter.WriteLine();
            }
        }
    }
}