using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GWC.FileSystem
{
    public class FileData
    {
        private FileInfo file;
        public FileData(string filepath)
        {

            this.file = new FileInfo(filepath);
        }
        public FileData(FileInfo file)
        {

            this.file = file;
        }
        public async Task<string> GetFileContents()
        {
            if (file == null)
                return null;
            return await Task<string>.Run(() =>
            {
                using (StreamReader fileReader = File.OpenText(file.FullName))
                {
                    return fileReader.ReadToEnd();
                }
            });
        }
        public async void WriteFileContents(string toWrite)
        {
            Console.WriteLine(toWrite);
            using (StreamWriter fileWriter = GetStreamWriter())
            {
                foreach (char c in toWrite)
                    await fileWriter.WriteAsync(c);
            }
        }
        public static FileData[] ConvertArray(FileInfo[] files)
        {
            FileData[] output = new FileData[files.Length];
            int i = 0;
            foreach (FileInfo file in files)
                output[i++] = new FileData(file);
            return output;
        }
        public StreamReader GetStreamReader()
        {
            return File.OpenText(file.FullName);
        }
        public StreamWriter GetStreamWriter()
        {
            return new StreamWriter(File.OpenWrite(file.FullName));
        }
        public async Task<string> WriteObject(IFileType toWrite)
        {
            await Task.Run(() =>
            {
                using (StreamWriter fileWrite = GetStreamWriter()) using (JsonTextWriter jsonWriter = new JsonTextWriter(fileWrite))
                {
                    jsonWriter.Formatting = Formatting.Indented;
                    (new JsonSerializer()).Serialize(jsonWriter, toWrite);
                }
            });
            return string.Format("{0} of data written to {1}", file.Length, file.Name);
        }
        public async Task<IGWCType> GetObjectFromJson<IGWCType>()
        {
            using (StreamReader fileRead = GetStreamReader())
            {
                string json = await fileRead.ReadToEndAsync();
                return JsonConvert.DeserializeObject<IGWCType>(json);
            }
        }
        public string GetFileName()
        {
            return file.Name;
        }
    }
}
