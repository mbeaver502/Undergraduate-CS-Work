all: server client

server: server.cpp
	g++ -Wall -g -o server server.cpp

client: client.cpp
	g++ -Wall -g -o SimplexTalk client.cpp

clean:
	rm -f *core server SimplexTalk *o
