using SelenMebel.Domain.Constants;

namespace SelenMebel.Service.Commons.Helpers;

public class TimeHelper
{
	public static DateTime GetCurrentServerTime()
	{
		var date = DateTime.UtcNow;
		return date.AddHours(TimeConstants.UTC);
	}
}
