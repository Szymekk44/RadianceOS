using Cosmos.System.Graphics;
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

                public FileInfo(string Path, byte[] FileData, DateTime? CreatedAt = null, DateTime? LastModifiedAt = null)
                {
                    this.Name = Path.Split('/').Last();
                    this.Path = Path;
                    this.FileData = FileData;
                    this.CreatedAt = (DateTime)CreatedAt;
                    this.LastModifiedAt = (DateTime)LastModifiedAt;
                }

                public FileInfo(string Name, string Path, byte[] FileData, DateTime? CreatedAt = null, DateTime? LastModifiedAt = null)
                {
                    this.Name = Name;
                    this.Path = Path;
                    this.FileData = FileData;
                    this.CreatedAt = (DateTime)CreatedAt;
                    this.LastModifiedAt = (DateTime)LastModifiedAt;
                }

                /// <summary>
                /// The function to read the current file. (You don't need to use this)
                /// </summary>
                /// <param name="encoding">The encoding to use</param>
                /// <returns>The data as a string</returns>
                public string ReadFile(Encoding encoding = default)
                {
                    if(encoding == default) return Encoding.UTF8.GetString(FileData);
                    return encoding.GetString(FileData);
                }
            }

            /// <summary>
            /// Read a file
            /// </summary>
            /// <param name="FilePath">The path of the file</param>
            /// <returns>The FileInfo for the file to read</returns>
            public static FileInfo ReadFile(string FilePath)
            {
                string Name = FilePath.Split('\\').Last();
                string Path = FilePath;
                byte[] FileData = File.ReadAllBytes(Path);

                return new FileInfo(Name, Path, FileData, default, default);
            }

            public static string ReadFile(FileInfo FileInfo, Encoding encoding = default) => FileInfo.ReadFile(encoding);

            /// <summary>
            /// Save a file as bytes
            /// </summary>
            /// <param name="FilePath">The path of the file</param>
            /// <param name="Data">The data in a byte array</param>
            /// <returns>The FileInfo for the new file</returns>
            public static FileInfo SaveFile(string FilePath, byte[] Data)
            {
                FileInfo fileInfo = new FileInfo(FilePath, Data);
                File.WriteAllBytes(FilePath, Data);
                return fileInfo;
            }

            /// <summary>
            /// Save a file as a string
            /// </summary>
            /// <param name="FilePath">The path of the file</param>
            /// <param name="Data">The data in a string form</param>
            /// <param name="encoding">The encoding to use (The default is UTF8)</param>
            /// <returns>The FileInfo for the new file</returns>
            public static FileInfo SaveFile(string FilePath, string Data, Encoding encoding = default) => SaveFile(FilePath, encoding == default ? Encoding.UTF8.GetBytes(Data) : encoding.GetBytes(Data));
        }

        public static class SystemSettings
        {
            
        }

        public static class UserInteraction
        {
            
        }
    }
}
