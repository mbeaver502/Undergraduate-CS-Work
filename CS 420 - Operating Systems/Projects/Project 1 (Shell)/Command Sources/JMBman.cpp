/*
	Michael Beaver
	CS 420, Fall 2013
	Programming Assignment #1
	October 24, 2013
	
	This program displays the man pages for each custom terminal command.
*/

#include <iostream>
#include <string>

using namespace std;

//--------------------------------------------------------------------------------------------

void drawMenu();
void dispatch(int sel);

void manJMBcom1();
void manJMBcom2();
void manJMBcom3();
void manJMBhelp();
void manJMBman();

//--------------------------------------------------------------------------------------------

/*
	Name: main
	Author: Michael Beaver
	Purpose: Show the menu and display the appropriate man page.
	Incoming: N/A
	Outgoing: N/A
	Return: Return 0 on successful execution.
*/
int main() {

	int sel;

	do {

		cout << endl;

		drawMenu();
		cin >> sel;
		if (sel != 0)
			dispatch(sel);	

	} while (sel != 0);

	return 0;

}

//--------------------------------------------------------------------------------------------

/*
	Name: drawMenu
	Author: Michael Beaver
	Purpose: Draws the menu and prompts for user selection.
	Incoming: N/A
	Outgoing: N/A
	Return: N/A (void)
*/
void drawMenu() {

	cout << "=====================================" << endl;
	cout << "         man Page Selection:         " << endl;
	cout << "=====================================" << endl << endl;

	cout << "     (1) JMBcom1     (4) JMBhelp     " << endl;
	cout << "     (2) JMBcom2     (5) JMBman      " << endl;
	cout << "     (3) JMBcom3     (0) Exit        " << endl << endl;
	

	cout << "Selection: ";

}

//--------------------------------------------------------------------------------------------

/*
	Name: dispatch
	Author: Michael Beaver
	Purpose: Calls the appropriate man page function.
	Incoming: sel is the user's man page selection.
	Outgoing: N/A
	Return: N/A (void)
*/
void dispatch(int sel) {

	cout << endl;

	switch (sel) {

		// JMBcom1
		case 1:
			manJMBcom1();
			break;
		
		// JMBcom2
		case 2:
			manJMBcom2();
			break;

		// JMBcom3
		case 3:
			manJMBcom3();
			break;

		// JMBhelp
		case 4:
			manJMBhelp();
			break;
		
		// JMBman
		case 5:
			manJMBman();
			break;

	}

	cout << endl;	// Space for nice formatting

}

//--------------------------------------------------------------------------------------------

/*
	Name: manJMBcom1
	Author: Michael Beaver
	Purpose: Displays the man page for JMBcom1.
	Incoming: N/A
	Outgoing: N/A
	Return: N/A (void)
*/
void manJMBcom1() {

	cout << "NAME" << endl;
	cout << "\tJMBcom1 - JMB command 1" << endl << endl;

	cout << "SYNOPSIS" << endl;
	cout << "\tJMBcom1 ARG1 ARG2 ARG3 ... ARGN" << endl << endl;

	cout << "DESCRIPTION" << endl;
	cout << "\tOutputs the separate arguments specified by the user via the terminal." << endl << endl;
	cout << "\t\tAt least one argument is required, even if it is arbitrary." << endl;
	cout << "\t\tAny arguments are accepted." << endl << endl;

	cout << "EXAMPLE USAGE" << endl;
	cout << "\t./JMBcom1 abc def" << endl << endl;

	cout << "AUTHOR" << endl;
	cout << "\tWritten by Michael Beaver." << endl << endl;

}

//--------------------------------------------------------------------------------------------

/*
	Name: manJMBcom2
	Author: Michael Beaver
	Purpose: Displays the man page for JMBcom2.
	Incoming: N/A
	Outgoing: N/A
	Return: N/A (void)
*/
void manJMBcom2() {

	cout << "NAME" << endl;
	cout << "\tJMBcom2 - JMB command 2" << endl << endl;

	cout << "SYNOPSIS" << endl;
	cout << "\tJMBcom2 X ARG1 ARG2 ARG3 ... ARGN" << endl << endl;

	cout << "DESCRIPTION" << endl;
	cout << "\tDisplays a list of arguments as one string, X-times." << endl << endl;
	cout << "\t\tX" << endl;
	cout << "\t\t\tMandatory. Specifies the number of times to display the" << endl << "\targument string." << endl << endl;
	cout << "\t\tAny arguments are accepted, even if they are arbitrary." << endl << endl;

	cout << "EXAMPLE USAGE" << endl;
	cout << "\t./JMBcom2 10 Hello World! ABC def" << endl << endl;

	cout << "AUTHOR" << endl;
	cout << "\tWritten by Michael Beaver." << endl << endl;

}

//--------------------------------------------------------------------------------------------

/*
	Name: manJMBcom3
	Author: Michael Beaver
	Purpose: Displays the man page for JMBcom3.
	Incoming: N/A
	Outgoing: N/A
	Return: N/A (void)
*/
void manJMBcom3() {

	cout << "NAME" << endl;
	cout << "\tJMBcom3 - JMB command 3" << endl << endl;

	cout << "SYNOPSIS" << endl;
	cout << "\tJMBcom3 CONVERSION [EVALUATION] EXPRESSION" << endl << endl;

	cout << "DESCRIPTION" << endl;
	cout << "\tConverts between infix and postfix notation for a given expression of" << endl 
	     << "\teither notation. The user must specify whether to convert to infix or" << endl 
	     << "\tpostfix. Evaluation is optional. If evaluation is not specified, the" << endl
	     << "\tsecond argument is assumed to be the expression. Infix expressions must" << endl
	     << "\tbe fully parenthesized, and postfix expressions must separate all" << endl
	     << "\toperands and operations with spaces. ALL expressions must be wrapped" << endl 
	     << "\tin quotation marks. Expressions are assumed to be entered correctly." << endl
	     <<	"\tThe user MUST specify a conversion mode." << endl << endl;
	cout << "\t\t-i =CONVERSION" << endl;
	cout << "\t\t\tConvert a postfix expression to infix notation." << endl << endl;
	cout << "\t\t-o =CONVERSION" << endl;
	cout << "\t\t\tConvert an infix expression to postfix notation." << endl << endl;
	cout << "\t\t-e =EVALUATION" << endl;
	cout << "\t\t\tOptional. Evaluate the given expression." << endl << endl;

	cout << "EXAMPLE USAGE" << endl;
	cout << "\t./JMBcom3 -i \"7 5 4 * 9 3 - - 1 * +\"" << endl;
	cout << "\t./JMBcom3 -o -e \"((12 + 3) / 4)\"" << endl << endl;

	cout << "AUTHOR" << endl;
	cout << "\tWritten by Michael Beaver." << endl << endl;

}

//--------------------------------------------------------------------------------------------

/*
	Name: manJMBhelp
	Author: Michael Beaver
	Purpose: Displays the man page for JMBhelp.
	Incoming: N/A
	Outgoing: N/A
	Return: N/A (void)
*/
void manJMBhelp() {

	cout << "NAME" << endl;
	cout << "\tJMBhelp - JMB Help" << endl << endl;

	cout << "SYNOPSIS" << endl;
	cout << "\tJMBhelp" << endl << endl;

	cout << "DESCRIPTION" << endl;
	cout << "\tDisplays a list of custom commands that may be called from the terminal." << endl << endl;
	cout << "\t\tNo arguments are accepted." << endl << endl;

	cout << "EXAMPLE USAGE" << endl;
	cout << "\t./JMBhelp" << endl << endl;

	cout << "AUTHOR" << endl;
	cout << "\tWritten by Michael Beaver." << endl << endl;

}

//--------------------------------------------------------------------------------------------

/*
	Name: manJMBman
	Author: Michael Beaver
	Purpose: Displays the man page for JMBman.
	Incoming: N/A
	Outgoing: N/A
	Return: N/A (void)
*/
void manJMBman() {

	cout << "NAME" << endl;
	cout << "\tJMBman - JMB man Pages" << endl << endl;

	cout << "SYNOPSIS" << endl;
	cout << "\tJMBman" << endl << endl;

	cout << "DESCRIPTION" << endl;
	cout << "\tLists man pages for each custom command." << endl << endl;
	cout << "\t\tNo arguments are accepted." << endl << endl;

	cout << "EXAMPLE USAGE" << endl;
	cout << "\t./JMBman" << endl << endl;

	cout << "AUTHOR" << endl;
	cout << "\tWritten by Michael Beaver." << endl << endl;

}


