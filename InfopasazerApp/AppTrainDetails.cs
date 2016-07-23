using InfopasazerApp.TrainsServiceReference;

namespace InfopasazerApp
{
    class AppTrainStation: TrainStation
    {
        public string Color { get; set; }
        public string BorderThickness { get; set; } = "0";

        public AppTrainStation(TrainStation t)
        {
            Name = t.Name;
            ArrivalDelay = t.ArrivalDelay;
            DepartureDelay = t.DepartureDelay;
            PlanedArrival = t.PlanedArrival;
            PlannedDeparture = t.PlannedDeparture;
        }

        public void SetColor(bool shouldShowLightColor)
        {
            int delay;
            if (!int.TryParse(DepartureDelay.Split(' ')[0], out delay))
            {
                if (!int.TryParse(ArrivalDelay.Split(' ')[0], out delay))
                {
                    return;
                }
            }
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
            if (shouldShowLightColor)
            {
                Color = "Light" + Color;
            }
        }
    }
}
