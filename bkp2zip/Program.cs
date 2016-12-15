using System;
using Ionic.Zip;
using System.IO;
using System.Configuration;

namespace Bkp2Zip
{
    class Program
    {
        static void Main()
        {
            string dtDate = DateTime.Now.ToString("yyyyMMdd");
            string dtHourMinSec = DateTime.Now.ToString("HHmmss");

            string strSourcePath = ConfigurationManager.AppSettings["SourcePath"];
            string strBackupPath = ConfigurationManager.AppSettings["BackupPath"];
            string strBkpFilePath = strBackupPath + "\\" + dtDate + ".zip";

            bool securityCopy = Convert.ToBoolean(ConfigurationManager.AppSettings["SecurityCopy"]);

            bool rotation = Convert.ToBoolean(ConfigurationManager.AppSettings["Rotation"]);
            int rotationType = Convert.ToInt32(ConfigurationManager.AppSettings["RotationType"]);
            
            
            ZipFile zip = new ZipFile();
            zip.AddDirectory(strSourcePath);
            if (securityCopy) 
            {
                if (File.Exists(strBkpFilePath))
                {
                    string strBkpFile = strBkpFilePath.Replace(".zip", "");
                    File.Move(strBkpFilePath, strBkpFile + dtHourMinSec + ".zip");
                }
            }
            zip.Save(strBkpFilePath);

            string[] zipList = Directory.GetFiles(strBackupPath);

            if(rotation)
            {
                foreach (string zipFile in zipList)
                {
                    DateTime dtZip = File.GetLastWriteTime(zipFile);
                    DateTime dtRotate = new DateTime();

                    switch (rotationType)
                    {
                        case 1: 
                            dtRotate = DateTime.Now.AddDays(-7);
                            break;
                        case 2:  
                            dtRotate = DateTime.Now.AddMonths(-1);
                            break;
                        case 3:  
                            dtRotate = DateTime.Now.AddYears(-1);
                            break;
                    }
                    if (dtZip < dtRotate)
                    {
                        File.Delete(zipFile);
                    }
                }
            }

        }
    }

}
