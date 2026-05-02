using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using SportsPlanet.Services;
using System.Windows.Controls;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace SportsPlanet.Views
{
    public partial class SuperAdminReports : Page
    {
        private Frame frame;
        private OrderService orderService;
        private UserService userService;

        public int TodayOrders { get; set; }
        public int PendingOrders { get; set; }
        public int DispatchedOrders { get; set; }
        public int TotalUsers { get; set; }

        public ISeries[] WeeklySeries { get; set; }
        public ISeries[] StatusSeries { get; set; }

        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }

        public SuperAdminReports(Frame fra)
        {
            InitializeComponent();
            frame = fra;

            HeaderControl.SetFrame(frame);
            HeaderControl.SetActive("Reports");

            orderService = new OrderService();
            userService = new UserService();

            DataContext = this;

            LoadDashboard();

        }

        private void LoadDashboard()
        {
            var orders = orderService.GetAllOrders();

            var today = DateTimeOffset.UtcNow
                        .ToOffset(TimeSpan.FromHours(5))
                        .Date;

            // ================= KPIs =================
            TodayOrders = orders.Count(o =>
                            DateTimeOffset
                                .FromUnixTimeSeconds(o.CreatedAt)
                                .ToOffset(TimeSpan.FromHours(5))
                                .Date == today);

            PendingOrders = orders.Count(o => o.Status.ToUpper() == "PENDING");
            DispatchedOrders = orders.Count(o => o.Status.ToUpper() == "DISPATCHED");

            TotalUsers = userService.GetTotalUsers();

            // ================= WEEKLY =================
            var todayPkt = DateTimeOffset.UtcNow
                        .ToOffset(TimeSpan.FromHours(5))
                        .Date;

            var last7Days = Enumerable.Range(0, 7)
                .Select(i => todayPkt.AddDays(-i))
                .Reverse()
                .ToList();

            var weeklyCounts = last7Days
                .Select(day => orders.Count(o =>
                    DateTimeOffset
                        .FromUnixTimeSeconds(o.CreatedAt)
                        .ToOffset(TimeSpan.FromHours(5))
                        .Date == day))
                .ToArray();

            WeeklySeries = new ISeries[]
            {
                new ColumnSeries<int>
                {
                    Name = "Orders",
                    Values = weeklyCounts,
                    DataLabelsSize = 14,
                    DataLabelsPosition = DataLabelsPosition.Top,
                    DataLabelsPaint = new SolidColorPaint(SKColors.White)
                }
            };

            XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = last7Days.Select(d => d.ToString("ddd")).ToArray(),
                    LabelsRotation = 0,
                    TextSize = 14
                }
            };

            YAxes = new Axis[]
            {
                new Axis
                {
                    MinStep = 1,
                    TextSize = 14
                }
            };

            // ================= PIE =================
            StatusSeries = new ISeries[]
            {
                new PieSeries<int>
                {
                    Values = new[] { PendingOrders },
                    Name = $"Pending ({PendingOrders})",
                    DataLabelsSize = 14
                },

                new PieSeries<int>
                {
                    Values = new[] { DispatchedOrders },
                    Name = $"Dispatched ({DispatchedOrders})",
                    DataLabelsSize = 14
                }
            };
        }
    }
}
