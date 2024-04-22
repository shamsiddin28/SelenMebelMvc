namespace SelenMebel.Service.Exceptions;

public class NotFoundException : Exception
{
	public string Point { get; set; } = string.Empty;

	public NotFoundException(string point, string message)
		: base(message)
	{
		this.Point = point;
	}
}
