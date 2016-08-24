/*
	Michael Beaver
	CS 420, Fall 2013
	Programming Assignment #1
	October 24, 2013
	
	This program displays in the terminal a list of custom commands
	available to the user to be executed from the terminal.
*/

#include <iostream>

using namespace std;

//--------------------------------------------------------------------------------------------

/*
	Name: main
	Author: Michael Beaver
	Purpose: Displays available custom commands.
	Incoming: N/A
	Outgoing: N/A
	Return: Returns 0 on successful execution.
*/
int main() {

	cout << "==========================" << endl;
	cout << "Available Custom Commands:" << endl;
	cout << "==========================" << endl << endl;

	cout << "JMBcom1: Outputs the arguments specified by the user via the terminal." << endl;
	cout << "JMBcom2: Displays (x-times) a specified list of arguments as one string." << endl;
	cout << "JMBcom3: Converts between infix and postfix notations and evaluates (optional)." << endl;
	cout << "JMBhelp: Displays available custom commands." << endl;
	cout << " JMBman: Lists man pages for each custom command." << endl << endl;

	return 0;

}




