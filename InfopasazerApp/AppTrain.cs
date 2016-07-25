using InfopasazerApp.TrainsServiceReference;

namespace InfopasazerApp
{
    public class AppTrain: Train
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
            if (delay < 5)
            {
                Color = "Green";
            }
            else if (delay < 20)
            {
                Color = "Yellow";
            }
            else if (delay < 60)
            {
                Color = "Salmon";
            }
            else
            {
                Color = "Coral";
            }
        }


    }
}
