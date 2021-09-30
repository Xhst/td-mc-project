using System;
using System.Collections.Generic;

using Godot;


namespace TowerDefenseMC.Utils
{
    public static class FileHelper
    {
        public static HashSet<string> FilesInDirectory(string dirPath)
        {
            HashSet<string> fileNames = new HashSet<string>();
            
            Directory dir = new Directory();
            dir.Open(dirPath);
            dir.ListDirBegin();

            string fileName = dir.GetNext();
            
            while (fileName != string.Empty)
            {
                if (!fileName.BeginsWith("."))
                {
                    fileNames.Add(fileName);
                }

                fileName = dir.GetNext();
            }
            
            dir.ListDirEnd();

            return fileNames;
        }
    }
}