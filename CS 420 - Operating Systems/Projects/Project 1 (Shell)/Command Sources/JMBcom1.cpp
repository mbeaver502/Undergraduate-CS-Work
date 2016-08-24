/*
	Michael Beaver
	CS 420, Fall 2013
	Programming Assignment #1
	October 24, 2013
	
	This simple program takes in a series of arguments from the terminal
	and then outputs them separately on new lines. At least one user-specified
	argument is required for proper execution.
*/

#include <iostream>

using namespace std;

//--------------------------------------------------------------------------------------------

/*
	Name: main
	Author: Michael Beaver
	Purpose: Outputs each separate argument provided by the user.
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

	// Very simple program to output the user's arguments, but not the program name
	cout << "Your arguments:" << endl;
	for (int i = 1; i < argc; i++)
		cout << argv[i] << endl;

	return 0;

}



