using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Process
{
    /// <summary>
    /// The handle for everything, FileSystem, System Settings, User interaction, etc...
    /// </summary>
    public class ProcessHandle
    {
        private Process process;
        public ProcessHandle(Process process)
        {
            this.process = process;
        }

        public void Reinitialise(Process process)
        {
            this.process = process;
        }

        public static class FileSystem
        {
            public class FileInfo
            {
                public string Name { get; private set; }
                public string Path { get; private set; }
                public byte[] FileData { get; private set; }
                public DateTime CreatedAt { get; private set; }
                public DateTime LastModifiedAt { get; private set; }

                public FileInfo(string Name, string Path, byte[] FileData, DateTime CreatedAt, DateTime LastModifiedAt)
                {
                    this.Name = Name;
                    this.Path = Path;
                    this.FileData = FileData;
                    this.CreatedAt = CreatedAt;
                    this.LastModifiedAt = LastModifiedAt;
                }

                public string ReadFile(Encoding encoding = default)
                {
                    if(encoding == default) return Encoding.UTF8.GetString(FileData);
                    return encoding.GetString(FileData);
                }
            }

            public static FileInfo ReadFile(string FilePath)
            {
                string Name = FilePath.Split('\\').Last();
                string Path = FilePath;
                byte[] FileData = File.ReadAllBytes(Path);

                return new FileInfo(Name, Path, FileData, default, default);
            }

            public static string ReadFile(FileInfo FileInfo, Encoding encoding = default) => FileInfo.ReadFile(encoding);
        }

        public static class SystemSettings
        {

        }

        public static class UserInteraction
        {

        }
    }
}
