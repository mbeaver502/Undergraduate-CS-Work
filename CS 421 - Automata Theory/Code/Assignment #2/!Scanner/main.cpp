/*
Michael Beaver
CS 421 - Automata Theory & Compiler Construction
Assignment #1 - MINI-P Scanner
20 March 2015
Program Description: 
	This program implements a Scanner for a MINI-P compiler. This program emulates
	the basic function of a Parser as it would continuously prompt the Scanner to
	scan tokens in the buffer. This implementation assumes that a source file is
	being read line-by-line. That is, the lookahead buffer is assumed to be a 
	single line rather than a continuous stream of bytes. This could easily be
	adjusted if necessary.
*/

#include <iostream>
#include <fstream>
#include <string>

#include "Constants.h"
#include "SymbolTable.h"
#include "Scanner.h"

using namespace std;

//--------------------------------------------------------------------------
//								PROTOTYPES
//--------------------------------------------------------------------------

void toUpper(string & str);
void trimSpaces(string & str);
void preProcess(string & str, bool & commentFlag);
void detectComment(string & str, bool & commentFlag);

void initST();
void printST();

//--------------------------------------------------------------------------
//							GLOBAL ("PARSER") VARS 
//--------------------------------------------------------------------------

SymbolTable ST(ST_ENTRIES);
Scanner 	SC;

//--------------------------------------------------------------------------
//								FUNCTIONS
//--------------------------------------------------------------------------

/*
Function: main
Purpose: This function emulates the operation of the Parser insofar as it calls
		 the Scanner to return scanned tokens.
Parameters: N/A
Return: 0 on termination
*/
int main() {

	ifstream file;
	string line, buffer, filename;
	int numLines = 0;
	int numErrors = 0;
	bool commentFlag = false;
	int ptr, len;

	// Prepopulate the Symbol Table
	initST();
	
	cout << "Source: ";
	cin >> filename;
	file.open(filename.c_str());

	if (file.is_open()) {

	    // "Parser" loop
        while (getline(file, line)) {
		    toUpper(line);
			buffer = line;
			preProcess(buffer, commentFlag);
			len = buffer.length();

			if (!commentFlag && len > 0) {
				numLines++;
				cout << numLines << "\t" << line << endl;

				// Scan tokens
				ptr = 0;
				while (ptr < len)
		  		    SC.scan(buffer, ptr, numErrors, &ST);

				if (DEBUG_MODE)
					cout << endl;
			}
		}

		file.close();

	}
	
	cout << DASHES << endl << endl;
	cout << numLines << " Line(s), " << numErrors << " Error(s)" << endl << endl;

	if (DEBUG_MODE)
		printST();

	system("PAUSE");
	
	return 0;

}

//--------------------------------------------------------------------------

/*
Function: toUpper
Purpose: This function converts a string to all uppercase characters.
Parameters: str is a reference to a string to be capitalized
Return: N/A (void)
*/
void toUpper(string & str) {

	int len = str.length();

	for (int i = 0; i < len; i++)
		str[i] = char(toupper(str[i]));

}

//--------------------------------------------------------------------------

/*
Function: trimSpaces
Purpose: This function replaces all series of spaces and tabs with a single space.
Parameters: str is a reference to a string whos spaces are to be trimmed
Return: N/A (void)
*/
void trimSpaces(string & str) {

	string temp;
	int idx = 0;
	int len = str.length();

	while (idx < len) {

		// Replace all sequences of spaces and tabs with a single space
		if (str[idx] == ' ' || str[idx] == '\t') {

			while ((str[idx] == ' ' || str[idx] == '\t') && (idx < len))
				idx++;

			if (idx < len)
				temp += ' ';
		
		} else {

			temp += str[idx];
			idx++;

		}

	}

	if (temp[0] == ' ')
		str = temp.substr(1, temp.length() - 1);
	else
		str = temp;

}

//--------------------------------------------------------------------------

/*
Function: preProcess
Purpose: This function uses trimSpaces and detectComment to reduce the number of
		 spaces and remove comments.
Parameters: str is a reference to the string to be processed, and commentFlag is
			a reference to a boolean flag used by the Parser to indicate a 
			comment has been encountered
Return: N/A (void)
*/
void preProcess(string & str, bool & commentFlag) {

	trimSpaces(str);
	detectComment(str, commentFlag);

}

//--------------------------------------------------------------------------

/*
Function: detectComment
Purpose: This function detects when a line (or lines) contains a comment. A bit 
		 hacky, but it works
Parameters: str is a reference to the string to be processed, commentFlag is a
			reference to a boolean flag used by the Parser
Return: N/A (void)
*/
void detectComment(string & str, bool & commentFlag) {

	int len = str.length();

	// Find the start
	if ((str[0] == '(') && (str[1] == '*'))
		commentFlag = true;

    // Find the end
	if (commentFlag && len > 1) {
		if ((str[len - 2] == '*') && (str[len - 1] == ')')) {
			commentFlag = false;
			str = "";
		}
	}

	else
		commentFlag = false;

}

//--------------------------------------------------------------------------

/*
Function: initST
Purpose: This function prepopulates the Symbol Table.
Parameters: N/A
Return: N/A (void)
*/
void initST() {

	for (int i = 0; i < NUM_KEYWORDS; i++)
		ST.Insert(KEYWORDS[i]);

}

//--------------------------------------------------------------------------

/*
Function: printST
Purpose: This function prints the contents of the Symbol Table.
Parameters: N/A
Return: N/A (void)
*/
void printST() {

	int code;
	int len = ST.getTableSize();

	cout << DASHES << endl;
	cout << "          SYMBOL TABLE CONTENTS          " << endl;
	cout << DASHES << endl;

	for (int i = 0; i < len; i++) {
		cout << i + 1 << "\t" << ST[i] << "\t";
		code = ST.getRecordCode(ST[i]);

		if (code >= 1)
			cout << code;

		cout << endl;
	}

}
