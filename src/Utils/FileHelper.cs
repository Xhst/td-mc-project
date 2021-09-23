using System;
using System.Collections.Generic;

using Godot;


namespace TowerDefenseMC.Utils
{
    public class FileHelper
    {
        public static List<string> FilesInDirectory(string dirPath)
        {
            List<string> fileNames = new List<string>();
            
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