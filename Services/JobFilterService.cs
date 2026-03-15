using JobAlertBot.Models;

namespace JobAlertBot.Services;

public class JobFilterService
{
    private readonly string[] allowedCompanies =
    {
        "Microsoft",
        "Cisco",
        "Adobe",
        "Amazon",
        "Walmart",
        "Oracle",
        "Google",
        "Paypal",
        "Swiggy",
        "Groww",
        "Cred"
    };

    private readonly string[] allowedLocations =
    {
        "bangalore",
        "bengaluru",
        "india",
        "remote"
    };
    private readonly string[] techKeywords =
    {
        "c#",
        ".net",
        "dotnet",
        "webapi",
        "web api",
        "microservice",
        "mvc",
        "wpf",
        "mvvm",
        "kafka",
        "angular"
    };

    public bool IsAllowed(JobPosting job)
    {
        bool companyMatch = allowedCompanies.Any(c =>
            job.Company.Contains(c, StringComparison.OrdinalIgnoreCase));

        if (!companyMatch)
            return false;

        bool locationMatch = allowedLocations.Any(l =>
            job.Location.Contains(l, StringComparison.OrdinalIgnoreCase));

        if (!locationMatch)
            return false;

        if (!IsWithinLast3Days(job.PostedTime))
            return false;

        return true;
    }

    private bool IsWithinLast3Days(string postedTime)
    {
        postedTime = postedTime.ToLower();

        if (postedTime.Contains("hour"))
            return true;

        if (postedTime.Contains("minute"))
            return true;

        if (postedTime.Contains("day"))
        {
            var number = int.Parse(postedTime.Split(" ")[0]);
            return number <= 3;
        }

        return false;
    }
}