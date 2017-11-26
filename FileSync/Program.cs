using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FileSync
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /* FileSync v1.0.3
         * This app is designed to compare two directories, and maintain sync between them.
         * 
         * in this context : MASTER rules SLAVE
         * MASTER = the original, ie. Fil2/Z/Output/STS
         * SLAVE = synced, ie. Xpression1/Z/STS
         * so SLAVE is read into list, 
         * and then each file is checked if exist at MASTER */

        static void Main()
        {            
            log.Info("Running FileSync");
                        
            List<FolderAtom> list;
            string pathRootMaster = Properties.Settings.Default.pathRootMaster;
            
            if (Directory.Exists(pathRootMaster)) // check if master is online, only run if yes!
            {
                foreach (string path in Properties.Settings.Default.pathSlaves) // running through the list of slaves-dirs
                {
                    if (Directory.Exists(path))
                    {
                        GetFileList slave = new GetFileList();
                        list = slave.GetDirectoryStructure(path);
                        if (Syncronizer.CompareSlaveToMaster(list, pathRootMaster))
                        {
                            log.Info("FileSync complete for " + path);
                        }
                        else
                        {
                            log.Info("Errors ocurred. Ending application.");
                        } 
                    }
                    else
                    {
                        log.Info("Error while acessing slavepath: " + path);
                    }
                }
            }
            else
            {
                log.Info("Masterpath not found. Stopping");
            }
            log.Info("Program End.");
        }        
    }
}
