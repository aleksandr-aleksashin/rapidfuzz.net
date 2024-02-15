OBJ_DIR := ./obj
SRC_FILES := $(wildcard ./src/*.cpp)

OBJ_FILES := $(SRC_FILES:.cpp=.o)

LDFLAGS := -I../externs/rapidfuzz -fPIC -O3 -std=c++17 -shared -ldl -fshort-wchar
CPPFLAGS := -I../externs/rapidfuzz -fPIC -O3 -std=c++17 -shared -ldl -fshort-wchar

NativeRapidFuzz.so: $(OBJ_FILES)
#	echo $(OBJ_FILES)
	echo $(TESTS)
	 g++ $(LDFLAGS) -o $@ $^

clean:
	rm $(OBJ_FILES)
.o: 
	 g++ $(CPPFLAGS)  -c -o $@ $<