ARG BASEIMAGE="arm64v8/debian"
FROM $BASEIMAGE
RUN apt update && apt install build-essential make git -y --no-install-recommends
RUN apt-get install -y --reinstall ca-certificates
WORKDIR /home/app/
ADD . .
RUN git submodule update --init
WORKDIR /home/app/rapidfuzz
RUN make

# from root / of repository

# linux-arm64
# docker build -t rapidfuzz-csharp:arm64-latest  --build-arg BASEIMAGE="arm64v8/debian" -f ./build/linux.Dockerfile .
# docker create --name rapidfuzz-csharp-latest-arm64 rapidfuzz-csharp:arm64-latest
# docker cp rapidfuzz-csharp-latest-arm64:/home/app/rapidfuzz/NativeRapidFuzz.so ./RapidFuzz.Net/RapidFuzz.Net/lib/linux-arm64/ 

# linux-x64
# docker build -t rapidfuzz-csharp:x64-latest  --build-arg BASEIMAGE="amd64/ubuntu:18.04" -f ./build/linux.Dockerfile .
# docker create --name rapidfuzz-csharp-latest-x64 rapidfuzz-csharp:x64-latest
# docker cp rapidfuzz-csharp-latest-x64:/home/app/rapidfuzz/NativeRapidFuzz.so ./RapidFuzz.Net/RapidFuzz.Net/lib/linux-x64/ 
