using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FileSync
{
    class Syncronizer
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static public bool CompareSlaveToMaster(IList<FolderAtom> list, string pathRootMaster)
        { //list = filelist from slave, pathRootMaster = root at master

            log.Info("Starting compare function @ " + list[0].FolderPath);

            try
            {
                string pathMask = list[0].FolderPath;   // pathMask will allways be set to root of master;     

                foreach (FolderAtom l in list)
                {
                    var checkpath = ExtractPath(l.FolderPath, pathMask, pathRootMaster); // final path for checking at MASTER
                    var filelist = l.FilesInAtom;
                    if (filelist.Count > 0)
                    {
                        foreach (var f in filelist)
                        {
                            var name = Path.GetFileName(f);
                            string checkAtMaster = checkpath + "\\" + name;
                            if (!File.Exists(checkAtMaster)) // if not found at Master, delete at SLave
                            {
                                DeleteFile(f);
                            }
                        }
                    }
                    // check if folder is now empty and delete if yes
                    if (Directory.GetFiles(l.FolderPath).Length == 0 && filelist.Count > 0)
                        {                        
                        //log.Info("This folder will be deleted: " + l.FolderPath);
                        DeleteFolder(l.FolderPath);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Info("Errors during Compare: " + ex.Message);
                return false;
            }           
        }

        static private void DeleteFile(string deleteThisFile)
        {
            try
            {
                log.Info("Attempting to delete file: " + deleteThisFile);
                if (File.Exists(deleteThisFile))
                { 
                    File.Delete(deleteThisFile);
                    log.Info("File deleted.");
                }
                else
                {
                    log.Info("File " + deleteThisFile + "not found.");
                }
                
            }
            catch (Exception ex)
            {
                log.Info("Could not delete file: " + deleteThisFile + " - " + ex.Message);           
            }           
        }

        static private void DeleteFolder(string deleteThisFolder)
        {
            try
            {
                log.Info("Attempting to delete folder: " + deleteThisFolder);
                if (Directory.Exists(deleteThisFolder))
                { 
                    Directory.Delete(deleteThisFolder);
                    log.Info("Folder deleted.");
                }
                else
                {
                    log.Info("Folder " + deleteThisFolder + "not found.");
                }
            }
            catch (Exception ex)
            {
                log.Info("Could not delete folder: " + deleteThisFolder + " - " + ex.Message);
            }
        }
        static private string ExtractPath(string pathSlave, string pathMask, string pathRootMaster ) 
        {
            // pathSlave = full SLAVE-path from atom, F:\temp\SYNC_TEST\SLAVE\SimpleModel
            // pathMask is root of SLAVE, ie. folderStructure[0].FolderPath
            // pathRootMaster is root of MASTER  

            if (pathSlave == pathMask) 
            {
                return pathRootMaster;  // in case it's looking at the first entry in folderStructure
            }
            return pathRootMaster + pathSlave.Substring(pathMask.Length);                 
        }
    }
}
