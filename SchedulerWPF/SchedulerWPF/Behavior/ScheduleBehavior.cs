using Microsoft.Xaml.Behaviors;
using Syncfusion.UI.Xaml.Scheduler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace SchedulerWPF
{
    public class ScheduleBehavior : Behavior<Grid>
    {
        Grid grid;
        SfScheduler schedule;
        protected override void OnAttached()
        {
            grid = this.AssociatedObject as Grid;
            schedule = grid.Children[0] as SfScheduler;
            schedule.QueryAppointments += OnScheduleQueryAppointments;
        }

        private async void OnScheduleQueryAppointments(object sender, QueryAppointmentsEventArgs e)
        {
            schedule.ShowBusyIndicator = true;
            await Task.Delay(1000);
            schedule.ItemsSource = this.GenerateSchedulerAppointments(e.VisibleDateRange);
            schedule.ShowBusyIndicator = false;
        }

        private IEnumerable GenerateSchedulerAppointments(DateRange dateRange)
        {
            var brush = new ObservableCollection<SolidColorBrush>();
            brush.Add(new SolidColorBrush(Color.FromArgb(0xFF, 0xA2, 0xC1, 0x39)));
            brush.Add(new SolidColorBrush(Color.FromArgb(0xFF, 0xD8, 0x00, 0x73)));
            brush.Add(new SolidColorBrush(Color.FromArgb(0xFF, 0x1B, 0xA1, 0xE2)));
            brush.Add(new SolidColorBrush(Color.FromArgb(0xFF, 0xE6, 0x71, 0xB8)));
            brush.Add(new SolidColorBrush(Color.FromArgb(0xFF, 0xF0, 0x96, 0x09)));
            brush.Add(new SolidColorBrush(Color.FromArgb(0xFF, 0x33, 0x99, 0x33)));
            brush.Add(new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xAB, 0xA9)));
            brush.Add(new SolidColorBrush(Color.FromArgb(0xFF, 0xE6, 0x71, 0xB8)));

            var subjectCollection = new ObservableCollection<string>();
            subjectCollection.Add("Business Meeting");
            subjectCollection.Add("Conference");
            subjectCollection.Add("Medical check up");
            subjectCollection.Add("Performance Check");
            subjectCollection.Add("Consulting");
            subjectCollection.Add("Project Status Discussion");
            subjectCollection.Add("Client Meeting");
            subjectCollection.Add("General Meeting");
            subjectCollection.Add("Yoga Therapy");
            subjectCollection.Add("GoToMeeting");
            subjectCollection.Add("Plan Execution");
            subjectCollection.Add("Project Plan");

            Random random = new Random();
            int daysCount = (dateRange.ActualEndDate - dateRange.ActualStartDate).Days;
            var appointments = new ObservableCollection<ScheduleAppointment>();

            for (int i = 0; i < 50; i++)
            {
                var startTime = dateRange.ActualStartDate.AddDays(random.Next(0, daysCount + 1)).AddHours(random.Next(0, 24));
                appointments.Add(new ScheduleAppointment
                {
                    StartTime = startTime,
                    EndTime = startTime.AddHours(1),
                    Subject = subjectCollection[random.Next(0, subjectCollection.Count)],
                    AppointmentBackground = brush[random.Next(0, brush.Count)],
                });
            }
            return appointments;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            schedule.QueryAppointments -= OnScheduleQueryAppointments;
            grid = null;
            schedule = null;
        }
    }
}
