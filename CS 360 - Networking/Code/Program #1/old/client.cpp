/*
Michael Beaver
CS 360 - Computer Networking
Program #0 - Daytime Small Server (RFC 867)
6 February 2015
Program Description: 
        This program implements a client that connects to a server, which
        returns the daytime. Then this client displays the daytime and closes
        the connection before terminating.
*/
#include <stdio.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <netdb.h>

#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <fcntl.h>
#include <sys/stat.h>

//------------------------------------------------------------------------

const int SERVER_PORT = 1113;
const int MAX_MSG = 256;

//------------------------------------------------------------------------

/*
Function: main
Purpose: main is the main entry-point and establishes a connection and 
        displays the daytime received from the daytime server (as specified 
        by RFC 867).
Parameters: N/A
Return: 0 on completion, 1 on failure
*/
int main(int argc, char * argv[]) {

	struct hostent * hp;
	struct sockaddr_in sin;
	char * host;
	char buffer[MAX_MSG];
	int s;

	if (argc == 2) 
		host = argv[1];	
	
	else {
		
		printf("Usage: SimplexTalk host\n");
		exit(1);

	}

	// Translate host name into peer's IP address
	hp = gethostbyname(host);
	if (!hp) {

		printf("SimplexTalk: Unknown host: %s\n", host);
		printf("SimplexTalk: Client terminating.\n");
		exit(1);

	}

	// Build address data structure
	memset((char *) &sin, 0, sizeof(sin));
	sin.sin_family = AF_INET;
	memcpy((char *) &sin.sin_addr, hp->h_addr, hp->h_length);
	sin.sin_port = htons(SERVER_PORT);

	// Active open
	if ((s = socket(PF_INET, SOCK_STREAM, 0)) < 0) {

		printf("SimplexTalk: Unable to create a socket.\n");
		printf("SimplexTalk: Unable to connect to the network.\n");
		printf("SimplexTalk: Client terminating.\n");
		exit(1);

	}

	if (connect(s, (struct sockaddr *) &sin, sizeof(sin)) < 0) {

		printf("SimplexTalk: Unable to connect to the server.\n");
		printf("SimplexTalk: Client terminating.\n");
		close(s);
		exit(1);

	}

        
        // Prompt the server to return the daytime and then display it
	send(s, 0, 0, 0);
	
	if (recv(s, buffer, sizeof(buffer), 0) > 0)
		printf("%s", buffer);
	
	else {

		printf("SimplexTalk: The server did not return the time.\n");
		printf("SimplexTalk: Client terminating.\n");

	}

	close(s);

	return 0; 	

}
