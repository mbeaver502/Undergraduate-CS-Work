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
#include <string>
#include "Scanner.h"
#include "SymbolTable.h"
#include "Constants.h"

using namespace std;

//--------------------------------------------------------------------------

/*
Function: isAlpha
Purpose: This function determines if a character is in [A..Z].
Parameters: c is the character to be tested
Return: true if in [A..Z], false otherwise
*/
bool Scanner::isAlpha(char c) const {

	bool result = false;

	for (int i = 0; i < NUM_ALPHA; i++) {
		if (c == ALPHA[i]) {
			result = true;
			break;
		}
	}

	return result;

}

//--------------------------------------------------------------------------

/*
Function: isDigit
Purpose: This function determines whether a character is in [0..9]
Parameters: c is the character to be tested
Return: true if in [0..9], false otherwise
*/
bool Scanner::isDigit(char c) const {

	bool result = false;

	for (int i = 0; i < NUM_DIGITS; i++) {
		if (c == DIGITS[i]) {
			result = true;
			break;
		}
	}

	return result;

}

//--------------------------------------------------------------------------

/*
Function: isDelimiter
Purpose: This function determines whether a character is a delimiter.
Parameters: c is the character to be tested
Return: true if a delimiter, false otherwise
*/
bool Scanner::isDelimiter(char c) const {

    for (int i = 0; i < NUM_DELIMITERS; i++)
        if (c == DELIMITERS[i])
            return true;

    return false;

}

//--------------------------------------------------------------------------

/*
Function: scan
Purpose: This is the main Scanner function that is used to recognize tokens.
Parameters: s is a reference to the string to be scanned, ptr is a reference to
			the lookahead buffer's current pointer location, numErrors is used
			in reporting the number of errors, ST is a pointer to the Symbol Table
Return: The token's code
*/
int Scanner::scan(const string & s, int & ptr, int & numErrors, SymbolTable * ST) const {

	int delta = 0;
	int code = 0;
	int len;
	char name[NAME_LEN + 1];
	string temp;
	Record entry;

	if ((code = recognizeKeyword(s, ptr, delta)) > 0) {
		// additional processing
	}

	else if ((code = recognizeOperator(s, ptr, delta)) > 0) {
		// additional processing
	}

	else if (recognizeIdentifier(s, ptr, delta)) {

		// Invalid identifiers have length > NAME_LEN or start with UNDERSCORE
		if ((delta > 0 && delta <= NAME_LEN) && (s[ptr] != UNDERSCORE))
			code = TOKEN_ID;

		else {
			code = TOKEN_INVALID;
			numErrors++;
		}
				
	}

	else if ((code = recognizeDelimiter(s, ptr, delta)) > 0) {
		// additional processing
	}

	else if (recognizeInteger(s, ptr, delta)) {
		code = TOKEN_INT;
	}
		
	//Unrecognized token
	else {

		if (s[ptr] == NEWLINE)
			code = TOKEN_NEWLINE;
		
		else if (s[ptr] != SPACE) {
			code = TOKEN_UNKNOWN;
			numErrors++;
		}

		delta = 1;
				
	}

	temp = s.substr(ptr, delta);
	len = temp.length();

	// Try to insert the token into the Symbol Table
	if ((temp != " ") && (code != TOKEN_UNKNOWN) && (code != TOKEN_INVALID)) {
		
		// We could use strcpy, but here we are guaranteed extra null bytes
		for (int i = 0; i <= NAME_LEN; i++) {
			if (i < len)
				name[i] = temp[i];
			else
				name[i] = '\0';
		}
		
		// Avoiding duplicates
		if (ST->Search(name) < 0) {

            entry.key = 0;
            for (int i = 0; i <= NAME_LEN; i++)
                (entry.name)[i] = name[i];
            entry.code = code;
            
			if (ST->Insert(entry) < 0) {
				//error
			}
		}

	}

	// Debug data
	//if ((DEBUG_MODE) && (temp != " ")) {
	//	cout << '\t' << code << '\t' << temp;
	//	if (code == TOKEN_UNKNOWN)
	//		cout << "\t<-- UNRECOGNIZED TOKEN";
	//	else if (code == TOKEN_INVALID)
	//		cout << "\t<-- INVALID IDENTIFIER";
	//	cout << endl;
	//}
	
	// Move the lookahead buffer's pointer forward
	ptr += delta;

    return code;

}

//--------------------------------------------------------------------------

/*
Function: recognizeKeyword
Purpose: This function recognizes keywords.
Parameters: str is a reference to the string to be scanned, ptrPos is the starting
			pointer position for the buffer, delta is a reference to the length
			of the keyword (i.e., distance traveled)
Return: Keyword token code or 0
*/
int Scanner::recognizeKeyword(const string & str, int ptrPos, int & delta) const {

	int cPtr = ptrPos;
	int lPtr = ptrPos;
	int result = 0;

	delta = 0;

	/* 
		This could be written as a loop iterating over a list of keywords,
		but that would have a runtime of O(N), where N is the number of
		keywords. This has a (relatively) better runtime and is extensible. 
	*/
	switch (str[cPtr]) {

		case 'B': {
			if ((delta = match(str, KEY_BEGIN, cPtr, lPtr)) > 0)
				result = KEYI_BEGIN;
			break;
		}

		case 'D': {
			if ((delta = match(str, KEY_DO, cPtr, lPtr)) > 0)
				result = KEYI_DO;
			break;
		}
		
		case 'E': {
             if (str[cPtr + 1] == 'N' && str[cPtr + 2] == 'D') {
                if (str[cPtr + 3] == '.') {
                     delta = 4;
                     result = KEYI_ENDP;
                }
                else {
                     delta = 3;
                     result = KEYI_END;
                }
             }
             break;
        }

		case 'F': {
			if ((delta = match(str, KEY_FOR, cPtr, lPtr)) > 0)
				result = KEYI_FOR;
			break;
		}

		case 'I': {
			if ((delta = match(str, KEY_INT, cPtr, lPtr)) > 0)
				result = KEYI_INT;
			break;
		}

		case 'P': {
			if ((delta = match(str, KEY_PROG, cPtr, lPtr)) > 0)
				result = KEYI_PROG;
			break;
		}

		case 'R': {
			if ((delta = match(str, KEY_READ, cPtr, lPtr)) > 0)
				result = KEYI_READ;
			break;
		}

		case 'T': {
			if ((delta = match(str, KEY_TO, cPtr, lPtr)) > 0)
				result = KEYI_TO;
			break;
		}

		case 'V': {
			if ((delta = match(str, KEY_VAR, cPtr, lPtr)) > 0)
				result = KEYI_VAR;
			break;
		}

		case 'W': {
			if ((delta = match(str, KEY_WRITE, cPtr, lPtr)) > 0)
				result = KEYI_WRITE;
			break;
		}

		default:
			result = 0;

	}

	return result;

}

//--------------------------------------------------------------------------

/*
Function: recognizeOperator
Purpose: This function recognizes operators.
Parameters: str is a reference to the string to be scanned, ptrPos is the starting
			pointer position for the buffer, delta is a reference to the length
			of the operator (i.e., distance traveled)
Return: Operator token code or 0
*/
int Scanner::recognizeOperator(const string & str, int ptrPos, int & delta) const {

	int cPtr = ptrPos;
	int lPtr = ptrPos;
	char c = str[cPtr];
	int result = 0;

	delta = 0;

	if (c == OP_PLUS) {
		delta = 1;
		result = OPI_PLUS;
	}

	else if (c == OP_MINUS) {
		delta = 1;
		result = OPI_MINUS;
	}

	else if (c == OP_MULT) {
		delta = 1;
		result = OPI_MULT;
	}

	else if (c == 'D') {
		if ((delta = match(str, OP_DIV, cPtr, lPtr)) > 0)
			result = OPI_DIV;
	}

	return result;

}

//--------------------------------------------------------------------------

/*
Function: recognizeIdentifier
Purpose: This function recognizes identifiers.
Parameters: str is a reference to the string to be scanned, ptrPos is the starting
			pointer position for the buffer, delta is a reference to the length
			of the identifier (i.e., distance traveled)
Return: true if valid identifier, false otherwise
*/
bool Scanner::recognizeIdentifier(const string & str, int ptrPos, int & delta) const {

	int cPtr = ptrPos;
	int lPtr = ptrPos;
	char c;
	bool result = false;

	delta = 0;
	c = str[cPtr];

	// We want to catch identifiers starting with underscore to report as an error
	if (isAlpha(c) || c == UNDERSCORE) {

	    // Collect all the characters, digits, and underscores
		while ((isAlpha(c)) || (isDigit(c)) || (c == UNDERSCORE)) {
			lPtr++;

			try {
				c = str[lPtr];
			}

			catch (exception&) {
				delta = 0;
				result = false;
				break;
			}
		}

		delta = lPtr - cPtr;
		result = true;

	}

	return result;

}

//--------------------------------------------------------------------------

/*
Function: recognizeDelimiter
Purpose: This function recognizes delimiters.
Parameters: str is a reference to the string to be scanned, ptrPos is the starting
			pointer position for the buffer, delta is a reference to the length
			of the delimiter (i.e., distance traveled)
Return: Delimiter token code or 0
*/
int Scanner::recognizeDelimiter(const string & str, int ptrPos, int & delta) const {

	int cPtr = ptrPos;
	int lPtr = ptrPos;
	char c = str[cPtr];
	int result = 0;

	delta = 0;

	if (c == DLM_SEMI) {
		delta = 1;
		result = DLMI_SEMI;
	}

	else if (c == DLM_COMMA) {
		delta = 1;
		result = DLMI_COMMA;
	}

	else if (c == DLM_LPAR) {
		delta = 1;
		result = DLMI_LPAR;
	}

	else if (c == DLM_RPAR) {
		delta = 1;
		result = DLMI_RPAR;
	}

	else if (c == DLM_COLON) {
		if ((delta = match(str, DLM_ASSIGN, cPtr, lPtr)) > 0)
			result = DLMI_ASSIGN;
		else if ((delta = match(str, string(1, DLM_COLON), cPtr, lPtr)) > 0)
			result = DLMI_COLON;
	}

	return result;

}

//--------------------------------------------------------------------------

/*
Function: recognizeInteger
Purpose: This function recognizes integers.
Parameters: str is a reference to the string to be scanned, ptrPos is the starting
			pointer position for the buffer, delta is a reference to the length
			of the integer (i.e., distance traveled)
Return: true if integer, false otherwise
*/
bool Scanner::recognizeInteger(const string & str, int ptrPos, int & delta) const {

	int cPtr = ptrPos;
	int lPtr = ptrPos;
	char c;
	bool result = false;

	c = str[cPtr];

    // Collect all the digits in the integer
	if (isDigit(c)) {

		while (isDigit(c)) {
			lPtr++;
			c = str[lPtr];
		}

		delta = lPtr - cPtr;
		result = true;

	}

	return result;

}

//--------------------------------------------------------------------------

/*
Function: match
Purpose: This function attempts to match a token within the buffer.
Parameters: str is a reference to the string to be scanned, token is a reference 
			to the string (i.e., token) to match, cPtr is the starting point,
			lPtr is the lookahead pointer (probably can be removed from params)
Return: Token code or 0
*/
int Scanner::match(const string & str, const string & token, int cPtr, int lPtr) const {

	unsigned int countMatch = 0;
	unsigned int len = token.length();
	char c = str[cPtr];
	int result = 0;

	// Go through the buffer and try to match the token within the buffer
	for (unsigned int i = 0; i < len; i++) {

		if (c == token[i])
			countMatch++;
		else
			break;

		lPtr++;

		try {
			c = str[lPtr];
		}

		catch (exception&) {
			result = 0;
			countMatch = 0;
			break;
		}

	}

	// Token found
	if ((countMatch > 0) && (countMatch == len) && 
        (isDelimiter(c) || c == '\0' || c == '\n'))
		result = countMatch;
	
	return result;

}
