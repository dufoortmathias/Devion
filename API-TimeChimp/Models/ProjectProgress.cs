namespace Api.Devion.Models
{
    public class ProjectProgress
    {
        public string? PROJECT {  get; set; }
        public string? SUBPROJECT { get; set; }
        public string? UURCODE { get; set; }
        public float? VCC_UREN { get; set; }
        public float? NCC_UREN { get; set; }
        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }

        public float PlannedHoursLeft()
        {
            if (VCC_UREN == null)
            {
                throw new Exception($"Progress of project: {PROJECT}{SUBPROJECT} doesn't have hours in precalculation");
            }

            return VCC_UREN.Value - (NCC_UREN ?? 0);
        }

        public int DaysLeft()
        {
            if (END_DATE == null)
            {
                throw new Exception($"Progress of project: {PROJECT}{SUBPROJECT} doesn't have an end date");
            }

            if (START_DATE == null)
            {
                throw new Exception($"Progress of project: {PROJECT}{SUBPROJECT} doesn't have a start date");
            }

            return (END_DATE.Value.Date - START_DATE.Value.Date).Days;
        }
    }
}
