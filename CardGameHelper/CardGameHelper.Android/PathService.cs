using CardGameHelper.DependencyServices;
using CardGameHelper.Droid;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace CardGameHelper.Droid
{
    public class FileHelper : IPathService
    {
        public string GetLocalPath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}