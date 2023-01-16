#include <algorithm>
#include <iostream>
#include <string>
#include <vector>
#include <memory>
#include "binding.h"
#include <rapidfuzz/fuzz.hpp>

using namespace std;

extern "C" {
	ADDExport double _cdecl ratio(char* const s1, char* const s2, double score_cut_off){
		return rapidfuzz::fuzz::ratio(s1, s2, score_cut_off);
	}
	ADDExport double _cdecl token_ratio(char* const s1, char* const s2, double score_cut_off){
		 return rapidfuzz::fuzz::token_ratio(s1, s2, score_cut_off);
	}
	ADDExport double _cdecl partial_ratio(char* const s1, char* const s2, double score_cut_off){
		return rapidfuzz::fuzz::partial_ratio(s1, s2, score_cut_off);
	}
	ADDExport double _cdecl token_set_ratio(char* const s1, char* const s2, double score_cut_off){
		return rapidfuzz::fuzz::token_set_ratio(s1, s2, score_cut_off);
	}
	ADDExport double _cdecl partial_token_ratio(char* const s1, char* const s2, double score_cut_off){
		return rapidfuzz::fuzz::partial_token_ratio(s1, s2, score_cut_off);
	}
	ADDExport double _cdecl partial_token_set_ratio(char* const s1, char* const s2, double score_cut_off){
		return rapidfuzz::fuzz::partial_token_set_ratio(s1, s2, score_cut_off);
	}
	ADDExport double _cdecl partial_token_sort_ratio(char* const s1, char* const s2, double score_cut_off){
		return rapidfuzz::fuzz::partial_token_sort_ratio(s1, s2, score_cut_off);
	}
	ADDExport double _cdecl token_sort_ratio(char* const s1, char* const s2, double score_cut_off){
		return rapidfuzz::fuzz::token_sort_ratio(s1, s2, score_cut_off);
	}
	ADDExport double _cdecl w_ratio(char* const s1, char* const s2, double score_cut_off){
		return rapidfuzz::fuzz::WRatio(s1, s2, score_cut_off);
	}
	ADDExport double _cdecl q_ratio(char* const s1, char* const s2, double score_cut_off){
		return rapidfuzz::fuzz::QRatio(s1, s2, score_cut_off);
	}
}
