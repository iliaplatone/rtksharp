using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libRTK;

internal static class Native
{
	[DllImport("rtk", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	internal static extern IntPtr librtk_start([In] string[] args, int narg, StringBuilder ret);

	[DllImport("rtk", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	internal static extern IntPtr librtk_stop([In] string[] args, int narg, StringBuilder ret);

	[DllImport("rtk", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	internal static extern IntPtr librtk_restart([In] string[] args, int narg, StringBuilder ret);

	[DllImport("rtk", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	internal static extern IntPtr librtk_solution([In] string[] args, int narg, StringBuilder ret);

	[DllImport("rtk", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	internal static extern IntPtr librtk_status([In] string[] args, int narg, StringBuilder ret);

	[DllImport("rtk", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	internal static extern IntPtr librtk_satellite([In] string[] args, int narg, StringBuilder ret);

	[DllImport("rtk", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	internal static extern IntPtr librtk_observ([In] string[] args, int narg, StringBuilder ret);

	[DllImport("rtk", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	internal static extern IntPtr librtk_navidata([In] string[] args, int narg, StringBuilder ret);

	[DllImport("rtk", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	internal static extern IntPtr librtk_stream([In] string[] args, int narg, StringBuilder ret);

	[DllImport("rtk", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	internal static extern IntPtr librtk_error([In] string[] args, int narg, StringBuilder ret);

	[DllImport("rtk", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	internal static extern IntPtr librtk_option([In] string[] args, int narg, StringBuilder ret);

	[DllImport("rtk", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	internal static extern IntPtr librtk_set([In] string[] args, int narg, StringBuilder ret);

	[DllImport("rtk", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	internal static extern IntPtr librtk_load([In] string[] args, int narg, StringBuilder ret);

	[DllImport("rtk", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	internal static extern IntPtr librtk_save([In] string[] args, int narg, StringBuilder ret);

	[DllImport("rtk", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	internal static extern IntPtr librtk_log([In] string[] args, int narg, StringBuilder ret);

	[DllImport("rtk", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	internal static extern IntPtr librtk_help([In] string[] args, int narg, StringBuilder ret);

	[DllImport("rtk", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	internal static extern int librtk_init(int argc, [In] string[] args);

	[DllImport("rtk", CallingConvention = CallingConvention.Cdecl)]
	internal static extern void librtk_exit();
}
