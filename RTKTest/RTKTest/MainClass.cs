using System;
using System.Globalization;
using System.Threading;
using libRTK;

namespace RTKTest;

internal class MainClass
{
	private static bool Running = true;

	private static void This_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
	{
		e.Cancel = true;
		MainClass.Running = false;
	}

	public static void Main(string[] args)
	{
		Console.CancelKeyPress += This_CancelKeyPress;
		RTK rTK;
		rTK = new RTK(args);
		if (rTK == null)
		{
			return;
		}
		rTK.SolutionStatusChanged += Rtk_SolutionStatusChanged;
		if (rTK.start())
		{
			while (MainClass.Running && rTK.ServerRunning)
			{
				Thread.Sleep(500);
				if (rTK.SolutionStatus != 0)
				{
					Console.Write("\rN=" + rTK.RoverLatitude.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")) + " E=" + rTK.RoverLongitude.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")) + " He=" + rTK.RoverHeight.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US")));
				}
			}
			rTK.stop();
		}
		rTK.Dispose();
	}

	private static void Rtk_SolutionStatusChanged(object sender, SolutionStatusEventArgs e)
	{
		Console.WriteLine("Solution status: " + ((RTK)sender).SolutionStatus);
	}

	private void RTK_SolutionStatusChanged(object sender, SolutionStatusEventArgs e)
	{
		Console.WriteLine(e.Status);
	}
}
