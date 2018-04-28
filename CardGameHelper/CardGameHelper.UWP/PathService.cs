using CardGameHelper.DependencyServices;
using CardGameHelper.UWP;
using System.IO;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace CardGameHelper.UWP
{
    public class FileHelper : IPathService
    {
        public string GetLocalPath(string filename) =>
            Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);
    }
}