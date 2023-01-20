using System.Drawing;
using System.IO;

namespace SmartBooks.Api.Helpers
{
    public class Helpers
    {
		public static int[] GetImageDimension(byte[] imageContent)
		{
			Stream stream = new MemoryStream(imageContent);
			Image image = Image.FromStream(stream);
			var response = new int[2];
			response[0] = image.Height;
			response[0] = image.Width;
			return response;
		}
	}
}
