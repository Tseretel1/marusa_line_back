namespace marusa_line.Dtos.ControlPanelDtos.Dashboard
{
    public class DashboardStatsByYear
    {
        public List<DashboardStatsByMonths> statsByMonth {get;set;}
        public DashboardYearSum YearStat { get; set; }
    }
    public class DashboardStatsByMonths
    {
        public int MonthNumber { get; set; }
        public int OrderCount { get; set; }
        public int TotalAmount { get; set; }
    }

    public class DashboardYearSum
    {
        public int OrderCount { get; set; }
        public int TotalAmount { get; set; }
    }

}
