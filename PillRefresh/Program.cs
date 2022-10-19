using System.Diagnostics;
using System.Globalization;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Res = PillRefresh.Resource;

Console.WriteLine(Res.Welcome);

var pillCount = GetPillCount();

var dosageCount = GetDosageCount();

var daysLeft = Math.Floor(pillCount / dosageCount);

var finalDay = DateTime.Now.Date.AddDays(daysLeft);

Console.WriteLine(string.Format(Res.PillEnd, finalDay.ToShortDateString()));

var reminderDay = GetReminderDay(finalDay);

var eventName = GetEventName();

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

CreateEvent(finalDay, reminderDay, eventName);

static uint GetPillCount()
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

static double GetDosageCount()
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

static DateTime GetReminderDay(DateTime finalDay)
{
    bool inputSuccess;
    DateTime reminderDay = default;
    do
    {
        Console.WriteLine(Res.ReminderDays);

        var reminderInput = Console.ReadLine();

        inputSuccess = uint.TryParse(reminderInput, out uint reminderCount);

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

    return reminderDay;
}

static string GetEventName()
{
    bool inputSuccess;
    string? eventName;
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

static void CreateEvent(DateTime finalDay, DateTime reminderDay, string eventName)
{
    var calendar = new Ical.Net.Calendar();

    var eventTime = new CalDateTime(finalDay.AddHours(6)); //midnight causes a faulty Outlook import where the end is 24 hours before the start

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

    calendar.Events.Add(finalDayEvent);

    finalDayEvent.Alarms.Add(new Alarm()
    {
        Trigger = new Trigger()
        {
            Duration = reminderDay - finalDay,
        },
        Action = "DISPLAY",
        Description = "Reminder",
    });

    var serializer = new CalendarSerializer();

    var tempFile = Path.Combine(Path.GetTempPath(), "pill.ics");

    File.WriteAllText(tempFile, serializer.SerializeToString(calendar));

    Process.Start("explorer.exe", tempFile);
}