/*
  *  RTKLIB C wrapper
  * Copyright (C) 2016 Ilia Platone <info@iliaplatone.com>
  *
  * This program is free software: you can redistribute it and/or modify
  * it under the terms of the GNU General Public License as published by
  * the Free Software Foundation, either version 3 of the License, or
  * (at your option) any later version.
  *
  * This program is distributed in the hope that it will be useful,
  * but WITHOUT ANY WARRANTY; without even the implied warranty of
  * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  * GNU General Public License for more details.
  *
  * You should have received a copy of the GNU General Public License
  * along with this program.  If not, see <http://www.gnu.org/licenses/>.
  *
  */

#ifndef __LIBRTK_H
#define __LIBRTK_H


#ifdef _WIN32
#define DLL_EXPORT __declspec(dllexport)
#else
#define _POSIX_C_SOURCE 2
#define DLL_EXPORT extern 
#endif

DLL_EXPORT char * librtk_start (char **args, int narg, char *ret);
DLL_EXPORT char * librtk_stop (char **args, int narg, char *ret);
DLL_EXPORT char * librtk_restart (char **args, int narg, char *ret);
DLL_EXPORT char * librtk_solution (char **args, int narg, char *ret);
DLL_EXPORT char * librtk_status (char **args, int narg, char *ret);
DLL_EXPORT char * librtk_satellite (char **args, int narg, char *ret);
DLL_EXPORT char * librtk_observ (char **args, int narg, char *ret);
DLL_EXPORT char * librtk_navidata (char **args, int narg, char *ret);
DLL_EXPORT char * librtk_stream (char **args, int narg, char *ret);
DLL_EXPORT char * librtk_error (char **args, int narg, char *ret);
DLL_EXPORT char * librtk_option (char **args, int narg, char *ret);
DLL_EXPORT char * librtk_set (char **args, int narg, char *ret);
DLL_EXPORT char * librtk_load (char **args, int narg, char *ret);
DLL_EXPORT char * librtk_save (char **args, int narg, char *ret);
DLL_EXPORT char * librtk_log (char **args, int narg, char *ret);
DLL_EXPORT char * librtk_help (char **args, int narg, char *ret);
DLL_EXPORT int librtk_init (int argc, char **argv);
DLL_EXPORT void librtk_exit();
#endif
