using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace Lab_1
{
    class Program
    {
        static void Main(string[] args)
        {
            #region ---TASK 1---
            Console.WriteLine("-------------TASK 1-------------\n");
            ResearchTeam researchTeam = new ResearchTeam("Topic 1", "Name 1", 345, TimeFrame.Long);
            for (int i = 0; i < 3; i++)
            {
                researchTeam.ParticipationsList.Add(new Person($"name {i+1}", $"Surname {i+1}", new DateTime(2000+i, i+2, i+2)));
            }
            for (int i = 0; i < 3; i++)
            {
                researchTeam.PublicationsList.Add(new Paper($"Title {i+1}", researchTeam.ParticipationsList[i], new DateTime(2000+2*i, i+3, i+3)));
            }

            //Console.WriteLine(researchTeam.ToString());

            ResearchTeam researchTeamCopy = researchTeam.DeepCopy();
            Console.WriteLine("-------------RESEARCH TEAM OBJECT-------------\n");
            Console.WriteLine(researchTeam);
            Console.WriteLine("\n\n-------------RESEARCH TEAM COPY OBJECT-------------");
            Console.WriteLine(researchTeamCopy);
            #endregion

            #region ---TASK 2---

            Console.WriteLine("Enter file name without extension");
            string fileName = Console.ReadLine();
            string workingDir = Environment.CurrentDirectory;
            string projectDir = Directory.GetParent(workingDir).Parent.Parent.FullName;
            fileName = projectDir + "/" + fileName + ".bin";

            FileInfo file = new FileInfo(fileName);
            if (file.Exists)
            {
                Console.WriteLine("File exists - load from file");
                researchTeam.Load(fileName);
            }
            else
            {
                Console.WriteLine("File doesn't exists - new file will be created");
                File.Create(fileName).Close();
            }

            #endregion

            #region ---TASK 3---

            Console.WriteLine("-------------TASK 3-------------\n");
            Console.WriteLine(researchTeam);

            #endregion

            #region ---TASK 4---

            researchTeam.AddFromConsole();
            researchTeam.Save(fileName);
            Console.WriteLine("-------------TASK 4-------------\n");
            Console.WriteLine(researchTeam);

            #endregion

            #region ---TASK 5---

            ResearchTeam.Load(fileName, researchTeam);
            researchTeam.AddFromConsole();
            ResearchTeam.Save(fileName, researchTeam);
            
            #endregion
            
            #region ---TASK 6---

            Console.WriteLine("-------------TASK 6-------------\n");
            Console.WriteLine(researchTeam);

            #endregion

        }
    }
}
