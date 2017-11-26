using System;
using System.Collections.Generic;
using System.Text;

namespace FileSync
{
    class FolderAtom    // each folder-atom contains names of files and subfolders + its own path
    { 
        public string FolderPath;               // path of folder
        public IList<string> FoldersInAtom;     // all directories in folder
        public IList<string> FilesInAtom;       // all files in folder
    }
}
