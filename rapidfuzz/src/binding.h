#pragma once
#define _cdecl
#ifdef __declspec(dllexport)
#define ADDExport __declspec (dllexport)
#else
#define ADDExport

#endif

#define RAPIDFUZZ_EXCLUDE_SIMD

extern "C" {
	ADDExport double _cdecl ratio(wchar_t* const s1, wchar_t* const s2, double score_cut_off);
	ADDExport double _cdecl token_ratio(wchar_t* const s1, wchar_t* const s2, double score_cut_off);
	ADDExport double _cdecl partial_ratio(wchar_t* const s1, wchar_t* const s2, double score_cut_off);
	ADDExport double _cdecl token_set_ratio(wchar_t* const s1, wchar_t* const s2, double score_cut_off);
	ADDExport double _cdecl partial_token_ratio(wchar_t* const s1, wchar_t* const s2, double score_cut_off);
	ADDExport double _cdecl partial_token_set_ratio(wchar_t* const s1, wchar_t* const s2, double score_cut_off);
	ADDExport double _cdecl partial_token_sort_ratio(wchar_t* const s1, wchar_t* const s2, double score_cut_off);
	ADDExport double _cdecl token_sort_ratio(wchar_t* const s1, wchar_t* const s2, double score_cut_off);
	ADDExport double _cdecl w_ratio(wchar_t* const s1, wchar_t* const s2, double score_cut_off);
	ADDExport double _cdecl q_ratio(wchar_t* const s1, wchar_t* const s2, double score_cut_off);
}
