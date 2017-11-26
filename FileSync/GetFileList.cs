using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FileSync
{
    class GetFileList
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private FolderAtom folderAtom;
        private List<FolderAtom> folderStructure = new List<FolderAtom>();

        public List<FolderAtom> GetDirectoryStructure(string path)
        {
            try     // recursively find all folders + files below starting directory.
            {
                //log.Info("Getting directory: " + path);   //not included - log-file inflation if used
                
                folderStructure.Add(GetFolderAtom(path));                
                if (folderAtom.FoldersInAtom.Count > 0)
                {
                    foreach (var folder in folderAtom.FoldersInAtom)
                    {
                        GetDirectoryStructure(folder);
                    }
                }                
                return folderStructure;
            }
            catch (Exception)
            {
                log.Info("Error occurred during directory-scan @ path " + path );
                return folderStructure;
            }
        }

        private FolderAtom GetFolderAtom(string path) // atom = one folder and its dir+file content
        {
            folderAtom = new FolderAtom();
            folderAtom.FilesInAtom = Directory.GetFiles(path);
            folderAtom.FoldersInAtom = Directory.GetDirectories(path);
            folderAtom.FolderPath = path;
            return folderAtom;
        }

        public void OutputToConsole(List<FolderAtom> list)   // inactive, only used for debug
        {            
            Console.BufferHeight = 2000;
            foreach (FolderAtom l in list)
            { 
                Console.WriteLine("path: " + l.FolderPath);
                Console.WriteLine("Folders: ");
                PrintIt(l.FoldersInAtom);
                Console.WriteLine("Files: ");
                PrintIt(l.FilesInAtom);
                Console.WriteLine("\n");
            }
            Console.ReadLine();
        }   

        private void PrintIt(IList<string> list)    // inactive, only used for debug
        {
            if (list.GetType() == typeof(string[]))
            foreach (var l in list)
            {
                Console.WriteLine(l);
            }
            if (list.GetType() == typeof(string))
            {
                Console.WriteLine(list);
            }
        }   
    }
}
