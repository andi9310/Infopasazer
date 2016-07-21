using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfopasazerApp.TrainsServiceReference;

namespace InfopasazerApp
{
    class AppTrain: Train
    {
        public string Color { get; set; }

        public AppTrain(Train t)
        {
            Name = t.Name;
            Id = t.Id;
            Company = t.Company;
            CompanyUrl = t.CompanyUrl;
            Source = t.Source;
            Destination = t.Destination;
            Delay = t.Delay;
            Date = t.Date;
            PlannedTime = t.PlannedTime;
            var delay = int.Parse(Delay.Split(' ')[0]);
            if (delay < 1)
            {
                Color = "";
            }
            else if (delay < 5)
            {
                Color = "Green";
            }
            else if (delay < 20)
            {
                Color = "Yellow";
            }
            else if (delay < 60)
            {
                Color = "Orange";
            }
            else
            {
                Color = "Red";
            }
        }
    }
}
