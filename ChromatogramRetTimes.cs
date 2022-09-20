using System;
/// <summary>
/// chromas will be singular
/// bins are each minute, [inclusive, exclusive)
/// binned till further use
/// </summary>

public class ChromatogramRetTimes
{
	public string name;
	public int day;
	public float[] retTimes;

	static private int base_size = 120 * 60 * 10 + 1;
	static private int padding = 5 * 10; ///extra five seconds just in case
	static private int arr_size = base_size + padding;

	public ChromatogramRetTimes(string new_name, int new_day, float[] rawtimes) ///add bool for attaching nulls to nearest RT value
	{
		name = new_name;
		day = new_day;
		retTimes = new float[arr_size]; ///upperbound for number arrays
		IndexRawTimes(rawtimes);
	}
	private void IndexRawTimes(float[] rawtimes)
	{ //add in seconds, minutes, then find deviation
		for (int i = 0; i < rawtimes.Length; i++)
		{
			retTimes[(int)rawtimes[i]] = rawtimes[i];
		}
		if (retTimes.Length > rawtimes.Length)
		{
			//Console.WriteLine("shortfile");
			AttachEmptyIndexes();
		}
	}

	private void AttachEmptyIndexes()
	{
		for (int i = 0; i < this.retTimes.Length; i++)
		{
			if (this.retTimes[i] == 0 && i != 0)
			{
				FindNearestRTValue(i);
			}
		}
	}
	private void FindNearestRTValue(int index)
	{
		int upperindex = -1;
		int lowerindex = -1;

		if (index > 0)
		{
			for (int i = index - 1; i > 0; i--)
			{
				if (this.retTimes[i] != 0)
				{
					lowerindex = i;
					break;
				}
			}
		}

		if (index < this.retTimes.Length - 1)
		{
			for (int i = index + 1; i < this.retTimes.Length; i++)
			{
				if (this.retTimes[i] != -1)
				{
					upperindex = i;
					break;
				}
			}
		}

		if (upperindex == -1 && lowerindex == 1)
		{
			this.retTimes[index] = 0;
		}
		if (upperindex == -1 && lowerindex != 1)
        {
			this.retTimes[index] = upperindex;
		}
		else if (upperindex != -1 && lowerindex == 1)
		{
			this.retTimes[index] = lowerindex;
		}
		else if ((index - lowerindex) > (upperindex - index))
		{
			this.retTimes[index] = upperindex;
		}
		else ///can change this later for equalities.
		{
			this.retTimes[index] = lowerindex;
		}
	}
}
