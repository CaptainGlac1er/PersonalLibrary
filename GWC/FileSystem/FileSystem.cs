using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWC.FileSystem
{
    public class FileSystem
    {
        DirectoryInfo dirInfo;
        public FileSystem(string dirname)
        {
            dirInfo = new DirectoryInfo(dirname);
        }
        public async Task<ICollection<FileData>> GetDirFiles()
        {
            return await Task<ICollection<FileData>>.Run(() =>
            {
                return new List<FileData>(FileData.ConvertArray(dirInfo.GetFiles()));
            });
        }
    }
}
