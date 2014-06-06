The IIS logs are an invaluable way to get to know your web application and 
your end users once it’s in production. For us, they are also the first stop 
when a web application has performance problems. Therefore, having a tool to 
analyze IIS logs is in invaluable asset in our bag of tricks. There are numerous 
commercial tools out there that do a reasonable job at analyzing IIS logs and 
often providing (more or less) great visual displays while doing it. There are 
some things wrong with commercial tools though:

They cost money (duh!).
We want to be able to add specific queries in a language familiar to us, 
should the need arise. We want to be able to add additional queries in Linq.
Usually these tools don’t have any intrinsic knowledge about 
SharePoint.
Since we came to the conclusion that having a tool that helps to analyze IIS 
log files is essential and that we weren’t fully happy with existing options, we 
decided to build such a tool ourselves which primarily gives us ultimate control 
when it comes to adding new overviews.

We call the tool the SharePoint Flavored WebLog Reader (sfwr.exe) and it can 
be used to analyze any IIS log file or batch of IIS log files. On top of that, 
it has specific knowledge about SharePoint, which adds a SharePoint flavor to 
the tool in the form of specific overviews that only make sense within a 
SharePoint context.

The SharePoint Flavored WebLog Reader has the following advantages:

It’s free.
It’s easy to use.
It contains a considerable amount of overviews.
It has a certain SharePoint flavor to it (because it also includes overviews 
specifically targeted towards SharePoint).
It leverages parallel programming techniques and is therefore pretty fast 
calculating the various  reports.
There are some drawbacks as well:

It’s not a rich tool visually.
Support is limited. Although, if you run into problems we’d be interested in 
the IIS log files that cause them and we’d be probably interested to look into 
them and improve the tool.
At this point, you won’t be able to extend the tool. We’d sure be interested 
in hearing requests for new overviews and we’d probably add them too.
It supports a little over (and is tested using) 2.3 million log entries. 
After that, you’ll have to divide the batch of log files in pieces.
So, what can it do?

The SharePoint Flavored WebLog Reader provides the following overviews:

The average request time per URI.
The max request time per URI.
The min request time per URI.
The average request time per InfoPath URI.
The max request time per InfoPath URI.
The min request time per InfoPath URI.
The average request time per Report Server URI.
The max request time per Report Server URI.
The min request time per Report Server URI.
Browser percentage.
Dead links.
Failed pages.
Failed InfoPath pages.
Most busy days of the week.
Most requested pages.
Requested pages per day.
Percentage error page requests.
Requests per hour per day.
Requests per hour.
Requests per user.
Requests per user per month.
Requests per user per week.
Slowest requests.
Slowest failed requests.
Slowest successful requests.
Slowest requests per URI.
Top requests per hour.
Top visitors.
Traffic per day in MB.
Traffic per week in MB.
Unique visitors.
Unique visitors per day.
Unique visitors per week.
Unique visitors per month.
Searches per user.
Slowest requests for a specific user.
Where can I get it?

You can download it here at the TechNet Gallery.

How do I use it?

Open a command prompt, navigate to the folder where sfwr is stored, and 
type:

sfwr.exe [items] [log path]

For example:

sfwr.exe 100 c:\temp\logs

If you also want to generate reports targeted towards a specific user, do the following:

sfwr.exe [items] [log path] [complete or partial user name]

For example:

sfwr.exe 100 “c:\temp\logs” margriet

Or

sfwr.exe 100 “c:\temp\logs” loisandclark\margrietbruggeman

What do I need to remember about the tool?

It requires the presence of the .NET 4 framework.
Depending on the IIS log settings, you may not be able to see all reports. 
For example, if you don’t record the bytes sent and bytes received, you can’t 
see the Traffic per day and per week in MB overviews. If you use the default IIS 
settings, you’ll be able to see every overview.
It outputs the results in a file called Overviews.txt. The file is saved in 
the same directory as sfwr.exe.
The sfwr tool only processes files that have an extension of .log, and 
ignores all other extensions.
The max limit of log items that is tested is 2.3 million. If you cross that 
boundary, you might experience memory exceptions (don’t worry, sfwr displays the 
current count for you, so you can see if and where you cross the line). This 
seems to be caused by our extensive use of the Dynamic Language Runtime (DLR), 
but for us, there wasn’t really a need to research this issue and see if we 
could boost the tool to even higher numbers.
The memory structure holding the IIS log entries is predefined at 2.4 
million items. After that, memory reallocation may cause a time delay and maybe 
additional memory issues.
How does it do it?

Since we realize that everyone  could define different IIS logging settings, 
we didn’t want to predefine a log structure that we depend on. Instead, we use 
the log header that every log file has to determine the structure, and use the 
Dynamic Language Runtime (DLR) ExpandoObject to create the structure 
dynamically.

Please note: This flexibility came at a cost. It seems that 
the extensive use of millions of expando objects causes the limit of (a little 
over) 2.3 million log entry items. This doesn’t seem to happen in a version that 
uses predefined structures. However, we feel this implementation has superior 
flexibility and allows us to generate overviews on a generate-if-possible basis 
and is therefore more generically applicable. Besides, 2.3 million items is a 
huge amount, so we decided to stick to the DLR approach.

At a high level, we do this:

Determine the structure of the IIS log file and store that in memory (in 
lineEntries).
Create an expando object that represents a log entry and add all properties 
in the IIS log file to it.
Assign values found in the log entry to the Expando object.
The code goes like this:

dynamic exp = new ExpandoObject();
for (int i = 
0; i < lineEntries.Length; i++)
{
    IDictionary<string, object> 
dict = exp;

    // If you were wondering at the strange guard 
clause below,

    // VS.NET Code Contracts made us do it!!!
    
if (dict == null) throw new SfwrException("No dictionary");

    if (PropertyNames.Count() != 
lineEntries.Count()) throw new SfwrException("Property names are different from 
line entries");
    dict[PropertyNames[i]] = lineEntries[i];
}

W3CImporter.Log.Add(exp);

Later on, we use these Expando objects to generate our overviews. The cool 
thing about Expando objects is that you can use the dictionary keys as actual 
property names. So, the following is perfectly valid:

dynamic exp = new ExpandoObject();

IDictionary<string, object> dict = 
exp;

dict[“Test”] = “Hello Expando!”;

Console.WriteLine(exp.Test);

This is a pretty cool feature that really helps us out in this tool. Now that 
we have a way to create a full-blown DTO object with properties on them and everything, 
we can use them to create overviews via Linq (or Plinq). Some of these queries 
are pretty straightforwards, others are niftier, such as the next one creating 
an overview of the number of unique visitors per week:

DateTimeFormatInfo dfi = 
DateTimeFormatInfo.CurrentInfo;
Calendar cal = dfi.Calendar;

var result = from log in Logs
               
group log by new { Week = cal.GetWeekOfYear(log.CurrentDateTime, 
dfi.CalendarWeekRule, dfi.FirstDayOfWeek) } into grouped
               
select new { Week = grouped.Key.Week, Visitors = grouped.Select(x => 
x.UserName).Distinct().Count() };

Anyways, this is the high level overview of how we do it. With this 
infrastructure in place, it’s going to be quite easy to extend the number of 
available overviews. We feel like we’ve covered all of the most important ones, 
but we need your help to come up with ideas for new overviews and features.

Support?

Most importantly? Use the tool to your benefit and provide feedback! Contact 
us at margriet at loisandclark dot eu if you have questions or requests.