using BaseProject.Application.Common.Interfaces;

namespace BaseProject.Application.Services
{
    public class CurrentTime : ICurrentTime
    {
        public DateTime GetCurrentTime() => DateTime.UtcNow;
    }

}
