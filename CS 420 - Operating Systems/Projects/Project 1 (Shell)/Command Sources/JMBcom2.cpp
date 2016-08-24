/*
	Michael Beaver
	CS 420, Fall 2013
	Programming Assignment #1
	October 24, 2013
	
	This simple program collects the user's arguments from the terminal and stores
	them as a single string. Then, the program outputs the string n-times, as
	specified by the user in the first argument.
*/

#include <iostream>
#include <stdlib.h>
#include <string>

using namespace std;

//--------------------------------------------------------------------------------------------

/*
	Name: main
	Author: Michael Beaver
	Purpose: Outputs a single string of user-specified arguments, n-times.
	Incoming: argc is the number of arguments; argv is the list of arguments.
	Outgoing: N/A
	Return: Returns 0 on successful execution; otherwise, return 1.
*/
int main(int argc, char ** argv) {

	// We want at least one user-specified argument
	if (argc == 1) {

		cout << "No arguments specified!" << endl;

		return 1;

	}

	// The first argument specifies the number of times to output the string of arguments
	int n = atoi(argv[1]);
	string output;

	// We want n to be positive and non-zero
	if (n <= 0) {

		cout << "Invalid loop value: " << n << ". Loop value must be greater than zero!" << endl;
		return 0;

	}

	// Build the string of arguments
	for (int i = 2; i < argc; i++)
		output += string(argv[i]) + " ";
	
	// Output the string of arguments, n-times
	for (int i = 0; i < n; i++)
		cout << output << endl;

	return 0;

}


