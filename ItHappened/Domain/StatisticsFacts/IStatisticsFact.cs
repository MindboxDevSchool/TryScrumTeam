using System.Collections.Generic;

namespace ItHappened.Domain.StatisticsFacts
{
    public interface IStatisticsFact
    {
        double Priority { get; }
        string Description { get; }
        bool IsApplicable { get; }
        void Process(IEnumerable<Event> events, string trackName);
    }
}