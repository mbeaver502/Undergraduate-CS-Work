/*
Michael Beaver
CS 421 - Automata Theory & Compiler Construction
Assignment #2 - MINI-P Parser
27 April 2015
Program Description:
	This program implements a parser for a MINI-P compiler. This program emulates
	the basic function of a parser as it would continuously prompt the Scanner to
	scan tokens in the buffer. Special note: This implementation assumes all
	executable statements (i.e., assign, read, write) end with a semicolon. This
	implementation also will stop trying to parse if it encounters a bad statement.
	Any statements after the initial bad statement will not be parsed (correctly),
	but the bad statement will be identified. In this case, the parser will also
	attempt to parse the END. keyword, which will obviously fail.
*/

#include <iostream>
#include <fstream>
#include <string>

#include "Constants.h"
#include "SymbolTable.h"
#include "Scanner.h"
#include "Parser.h"

using namespace std;

/*
Function: main
Purpose: This function calls the Parser to parse a source file.
Parameters: N/A
Return: 0 on termination
*/
int main() {

	string filename;
	Parser P;

	cout << "Source: ";
	cin >> filename;
	P.init(filename);

	if (P.parseProg())
		cout << endl << "Success!" << endl;

	cout << endl << DASHES << endl << endl;
	cout << P.getNumLines() << " Line(s), " << P.getNumErrors() << " Error(s)" << endl << endl;

	system("PAUSE");

	return 0;

}
