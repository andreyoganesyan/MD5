using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MD5
{
    class DuplicatesFinder
    {
        public Dictionary<string, List<string>> findDuplicates(string folderPath)
        {
            Dictionary<string, List<string>> duplicates = new Dictionary<string, List<string>>();
            Dictionary<string, string> hashes = findHashes(folderPath);
            foreach (string fileName in hashes.Keys)
            {
                if (duplicates.Keys.Contains(hashes[fileName]))
                {
                    duplicates[hashes[fileName]].Add(fileName);
                }
                else
                {
                    duplicates.Add(hashes[fileName], new List<string>());
                    duplicates[hashes[fileName]].Add(fileName);
                }
            }

            return duplicates;
        }

        Dictionary<string, string> findHashes(string folderPath)
        {
            Dictionary<string, string> hashes = new Dictionary<string, string>();
            MD5HashMaker md5HashMaker = new MD5HashMaker();

            string[] filePaths = Directory.GetFiles(folderPath);
            string[] fileNames = new string[filePaths.Length];
            for (int i = 0; i < filePaths.Length; i++)
            {
                fileNames[i] = filePaths[i].Substring(filePaths[i].LastIndexOf('\\')+1);
            }

            for (int i = 0; i < filePaths.Length; i++)
            {
                
                try
                {
                    Stream file = File.OpenRead(filePaths[i]);
                    hashes.Add(fileNames[i], md5HashMaker.GetHash(file));
                    file.Close();
                }
                catch (IOException)
                {
                    continue;
                }
            }

            return hashes;
        }


    }
}
