namespace JobAlertBot.Storage;

public class SeenJobStore
{
    private readonly string filePath = "seen_jobs.txt";
    private HashSet<string> seenJobs = new();

    public SeenJobStore()
    {
        if (File.Exists(filePath))
        {
            var lines = File.ReadAllLines(filePath);
            seenJobs = new HashSet<string>(lines);
        }
    }

    public bool IsNew(string jobId)
    {
        if (seenJobs.Contains(jobId))
            return false;

        seenJobs.Add(jobId);
        File.AppendAllLines(filePath, new[] { jobId });

        return true;
    }
}