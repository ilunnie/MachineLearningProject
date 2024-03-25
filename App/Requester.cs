using System;
using System.Drawing;

public static class Requester
{
    public static int Option { get; set; }
    public static string Algorithm
    {
        get
        {
            return (Option % 4) switch
            {
                0 => "DecisionTree",
                1 => "ElasticNet"
            };
        }
    }

    public static Bitmap toColor(this Bitmap bitmap)
    {
        //ToDo request to the api { http://127.0.0.1:5000/ + Algorithm }
        throw new NotImplementedException();
    }
}