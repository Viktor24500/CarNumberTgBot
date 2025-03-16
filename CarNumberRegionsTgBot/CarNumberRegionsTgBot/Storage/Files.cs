namespace CarNumberRegionsTgBot.Storage
{
	public class Files : IStorage
	{
		private string _folderPath = "../../../../../History";
		public void WriteToStorage(string input)
		{
			if (!Directory.Exists(_folderPath))
			{
				Directory.CreateDirectory(_folderPath);
				Console.WriteLine($"{DateTime.Now}: create directory on {_folderPath}");
			}
			string fileName = "history.txt";
			string fullPath = Path.Combine(_folderPath, fileName);
			if (!File.Exists(fullPath))
			{
				File.Create(fullPath);
				File.Create(fullPath).Close();
				Console.WriteLine($"{DateTime.Now}: create file on {fullPath}");
			}

			//use stream writer for append text in file
			using (StreamWriter writer = new StreamWriter(fullPath, true))
			{
				writer.WriteLine(input);
				writer.Close();
			}
		}
	}
}
