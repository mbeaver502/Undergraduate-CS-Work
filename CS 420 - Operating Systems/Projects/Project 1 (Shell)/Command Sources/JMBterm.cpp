/*
	Michael Beaver
	CS 420, Fall 2013
	Programming Assignment #1
	October 24, 2013
	
	This program emulates the function of the *nix terminal. This program prompts
	the user for a program name and arguments, just like the real terminal. Given
	the program and arguments, this terminal forks and uses the execvp() version
	of exec() to execute the program with the specified arguments. The user may
	continue using the terminal after each command is executed, or the user may
	exit the terminal by entering 'exit.' This terminal relies fairly heavily on
	outside source code. Functions borrowed from other sources are marked by a
	leading underscore in the name. All borrowed source code is documented with
	the appropriate information and a hyperlink to the original source.
*/

#include <sys/types.h>
#include <sys/wait.h>
#include <stdio.h>
#include <unistd.h>
#include <iostream>
#include <string>
#include <string.h>
#include <vector>

using namespace std;

//--------------------------------------------------------------------------------------------

// Vasudevan
void _tokenize(const string & str, vector<string> & tokens, const string & delimiters);

// McKenzie
char ** _createArgVFromVector(const vector<string> & allArgs, int nArgs);
void _destroyArgV(char ** argv, int nArgs);

//--------------------------------------------------------------------------------------------

/*
	Name: main
	Author: Michael Beaver
	Purpose: Fork and execute a user's command.
	Incoming: N/A
	Outgoing: N/A
	Return: Returns 0 on successful execution; otherwise, returns 1.
*/
int main() {

	string termInput;
	int argc = 0;
	vector<string> argVec;
	
	pid_t pid;

	cout << "JMB$ ";
	getline(cin, termInput);

	// Use 'exit' to exit the terminal
	while (termInput != "exit") {
		
		pid = fork();

		// Fork error
		if (pid < 0) {

			fprintf(stderr, "ERROR: Fork Failed\n");
			return 1;

		}

		// Child Process
		else if (pid == 0) {

			// Quit conditions
			if (termInput == "exit")
				return 0;

			// Separate the program arguments
			_tokenize(termInput, argVec, " ");
			argc = argVec.size();

			// Generate the new argv for the child
			char ** argv = _createArgVFromVector(argVec, argc);

			// Execute the child and clean up memory if the execution fails
			if (execvp(argv[0], argv) == -1) {

				_destroyArgV(argv, argc);
				argVec.clear();
				argc = 0;

				// Print error
				perror("Execution Error");

				return 1;

			}
		
		}

		// Parent Process
		else {		

			wait(NULL);  // Wait for the child to finish execution
	
		}	

		cout << "JMB$ ";
		getline(cin, termInput);

	} 

	return 0;

}

//--------------------------------------------------------------------------------------------

/*
	Name: _tokenize
	Author: Alavoor Vasudevan, modified by Michael Beaver
	Purpose: Separates ('tokenizes') a string into a vector of strings based on
		 a particular delimiter or delimeters. In our case, the delimiter is a space.
	Incoming: str is the string to tokenize, tokens is the vector of tokenized strings,	
		  delimiters is/are the delimiters by which to separate the string
	Outgoing: tokens is the vector of tokenized strings
	Return: N/A (void)
	Source: http://oopweb.com/CPP/Documents/CPPHOWTO/Volume/C++Programming-HOWTO-7.html
*/
void _tokenize(const string & str, vector<string> & tokens, const string & delimiters = " ") {

	// Skip delimiters at beginning.
	string::size_type lastPos = str.find_first_not_of(delimiters, 0);

	// Find first "non-delimiter".
	string::size_type pos = str.find_first_of(delimiters, lastPos);

	while (string::npos != pos || string::npos != lastPos) {

		// Allow for arguments contained in quotation marks (see: JMBcom3) -- Michael Beaver
		if (str[lastPos] == '"') {
	
			pos = str.find('"', lastPos + 1);

			tokens.push_back(str.substr(lastPos, pos - lastPos + 1));

		}

		else
			tokens.push_back(str.substr(lastPos, pos - lastPos)); 
		
		// Skip delimiters.  Note the "not_of"
		lastPos = str.find_first_not_of(delimiters, pos);    	      
		
		// Find next "non-delimiter"
		pos = str.find_first_of(delimiters, lastPos);        	      

	}

}

//--------------------------------------------------------------------------------------------

/*
	Name: _createArgVFromVector
	Author: Paul McKenzie, comments by Michael Beaver
	Purpose: Conveniently creates a new 'argv' from a vector of strings to be given to exec().
	Incoming: allArgs is the vector of arguments (see: _tokenize), and nArgs is the 
		  number of arguments
	Outgoing: N/A
	Return: Returns a pointer to an array of C-String arguments.
	Source: http://forums.codeguru.com/showthread.php?462070-Linux-Execvp-wont-take-argv
*/
char ** _createArgVFromVector(const vector<string> & allArgs, int nArgs) {

	// No arguments => No argv
	if ( nArgs == 0 )
        	return NULL;

	// Copy the strings from the vector (as C-Strings) into a new argv
	char ** buf = new char * [nArgs + 1];
	for (int i = 0; i < nArgs; ++i) {

		buf[i] = new char [allArgs[i].size() + 1];
		strcpy(buf[i], allArgs[i].c_str());

	}

	// Argv must end with NULL
	buf[nArgs] = NULL;

	return buf;

}

//--------------------------------------------------------------------------------------------

/*
	Name: _destroyArgV
	Author: Paul McKenzie, comments by Michael Beaver
	Purpose: Returns memory dynamically allocated when creating the new 'argv.'
	Incoming: argv is a pointer to an array of C-Strings, and nArgs is the number of
		  arguments in argv
	Outgoing: N/A
	Return: N/A (void)
	Source: http://forums.codeguru.com/showthread.php?462070-Linux-Execvp-wont-take-argv
*/
void _destroyArgV(char ** argv, int nArgs) {

	// Return the memory for the C-Strings
	for (int i = 0; i < nArgs; ++i)
		delete [] argv[i];

	// Return the memory for the array
	delete [] argv;

}


