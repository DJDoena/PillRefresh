using System.Diagnostics;
using System.Globalization;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;

Console.WriteLine("Welcome to pill calculation!");

bool inputSuccess;
uint pillCount;
do
{
    Console.WriteLine("How many pills have you left?");

    var pillInput = Console.ReadLine();

    inputSuccess = uint.TryParse(pillInput, out pillCount);

    if (!inputSuccess)
    {
        Console.WriteLine("Not a number!");
    }
} while (!inputSuccess);

double dosageCount;
do
{
    Console.WriteLine("How many do you need to take per day?");

    var dosageInput = Console.ReadLine();

    inputSuccess = double.TryParse(dosageInput, NumberStyles.Float, CultureInfo.CurrentCulture, out dosageCount);

    if (!inputSuccess)
    {
        Console.WriteLine("Not a number!");
    }
} while (!inputSuccess);


var daysLeft = Math.Floor(pillCount / dosageCount);

var finalDay = DateTime.Now.Date.AddDays(daysLeft);

Console.WriteLine($"You run out of pills on {finalDay.ToShortDateString()}");

uint reminderCount;
DateTime reminderDay = default;
do
{
    Console.WriteLine("How many days before that do you want a reminder?");

    var reminderInput = Console.ReadLine();

    inputSuccess = uint.TryParse(reminderInput, out reminderCount);

    if (!inputSuccess)
    {
        Console.WriteLine("Not a number!");
    }
    else
    {
        reminderDay = finalDay.AddDays(-reminderCount);

        if (reminderDay < DateTime.Now.Date)
        {
            Console.WriteLine("Reminder is older tan today!");

            inputSuccess = false;
        }
    }
} while (!inputSuccess);

string? eventName;
do
{
    Console.WriteLine("What should be the name of the reminder?");

    eventName = Console.ReadLine();

    inputSuccess = !string.IsNullOrWhiteSpace(eventName);

    if (!inputSuccess)
    {
        Console.WriteLine("No name was given!");
    }
} while (!inputSuccess);

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

CreateEvent(finalDay, reminderDay, eventName!);

static void CreateEvent(DateTime finalDay, DateTime reminderDay, string eventName)
{
    var calendar = new Ical.Net.Calendar();

    //var finalDayEvent = new CalendarEvent()
    //{
    //    Start = new CalDateTime(finalDay),
    //    End = new CalDateTime(finalDay),
    //    Description = finalDay.ToShortDateString(),
    //    IsAllDay = true,
    //    Summary = eventName,
    //};

    //calendar.Events.Add(finalDayEvent);

    //finalDayEvent.Alarms.Add(new Alarm()
    //{
    //    Trigger = new Trigger()
    //    {
    //        Duration = reminderDay - finalDay,
    //    },
    //    Action = "DISPLAY",
    //    Description = "Reminder",
    //});

    var reminderDayEvent = new CalendarEvent()
    {
        Start = new CalDateTime(reminderDay),
        End = new CalDateTime(reminderDay),
        Description = finalDay.ToShortDateString(),
        IsAllDay = true,
        Summary = eventName,
    };

    calendar.Events.Add(reminderDayEvent);

    reminderDayEvent.Alarms.Add(new Alarm()
    {
        Trigger = new Trigger()
        {
            Duration = TimeSpan.Zero,
        },
        Action = "DISPLAY",
        Description = "Reminder",
    });

    var serializer = new CalendarSerializer();

    var tempFile = Path.Combine(Path.GetTempPath(), "pill.ics");

    File.WriteAllText(tempFile, serializer.SerializeToString(calendar));

    Process.Start("explorer.exe", tempFile);
}