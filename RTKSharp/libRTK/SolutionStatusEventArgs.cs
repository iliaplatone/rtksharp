using System;

namespace libRTK;

public class SolutionStatusEventArgs : EventArgs
{
	public solstatus Status;

	public SolutionStatusEventArgs(solstatus status)
	{
		this.Status = status;
	}
}
