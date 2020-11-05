using System;
using System.Collections.Generic;

namespace ItHappened.Application
{
    public interface IStatisticsService
    {
        List<string> GetTrackStatistics(Guid userId, Guid trackId);
    }
}