#pragma once
#define _cdecl
#ifdef __declspec(dllexport)
#define ADDExport __declspec (dllexport)
#else
#define ADDExport

#endif

#define RAPIDFUZZ_EXCLUDE_SIMD

extern "C" {
	ADDExport double _cdecl ratio(char* const s1, char* const s2, double score_cut_off);
	ADDExport double _cdecl token_ratio(char* const s1, char* const s2, double score_cut_off);
	ADDExport double _cdecl partial_ratio(char* const s1, char* const s2, double score_cut_off);
	ADDExport double _cdecl token_set_ratio(char* const s1, char* const s2, double score_cut_off);
	ADDExport double _cdecl partial_token_ratio(char* const s1, char* const s2, double score_cut_off);
	ADDExport double _cdecl partial_token_set_ratio(char* const s1, char* const s2, double score_cut_off);
	ADDExport double _cdecl partial_token_sort_ratio(char* const s1, char* const s2, double score_cut_off);
}
