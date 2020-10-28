using ItHappened.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ItHappend.RestAPI.Filters
{
    public class SaveChangesFilter : ActionFilterAttribute
    {
        private readonly TrackDbContext _dbContext;

        public SaveChangesFilter(TrackDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null)
            {
                _dbContext.SaveChanges();
            }
        }
    }
}