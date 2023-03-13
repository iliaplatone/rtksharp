using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace libRTK;

public class RTK : IDisposable
{
	private Thread MainThread;

	private bool _ServerRunning;

	private solstatus _SolutionStatus;

	private solstatus _OldSolutionStatus;

	private int _SatellitesRover;

	private int _SatellitesBase;

	private int _SatellitesValid;

	private double _RoverLatitude;

	private double _RoverLongitude;

	private double _RoverHeight;

	private double _BaseLatitude;

	private double _BaseLongitude;

	private double _BaseHeight;

	private double _BaselineFloat;

	private double _BaselineFixed;

	private bool Running;

	public static bool IsLinux
	{
		get
		{
			int platform;
			platform = (int)Environment.OSVersion.Platform;
			if (platform != 4 && platform != 6)
			{
				return platform == 128;
			}
			return true;
		}
	}

	public bool ServerRunning => this._ServerRunning;

	public solstatus SolutionStatus => this._SolutionStatus;

	public int SatellitesRover => this._SatellitesRover;

	public int SatellitesBase => this._SatellitesBase;

	public int SatellitesValid => this._SatellitesValid;

	public double RoverLatitude => this._RoverLatitude;

	public double RoverLongitude => this._RoverLongitude;

	public double RoverHeight => this._RoverHeight;

	public double BaseLatitude => this._BaseLatitude;

	public double BaseLongitude => this._BaseLongitude;

	public double BaseHeight => this._BaseHeight;

	public double BaselineFloat => this._BaselineFloat;

	public double BaselineFixed => this._BaselineFixed;

	public string console_passwd
	{
		get
		{
			return this.option(new string[1] { "console-passwd" });
		}
		set
		{
			this.set(new string[2] { "console-passwd", value });
		}
	}

	public timetype console_timetype
	{
		get
		{
			return (timetype)Enum.Parse(value: this.optStr(this.option(new string[1] { "console-timetype" })), enumType: typeof(timetype));
		}
		set
		{
			this.set(new string[2]
			{
				"console-timetype",
				value.ToString()
			});
		}
	}

	public byte console_soltype
	{
		get
		{
			return byte.Parse(this.optStr(this.option(new string[1] { "console-soltype" })));
		}
		set
		{
			this.set(new string[2]
			{
				"console-soltype",
				value.ToString()
			});
		}
	}

	public solflag console_solflag
	{
		get
		{
			return (solflag)Enum.Parse(value: this.optStr(this.option(new string[1] { "console-solflag" })).Substring(0, 3), enumType: typeof(solflag));
		}
		set
		{
			this.set(new string[2]
			{
				"console-solflag",
				value.ToString()
			});
		}
	}

	public strtype inpstr1_type
	{
		get
		{
			return (strtype)Enum.Parse(value: this.optStr(this.option(new string[1] { "inpstr1-type" })), enumType: typeof(strtype));
		}
		set
		{
			this.set(new string[2]
			{
				"inpstr1-type",
				value.ToString()
			});
		}
	}

	public strtype inpstr2_type
	{
		get
		{
			return (strtype)Enum.Parse(value: this.optStr(this.option(new string[1] { "inpstr2-type" })), enumType: typeof(strtype));
		}
		set
		{
			this.set(new string[2]
			{
				"inpstr2-type",
				value.ToString()
			});
		}
	}

	public strtype inpstr3_type
	{
		get
		{
			return (strtype)Enum.Parse(value: this.optStr(this.option(new string[1] { "inpstr3-type" })), enumType: typeof(strtype));
		}
		set
		{
			this.set(new string[2]
			{
				"inpstr3-type",
				value.ToString()
			});
		}
	}

	public string inpstr1_path
	{
		get
		{
			return this.option(new string[1] { "inpstr1-path" });
		}
		set
		{
			this.set(new string[2] { "inpstr1-path", value });
		}
	}

	public string inpstr2_path
	{
		get
		{
			return this.option(new string[1] { "inpstr2-path" });
		}
		set
		{
			this.set(new string[2] { "inpstr2-path", value });
		}
	}

	public string inpstr3_path
	{
		get
		{
			return this.option(new string[1] { "inpstr3-path" });
		}
		set
		{
			this.set(new string[2] { "inpstr3-path", value });
		}
	}

	public strfmt inpstr1_format
	{
		get
		{
			return (strfmt)Enum.Parse(value: this.optStr(this.option(new string[1] { "inpstr1-format" })), enumType: typeof(strfmt));
		}
		set
		{
			this.set(new string[2]
			{
				"inpstr1-format",
				value.ToString()
			});
		}
	}

	public strfmt inpstr2_format
	{
		get
		{
			return (strfmt)Enum.Parse(value: this.optStr(this.option(new string[1] { "inpstr2-format" })), enumType: typeof(strfmt));
		}
		set
		{
			this.set(new string[2]
			{
				"inpstr2-format",
				value.ToString()
			});
		}
	}

	public strfmt inpstr3_format
	{
		get
		{
			return (strfmt)Enum.Parse(value: this.optStr(this.option(new string[1] { "inpstr3-format" })), enumType: typeof(strfmt));
		}
		set
		{
			this.set(new string[2]
			{
				"inpstr3-format",
				value.ToString()
			});
		}
	}

	public nmeareq inpstr2_nmeareq
	{
		get
		{
			return (nmeareq)Enum.Parse(value: this.optStr(this.option(new string[1] { "inpstr2-nmeareq" })), enumType: typeof(nmeareq));
		}
		set
		{
			this.set(new string[2]
			{
				"inpstr2-nmeareq",
				value.ToString()
			});
		}
	}

	public double inpstr2_nmealat
	{
		get
		{
			return double.Parse(this.option(new string[1] { "inpstr2-nmealat" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"inpstr2-nmealat",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double inpstr2_nmealon
	{
		get
		{
			return double.Parse(this.option(new string[1] { "inpstr2-nmealon" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"inpstr2-nmealon",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public strtype outstr1_type
	{
		get
		{
			return (strtype)Enum.Parse(value: this.optStr(this.option(new string[1] { "outstr1-type" })), enumType: typeof(strtype));
		}
		set
		{
			this.set(new string[2]
			{
				"outstr1-type",
				value.ToString()
			});
		}
	}

	public strtype outstr2_type
	{
		get
		{
			return (strtype)Enum.Parse(value: this.optStr(this.option(new string[1] { "outstr2-type" })), enumType: typeof(strtype));
		}
		set
		{
			this.set(new string[2]
			{
				"outstr2-type",
				value.ToString()
			});
		}
	}

	public string outstr1_path
	{
		get
		{
			return this.option(new string[1] { "outstr1-path" });
		}
		set
		{
			this.set(new string[2] { "outstr1-path", value });
		}
	}

	public string outstr2_path
	{
		get
		{
			return this.option(new string[1] { "outstr2-path" });
		}
		set
		{
			this.set(new string[2] { "outstr2-path", value });
		}
	}

	public ostrfmt outstr1_format
	{
		get
		{
			return (ostrfmt)Enum.Parse(value: this.optStr(this.option(new string[1] { "outstr1-format" })), enumType: typeof(ostrfmt));
		}
		set
		{
			this.set(new string[2]
			{
				"outstr1-format",
				value.ToString()
			});
		}
	}

	public ostrfmt outstr2_format
	{
		get
		{
			return (ostrfmt)Enum.Parse(value: this.optStr(this.option(new string[1] { "outstr2-format" })), enumType: typeof(ostrfmt));
		}
		set
		{
			this.set(new string[2]
			{
				"outstr2-format",
				value.ToString()
			});
		}
	}

	public strtype logstr1_type
	{
		get
		{
			return (strtype)Enum.Parse(value: this.optStr(this.option(new string[1] { "logstr1-type" })), enumType: typeof(strtype));
		}
		set
		{
			this.set(new string[2]
			{
				"logstr1-type",
				value.ToString()
			});
		}
	}

	public strtype logstr2_type
	{
		get
		{
			return (strtype)Enum.Parse(value: this.optStr(this.option(new string[1] { "logstr2-type" })), enumType: typeof(strtype));
		}
		set
		{
			this.set(new string[2]
			{
				"logstr2-type",
				value.ToString()
			});
		}
	}

	public strtype logstr3_type
	{
		get
		{
			return (strtype)Enum.Parse(value: this.optStr(this.option(new string[1] { "logstr3-type" })), enumType: typeof(strtype));
		}
		set
		{
			this.set(new string[2]
			{
				"logstr3-type",
				value.ToString()
			});
		}
	}

	public string logstr1_path
	{
		get
		{
			return this.option(new string[1] { "logstr1-path" });
		}
		set
		{
			this.set(new string[2] { "logstr1-path", value });
		}
	}

	public string logstr2_path
	{
		get
		{
			return this.option(new string[1] { "logstr2-path" });
		}
		set
		{
			this.set(new string[2] { "logstr2-path", value });
		}
	}

	public string logstr3_path
	{
		get
		{
			return this.option(new string[1] { "logstr3-path" });
		}
		set
		{
			this.set(new string[2] { "logstr3-path", value });
		}
	}

	public int misc_svrcycle
	{
		get
		{
			return int.Parse(this.option(new string[1] { "misc-svrcycle" }));
		}
		set
		{
			this.set(new string[2]
			{
				"misc-svrcycle",
				value.ToString()
			});
		}
	}

	public int misc_timeout
	{
		get
		{
			return int.Parse(this.option(new string[1] { "misc-timeout" }));
		}
		set
		{
			this.set(new string[2]
			{
				"misc-timeout",
				value.ToString()
			});
		}
	}

	public int misc_reconnect
	{
		get
		{
			return int.Parse(this.option(new string[1] { "misc-reconnect" }));
		}
		set
		{
			this.set(new string[2]
			{
				"misc-reconnect",
				value.ToString()
			});
		}
	}

	public int misc_nmeacycle
	{
		get
		{
			return int.Parse(this.option(new string[1] { "misc-nmeacycle" }));
		}
		set
		{
			this.set(new string[2]
			{
				"misc-nmeacycle",
				value.ToString()
			});
		}
	}

	public int misc_buffsize
	{
		get
		{
			return int.Parse(this.option(new string[1] { "misc-buffsize" }));
		}
		set
		{
			this.set(new string[2]
			{
				"misc-buffsize",
				value.ToString()
			});
		}
	}

	public peer misc_navmsgsel
	{
		get
		{
			return (peer)Enum.Parse(value: this.optStr(this.option(new string[1] { "misc-navmsgsel" })), enumType: typeof(peer));
		}
		set
		{
			this.set(new string[2]
			{
				"misc-navmsgsel",
				value.ToString()
			});
		}
	}

	public string misc_startcmd
	{
		get
		{
			return this.option(new string[1] { "misc-startcmd" });
		}
		set
		{
			this.set(new string[2] { "misc-startcmd", value });
		}
	}

	public string misc_stopcmd
	{
		get
		{
			return this.option(new string[1] { "misc-stopcmd" });
		}
		set
		{
			this.set(new string[2] { "misc-stopcmd", value });
		}
	}

	public string file_cmdfile
	{
		get
		{
			return this.option(new string[1] { "file-cmdfile" });
		}
		set
		{
			this.set(new string[2] { "file-cmdfile", value });
		}
	}

	public string file_cmdfile2
	{
		get
		{
			return this.option(new string[1] { "file-cmdfile2" });
		}
		set
		{
			this.set(new string[2] { "file-cmdfile2", value });
		}
	}

	public string file_cmdfile3
	{
		get
		{
			return this.option(new string[1] { "file-cmdfile3" });
		}
		set
		{
			this.set(new string[2] { "file-cmdfile3", value });
		}
	}

	public posmode pos1_posmode
	{
		get
		{
			return (posmode)Enum.Parse(value: this.optStr(this.option(new string[1] { "pos1-posmode" })).Replace("static", "stationary").Replace("-", "_"), enumType: typeof(posmode));
		}
		set
		{
			this.set(new string[2]
			{
				"pos1-posmode",
				value.ToString().Replace("stationary", "static").Replace("_", "-")
			});
		}
	}

	public frequency pos1_frequency
	{
		get
		{
			return (frequency)Enum.Parse(value: this.optStr(this.option(new string[1] { "pos1-frequency" })).Replace("+", "_"), enumType: typeof(frequency));
		}
		set
		{
			this.set(new string[2]
			{
				"pos1-frequency",
				value.ToString().Replace("_", "+")
			});
		}
	}

	public soltype pos1_soltype
	{
		get
		{
			return (soltype)Enum.Parse(value: this.optStr(this.option(new string[1] { "pos1-soltype" })), enumType: typeof(soltype));
		}
		set
		{
			this.set(new string[2]
			{
				"pos1-soltype",
				value.ToString()
			});
		}
	}

	public int pos1_elmask
	{
		get
		{
			return int.Parse(this.option(new string[1] { "pos1-elmask" }));
		}
		set
		{
			this.set(new string[2]
			{
				"pos1-elmask",
				value.ToString()
			});
		}
	}

	public int pos1_snrmask
	{
		get
		{
			return int.Parse(this.option(new string[1] { "pos1-snrmask" }));
		}
		set
		{
			this.set(new string[2]
			{
				"pos1-snrmask",
				value.ToString()
			});
		}
	}

	public bool pos1_dynamics
	{
		get
		{
			return bool.Parse(this.option(new string[1] { "pos1-dynamics" }));
		}
		set
		{
			this.set(new string[2]
			{
				"pos1-dynamics",
				value.ToString()
			});
		}
	}

	public bool pos1_tidecorr
	{
		get
		{
			return bool.Parse(this.option(new string[1] { "pos1-tidecorr" }));
		}
		set
		{
			this.set(new string[2]
			{
				"pos1-tidecorr",
				value ? "on" : "off"
			});
		}
	}

	public ionoopt pos1_ionoopt
	{
		get
		{
			return (ionoopt)Enum.Parse(value: this.optStr(this.option(new string[1] { "pos1-ionoopt" })).Replace("-", "_"), enumType: typeof(ionoopt));
		}
		set
		{
			this.set(new string[2]
			{
				"pos1-ionoopt",
				value.ToString().Replace("_", "-")
			});
		}
	}

	public tropopt pos1_tropopt
	{
		get
		{
			return (tropopt)Enum.Parse(value: this.optStr(this.option(new string[1] { "pos1-tropopt" })).Replace("-", "_"), enumType: typeof(tropopt));
		}
		set
		{
			this.set(new string[2]
			{
				"pos1-tropopt",
				value.ToString().Replace("_", "-")
			});
		}
	}

	public eph pos1_sateph
	{
		get
		{
			return (eph)Enum.Parse(value: this.optStr(this.option(new string[1] { "pos1-sateph" })).Replace("+", "_"), enumType: typeof(eph));
		}
		set
		{
			this.set(new string[2]
			{
				"pos1-sateph",
				value.ToString().Replace("_", "+")
			});
		}
	}

	public string pos1_exclsats
	{
		get
		{
			return this.option(new string[1] { "pos1-exclsats" });
		}
		set
		{
			this.set(new string[2] { "pos1-exclsats", value });
		}
	}

	public byte pos1_navsys
	{
		get
		{
			return byte.Parse(this.optStr(this.option(new string[1] { "pos1-navsys" })));
		}
		set
		{
			this.set(new string[2]
			{
				"pos1-navsys",
				value.ToString()
			});
		}
	}

	public armode pos2_armode
	{
		get
		{
			return (armode)Enum.Parse(value: this.optStr(this.option(new string[1] { "pos2-armode" })).Replace("-", "_"), enumType: typeof(armode));
		}
		set
		{
			this.set(new string[2]
			{
				"pos2-armode",
				value.ToString().Replace("_", "-")
			});
		}
	}

	public gloarmode pos2_gloarmode
	{
		get
		{
			return (gloarmode)Enum.Parse(value: this.optStr(this.option(new string[1] { "pos2-gloarmode" })), enumType: typeof(gloarmode));
		}
		set
		{
			this.set(new string[2]
			{
				"pos2-gloarmode",
				value.ToString()
			});
		}
	}

	public double pos2_arthres
	{
		get
		{
			return double.Parse(this.option(new string[1] { "pos2-arthres" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"pos2-arthres",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double pos2_arlockcnt
	{
		get
		{
			return double.Parse(this.option(new string[1] { "pos2-arlockcnt" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"pos2-arlockcnt",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double pos2_arelmask
	{
		get
		{
			return double.Parse(this.option(new string[1] { " pos2-arelmask" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				" pos2-arelmask",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double pos2_aroutcnt
	{
		get
		{
			return double.Parse(this.option(new string[1] { " pos2-aroutcnt" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				" pos2-aroutcnt",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double pos2_arminfix
	{
		get
		{
			return double.Parse(this.option(new string[1] { "pos2-arminfix" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"pos2-arminfix",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double pos2_slipthres
	{
		get
		{
			return double.Parse(this.option(new string[1] { "pos2-slipthres" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"pos2-slipthres",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double pos2_maxage
	{
		get
		{
			return double.Parse(this.option(new string[1] { "pos2-maxage" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"pos2-maxage",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double pos2_rejionno
	{
		get
		{
			return double.Parse(this.option(new string[1] { "pos2-rejionno" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"pos2-rejionno",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double pos2_niter
	{
		get
		{
			return double.Parse(this.option(new string[1] { "pos2-niter" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"pos2-niter",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double pos2_baselen
	{
		get
		{
			return double.Parse(this.option(new string[1] { "pos2-baselen" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"pos2-baselen",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double pos2_basesig
	{
		get
		{
			return double.Parse(this.option(new string[1] { "pos2-basesig" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"pos2-basesig",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public solformat out_solformat
	{
		get
		{
			return (solformat)Enum.Parse(value: this.optStr(this.option(new string[1] { "out-solformat" })), enumType: typeof(solformat));
		}
		set
		{
			this.set(new string[2]
			{
				"out-solformat",
				value.ToString()
			});
		}
	}

	public bool out_outhead
	{
		get
		{
			return bool.Parse(this.option(new string[1] { "out-outhead" }));
		}
		set
		{
			this.set(new string[2]
			{
				"out-outhead",
				value ? "on" : "off"
			});
		}
	}

	public bool out_outopt
	{
		get
		{
			return bool.Parse(this.option(new string[1] { "out-outopt" }));
		}
		set
		{
			this.set(new string[2]
			{
				"out-outopt",
				value ? "on" : "off"
			});
		}
	}

	public timesys out_timesys
	{
		get
		{
			return (timesys)Enum.Parse(value: this.optStr(this.option(new string[1] { "out-timesys" })), enumType: typeof(timesys));
		}
		set
		{
			this.set(new string[2]
			{
				"out-timesys",
				value.ToString()
			});
		}
	}

	public timeform out_timeform
	{
		get
		{
			return (timeform)Enum.Parse(value: this.optStr(this.option(new string[1] { "out-timeform" })), enumType: typeof(timeform));
		}
		set
		{
			this.set(new string[2]
			{
				"out-timeform",
				value.ToString()
			});
		}
	}

	public int out_timendec
	{
		get
		{
			return int.Parse(this.option(new string[1] { "out-timendec" }));
		}
		set
		{
			this.set(new string[2]
			{
				"out-timendec",
				value.ToString()
			});
		}
	}

	public degform out_degform
	{
		get
		{
			return (degform)Enum.Parse(value: this.optStr(this.option(new string[1] { "out-degform" })), enumType: typeof(degform));
		}
		set
		{
			this.set(new string[2]
			{
				"out-degform",
				value.ToString()
			});
		}
	}

	public string out_fieldsep
	{
		get
		{
			return this.option(new string[1] { "out-fieldsep" });
		}
		set
		{
			this.set(new string[2] { "out-fieldsep", value });
		}
	}

	public height out_height
	{
		get
		{
			return (height)Enum.Parse(value: this.optStr(this.option(new string[1] { "out-height" })), enumType: typeof(height));
		}
		set
		{
			this.set(new string[2]
			{
				"out-height",
				value.ToString()
			});
		}
	}

	public geoid out_geoid
	{
		get
		{
			return (geoid)Enum.Parse(value: this.optStr(this.option(new string[1] { "out-geoid" })).Replace(".", "_"), enumType: typeof(geoid));
		}
		set
		{
			this.set(new string[2]
			{
				"out-geoid",
				value.ToString().Replace("egm08_2_5", "egm08_2.5")
			});
		}
	}

	public int out_nmeaintv1
	{
		get
		{
			return int.Parse(this.option(new string[1] { "out-nmeaintv1" }));
		}
		set
		{
			this.set(new string[2]
			{
				"out-nmeaintv1",
				value.ToString()
			});
		}
	}

	public solstatic out_solstatic
	{
		get
		{
			return (solstatic)Enum.Parse(value: this.optStr(this.option(new string[1] { "out-solstatic" })), enumType: typeof(solstatic));
		}
		set
		{
			this.set(new string[2]
			{
				"out-solstatic",
				value.ToString()
			});
		}
	}

	public int out_nmeaintv2
	{
		get
		{
			return int.Parse(this.option(new string[1] { "out-nmeaintv2" }));
		}
		set
		{
			this.set(new string[2]
			{
				"out-nmeaintv2",
				value.ToString()
			});
		}
	}

	public stat out_outstat
	{
		get
		{
			return (stat)Enum.Parse(value: this.optStr(this.option(new string[1] { "out-outstat" })), enumType: typeof(stat));
		}
		set
		{
			this.set(new string[2]
			{
				"out-outstat",
				value.ToString()
			});
		}
	}

	public int stats_errratio
	{
		get
		{
			return int.Parse(this.option(new string[1] { "stats-errratio" }));
		}
		set
		{
			this.set(new string[2]
			{
				"stats-errratio",
				value.ToString()
			});
		}
	}

	public double stats_errphase
	{
		get
		{
			return double.Parse(this.option(new string[1] { "stats-errphase" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"stats-errphase",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double stats_errphaseel
	{
		get
		{
			return double.Parse(this.option(new string[1] { "stats-errphaseel" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"stats-errphaseel",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double stats_errphasebl
	{
		get
		{
			return double.Parse(this.option(new string[1] { "stats-errphasebl" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"stats-errphasebl",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double stats_errdoppler
	{
		get
		{
			return double.Parse(this.option(new string[1] { "stats-errdoppler" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"stats-errdoppler",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double stats_stdbias
	{
		get
		{
			return double.Parse(this.option(new string[1] { "stats-stdbias" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"stats-stdbias",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double stats_stdiono
	{
		get
		{
			return double.Parse(this.option(new string[1] { "stats-stdiono" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"stats-stdiono",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double stats_stdtrop
	{
		get
		{
			return double.Parse(this.option(new string[1] { "stats-stdtrop" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"stats-stdtrop",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double stats_prnaccelh
	{
		get
		{
			return double.Parse(this.option(new string[1] { "stats-prnaccelh" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"stats-prnaccelh",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double stats_prnaccelv
	{
		get
		{
			return double.Parse(this.option(new string[1] { "stats-prnaccelv" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"stats-prnaccelv",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double stats_prnbias
	{
		get
		{
			return double.Parse(this.option(new string[1] { "stats-prnbias" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"stats-prnbias",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double stats_prniono
	{
		get
		{
			return double.Parse(this.option(new string[1] { "stats-prniono" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"stats-prniono",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double stats_prntrop
	{
		get
		{
			return double.Parse(this.option(new string[1] { "stats-prntrop" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"stats-prntrop",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double stats_clkstab
	{
		get
		{
			return double.Parse(this.option(new string[1] { "stats-clkstab" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"stats-clkstab",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public postype ant1_postype
	{
		get
		{
			return (postype)Enum.Parse(value: this.optStr(this.option(new string[1] { "ant1-postype" })), enumType: typeof(postype));
		}
		set
		{
			this.set(new string[2]
			{
				"ant1-postype",
				value.ToString()
			});
		}
	}

	public double ant1_pos1
	{
		get
		{
			return double.Parse(this.option(new string[1] { "ant1-pos1" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"ant1-pos1",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double ant1_pos2
	{
		get
		{
			return double.Parse(this.option(new string[1] { "ant1-pos2" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"ant1-pos2",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double ant1_pos3
	{
		get
		{
			return double.Parse(this.option(new string[1] { "ant1-pos3" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"ant1-pos3",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public string ant1_anttype
	{
		get
		{
			return this.option(new string[1] { "ant1-anttype" });
		}
		set
		{
			this.set(new string[2] { "ant1-anttype", value });
		}
	}

	public double ant1_antdele
	{
		get
		{
			return double.Parse(this.option(new string[1] { "ant1-antdele" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"ant1-antdele",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double ant1_antdeln
	{
		get
		{
			return double.Parse(this.option(new string[1] { "ant1-antdeln" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"ant1-antdeln",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double ant1_antdelu
	{
		get
		{
			return double.Parse(this.option(new string[1] { "ant1-antdelu" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"ant1-antdelu",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public postype ant2_postype
	{
		get
		{
			return (postype)Enum.Parse(value: this.optStr(this.option(new string[1] { "ant2-postype" })), enumType: typeof(postype));
		}
		set
		{
			this.set(new string[2]
			{
				"ant2-postype",
				value.ToString()
			});
		}
	}

	public double ant2_pos1
	{
		get
		{
			return double.Parse(this.option(new string[1] { "ant2-pos1" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"ant2-pos1",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double ant2_pos2
	{
		get
		{
			return double.Parse(this.option(new string[1] { "ant2-pos2" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"ant2-pos2",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double ant2_pos3
	{
		get
		{
			return double.Parse(this.option(new string[1] { "ant2-pos3" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"ant2-pos3",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public string ant2_anttype
	{
		get
		{
			return this.option(new string[1] { "ant2-anttype" });
		}
		set
		{
			this.set(new string[2] { "ant2-anttype", value });
		}
	}

	public double ant2_antdele
	{
		get
		{
			return double.Parse(this.option(new string[1] { "ant2-antdele" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"ant2-antdele",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double ant2_antdeln
	{
		get
		{
			return double.Parse(this.option(new string[1] { "ant2-antdeln" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"ant2-antdeln",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public double ant2_antdelu
	{
		get
		{
			return double.Parse(this.option(new string[1] { "ant2-antdelu" }), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
		}
		set
		{
			this.set(new string[2]
			{
				"ant2-antdelu",
				value.ToString(CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"))
			});
		}
	}

	public bool misc_timeinterp
	{
		get
		{
			return bool.Parse(this.option(new string[1] { "misc-timeinterp" }));
		}
		set
		{
			this.set(new string[2]
			{
				"misc-timeinterp",
				value ? "on" : "off"
			});
		}
	}

	public int misc_sbasatsel
	{
		get
		{
			return int.Parse(this.option(new string[1] { "misc-sbasatsel" }));
		}
		set
		{
			this.set(new string[2]
			{
				"misc-sbasatsel",
				value.ToString()
			});
		}
	}

	public string file_satantfile
	{
		get
		{
			return this.option(new string[1] { "file-satantfile" });
		}
		set
		{
			this.set(new string[2] { "file-satantfile", value });
		}
	}

	public string file_rcvantfile
	{
		get
		{
			return this.option(new string[1] { "file-rcvantfile" });
		}
		set
		{
			this.set(new string[2] { "file-rcvantfile", value });
		}
	}

	public string file_staposfile
	{
		get
		{
			return this.option(new string[1] { "file-staposfile" });
		}
		set
		{
			this.set(new string[2] { "file-staposfile", value });
		}
	}

	public string file_geoidfile
	{
		get
		{
			return this.option(new string[1] { "file-geoidfile" });
		}
		set
		{
			this.set(new string[2] { "file-geoidfile", value });
		}
	}

	public string file_dcbfile
	{
		get
		{
			return this.option(new string[1] { "file-dcbfile" });
		}
		set
		{
			this.set(new string[2] { "file-dcbfile", value });
		}
	}

	public string file_tempdir
	{
		get
		{
			return this.option(new string[1] { "file-tempdir" });
		}
		set
		{
			this.set(new string[2] { "file-tempdir", value });
		}
	}

	public event EventHandler<SolutionStatusEventArgs> SolutionStatusChanged;

	public RTK(string[] args = null)
	{
		this.SetPath();
		Thread.Sleep(1000);
		bool num;
		num = this.init(args);
		Thread.Sleep(1000);
		if (!num)
		{
			this.Dispose();
		}
	}

	private void SetPath()
	{
		string text;
		text = "";
		string directoryName;
		directoryName = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
		directoryName = ((IntPtr.Size != 4) ? (directoryName + "/x64") : (directoryName + "/x86"));
		if (RTK.IsLinux)
		{
			File.Copy(directoryName + "/librtk.so", Environment.CurrentDirectory + "/librtk.so", overwrite: true);
		}
		else
		{
			Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + directoryName, EnvironmentVariableTarget.Process);
		}
	}

	public void Dispose()
	{
		this.shut();
		if (File.Exists(Environment.CurrentDirectory + "/librtk.so"))
		{
			File.Delete(Environment.CurrentDirectory + "/librtk.so");
		}
	}

	private bool init(string[] args = null)
	{
		if (args == null)
		{
			args = new string[1];
		}
		else
		{
			Array.Resize(ref args, args.Length + 1);
			for (int num = args.Length - 1; num > 0; num--)
			{
				args[num] = args[num - 1];
			}
		}
		args[0] = "RTK";
		if (Native.librtk_init(args.Length, args) == 0)
		{
			return false;
		}
		return true;
	}

	public bool start(string[] args = null)
	{
		if (args == null)
		{
			args = new string[1];
		}
		else
		{
			Array.Resize(ref args, args.Length + 1);
			for (int num = args.Length - 1; num > 0; num--)
			{
				args[num] = args[num - 1];
			}
		}
		args[0] = "start";
		StringBuilder stringBuilder;
		stringBuilder = new StringBuilder(16384);
		Native.librtk_start(args, args.Length, stringBuilder);
		this._ServerRunning = !stringBuilder.ToString().Contains("error");
		if (this._ServerRunning)
		{
			this.MainThread = new Thread(_MainThread);
			this.MainThread.Start();
		}
		return this._ServerRunning;
	}

	public void stop(string[] args = null)
	{
		if (args == null)
		{
			args = new string[1];
		}
		else
		{
			Array.Resize(ref args, args.Length + 1);
			for (int num = args.Length - 1; num > 0; num--)
			{
				args[num] = args[num - 1];
			}
		}
		args[0] = "stop";
		Native.librtk_stop(ret: new StringBuilder(16384), args: args, narg: args.Length);
		this.Running = false;
		if (this.MainThread != null && (this.MainThread.ThreadState & (ThreadState.Unstarted | ThreadState.Stopped | ThreadState.Aborted)) == 0)
		{
			this.MainThread.Abort();
		}
	}

	public void restart(string[] args = null)
	{
		if (args == null)
		{
			args = new string[1];
		}
		else
		{
			Array.Resize(ref args, args.Length + 1);
			for (int num = args.Length - 1; num > 0; num--)
			{
				args[num] = args[num - 1];
			}
		}
		args[0] = "restart";
		Native.librtk_restart(ret: new StringBuilder(16384), args: args, narg: args.Length);
	}

	public string solution(string[] args = null)
	{
		if (args == null)
		{
			args = new string[1];
		}
		else
		{
			Array.Resize(ref args, args.Length + 1);
			for (int num = args.Length - 1; num > 0; num--)
			{
				args[num] = args[num - 1];
			}
		}
		args[0] = "solution";
		StringBuilder stringBuilder;
		stringBuilder = new StringBuilder(16384);
		Native.librtk_solution(args, args.Length, stringBuilder);
		return stringBuilder.ToString();
	}

	private string status(string[] args = null)
	{
		if (args == null)
		{
			args = new string[1];
		}
		else
		{
			Array.Resize(ref args, args.Length + 1);
			for (int num = args.Length - 1; num > 0; num--)
			{
				args[num] = args[num - 1];
			}
		}
		args[0] = "status";
		StringBuilder stringBuilder;
		stringBuilder = new StringBuilder(16384);
		Native.librtk_status(args, args.Length, stringBuilder);
		return stringBuilder.ToString();
	}

	public void satellite(string[] args = null)
	{
		if (args == null)
		{
			args = new string[1];
		}
		else
		{
			Array.Resize(ref args, args.Length + 1);
			for (int num = args.Length - 1; num > 0; num--)
			{
				args[num] = args[num - 1];
			}
		}
		args[0] = "satellite";
		Native.librtk_satellite(ret: new StringBuilder(16384), args: args, narg: args.Length);
	}

	public void observ(string[] args = null)
	{
		if (args == null)
		{
			args = new string[1];
		}
		else
		{
			Array.Resize(ref args, args.Length + 1);
			for (int num = args.Length - 1; num > 0; num--)
			{
				args[num] = args[num - 1];
			}
		}
		args[0] = "observ";
		Native.librtk_observ(ret: new StringBuilder(16384), args: args, narg: args.Length);
	}

	public void navidata(string[] args = null)
	{
		if (args == null)
		{
			args = new string[1];
		}
		else
		{
			Array.Resize(ref args, args.Length + 1);
			for (int num = args.Length - 1; num > 0; num--)
			{
				args[num] = args[num - 1];
			}
		}
		args[0] = "navidata";
		Native.librtk_navidata(ret: new StringBuilder(16384), args: args, narg: args.Length);
	}

	public void stream(string[] args = null)
	{
		if (args == null)
		{
			args = new string[1];
		}
		else
		{
			Array.Resize(ref args, args.Length + 1);
			for (int num = args.Length - 1; num > 0; num--)
			{
				args[num] = args[num - 1];
			}
		}
		args[0] = "stream";
		Native.librtk_stream(ret: new StringBuilder(16384), args: args, narg: args.Length);
	}

	private string option(string[] args = null)
	{
		if (args == null)
		{
			args = new string[1];
		}
		else
		{
			Array.Resize(ref args, args.Length + 1);
			for (int num = args.Length - 1; num > 0; num--)
			{
				args[num] = args[num - 1];
			}
		}
		args[0] = "option";
		StringBuilder stringBuilder;
		stringBuilder = new StringBuilder(16384);
		Native.librtk_option(args, args.Length, stringBuilder);
		return stringBuilder.ToString();
	}

	private void set(string[] args = null)
	{
		if (args != null)
		{
			Array.Resize(ref args, args.Length + 1);
			for (int num = args.Length - 1; num > 0; num--)
			{
				args[num] = args[num - 1];
			}
			args[0] = "set";
			Native.librtk_set(ret: new StringBuilder(16384), args: args, narg: args.Length);
		}
	}

	public void load(string[] args = null)
	{
		if (args == null)
		{
			args = new string[1];
		}
		else
		{
			Array.Resize(ref args, args.Length + 1);
			for (int num = args.Length - 1; num > 0; num--)
			{
				args[num] = args[num - 1];
			}
		}
		args[0] = "load";
		Native.librtk_load(ret: new StringBuilder(16384), args: args, narg: args.Length);
	}

	public void save(string[] args = null)
	{
		if (args == null)
		{
			args = new string[1];
		}
		else
		{
			Array.Resize(ref args, args.Length + 1);
			for (int num = args.Length - 1; num > 0; num--)
			{
				args[num] = args[num - 1];
			}
		}
		args[0] = "save";
		Native.librtk_save(ret: new StringBuilder(16384), args: args, narg: args.Length);
	}

	public void log(string[] args = null)
	{
		if (args == null)
		{
			args = new string[1];
		}
		else
		{
			Array.Resize(ref args, args.Length + 1);
			for (int num = args.Length - 1; num > 0; num--)
			{
				args[num] = args[num - 1];
			}
		}
		args[0] = "log";
		Native.librtk_log(ret: new StringBuilder(16384), args: args, narg: args.Length);
	}

	public void help(string[] args = null)
	{
		if (args == null)
		{
			args = new string[1];
		}
		Native.librtk_help(ret: new StringBuilder(16384), args: args, narg: args.Length);
	}

	private void shut()
	{
		Native.librtk_exit();
	}

	private string optStr(string lastMessage)
	{
		string text;
		text = lastMessage;
		if (text != "")
		{
			text = text.Substring(text.LastIndexOf("=") + 1);
			if (text.Contains("#"))
			{
				text = text.Split("#".ToCharArray())[0];
			}
		}
		return text;
	}

	private void _MainThread()
	{
		this.Running = true;
		while (this.Running)
		{
			string[] array;
			array = this.status().Split("\n".ToCharArray());
			foreach (string text in array)
			{
				if (text.Length < 28)
				{
					continue;
				}
				try
				{
					switch (text.Substring(0, 28))
					{
					case "rtk server state            ":
						this._ServerRunning = text.Substring(30, 3) == "run";
						if (!this._ServerRunning)
						{
							this.Running = false;
						}
						break;
					case "solution status             ":
						this._SolutionStatus = (solstatus)Enum.Parse(typeof(solstatus), text.Substring(30).Replace("float", "flot").Replace("-", "NA"));
						if (this._SolutionStatus != this._OldSolutionStatus && this.SolutionStatusChanged != null)
						{
							this._OldSolutionStatus = this._SolutionStatus;
							this.SolutionStatusChanged(this, new SolutionStatusEventArgs(this._SolutionStatus));
						}
						break;
					case "# of satellites rover       ":
						this._SatellitesRover = int.Parse(text.Substring(30));
						break;
					case "# of satellites base        ":
						this._SatellitesBase = int.Parse(text.Substring(30));
						break;
					case "# of valid satellites       ":
						this._SatellitesValid = int.Parse(text.Substring(30));
						break;
					case "pos llh single (deg,m) rover":
					{
						string[] array2;
						array2 = text.Substring(30).Split(",".ToCharArray());
						this._RoverLatitude = double.Parse(array2[0], NumberStyles.Any, NumberFormatInfo.InvariantInfo);
						this._RoverLongitude = double.Parse(array2[1], NumberStyles.Any, NumberFormatInfo.InvariantInfo);
						this._RoverHeight = double.Parse(array2[2], NumberStyles.Any, NumberFormatInfo.InvariantInfo);
						break;
					}
					case "pos llh (deg,m) base        ":
					{
						string[] array2;
						array2 = text.Substring(30).Split(",".ToCharArray());
						this._BaseLatitude = double.Parse(array2[0], NumberStyles.Any, NumberFormatInfo.InvariantInfo);
						this._BaseLongitude = double.Parse(array2[1], NumberStyles.Any, NumberFormatInfo.InvariantInfo);
						this._BaseHeight = double.Parse(array2[2], NumberStyles.Any, NumberFormatInfo.InvariantInfo);
						break;
					}
					case "baseline length float (m)   ":
						this._BaselineFloat = double.Parse(text.Substring(30), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
						break;
					case "baseline length fixed (m)   ":
						this._BaselineFixed = double.Parse(text.Substring(30), NumberStyles.Any, NumberFormatInfo.InvariantInfo);
						break;
					}
				}
				catch
				{
					Console.WriteLine("Parse error.");
				}
			}
			Thread.Sleep(500);
		}
	}
}
