using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace GuardameLugar.Common.Helpers
{
	public sealed class Throws
	{
		private const string _message0 = "A non-null object was expected.";
		private const string _message1 = "A non-empty Guid was expected.";
		private const string _message2 = "A non-empty string was expected.";
		private const string _message22 = "A non-empty collection was expected.";
		private const string _message3 = "A non-empty collection was expected.";
		private const string _message4 = "A null or empty text was expected.";
		private const string _message5 = "Expected value between {0} and {1} inclusive.";
		private const string _message6 = "Unable to perform IsDefined operation on non-enum types.";
		private const string _message7 = "Value for this parameter must be defined by the enum.";
		private const string _message8 = "{0} must implement the required interface: {1}.";
		///private const string _message9 = "Value for this parameter must be greater than zero.";
		private const string _message10 = "Value for parameter '{0}' must be greater than zero.";
		private const string _message101 = "Value for parameter '{0}' It is not a valid value for a percentage.";
		private const string _message11 = "Value for this parameter must be equal or greater than zero.";
		private const string _message12 = "Range {0} was expected.";
		private const string _message13 = "The date for parameter {0} must be lower or equal than the date of parameter {1}.";
		private const string _message14 = "A non-default date was expected.";

		public static void ThrowIfNull(object parameter, string name, string message = _message0)
		{
			if (parameter == null)
			{
				message = string.IsNullOrEmpty(message) ? _message0 : message;
				throw new ArgumentNullException(name, message);
			}
		}

		public static void ThrowIfNull(object parameter, Exception exceptionToThrow)
		{
			if (parameter == null)
			{
				throw exceptionToThrow;
			}
		}

		public static void ThrowIfEmpty(ICollection value, string name, string message = _message22)
		{
			ThrowIfNull(value, name);

			if (value.Count == 0)
			{
				message = string.IsNullOrEmpty(message) ? _message22 : message;
				throw new ArgumentException(message, name);
			}
		}

		public static void ThrowIfEmpty(string value, string name, string message = _message2)
		{
			ThrowIfNull(value, name);

			if (value.Length == 0)
			{
				message = string.IsNullOrEmpty(message) ? _message2 : message;
				throw new ArgumentException(message, name);
			}
		}

		public static void ThrowIfEmpty(string value, Exception exceptionToThrow)
		{
			ThrowIfNull(value, exceptionToThrow);

			if (value.Length == 0)
			{
				throw exceptionToThrow;
			}
		}

		public static void ThrowIfNullOrEmpty(string secretKey)
		{
			throw new NotImplementedException();
		}

		public static void ThrowIfNullOrEmpty(string parameter, string name, string message = null)
		{
			ThrowIfNull(parameter, name, message);
			ThrowIfEmpty(parameter, name, message);
		}

		public static void ThrowIfNullOrEmpty(string parameter, Exception exceptionToThrow)
		{
			ThrowIfNull(parameter, exceptionToThrow);
			ThrowIfEmpty(parameter, exceptionToThrow);
		}

		public static void ThrowIfGuidEmpty(Guid parameter, string name)
		{
			if (parameter == Guid.Empty)
			{
				throw new ArgumentException(_message1, name);
			}
		}

		public static void ThrowIfCollectionEmpty<T>(ICollection<T> value, string name)
		{
			ThrowIfNull(value, name);

			if (value.Count == 0)
			{
				throw new ArgumentException(_message3, name);
			}
		}

		public static void ThrowIfNotEmpty(string value, string name)
		{
			if (value != null && (value.Length != 0))
			{
				///(value.Length != 0)
				throw new ArgumentException(_message4, name);
			}
		}

		public static void ThrowIfOutOfRange(int value, int min, int max, string name)
		{
			if (value < min || value > max)
			{
				string message = string.Format(CultureInfo.InvariantCulture, _message5, min, max);
				throw new ArgumentOutOfRangeException(name, value, message);
			}
		}

		public static void ThrowIfOutOfRange(int value, int min, int max, Exception exceptionToThrow)
		{
			if (value < min || value > max)
			{
				throw exceptionToThrow;
			}
		}

		public static void ThrowIfOutOfRange(int value, int range, string name, string message = null)
		{
			if (value != range)
			{
				if (string.IsNullOrEmpty(message))
					message = string.Format(CultureInfo.InvariantCulture, _message12, range);
				throw new ArgumentOutOfRangeException(name, value, message);
			}
		}

		public static void ThrowIfOutOfRange(long value, long range, string name, string message = null)
		{
			if (value != range)
			{
				if (string.IsNullOrEmpty(message))
					message = string.Format(CultureInfo.InvariantCulture, _message12, range);
				throw new ArgumentOutOfRangeException(name, value, message);
			}
		}

		public static void ThrowIfMaxLenght(int value, int lenght, Exception exceptionToThrow)
		{
			if (value > lenght)
			{
				throw exceptionToThrow;
			}
		}

		public static void ThrowIfNotDefined(Type enumType, object value, string name)
		{
			if (!enumType.IsSubclassOf(typeof(Enum)))
			{
				throw new InvalidOperationException(_message6);
			}

			if (!Enum.IsDefined(enumType, value))
			{
				throw new ArgumentOutOfRangeException(name, value, _message7);
			}
		}

		public static void ThrowIfInterfaceNotImplemented(Type type, Type interfaceInQuestion)
		{
			Type[] interfaceTypes = type.GetInterfaces();

			foreach (Type interfaceType in interfaceTypes)
			{
				if (interfaceType == interfaceInQuestion)
				{
					return;
				}
			}
			string message = string.Format(CultureInfo.InvariantCulture, _message8, type.Name, interfaceInQuestion.Name);
			throw new InvalidOperationException(message);
		}

		public static void ThrowIfNotPositive(int value, string name)
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException(name, value, string.Format(_message10, name));
			}
		}

		public static void ThrowIfNotPositive(decimal value, string name)
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException(name, value, string.Format(_message10, name));
			}
		}

		public static void ThrowIfNotPositive(uint value, string name)
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException(name, value, string.Format(_message10, name));
			}
		}

		public static void ThrowIfNotPositive(decimal value, Exception exceptionToThrow)
		{
			if (value <= 0)
			{
				throw exceptionToThrow;
			}
		}

		public static void ThrowIfNotPositive(int value, Exception exceptionToThrow)
		{
			if (value <= 0)
			{
				throw exceptionToThrow;
			}
		}

		public static void ThrowIfNotPositive(long value, Exception exceptionToThrow)
		{
			if (value <= 0)
			{
				throw exceptionToThrow;
			}
		}

		public static void ThrowIfNotPositive(long value, string name)
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException(name, value, string.Format(_message10, name));
			}
		}

		public static void ThrowIfNegative(int value, string name)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException(name, value, _message11);
			}
		}

		public static void ThrowIfNegative(TimeSpan value, string name)
		{
			if (value < TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException(name, value, _message11);
			}
		}

		public static void ThrowIfNotPercentage(decimal value, string name)
		{
			if (value <= 0 || value > 100)
			{
				throw new ArgumentOutOfRangeException(name, value, string.Format(_message101, name));
			}
		}

		public static void ThrowIfNullOrWhiteSpace(string parameter, string name)
		{
			ThrowIfNull(parameter, name);
			ThrowIfEmpty(parameter.Trim(), name);
		}

		public static void ThrowIfItsDefaultDatetime(DateTime parameter, string name, string message = null)
		{
			if (parameter.Equals(DateTime.MinValue))
			{
				message = string.IsNullOrEmpty(message) ? _message14 : message;
				throw new ArgumentNullException(name, message);
			}
		}

		public static void ThrowIfFirstDateIsGreaterThanSecondDate(DateTime firstDate, DateTime secondDate, string firstDateName, string secondDateName)
		{
			if (DateTime.Compare(firstDate, secondDate) > 0)
			{
				string message = string.Format(CultureInfo.InvariantCulture, _message13, firstDateName, secondDateName);
				throw new ArgumentException(message, firstDateName);
			}
		}

		private Throws()
		{
		}
	}
}
