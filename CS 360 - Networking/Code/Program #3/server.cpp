/*
Michael Beaver
CS 360 - Computer Networking
Program #3 - Daytime Small Server UDP (RFC 867)
29 April 2015
Program Description: 
	This program implements a server that shall accept connections
	from a client via UDP. Once a connection is established, the current
	time is reported back to the client (i.e., RFC 867). The server may
        be killed by sending the SIGTERM signal. All open sockets are closed
	prior to program termination. In the event that a socket cannot be
	bound, the server will retry binding every BIND_RETRY seconds.
	If the number of retries exceeds BIND_LIMIT, then the server aborts
	any further attempts and terminates.
Resources consulted:
	http://www.cs.ucsb.edu/~almeroth/classes/W01.176B/hw2/examples/udp-server.c
	Various man pages
*/
#include <stdio.h>
#include <sys/types.h>
#include <netinet/in.h>
#include <netdb.h>
#include <sys/socket.h>

#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <fcntl.h>
#include <sys/stat.h>

#include <time.h>
#include <signal.h>

//------------------------------------------------------------------------

const int SERVER_PORT = 1113;
const int MAX_PENDING = 5;
const int MAX_MSG     = 256;
const unsigned int BIND_RETRY = 15;
const unsigned int BIND_LIMIT = 5;

static volatile sig_atomic_t timeToQuit = 0;

//------------------------------------------------------------------------

/*
Function: signalHandler
Purpose: signalHandler is the handler to catch the SIGTERM signal (via
	sigaction). This function will set the global flag timeToQuit
	to 1, which will lead to the abortion of the main loop in 
	main() and terminate the server.
Parameters: signal is the integer value of the handled signal.
Return: N/A (void)
*/
static void signalHandler(int signal) { timeToQuit = 1; }


/*
Function: main
Purpose: main is the main entry-point and contains the main loop that
	establishes connections and reports the current time (as
	specified by RFC 867).
Parameters: N/A
Return: 0 on completion, 1 on failure or on SIGTERM
*/
int main() {

        struct sockaddr_in sin, client;
        unsigned int bindAttempts = 0;
        int s;
        time_t currentTime;
        struct sigaction sa;
	char buffer[MAX_MSG];
	socklen_t clientLength = sizeof(client);
	int recvLength;

        printf("DSS: Shutdown: kill -TERM %d\n\n", getpid()); 

	
        // Create and register the signal handler for SIGTERM
        sa.sa_handler = &signalHandler;
        sa.sa_flags = 0;
        sigemptyset(&sa.sa_mask);

        if (sigaction(SIGTERM, &sa, NULL) < 0) {
		
                printf("DSS: Unable to hook SIGTERM handler.\n");
                exit(1);

        }

	
        // Build address data structure
        memset((char *) &sin, 0, sizeof(sin));
        sin.sin_family = AF_INET;
        sin.sin_addr.s_addr = htonl(INADDR_ANY);
        sin.sin_port = htons(SERVER_PORT); 
       
	
        // Passive open
        if ((s = socket(AF_INET, SOCK_DGRAM, 0)) < 0) {
    
                printf("DSS: Unable to open a socket.\n");
                exit(1);
    
        }

	/* Here we will continue attempting to bind every BIND_RETRY seconds
	until success or until a limit (BIND_LIMIT) is exceeded. */
        while (bind(s, (struct sockaddr *) &sin, sizeof(sin)) < 0) {	
		
		bindAttempts++;

                printf("DSS: Unable to bind socket.\n");

                if (bindAttempts > BIND_LIMIT) {
			
                        printf("DSS: Exceeded allotted tries (%d).\n",
                                BIND_LIMIT);
                        printf("DSS: Closing socket.\n");
			close(s);
			printf("\nDSS: Server terminated.\n");
                        exit(1);

                } else {

			printf("DSS: Retrying in %d seconds.\n", BIND_RETRY);
                	sleep(BIND_RETRY);

		}

		// In case of termination while retrying bind
		if (timeToQuit) {
			
			printf("DSS: Closing socket.\n");
			close(s);
			printf("\nDSS: Server terminated.\n");			
			exit(1);

		}

        }
    
        listen(s, MAX_PENDING);
   
        printf("DSS: Server is now accepting connections.\n");
	


        // Main loop: Accept connections and output the current time 
        while (!timeToQuit) {

		memset(buffer, 0, MAX_MSG);
        	memset((char *) &client, 0, sizeof(client));
		
		recvLength = recvfrom(s, buffer, MAX_MSG, 0, 
				(struct sockaddr *) &client, &clientLength);

		// Reply to the client
		if (recvLength >= 0) {
			
			memset(buffer, 0, MAX_MSG);
			time(&currentTime);
			sendto(s, ctime(&currentTime), MAX_MSG, 0, 
				(struct sockaddr *) &client, clientLength);

		}		

		else {

			printf("DSS: Unable to receive datagram.\n");
			printf("DSS: Closing socket.\n");
			close(s);
			printf("DSS: Server terminated.\n");
			exit(1);

		}
    	
	}
   
 
        printf("DSS: Closing socket.\n");
        close(s);
        printf("\nDSS: Server terminated.\n");

        return 0;

}
