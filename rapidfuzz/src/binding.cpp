#include <algorithm>
#include <iostream>
#include <string>
#include <vector>
#include <memory>
#include "binding.h"
#include <rapidfuzz/fuzz.hpp>

using namespace std;

extern "C" {
	ADDExport double _cdecl ratio(wchar_t* const s1, wchar_t* const s2, double score_cut_off){
		return rapidfuzz::fuzz::ratio(s1, s2, score_cut_off);
	}
	ADDExport double _cdecl token_ratio(wchar_t* const s1, wchar_t* const s2, double score_cut_off){
		 return rapidfuzz::fuzz::token_ratio(s1, s2, score_cut_off);
	}
	ADDExport double _cdecl partial_ratio(wchar_t* const s1, wchar_t* const s2, double score_cut_off){
		return rapidfuzz::fuzz::partial_ratio(s1, s2, score_cut_off);
	}
	ADDExport double _cdecl token_set_ratio(wchar_t* const s1, wchar_t* const s2, double score_cut_off){
		return rapidfuzz::fuzz::token_set_ratio(s1, s2, score_cut_off);
	}
	ADDExport double _cdecl partial_token_ratio(wchar_t* const s1, wchar_t* const s2, double score_cut_off){
		return rapidfuzz::fuzz::partial_token_ratio(s1, s2, score_cut_off);
	}
	ADDExport double _cdecl partial_token_set_ratio(wchar_t* const s1, wchar_t* const s2, double score_cut_off){
		return rapidfuzz::fuzz::partial_token_set_ratio(s1, s2, score_cut_off);
	}
	ADDExport double _cdecl partial_token_sort_ratio(wchar_t* const s1, wchar_t* const s2, double score_cut_off){
		return rapidfuzz::fuzz::partial_token_sort_ratio(s1, s2, score_cut_off);
	}
	ADDExport double _cdecl token_sort_ratio(wchar_t* const s1, wchar_t* const s2, double score_cut_off){
		return rapidfuzz::fuzz::token_sort_ratio(s1, s2, score_cut_off);
	}
	ADDExport double _cdecl w_ratio(wchar_t* const s1, wchar_t* const s2, double score_cut_off){
		return rapidfuzz::fuzz::WRatio(s1, s2, score_cut_off);
	}
	ADDExport double _cdecl q_ratio(wchar_t* const s1, wchar_t* const s2, double score_cut_off){
		return rapidfuzz::fuzz::QRatio(s1, s2, score_cut_off);
	}
}
