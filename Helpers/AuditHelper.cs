using DLARS.Abstractions;

namespace DLARS.Helpers
{
    public class AuditHelper
    {
        public static void SetCreatedAndModified(IAuditable entity)
        {
            var now = TimeHelper.GetPhilippineTimeNow();
            entity.CreatedOn = now;
            entity.ModifiedOn = now;
        }

        public static void SetModified(IAuditable entity)
        {
            entity.ModifiedOn = TimeHelper.GetPhilippineTimeNow();
        }

    }
}
