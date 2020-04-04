using System;
using System.Collections.Generic;
using System.Linq;

namespace OsuRandomizerTool
{
    class DataBase
    {
        Random _rnd = new Random();
        public List<string> Star1 = new List<string>();
        public List<string> Star2 = new List<string>();
        public List<string> Star3 = new List<string>();
        public List<string> Star4 = new List<string>();
        public List<string> Star5 = new List<string>();
        public List<string> Star6 = new List<string>();
        public List<string> Star7 = new List<string>();
        public List<string> Star8 = new List<string>();
        public List<string> Star9 = new List<string>();
        public List<string> Star10 = new List<string>();
        public List<string> Requests = new List<string>();

        public void SetRandomMap(string beatmap, int star)
        {
            switch (star)
            {
                case 1:
                    Star1.Add(beatmap);
                    break;
                case 2:
                    Star2.Add(beatmap);
                    break;
                case 3:
                    Star3.Add(beatmap);
                    break;
                case 4:
                    Star4.Add(beatmap);
                    break;
                case 5:
                    Star5.Add(beatmap);
                    break;
                case 6:
                    Star6.Add(beatmap);
                    break;
                case 7:
                    Star7.Add(beatmap);
                    break;
                case 8:
                    Star8.Add(beatmap);
                    break;
                case 9:
                    Star9.Add(beatmap);
                    break;
                case 10:
                    Star10.Add(beatmap);
                    break;
            }
        }
        public String GetRandomMap(int star)
        {
            int random = 0;
            switch (star)
            {
                
                case 1:
                    random = _rnd.Next(0, Star1.Count());
                    return Star1[random];
                case 2:
                    random = _rnd.Next(0, Star2.Count());
                    return Star2[random];
                case 3:
                    random = _rnd.Next(0, Star3.Count());
                    return Star3[random];
                case 4:
                    random = _rnd.Next(0, Star4.Count());
                    return Star4[random];
                case 5:
                    random = _rnd.Next(0, Star5.Count());
                    return Star5[random];
                case 6:
                    random = _rnd.Next(0, Star6.Count());
                    return Star6[random];
                case 7:
                    random = _rnd.Next(0, Star7.Count());
                    return Star7[random];
                case 8:
                    random = _rnd.Next(0, Star8.Count());
                    return Star8[random];
                case 9:
                    random = _rnd.Next(0, Star9.Count());
                    return Star9[random];
                case 10:
                    random = _rnd.Next(0, Star10.Count());
                    return Star10[random];
                default:
                    return "Error";
            }
        }

        public void UserRequest(string request)
        {
            Requests.Add(request);
        }
    }
}
