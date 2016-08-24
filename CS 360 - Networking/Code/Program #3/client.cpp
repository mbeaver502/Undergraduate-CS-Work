/*
Michael Beaver
CS 360 - Computer Networking
Program #3 - Daytime Small Server UDP (RFC 867)
29 April 2015
Program Description: 
        This program implements a client that connects to a server via UDP, which
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
	socklen_t addrLength = sizeof(sin);

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

	memcpy(buffer, "Hello World!", strlen("Hello World!"));

	s = socket(AF_INET, SOCK_DGRAM, 0);

	// Prompt the server for a response	
	if (sendto(s, buffer, MAX_MSG, 0,
		(struct sockaddr *) &sin, sizeof(sin)) < 0) {

		printf("SimplexTalk: Error sending datagram.\n");
		close(s);
		exit(1);	

	}	

	// Output results from server
	if (recvfrom(s, buffer, MAX_MSG, 0, (struct sockaddr *) &sin, &addrLength) > 0)
		printf("%s", buffer);

	else {

		printf("SimplexTalk: Unable to receive datagrams.\n");
		printf("SimplexTalk: Closing socket.\n");

	}

	close(s);

	return 0; 	

}
