using System.Diagnostics;
using System.Globalization;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using mitoSoft.Holidays;
using Res = PillRefresh.Resource;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine(Res.Welcome);

        var (countryCode, administrativeDivision) = GetParameters(args);

        if (string.IsNullOrWhiteSpace(countryCode))
        {
            countryCode = "de";

            Console.WriteLine($"No countryCode parameter given. Assuming '{countryCode}'.");
        }

        if (string.IsNullOrWhiteSpace(administrativeDivision))
        {
            administrativeDivision = "RheinlandPfalz";

            Console.WriteLine($"No administrativeDivision parameter given. Assuming '{administrativeDivision}'.");
        }

        var pillCount = GetPillCount();

        var dosageCount = GetDosageCount();

        var daysLeft = Math.Floor(pillCount / dosageCount) - 1;

        var finalDay = DateTime.Now.Date.AddDays(daysLeft);

        Console.WriteLine(string.Format(Res.PillEnd, finalDay.ToShortDateString()));

        var reminderDay = GetReminderDay(finalDay, countryCode, administrativeDivision);

        var eventName = GetEventName();

        CreateEvent(finalDay, reminderDay, eventName);

        //Console.WriteLine("Press <enter> to exit.");
        //Console.ReadLine();
    }

    private static (string countryCode, string administrativeDivision) GetParameters(string[] args)
    {
        string countryCode = null;

        string administrativeDivision = null;

        if (args?.Length > 0)
        {
            var countryParam = args.FirstOrDefault(a =>
            {
                var lowered = a.ToLowerInvariant();

                var startsWith = lowered.StartsWith("/countrycode=")
                    || lowered.StartsWith("/country=");

                return startsWith;
            });

            countryCode = countryParam?.Split('=')[1];

            var administrativeDivisionParam = args.FirstOrDefault(a =>
            {
                var lowered = a.ToLowerInvariant();

                var startsWith = lowered.StartsWith("/administrativedivision=")
                    || lowered.StartsWith("/state=")
                    || lowered.StartsWith("/bundesland=")
                    || lowered.StartsWith("/land=")
                    || lowered.StartsWith("/kanton=")
                    || lowered.StartsWith("/bezirk=")
                    || lowered.StartsWith("/region=")
                    || lowered.StartsWith("/department=");

                return startsWith;
            });

            administrativeDivision = administrativeDivisionParam?.Split('=')[1];
        }

        return (countryCode, administrativeDivision);
    }

    private static uint GetPillCount()
    {
        bool inputSuccess;
        uint pillCount;
        do
        {
            Console.WriteLine(Res.PillsLeft);

            var pillInput = Console.ReadLine();

            inputSuccess = uint.TryParse(pillInput, out pillCount);

            if (!inputSuccess)
            {
                Console.WriteLine(Res.ErrorNotANumber);
            }
        } while (!inputSuccess);

        return pillCount;
    }

    private static double GetDosageCount()
    {
        bool inputSuccess;
        double dosageCount;
        do
        {
            Console.WriteLine(Res.PillsPerDay);

            var dosageInput = Console.ReadLine();

            inputSuccess = double.TryParse(dosageInput, NumberStyles.Float, CultureInfo.CurrentCulture, out dosageCount);

            if (!inputSuccess)
            {
                Console.WriteLine(Res.ErrorNotANumber);
            }
        } while (!inputSuccess);

        return dosageCount;
    }

    private static DateTime GetReminderDay(DateTime finalDay, string countryCode, string administrativeDivision)
    {
        bool inputSuccess;
        DateTime reminderDay = default;
        do
        {
            Console.WriteLine(Res.ReminderDays);

            var reminderInput = Console.ReadLine();

            inputSuccess = uint.TryParse(reminderInput, out var reminderCount);

            if (!inputSuccess)
            {
                Console.WriteLine(Res.ErrorNotANumber);
            }
            else
            {
                reminderDay = finalDay.AddDays(-reminderCount);

                if (reminderDay < DateTime.Now.Date)
                {
                    Console.WriteLine(Res.ErrorReminder);

                    inputSuccess = false;
                }
            }
        } while (!inputSuccess);

        var holidays = HolidaysHelper.GetHolidays(countryCode);

        if (holidays != null)
        {
            while (holidays.IsHoliday(reminderDay, administrativeDivision))
            {
                reminderDay = reminderDay.AddDays(-1);
            }
        }
        else
        {
            Console.WriteLine("No holiday calendar found. Proceeding without holidays.");
        }

        if (reminderDay.DayOfWeek == DayOfWeek.Saturday)
        {
            reminderDay = reminderDay.AddDays(-1);
        }
        else if (reminderDay.DayOfWeek == DayOfWeek.Sunday)
        {
            reminderDay = reminderDay.AddDays(-2);
        }
        else if (reminderDay.DayOfWeek == DayOfWeek.Monday) //in case the doctor takes two weeks vacation
        {
            reminderDay = reminderDay.AddDays(-3);
        }

        return reminderDay;
    }

    private static string GetEventName()
    {
        bool inputSuccess;
        string eventName;
        do
        {
            Console.WriteLine(Res.ReminderName);

            eventName = Console.ReadLine();

            inputSuccess = !string.IsNullOrWhiteSpace(eventName);

            if (!inputSuccess)
            {
                Console.WriteLine(Res.ErrorReminderName);
            }
        } while (!inputSuccess);

        return eventName!;
    }

    private static void CreateEvent(DateTime finalDay, DateTime reminderDay, string eventName)
    {
        var eventTime = new CalDateTime(finalDay.AddHours(6), TimeZoneInfo.Local.Id); //midnight causes a faulty Outlook import where the end is 24 hours before the start

        var finalDayEvent = new CalendarEvent()
        {
            Start = eventTime,
            End = eventTime,
            Description = finalDay.ToShortDateString(),
            IsAllDay = false, //with IsAllDay the reminder is not imported by Outlook
            Summary = eventName,
            Uid = Guid.NewGuid().ToString(),
        };

        finalDayEvent.AddProperty(new CalendarProperty("X-MICROSOFT-CDO-BUSYSTATUS", "FREE"));

        finalDayEvent.Alarms.Add(new Alarm()
        {
            Trigger = new Trigger()
            {
                Duration = reminderDay - finalDay,
            },
            Action = "DISPLAY",
            Description = "Reminder",
        });

        var calendar = new Ical.Net.Calendar();

        calendar.Events.Add(finalDayEvent);

        var icsFile = Path.Combine(Path.GetTempPath(), "pill.ics");

        var ics = (new CalendarSerializer()).SerializeToString(calendar);

        File.WriteAllText(icsFile, ics);

        Process.Start("explorer.exe", icsFile);
    }
}