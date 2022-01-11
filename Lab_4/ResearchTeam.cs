using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lab_1
{
    [Serializable]
    public class ResearchTeam: Team, IEnumerable, System.ComponentModel.INotifyPropertyChanged
    {
        #region FIELDS
        
        private string topic;
        private TimeFrame duration_info;
        private System.Collections.Generic.List<Person> participant_list;
        private System.Collections.Generic.List<Paper> publication_list;
        public event PropertyChangedEventHandler PropertyChanged;
        
        #endregion

        #region CONSTRUCTORS

        public ResearchTeam(string c_topic, string c_org_n, int c_reg_n, TimeFrame c_duration_i)
        {
            topic = c_topic;
            org_name = c_org_n;
            reg_number = c_reg_n;
            duration_info = c_duration_i;
            publication_list = new List<Paper>();
            participant_list = new List<Person>();
        }
        public ResearchTeam()
        {
            topic = "Default topic";
            org_name = "Default organisation";
            reg_number = 1;
            duration_info = TimeFrame.Long;
            publication_list = new List<Paper>();
            participant_list = new List<Person>();
        }

        #endregion
        
        #region PROPERTIES

        public System.Collections.Generic.List<Paper> PublicationsList
        {
            get => publication_list;
            set
            {
                publication_list = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Publication list"));
            }
        }
        
        public System.Collections.Generic.List<Person> ParticipationsList
        {
            get => participant_list;
            set
            {
                participant_list = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Participants list"));
            }
        }
        public string Topic
        {
            get => topic;
            set
            {
                topic = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Topic"));
            } 
        }
        public int RegN
        {
            get => reg_number;
            set
            {
                reg_number = value;
            }
        }
        public TimeFrame DurationInf
        {
            get => duration_info;
            set
            {
                duration_info = value; 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Duration information"));
            }
        }
        private string Name
        {
            get => org_name;
            set
            {
                org_name = value;
            }
        }

        #endregion

        #region METHODS
        
        public override string ToString()
        {
            string d_i;
            switch (duration_info)
            {
                case TimeFrame.Year: d_i = "Year";
                    break;
                case TimeFrame.TwoYears: d_i = "Two years";
                    break;
                default: d_i = "Long";
                    break;
            }

            if (publication_list[0] != null)
            {
                string list_of_pub = "\n--1 PUBLICATION--\n" + publication_list[0];
                for (int i = 1; i < publication_list.Count; i++)
                {
                    if (publication_list[i] != null)
                    {
                        int k = i + 1;
                        list_of_pub += "\n--" + k + " PUBLICATION--\n" + publication_list[i];
                    }
                    else
                    {
                        list_of_pub += "\n--EMPTY PUBLICATION--\n";
                    }
                }

                return "Topic: " + topic + "\nOrganisation name: " + org_name + "\nRegistration number: " +
                       reg_number + "\nDuration info: " + d_i + "\nList of publications: " + list_of_pub;
            }
            else
            {
                return "Topic: " + topic + "\nOrganisation name: " + org_name + "\nRegistration number: " +
                       reg_number + "\nDuration info: " + d_i + "\nNO PUBLICATIONS";
            }
        }
        public string ToShortString()
        {
            string d_i;
            switch (duration_info)
            {
                case TimeFrame.Year: d_i = "Year";
                    break;
                case TimeFrame.TwoYears: d_i = "Two years";
                    break;
                default: d_i = "Long";
                    break;
            }
            
            return "Topic: " + topic + "\nOrganisation name: " + org_name + "\nRegistration number: " +
                   reg_number + "\nDuration info: " + d_i;
        }
        
        public ResearchTeam DeepCopy()
        {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();

            try
            {
                formatter.Serialize(stream, this);
                stream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(stream) as ResearchTeam;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public bool Save(string filename)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(stream, this);
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Load(string filename)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    if (stream == Stream.Null){
                        return false;
                    }
                    ResearchTeam rt = formatter.Deserialize(stream) as ResearchTeam;
                    this.topic = rt.Topic;
                    this.duration_info = rt.DurationInf;
                    this.participant_list = rt.ParticipationsList;
                    this.publication_list = rt.PublicationsList;
                    this.org_name = rt.OrgName;
                    this.reg_number = rt.RegN;
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool AddFromConsole()
        {
            Console.WriteLine("You are adding a new Paper object");
            Console.WriteLine(
                "Enter data in format: 'Title-AuthorsName-AuthorsSurname-AuthorsBDYear-AuthorsBDMonth-AuthorsBDDay-PublicationYear-PublicationMonth-PublicationDay'");
            Console.WriteLine("You can use '-', ',', '*' as separators");
            string[] data = Console.ReadLine().Split(new Char [] {'-' , ',' , '*'}, StringSplitOptions.RemoveEmptyEntries);
            Paper paper = new Paper();
            try
            {
                DateTime dateBD = new DateTime(Convert.ToInt32(data[3]), Convert.ToInt32(data[4]),
                    Convert.ToInt32(data[5]));
                DateTime datePub = new DateTime(Convert.ToInt32(data[6]), Convert.ToInt32(data[7]),
                    Convert.ToInt32(data[8]));
                paper = new Paper(data[0], new Person(data[1], data[2], dateBD), datePub);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            this.publication_list.Add(paper);
            return true;
        }

        public static bool Save(string filename, ResearchTeam obj)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(stream, obj);
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public static bool Load(string filename, ResearchTeam obj)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    ResearchTeam rt = formatter.Deserialize(stream) as ResearchTeam;
                    obj.Topic = rt.Topic;
                    obj.DurationInf = rt.DurationInf;
                    obj.ParticipationsList = rt.ParticipationsList;
                    obj.PublicationsList = rt.PublicationsList;
                    obj.OrgName = rt.OrgName;
                    obj.RegN = rt.RegN;
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        #endregion
        
        #region OPERATORS

        public static bool operator==(ResearchTeam rt1, ResearchTeam rt2)
        {
            return rt1.Name == rt2.Name && rt1.Topic == rt2.Topic && rt1.DurationInf == rt2.DurationInf;
        }

        public static bool operator !=(ResearchTeam rt1, ResearchTeam rt2)
        {
            return !(rt1 == rt2);
        }

        #endregion

        
        
        
        
        

        #region UNUSED FROM PREVIOUS LABS
        
        public IEnumerable LastNYearsPublications(int n)
        {
            for (int i = 0; i < publication_list.Count; i++)
            {
                Paper publication = publication_list[i] as Paper;
                if (DateTime.Now.Year - publication.publication_date.Year <= n)
                {
                    yield return publication;
                }
            }
        }
        
        public IEnumerator GetEnumerator()
        {
            return new ResearchTeamEnumerator(participant_list, publication_list);
        }
        
        public int HasPublication(Person pers)
        {
            int count = 0;
            for (int i = 0; i < publication_list.Count; i++)
            {
                Paper publication = publication_list[i] as Paper;
                if (pers == publication.author)
                {
                    count++;
                }
            }
            return count;
        }
        public Paper FindRecent(System.Collections.Generic.List<Paper> p_list)
        {
            if (p_list.Count > 0)
            {
                Paper recent_p = p_list[0] as Paper;
                int max = 0;
                
                for (int i = 1; i < p_list.Count; i++)
                {
                    Paper current = p_list[i] as Paper;
                    if (current.publication_date > recent_p.publication_date)
                    {
                        recent_p = current;
                    }
                }

                ref Paper reference = ref recent_p;
                
                return reference;
            }
            else
            {
                return null;
            }
        }

        void SortByPublicationDate(List<Paper> list)
        {
            list.Sort();
        }
        
        void SortByPublicationTitle(List<Paper> list)
        {
            list.Sort(new Paper());
        }
        
        void SortByPublicationAuthorSurname(List<Paper> list)
        {
            list.Sort(new AuthorSurnameComparer());
        }
        
        public IEnumerable GetAuthorsWithMoreThanNPublications(int n)
        {
            for (int i = 0; i < participant_list.Count; i++)
            {
                Person pers = participant_list[i] as Person;
                if (HasPublication(pers) > n)
                {
                    yield return pers;
                }
            }
        }

        public IEnumerable GetPublicationsForLastYear()
        {
            return this.LastNYearsPublications(1);
        }
        
        public IEnumerable GetAuthorsWithNoPublications()
        {
            for (int i = 0; i < participant_list.Count; i++)
            {
                Person pers = participant_list[i] as Person;
                if (!Convert.ToBoolean(HasPublication(pers)))
                {
                    yield return pers;
                }
            }
        }
        
        public Paper RecentPublication
        {
            get
            {
                Paper reference =  FindRecent(publication_list);
                return reference;
            }
        }

        public bool this[TimeFrame frame]
        {
            get => duration_info == frame;
        }

        public void AddPapers(params Paper[] papers)
        {
            for (int i = 0; i < papers.Length; i++)
            {
                publication_list.Add(papers[i]);
            }
        }
        
        public void AddParticipants(params Person[] pers)
        {
            for (int i = 0; i < pers.Length; i++)
            {
                participant_list.Add(pers[i]);
            }
        }
        
        static void Swap(ref Paper el1, ref Paper el2)
        {
            var temp = el2;
            el2 = el1;
            el1 = el2;

        }
        
       /* public virtual object DeepCopy()
        {
            ResearchTeam new_team = new ResearchTeam(this.Topic, this.OrgName, this.RegN, this.DurationInf);
            new_team.participant_list.InsertRange(0, this.participant_list);
            new_team.publication_list.InsertRange(0, publication_list);
            return new_team;
        } */

        public Team TeamObject
        {
            get
            {
                Team new_team = new Team(this.org_name, this.reg_number);
                return new_team;
            }
            set
            {
                org_name = value.OrgName;
                reg_number = value.RegNumber;
            }
        }

        #endregion
    }
}