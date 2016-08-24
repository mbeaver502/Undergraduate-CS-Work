/*
Michael Beaver
CS 360 - Computer Networking
Program #1 - Knock-knock Protocol 
4 March 2015
Program Description: 
	This program implements the server-side of the Knock-knock protocol. This
    	server will receive prompts from the client and respond to those prompts
    	either with the appropriate portions of a knock-knock joke or an error.
    	All data sent and received is printed to the screen.
	Fun point: You can slide right through the joke protocol by overflowing
		the buffer with a sufficiently long string and protocol codes
		placed in certain locations within the string.
Resources consulted:
	http://www.jokes4us.com/knockknockjokes
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

#include <signal.h>

//---------------------------------------------------------------------------
//                              CONSTANTS
//---------------------------------------------------------------------------

const int SERVER_PORT = 2999;
const int MAX_PENDING = 5;
const int CODE_LEN    = 3;
const int MAX_MSG     = 64; 
const int NUM_JOKES   = 8;
const int NUM_CODES   = 7;
const unsigned int BIND_RETRY = 15;
const unsigned int BIND_LIMIT = 5;

const char NUMBERS[10] = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
const char * CODES[NUM_CODES] = {"000", "100", "200", "300", "400", "500", "600"};

const int STATE_INIT  = 0;
const int STATE_KNOCK = 1;
const int STATE_WHO   = 2;
const int STATE_HOOK  = 3;
const int STATE_REPLY = 4;
const int STATE_PUNCH = 5;
const int STATE_ERROR = 6;

const char KNOCK_KNOCK[] = "Knock knock";
const char ERROR[]       = "Errors are never funny";

// Admittedly, there are better data structures for accomplishing this
const char * HOOK[NUM_JOKES] = {
			"Doughnut",
			"Little old lady",
			"Who",
			"Dewey",
			"Art",
			"Yah",
			"Orange",
			"Doctor" };
const char * PUNCH[NUM_JOKES] = {
			"Doughnut ask, it is a secret",
			"I didn't know you could yodel",
			"Are you an owl",
			"Dewey have to keep telling silly jokes",
			"R2-D2",
			"I prefer DuckDuckGo",
			"Orange you going to answer the door",
			"That's a great show" };

static volatile sig_atomic_t timeToQuit = 0;

//---------------------------------------------------------------------------
//                              STRUCTURES                     
//---------------------------------------------------------------------------

struct message  {

        char code[CODE_LEN + 1];
        char text[MAX_MSG];

};

//---------------------------------------------------------------------------
//                              PROTOTYPES
//---------------------------------------------------------------------------

void sendMessage(int new_s, const char code[], const char msg[]);

int toDigit(char ch);
int isCode(const char code[]);

void setJokeState(char code[], int & jokeState);
void jokeHandler(int new_s, int & jokeState, int jokeNum);

void messageInit(struct message * msg);
bool messageHandler(struct message * msg, char buffer[]);

static void signalHandler(int signal);

//---------------------------------------------------------------------------
//                              FUNCTIONS
//---------------------------------------------------------------------------

/*
Function: main
Purpose: This is the main entry-point and contains the main loop that
	establishes connections and carries out the Knock-knock protocol.
Parameters: N/A
Return: 0 on completion, 1 on failure or on SIGTERM
*/
int main() {

        struct sockaddr_in sin;
        int len;
        unsigned int bindAttempts = 0;
        int s, new_s;
        struct sigaction sa;
        char buffer[CODE_LEN + MAX_MSG];
        struct message msg;
        int jokeState = -1;
        int jokeNum = -1;

        printf("KKS: Shutdown: kill -TERM %d\n\n", getpid()); 

	
        // Create and register the signal handler for SIGTERM
        sa.sa_handler = &signalHandler;
        sa.sa_flags = 0;
        sigemptyset(&sa.sa_mask);

        if (sigaction(SIGTERM, &sa, NULL) < 0) {
                printf("KKS: Unable to hook SIGTERM handler.\n");
                exit(1);
        }

	
        // Build address data structure
        memset((char *) &sin, 0, sizeof(sin));
        sin.sin_family = AF_INET;
        sin.sin_addr.s_addr = INADDR_ANY;
        sin.sin_port = htons(SERVER_PORT); 
       
	
        // Passive open
        if ((s = socket(PF_INET, SOCK_STREAM, 0)) < 0) {
                printf("KKS: Unable to open a socket.\n");
                exit(1);
        }

        /* Here we will continue attempting to bind every BIND_RETRY seconds
        until success or until a limit (BIND_LIMIT) is exceeded. */
        while (bind(s, (struct sockaddr *) &sin, sizeof(sin)) < 0) {	
		
                bindAttempts++;

                printf("KKS: Unable to bind socket.\n");

                if (bindAttempts > BIND_LIMIT) {
                        printf("KKS: Exceeded allotted tries (%d).\n",
                                BIND_LIMIT);
                        printf("KKS: Closing socket.\n");
                        close(s);
                        printf("\nKKS: Server terminated.\n");
                        exit(1);
                } else {
                    	printf("KKS: Retrying in %d seconds.\n", BIND_RETRY);
                	sleep(BIND_RETRY);
                }

                // In case of termination while retrying bind
                if (timeToQuit) {
                        printf("KKS: Closing socket.\n");
                        close(s);
                        printf("\nKKS: Server terminated.\n");			
                        exit(1);
                }

        }
    
        listen(s, MAX_PENDING);
   
        printf("KKS: Server is now accepting connections.\n\n");
	

        // Main loop: Accept connections and output joke parts
        while (!timeToQuit) {
    		
                len = sizeof(sin); 
                new_s = accept(s, (struct sockaddr*)&sin, (socklen_t*)&len);	

                if (new_s < 0) {
    
                        if (!timeToQuit) { 
                                printf("KKS: Unable to connect.\n");
                                printf("KKS: Closing socket.\n");
                                close(s);
                                printf("\nKKS: Server terminated.\n");	
                                exit(1);
                        }
			
                } else {

                        jokeNum = arc4random() % NUM_JOKES;
                        jokeState = -1;
			
                        /* Keep sending data until the joke finishes or there
                        is an error. The connection will close then. */
                        while (jokeState != STATE_PUNCH && jokeState != STATE_ERROR) {

                                bzero(buffer, MAX_MSG + CODE_LEN);
				messageInit(&msg);
	
                                if (recv(new_s, buffer, CODE_LEN + MAX_MSG, 0) < 1) {
                                    
                                        jokeState = STATE_ERROR;
                                        jokeHandler(new_s, jokeState, jokeNum);
                                        
                                } else {

					printf("\n\nReceived: %s\n\n", buffer);
                                    
                                        /* Somewhat redundant, but just in case
                                        there are errors in the message. */
                                        if (!messageHandler(&msg, buffer))
                                                jokeState = STATE_ERROR;
				
                                        printf("%s%s\n", msg.code, msg.text);
                                        setJokeState(msg.code, jokeState);
                                        jokeHandler(new_s, jokeState, jokeNum);
                                        
                                }	

                        }
		
                        close(new_s);	
					
                }
    	
        }
   
        printf("KKS: Closing sockets.\n");
        close(new_s);
        close(s);
        printf("KKS: Server terminated.\n");

        return 0;

}

//---------------------------------------------------------------------------

/*
Function: sendMessage
Purpose: This function is used to generically send codes and text (as 
    part of the protocol's message format) to the client.
Parameters: new_s is the socket on which to send, code is the C-string code
    portion of the message, msg is the C-string text part of the message
Return: N/A (void)
*/
void sendMessage(int new_s, const char code[], const char msg[]) {

        send(new_s, code, CODE_LEN, 0);
        send(new_s, " ", 1, 0);
        send(new_s, msg, strlen(msg), 0);
        send(new_s, "\n", 1, 0);

}

//---------------------------------------------------------------------------

/*
Function: toDigit
Purpose: This function is meant to "convert" a character to its respective
    decimal digit value. atoi is useful for C-strings, but this also works.
Parameters: ch is the character to convert
Return: An integer in [0..9] on success, -1 on failure
*/
int toDigit(char ch) {

        int result = -1;

        // Try to match the character to a digit in ['0'..'9']
        for (int i = 0; i < 10; i++) {
                if (ch == NUMBERS[i]) 
                        result = i;
        }

        return result;

}

//---------------------------------------------------------------------------

/*
Function: isCode
Purpose: This function is used to determine whether a given C-string is a 
    valid code in the Knock-knock protocol.
Parameters: code is the C-string to check
Return: An integer in [0..NUM_CODES-1] on success, -1 on failure
*/
int isCode(const char code[]) {

        bool result = -1;

        // Try to match code to one of the predefined codes in the protocol
        for (int i = 0; i < NUM_CODES; i++) {
                if (strcmp(code, CODES[i]) == 0) {
                        result = i;
                        break;
                }
        }
	
        return result;	

}

//---------------------------------------------------------------------------

/*
Function: setJokeState
Purpose: This function will appropriately alter the jokeState of the server 
    based on the given code.
Parameters: code is the C-string specifying the protocol code, jokeState is
    the state of the server that will be altered
Return: N/A (void)
*/
void setJokeState(char code[], int & jokeState) {

        switch (toDigit(code[0])) {

                // Initial state, code 000, state 0
                case STATE_INIT: {
                        if (jokeState > STATE_INIT)
                                jokeState = STATE_ERROR;
                        else if (code[1] == '0' && code[2] == '0')
                                jokeState = STATE_INIT;
                        break;
                }

                // "Knock knock" state, code 100, state 1
                case STATE_KNOCK: {
                        if (jokeState >= STATE_KNOCK)
                                jokeState = STATE_ERROR;
                        else if (code[1] == '0' && code[2] == '0')
                                jokeState = STATE_KNOCK;
                        break;		
                }

                // "Who's there?" state, code 200, state 2
                case STATE_WHO: {
                        if (jokeState >= STATE_WHO)
                                jokeState = STATE_ERROR;
                        else if (code[1] == '0' && code[2] == '0')
                                jokeState = STATE_WHO;
                        break;
                }

                // Joke hook state, code 300, state 3
                case STATE_HOOK: {
                        if (jokeState >= STATE_HOOK)
                                jokeState = STATE_ERROR;
                        else if (code[1] == '0' && code[2] == '0')
                                jokeState = STATE_HOOK;
                        break;
                }

                // "X who?" state, code 400, state 4
                case STATE_REPLY: {
                        if (jokeState >= STATE_REPLY)
                                jokeState = STATE_ERROR;
                        else if (code[1] == '0' && code[2] == '0')
                                jokeState = STATE_REPLY;
                        break;
                }

                // Punch line state, code 500, state 5
                case STATE_PUNCH: {
                        if (jokeState >= STATE_PUNCH)
                                jokeState = STATE_ERROR;
                        else if (code[1] == '0' && code[2] == '0')
                                jokeState = STATE_PUNCH;
                        break;
                }

                // Error states, code 600, state 6
                case STATE_ERROR:
                default:
                        jokeState = STATE_ERROR;

        }

}

//---------------------------------------------------------------------------

/*
Function: jokeHandler
Purpose: This function will output the next part of the joke to the server
    and send that part to the client. Also, the joke state is updated.
Parameters: new_s is the open socket, jokeState is the joke state of the
    server to be updated, jokeNum is the joke being sent to the client
Return: N/A (void)
*/
void jokeHandler(int new_s, int & jokeState, int jokeNum) {

        switch (jokeState) {
	
                // Send knock knock, code 000->100, state 0->1
                case STATE_INIT: {
                        jokeState = STATE_KNOCK;
                        sendMessage(new_s, CODES[jokeState], KNOCK_KNOCK);
                        printf("%s %s\n", CODES[jokeState], KNOCK_KNOCK);
                        break;
                }
	
                // Send hook line, code 200->300, state 2->3
                case STATE_WHO: {
                        jokeState = STATE_HOOK;
                        sendMessage(new_s, CODES[jokeState], HOOK[jokeNum]);
                        printf("%s %s\n", CODES[jokeState], HOOK[jokeNum]);
                        break;
                }

                // Send punch line, code 400->500, state 4->5
                case STATE_REPLY: {
                        jokeState = STATE_PUNCH;
                        sendMessage(new_s, CODES[jokeState], PUNCH[jokeNum]);
                        printf("%s %s\n\n", CODES[jokeState], PUNCH[jokeNum]);
                        break;
                }

                // Send error, most likely because codes were out of order
                case STATE_KNOCK:
                case STATE_HOOK:
                case STATE_PUNCH:
                case STATE_ERROR:
                default: {
                        jokeState = STATE_ERROR;
                        sendMessage(new_s, CODES[jokeState], ERROR);
                        printf("%s %s\n\n", CODES[jokeState], ERROR);
                }	

        }

} 

//---------------------------------------------------------------------------

/*
Function: messageInit
Purpose: This function will initialize (i.e., zero-out) the memory of a 
    message struct object.
Parameters: msg is a pointer to the message struct object to initialize
Return: N/A (void)
*/
void messageInit(struct message * msg) {

	bzero(msg->code, CODE_LEN);
	bzero(msg->text, MAX_MSG);

}

//---------------------------------------------------------------------------

/*
Function: messageHandler
Purpose: This function will verify the correctness of a message from the 
    client and store the two parts of the message (code and text) in a 
    message struct object.
Parameters: msg is a pointer to the message struct to hold the client's message,
    buffer is the client's message (potentially MAX_MSG + CODE_LEN bytes)
Return: true if the message is valid, false if invalid
*/
bool messageHandler(struct message * msg, char buffer[]) {

        bool result = true;
        int len = strlen(buffer);
        int offset = 0;

	printf("\n\nMessage Handler Input: %s\n\n", buffer);

        // Minimum message size
        if (len <= CODE_LEN + 1)
                result = false;

        for (int i = 0; i < CODE_LEN; i++) {
                if (toDigit(buffer[i]) < 0)
                        result = false;
                msg->code[i] = buffer[i];
        }
        msg->code[CODE_LEN] = '\0';	
	
        // Verify the code is valid (i.e., in the protocol)
        if (isCode(msg->code) < 0)
                result = false;

        // For formatting purposes, separate the code and text of the message
        if (buffer[CODE_LEN] != ' ') {
                msg->text[0] = ' ';
                offset = 1;
        }
	
        for (int i = CODE_LEN; i < len; i++)
                msg->text[i - CODE_LEN + offset] = buffer[i];
        msg->text[MAX_MSG] = '\0';	

	printf("\n\nMessage Handler Result: %s%s\n\n", msg->code, msg->text);

        return result;	

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
