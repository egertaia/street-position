using System;
using System.Reflection;
using System.Text;

namespace StreetPosition.Client.Helpers
{
	public static class StringFormatter
	{
		public static string FormatWith(this string format, object source)
		{
			var formatChars = format.ToCharArray();
			var result = new StringBuilder(format.Length);
			var currentTerm = new StringBuilder();
			var term = false;
			var currentPropValue = source;

			for (var i = 0; i < format.Length; i++)
			{
				PropertyInfo pi;

				switch (formatChars[i])
				{
					case '{':
						term = true;
						break;
					case '}':
						pi = currentPropValue.GetType().GetProperty(currentTerm.ToString());
						result.Append((string)pi.PropertyType.GetMethod("ToString", new Type[] { }).Invoke(pi.GetValue(currentPropValue, null), null));
						currentTerm.Clear();
						term = false;
						currentPropValue = source;
						break;
					default:
						if (term)
						{
							if (formatChars[i] == '.')
							{
								pi = currentPropValue.GetType().GetProperty(currentTerm.ToString());
								currentPropValue = pi.GetValue(source, null);
								currentTerm.Clear();
							}
							else
							{
								currentTerm.Append(formatChars[i]);
							}
						}
						else
						{
							result.Append(formatChars[i]);
						}
						break;
				}
			}

			return result.ToString();
		}
	}
}
