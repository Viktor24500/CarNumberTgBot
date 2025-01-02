namespace CarNumberRegionsTgBot.Result
{
	public class Result
	{
		public int ErrorCode { get; set; }
		public string ErrorMessage { get; set; }
	}

	public class Result<T>
	{
		public int ErrorCode { get; set; }
		public string ErrorMessage { get; set; }
		public T Data { get; set; }
	}
}
