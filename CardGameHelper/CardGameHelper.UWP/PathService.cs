using CardGameHelper.DependencyServices;
using System.IO;
using Todo.UWP;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace Todo.UWP
{
    public class FileHelper : IPathService
    {
        public string GetLocalPath(string filename) =>
            Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);
    }
}