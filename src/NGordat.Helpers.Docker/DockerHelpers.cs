namespace NGordat.Helpers.Docker
{
    public class DockerHelpers
    {
        public static void UpdateCaCertificates()
        {
            "update-ca-certificates".Bash();
        }
    }
}