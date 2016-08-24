/*
Michael Beaver
CS 360 - Computer Networking
Program #2 - Simple Web Server 
8 April 2015
Program Description:
	This program implements a simple HTTP 0.9 web server. This server is capable of
	handling only the GET method and supports only the following file types: HTML, CSS,
	JS, JPG, PNG, and GIF. This implementation does not allow specification of the root
	folder via the command line. This implementation does, however, keep a primitive
	log on the console of files accessed and the daytime they are accessed.
Resources consulted:
	Various man pages
	http://beej.us/guide/bgnet/output/html/multipage/advanced.html#select
	http://www.w3.org/Protocols/HTTP/AsImplemented.html
	http://www.lowtek.com/sockets/select.html
	http://www.go4expert.com/articles/understanding-linux-fstat-example-t27449/
	http://stackoverflow.com/questions/5840148/how-can-i-get-a-files-size-in-c
	http://stackoverflow.com/questions/12975341/to-string-is-not-a-member-of-std-says-so-g
*/


#include <stdio.h>
#include <sys/types.h>
#include <sys/time.h>
#include <netinet/in.h>
#include <netdb.h>
#include <sys/socket.h>

#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <fcntl.h>
#include <sys/stat.h>

#include <signal.h>
#include <string>
#include <iostream>
#include <fstream>
#include <sstream>
#include <time.h>

using namespace std;


//---------------------------------------------------------------------------
//                              CONSTANTS
//---------------------------------------------------------------------------

const int SERVER_PORT 			= 8181;
const int MAX_PENDING 			= 5;
const int TIMEOUT_SECONDS		= 1;
const int TIMEOUT_USECONDS		= 0;
const unsigned int BIND_RETRY 		= 15;
const unsigned int BIND_LIMIT 		= 5;
const int URI_MAX_LEN	 		= 64;

const string STATUS_OK 			= "200 OK";
const string STATUS_BAD_REQUEST		= "400 Bad Request";
const string STATUS_NOT_FOUND 		= "404 Not Found";
const string STATUS_URI_TOO_LONG	= "414 Request-URI Too Long";
const string STATUS_NOT_IMPLEMENTED	= "501 Not Implemented";

const string MIME_HTML	 		= "text/html";
const string MIME_CSS	 		= "text/css";
const string MIME_JS	 		= "application/js";
const string MIME_JPEG	 		= "image/jpeg";
const string MIME_PNG			= "image/png";
const string MIME_GIF	 		= "image/gif";

const string HEADER_STATUS		= "HTTP/1.0";
const string HEADER_CONN		= "Connection: close";
const string HEADER_TYPE		= "Content-Type:";
const string HEADER_LENGTH		= "Content-Length:";
const string HEADER_SERVER		= "Server: cs360httpd/1.0 (Unix)";

const char * INDEX_HTML 		= "index.html";

static volatile sig_atomic_t timeToQuit = 0;


//---------------------------------------------------------------------------
//                              PROTOTYPES
//---------------------------------------------------------------------------

static void signalHandler(int signal);

void handler(int s, struct sockaddr_in sin);
string getURI(char buffer[URI_MAX_LEN], bool & flag);
string getType(char uri[URI_MAX_LEN]);


//---------------------------------------------------------------------------
//                              FUNCTIONS
//---------------------------------------------------------------------------

/*
Function: main
Purpose: Main entrypoint that selects connections and has handler() handle
	them appropriately.
Parameters: N/A
Return: 0 on completion, 1 on failure or on SIGTERM
*/
int main() {

        struct sockaddr_in sin;
        unsigned int bindAttempts = 0;
        int s;
        struct sigaction sa;
	fd_set rfds;
	struct timeval tv;
	int retval;

        printf("SWS: Shutdown: kill -TERM %d\n\n", getpid()); 

	
        // Create and register the signal handler for SIGTERM
        sa.sa_handler = &signalHandler;
        sa.sa_flags = 0;
        sigemptyset(&sa.sa_mask);

        if (sigaction(SIGTERM, &sa, NULL) < 0) {
                printf("SWS: Unable to hook SIGTERM handler.\n");
                exit(1);
        }

	
        // Build address data structure
        memset((char *) &sin, 0, sizeof(sin));
        sin.sin_family = AF_INET;
        sin.sin_addr.s_addr = INADDR_ANY;
        sin.sin_port = htons(SERVER_PORT); 
       
	
        // Passive open
        if ((s = socket(PF_INET, SOCK_STREAM, 0)) < 0) {
                printf("SWS: Unable to open a socket.\n");
                exit(1);
        }

        /* Here we will continue attempting to bind every BIND_RETRY seconds
        until success or until a limit (BIND_LIMIT) is exceeded. */
        while (bind(s, (struct sockaddr *) &sin, sizeof(sin)) < 0) {	
		
                bindAttempts++;

                printf("SWS: Unable to bind socket.\n");

                if (bindAttempts > BIND_LIMIT) {
                        printf("SWS: Exceeded allotted tries (%d).\n",
                                BIND_LIMIT);
                        printf("SWS: Closing socket.\n");
                        close(s);
                        printf("\nSWS: Server terminated.\n");
                        exit(1);
                } else {
                    	printf("SWS: Retrying in %d seconds.\n", BIND_RETRY);
                	sleep(BIND_RETRY);
                }

                // In case of termination while retrying bind
                if (timeToQuit) {
                        printf("SWS: Closing socket.\n");
                        close(s);
                        printf("\nSWS: Server terminated.\n");			
                        exit(1);
                }

        }
    
        listen(s, MAX_PENDING);
   
        printf("SWS: Server is now accepting connections.\n\n");
	

        // Main loop: Nonblocking select that will connect clients
        while (!timeToQuit) {
    	
		FD_ZERO(&rfds);
		FD_SET(s, &rfds);
		tv.tv_sec = TIMEOUT_SECONDS;
		tv.tv_usec = TIMEOUT_USECONDS;

		retval = select(s+1, &rfds, NULL, NULL, &tv);

		if (retval == -1) {
			printf("SWS: Unable to connect.\n");
			printf("SWS: Closing socket.\n");
			close(s);
		}

		// Selection made, proceed to accept connection
		else if (retval) {
			if (FD_ISSET(s, &rfds))
				handler(s, sin);
		}

        }
   
        printf("SWS: Closing sockets.\n");
        close(s);
        printf("SWS: Server terminated.\n");

        return 0;

}

//---------------------------------------------------------------------------


/*
Function: signalHandler
Purpose: This function is the handler to catch the SIGTERM signal (via
	sigaction). This function will set the global flag timeToQuit
	to 1, which will lead to the abortion of the main loop in 
	main() and terminate the server.
Parameters: signal is the integer value of the handled signal
Return: N/A (void)
*/
static void signalHandler(int signal) { 

        timeToQuit = 1; 

}

//---------------------------------------------------------------------------


/*
Function: handler
Purpose: This is the function that handles all incoming connections once they
	are selected in main(). This function will build the header and send
	the requested file data over the connection.
Parameters: s is the socket, sin is the socket information
Return: N/A (void)
*/
void handler(int s, struct sockaddr_in sin) {

	int len = sizeof(sin);
	int new_s = accept(s, (struct sockaddr*)&sin, (socklen_t*)&len);	
	char buffer[URI_MAX_LEN];
	string header = HEADER_STATUS + " ";
	char uri[URI_MAX_LEN];
	bool goodURI = false;
	ifstream file;
	struct stat fileStat;
	int fileSize = 0;
	bool notFound = false;
	time_t currentTime;

	if (new_s < 0) {
		printf("SWS: Unable to connect.\n");
		printf("SWS: Closing socket.\n");
		close(s);
	}

	else {

		if (recv(new_s, buffer, URI_MAX_LEN, 0) < 1) 
			header += STATUS_BAD_REQUEST;
		
		else {

			// Accept only the GET method
			if (buffer[0] == 'G' && buffer[1] == 'E' && buffer[2] == 'T') {
				if (buffer[3] == ' ' && buffer[4] == '/') {

					// Extract the requested file from the GET request
					strncpy(uri, 
						getURI(buffer, goodURI).c_str(), 
						URI_MAX_LEN);

					if (goodURI) {

						// Does the file exist?
						file.open(uri, ios::binary);

						if (!file.is_open()) {
							header += STATUS_NOT_FOUND;
							notFound = true;
						}		
						
						// Gather file information if possible
						else if (stat(uri, &fileStat) < 0) {
							header += STATUS_BAD_REQUEST;
							notFound = true;
						}
						
						else {
							fileSize = fileStat.st_size;
							header += STATUS_OK;
						}

						file.close();
					}	

					else {		
						header += STATUS_URI_TOO_LONG;
						notFound = true;
					}
				}

				else {
					header += STATUS_BAD_REQUEST;
					notFound = true;
				}	
			}

			else { 
				header += STATUS_NOT_IMPLEMENTED;
				notFound = true;
			}

		}	
			
		char memblock[fileSize];
		memset(memblock, 0, fileSize);
		
		// Do the following _only_ if the file can be served
		if (!notFound) {

			ostringstream ss;
			ss << fileSize;
		
			header += "\n" + HEADER_CONN; 
			header += "\n" + HEADER_TYPE + " " + getType(uri);
			header += "\n" + HEADER_LENGTH + " " + ss.str();
			header += "\n" + HEADER_SERVER;
			header += "\n\n";

			file.open(uri, ios::binary);
		
			// It's easier to send everything at once
			// Let the network figure it out
			if (file.is_open()) {
				file.seekg(0, ios::beg);
				file.read(memblock, fileSize);
				file.close(); 
			}

			time(&currentTime);
			printf("%s\tFile served: %s\n\n", ctime(&currentTime), uri);			

		}

		send(new_s, header.c_str(), header.length(), 0);
		send(new_s, memblock, fileSize, 0);
		send(new_s, "\n", 1, 0);
		
		close(new_s);
	}
}

//---------------------------------------------------------------------------


/*
Function: getURI
Purpose: This function extracts the URI (requested file) from the GET request.
Parameters: buffer contains the GET request, flag is used to indicate if the
	URI is valid (i.e., not too long)
Return: The URI as a string
*/
string getURI(char buffer[URI_MAX_LEN], bool & flag ) {

	bool tooLong = false;
	char uri[URI_MAX_LEN];
	int i = 0;

	memset(uri, '\0', URI_MAX_LEN);
	
	// Lazy character copy the URI	
	while (buffer[i + 5] != ' ' && buffer[i + 5] != '\n' && !tooLong) {

		if (i > URI_MAX_LEN) 
			tooLong = true;
		else 
			uri[i] = buffer[i + 5];
						
		i++;
	}
		
	// Special case: Just a slash means use the index.html file			
	if (i == 0)
		strncpy(uri, INDEX_HTML, strlen(INDEX_HTML));

	flag = !tooLong;

	return string(uri);

}

//---------------------------------------------------------------------------


/*
Function: getType
Purpose: This function returns the MIME type of the requested file. This
	function assumes the filename (uri) is lowercase.
Parameters: uri is the char array containing the URI (i.e., requested file name)
Return: MIME type on success, empty string on failure
*/
string getType(char uri[URI_MAX_LEN]) {

	string result;
	int pos = 0;
	int len = 0;

	// Lazy way to find the period in the file extension
	while (pos < URI_MAX_LEN) {
		if (uri[pos] == '.')
			break;
		pos++;
	}

	len = int(strlen(uri)) - pos - 1;
	
	// Unknown file format
	if (len < 2 || len > 4)
		result =  " ";

	else if (len == 4) {

		// HTML
		if (uri[pos + 1] == 'h' && 
			uri[pos + 2] == 't' && 
			uri[pos + 3] == 'm' && 
			uri[pos + 4] == 'l')
			result = MIME_HTML;

	}

	else if (len == 3) {

		// CSS
		if (uri[pos + 1] == 'c' &&
			uri[pos + 2] == 's' &&
			uri[pos + 3] == 's')
			result = MIME_CSS;

		// JPG
		else if (uri[pos + 1] == 'j' &&
			uri[pos + 2] == 'p' &&
			uri[pos + 3] == 'g')
			result = MIME_JPEG;

		// PNG
		else if (uri[pos + 1] == 'p' &&
			uri[pos + 2] == 'n' &&
			uri[pos + 3] == 'g')
			result = MIME_PNG;

		// GIF
		else if (uri[pos + 1] == 'g' &&
			uri[pos + 2] == 'i' &&
			uri[pos + 3] == 'f')
			result = MIME_GIF;

	}
	
	else if (len == 2) {

		// JS
		if (uri[pos + 1] == 'j' && uri[pos + 2] == 's')
			result = MIME_JS;

	}


	return result;
}

