/*
Michael Beaver
CS 360 - Computer Networking
Program #1 - Knock-knock Protocol 
4 March 2015
Program Description: 
	
Resources consulted:
	
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

#include <string>
#include <iostream>

//---------------------------------------------------------------------------
//                              CONSTANTS
//---------------------------------------------------------------------------

const int SERVER_PORT = 2983;
const int CODE_LEN    = 3;
const int MAX_MSG     = 64;
const int NUM_CODES   = 7;

const char NUMBERS[10] = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
const char * CODES[NUM_CODES] = {"000", "100", "200", "300", "400", "500", "600"};

const int STATE_INIT  = 0;
const int STATE_KNOCK = 1;
const int STATE_WHO   = 2;
const int STATE_HOOK  = 3;
const int STATE_REPLY = 4;
const int STATE_PUNCH = 5;
const int STATE_ERROR = 6;

const char ERROR[]	= "Errors are never funny";
const char WHOS_THERE[] = "Who's there";
const char TELL_JOKE[]	= "Tell me a joke";

//---------------------------------------------------------------------------
//                              STRUCTURES                     
//---------------------------------------------------------------------------

struct message  {

        char code[CODE_LEN + 1];
        char text[MAX_MSG + 1];

};

//---------------------------------------------------------------------------
//                              PROTOTYPES
//---------------------------------------------------------------------------

void sendMessage(int new_s, const char code[], const char msg[]);

int toDigit(char ch);
int isCode(const char code[]);

void setJokeState(char code[], int & jokeState);
void jokeHandler(int new_s, int & jokeState, struct message * msg);

void messageInit(struct message * msg);
bool messageHandler(struct message * msg, char buffer[]);

//---------------------------------------------------------------------------
//                              FUNCTIONS
//---------------------------------------------------------------------------

/*
Function: main
Purpose: 
Parameters: N/A
Return: 0 on completion, 1 on failure
*/
int main(int argc, char * argv[]) {

	struct hostent * hp;
	struct sockaddr_in sin;
	char * host;
	char buffer[CODE_LEN + MAX_MSG];
	int s;
	struct message msg;
	int jokeState = -1;

	if (argc == 2) 
		host = argv[1];	
	
	else {
		
		printf("Usage: KnockKnock host\n");
		exit(1);

	}

	// Translate host name into peer's IP address
	hp = gethostbyname(host);
	if (!hp) {

		printf("KnockKnock: Unknown host: %s\n", host);
		printf("KnockKnock: Client terminating.\n");
		exit(1);

	}

	// Build address data structure
	memset((char *) &sin, 0, sizeof(sin));
	sin.sin_family = AF_INET;
	memcpy((char *) &sin.sin_addr, hp->h_addr, hp->h_length);
	sin.sin_port = htons(SERVER_PORT);

	// Active open
	if ((s = socket(PF_INET, SOCK_STREAM, 0)) < 0) {

		printf("KnockKnock: Unable to create a socket.\n");
		printf("KnockKnock: Unable to connect to the network.\n");
		printf("KnockKnock: Client terminating.\n");
		exit(1);

	}

	if (connect(s, (struct sockaddr *) &sin, sizeof(sin)) < 0) {

		printf("KnockKnock: Unable to connect to the server.\n");
		printf("KnockKnock: Client terminating.\n");
		close(s);
		exit(1);

	}

        
        // Prompt the server to return the daytime and then display it
	//send(s, 0, 0, 0);
/*
	while (jokeState != STATE_PUNCH && jokeState != STATE_ERROR) {
	
		bzero(buffer, MAX_MSG + CODE_LEN);
		messageInit(&msg);

	}
*/
	
	jokeState = 0;
	sendMessage(s, CODES[STATE_INIT], TELL_JOKE);
	printf("%s %s\n", CODES[STATE_INIT], TELL_JOKE);

	while (jokeState != STATE_PUNCH && jokeState != STATE_ERROR) {
	
//		bzero(buffer, MAX_MSG + CODE_LEN);
		memset(buffer, 0, MAX_MSG + CODE_LEN);
		messageInit(&msg);
	
		if (recv(s, buffer, CODE_LEN + MAX_MSG, 0) < 1) {

			jokeState = STATE_ERROR;
			sendMessage(s, CODES[STATE_ERROR], ERROR);

		} else {

		//	printf("Received: %s\n", buffer);			

			if (!messageHandler(&msg, buffer))
				jokeState = STATE_ERROR;

			printf("%s %s\n", msg.code, msg.text);
			setJokeState(msg.code, jokeState);
			jokeHandler(s, jokeState, &msg);

		}
	
//		bzero(buffer, MAX_MSG + CODE_LEN);
//		messageInit(&msg);
		
	}

	close(s);

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

//	printf("\n\nSending message: %s %s\n\n", code, msg);

	std::string temp(code);
	temp.append(msg);
//	printf("Sending: %s\n", temp.c_str());
	send(new_s, temp.c_str(), CODE_LEN + MAX_MSG, 0); 


/*
	char temp [CODE_LEN + MAX_MSG + 1];
	strlcpy(temp, code, CODE_LEN + 1);
	strlcat(temp, msg, MAX_MSG + 1);
	temp[MAX_MSG + CODE_LEN] = '\0';
	printf("\nSending: %s\n", temp);
	send(new_s, temp, CODE_LEN + MAX_MSG + 1, 0);	
*/	
//        send(new_s, code, CODE_LEN, 0);
//        send(new_s, " ", 1, 0);
//        send(new_s, msg, strlen(msg), 0);
//        send(new_s, "\n", 1, 0);

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
void jokeHandler(int new_s, int & jokeState, struct message * msg) {

	switch(jokeState) {

		case STATE_KNOCK: {
			jokeState = STATE_WHO;
			sendMessage(new_s, CODES[jokeState], WHOS_THERE);
			printf("%s %s\n", CODES[jokeState], WHOS_THERE);;
			break;
		}

		case STATE_HOOK: {
			jokeState = STATE_REPLY;
			char temp[MAX_MSG + 4];
			strlcpy(temp, msg->text, strlen(msg->text) + 1);
			strncat(temp, " who", 4);
			sendMessage(new_s, CODES[jokeState], temp);
			printf("%s %s\n", CODES[jokeState], temp);
			break;
		}

		case STATE_PUNCH: {
			jokeState = STATE_PUNCH;
			break;
		}

		case STATE_INIT:
		case STATE_WHO:
		case STATE_REPLY:
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

	memset(msg->code, 0, CODE_LEN);
	memset(msg->text, 0, MAX_MSG);

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
//        int offset = 0;
//	printf("\nbuffer input: %s\n", buffer);

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
//        if (buffer[CODE_LEN] != ' ') {
//                msg->text[0] = ' ';
//                offset = 1;
//        }
	
        for (int i = CODE_LEN; i < len; i++)
                msg->text[i - CODE_LEN] = buffer[i];
        msg->text[MAX_MSG] = '\0';	

//	printf("\n\nMessage Handler Result: %s%s\n\n", msg->code, msg->text);

        return result;	

}
