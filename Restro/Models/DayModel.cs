using System;
namespace Restro.Models
{
    public class DayModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan? From { get; set; }
        public TimeSpan? To { get; set; }
        public bool? IsWeeknd { get; set; }
        public bool? IsRoundClock { get; set; }
        public int PlaceModelId { get; set; }
        public PlaceModel PlaceModel { get; set; }
    }
}