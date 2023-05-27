namespace Yogeshwar.Helper.Extension;

public class LogHelper
{
    public static void Logs(string msg)
    {
        string folder = "wwwroot/";
        string fileName = "logs.txt";
        string fullPath = folder + fileName;
        string[] authors = { msg };
        System.IO.File.AppendAllLines(fullPath, authors.Append(DateTime.Now.ToString()));
    }
}
