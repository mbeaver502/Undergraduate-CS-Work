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

//--------------------------------------------------------------------------

/*
Function: Parser
Purpose: Default constructor.
Parameters: N/A
Return: N/A
*/
Parser::Parser() {

	ST = SymbolTable(ST_ENTRIES);
	numErrors = 0;
	numLine = 0;
	numLines = 0;
	ptr = 0;
	tc = 0;

}

//--------------------------------------------------------------------------

/*
Function: init
Purpose: Initializes the parser, including initializing the symbol table.
Parameters: filename is the source file
Return: N/A (void)
*/
void Parser::init(const string & filename) {

	ifstream file;
	string line;
	bool commentFlag = false;
	int len;

	// Prepopulate the Symbol Table
	initST();
	
	file.open(filename.c_str());

	if (file.is_open()) {

		string temp;

		while (getline(file, line)) {

			toUpper(line);
			buffer = line;
			preProcess(buffer, commentFlag);
			len = buffer.length();

			if (!commentFlag && len > 0) {

				numLines++;
				cout << numLines << "\t" << line << endl;

				// Building the buffer to be scanned and parsed
				temp += buffer;
				if (buffer != KEY_ENDP)
					temp += NEWLINE;

			}

		}

		file.close();

		cout << endl;
		buffer = temp;

	}

}

//--------------------------------------------------------------------------

/*
Function: getNumErrors
Purpose: Returns the number of errors encountered.
Parameters: N/A
Return: Number of errors encountered
*/
int Parser::getNumErrors() const {

	return numErrors;

}

//--------------------------------------------------------------------------

/*
Function: getNumLines
Purpose: Returns the number of lines in the source.
Parameters: N/A
Return: Number of lines in the source
*/
int Parser::getNumLines() const {

	return numLines;

}

//--------------------------------------------------------------------------

/*
Function: advance
Purpose: Moves the pointer forward (i.e., scans forward) in the buffer.
Parameters: N/A
Return: N/A (void)
*/
void Parser::advance() {

	// Scan to the next token in the buffer
	tc = SC.scan(buffer, ptr, numErrors, &ST);
	while (tc == 0)
		tc = SC.scan(buffer, ptr, numErrors, &ST);

	if (tc == TOKEN_NEWLINE) {
		numLine++;
		advance();
	}

	else if (tc == TOKEN_INVALID || tc == TOKEN_UNKNOWN) {
		cout << "Line " << numLine << ": Invalid or unknown token" << endl;
		advance();
	}

}

//--------------------------------------------------------------------------

/*
Function: parseProg
Purpose: Implements the <prog> rule.
Parameters: N/A
Return: True on success, false otherwise.
*/
bool Parser::parseProg() {

	bool result = false;

	advance();
	numLine++;

	if (tc != KEYI_PROG) {
		cout << "Line " << numLine << ": Missing " << KEY_PROG << endl;
		numErrors++;
	}

	advance();

	if (!parseProgName()) {
		cout << "Line " << numLine << ": Invalid program name" << endl;
		numErrors++;
	}

	else
		cout << "Line " << numLine << ": Okay" << endl;

	advance();

	if (tc != KEYI_VAR) {
		cout << "Line " << numLine << ": Missing " << KEY_VAR << endl;
		numErrors++;
	}

	else
		cout << "Line " << numLine << ": Okay" << endl;

	advance();

	if (!parseDecList()) {

		while (tc != KEYI_BEGIN && tc != KEYI_ENDP)
			advance();

	}

	if (tc != KEYI_BEGIN) {
		cout << "Line " << numLine << ": Missing " << KEY_BEGIN << endl;
		numErrors++;
	}

	else
		cout << "Line " << numLine << ": Okay" << endl;

	advance();

	parseStmtList();

	if (tc != KEYI_ENDP) {
		cout << "Line " << numLine << ": Missing " << KEY_ENDP << endl;
		numErrors++;
	}

	else
		cout << "Line " << numLine << ": Okay" << endl;

	if (numErrors == 0)
		result = true;

	return result;

}

//--------------------------------------------------------------------------

/*
Function: parseProgName
Purpose: Implements the <prog-name> rule.
Parameters: N/A
Return: True on success, false otherwise
*/
bool Parser::parseProgName() {

	bool result = false;

	if (tc == TOKEN_ID)
		result = true;

	return result;

}

//--------------------------------------------------------------------------

/*
Function: parseDecList
Purpose: Implements the <dec-list> rule.
Parameters: N/A
Return: True on success, false otherwise
*/
bool Parser::parseDecList() {

	bool result = false;
	int nDec = 0;
	int nSemi = 0;

	// Parse the declarations
	while (parseDec()) {

		cout << "Line " << numLine << ": Okay" << endl;

		nDec++;
		advance();

		if (tc == DLMI_SEMI) {

			nSemi++;
			advance();

		}

	}

	// We should expect 1 fewer semicolons than declarations
	if (nSemi == nDec - 1)
		result = true;

	else {

		cout << "Line " << numLine << ": Invalid declaration(s)" << endl;
		numErrors++;

	}

	return result;

}

//--------------------------------------------------------------------------

/*
Function: parseDec
Purpose: Implements the <dec> rule.
Parameters: N/A
Return: True on success, false otherwise
*/
bool Parser::parseDec() {

	bool result = false;

	if (parseIdList()) {

		if (tc == DLMI_COLON) {

			advance();

			if (parseType())
				result = true;
	
		}

	}

	return result;

}

//--------------------------------------------------------------------------

/*
Function: parseType
Purpose: Implements the <type> rule.
Parameters: N/A
Return: True on success, false otherwise
*/
bool Parser::parseType() {

	bool result = false;

	if (tc == KEYI_INT)
		result = true;

	return result;

}

//--------------------------------------------------------------------------

/*
Function: parseIdList
Purpose: Implements the <id-list> rule.
Parameters: N/A
Return: True on success, false otherwise
*/
bool Parser::parseIdList() {

	bool result = false;
	int nId = 0;
	int nComma = 0;

	// Try to find all the identifiers and commas
	while (tc == TOKEN_ID) {

		nId++;
		advance();

		if (tc == DLMI_COMMA) {

			nComma++;
			advance();

		}

	}

	// We should expect 1 fewer commas than identifiers
	if (nComma == nId - 1)
		result = true;

	return result;

}

//--------------------------------------------------------------------------

/*
Function: parseStmtList
Purpose: Implements the <stmt-list> rule.
Parameters: N/A
Return: True on success, false otherwise
*/
bool Parser::parseStmtList() {

	bool result = false;
	int nStmt = 0;
	int nSemi = 0;

	// Process all statements
	while (parseStmt()) {

		nStmt++;
		if (tc == DLMI_SEMI)
			nSemi++;

		cout << "Line " << numLine << ": Okay" << endl;

		advance();

	}

	// Stipulation: ALL statements end with semicolon
	if (nSemi == nStmt && nStmt > 0)
		result = true;

	return result;

}

//--------------------------------------------------------------------------

/*
Function: parseStmt
Purpose: Implements the <stmt> rule.
Parameters: N/A
Return: True on success, false otherwise
*/
bool Parser::parseStmt() {

	bool result = false;

	if (parseAssign())
		result = true;

	else if (parseFor())
		result = true;

	else if (parseRead())
		result = true;

	else if (parseWrite())
		result = true;

	if (!result && tc != KEYI_ENDP && tc != KEYI_END) {

		cout << "Line " << numLine << ": Invalid statement(s)" << endl;
		numErrors++;

	}

	return result;

}

//--------------------------------------------------------------------------

/*
Function: parseAssign
Purpose: Implements the <assign> rule.
Parameters: N/A
Return: True on success, false otherwise
*/
bool Parser::parseAssign() {

	bool result = false;

	if (tc == TOKEN_ID) {

		advance();

		if (tc == DLMI_ASSIGN) {

			advance();

			if (parseExp())
				result = true;

		}

	}

	return result;

}

//--------------------------------------------------------------------------

/*
Function: parseExp
Purpose: Implements the <exp> rule.
Parameters: N/A
Return: True on success, false otherwise
*/
bool Parser::parseExp() {

	bool result = false;
	int nTerm = 0;
	int nOp = 0;

	// Process all terms
	while (parseTerm()) {

		nTerm++;

		if (tc == OPI_PLUS || tc == OPI_MINUS) {

			nOp++;
			advance();

		}

	}

	// We should expect term-1 operators
	if (nOp == nTerm - 1)
		result = true;

	return result;

}

//--------------------------------------------------------------------------

/*
Function: parseTerm
Purpose: Implements the <term> rule.
Parameters: N/A
Return: True on success, false otherwise
*/
bool Parser::parseTerm() {

	bool result = false;
	int nFactor = 0;
	int nOp = 0;

	// Process all factors
	while (parseFactor()) {

		nFactor++;
		advance();

		if (tc == OPI_MULT || tc == OPI_DIV) {

			nOp++;
			advance();

		}

	}

	// We should expect factor-1 operators
	if (nOp == nFactor - 1)
		result = true;

	return result;

}

//--------------------------------------------------------------------------

/*
Function: parseFactor
Purpose: Implements the <factor> rule.
Parameters: N/A
Return: True on success, false otherwise
*/
bool Parser::parseFactor() {

	bool result = false;

	if (tc == TOKEN_ID || tc == TOKEN_INT)
		result = true;

	// ( <exp> )
	else if (tc == DLMI_LPAR) {

		advance();

		if (parseExp()) {

			if (tc == DLMI_RPAR)
				result = true;

		}

	}

	return result;

}

//--------------------------------------------------------------------------

/*
Function: parseRead
Purpose: Implements the <read> rule.
Parameters: N/A
Return: True on success, false otherwise
*/
bool Parser::parseRead() {

	bool result = false;

	// read(id, ...)
	if (tc == KEYI_READ) {

		advance();

		if (tc == DLMI_LPAR) {

			advance();

			if (parseIdList()) {

				if (tc == DLMI_RPAR) {

					advance();
					result = true;

				}

			}

		}

	}

	return result;

}

//--------------------------------------------------------------------------

/*
Function: parseWrite
Purpose: Implements the <write> rule.
Parameters: N/A
Return: True on success, false otherwise
*/
bool Parser::parseWrite() {

	bool result = false;

	// write(id, ...)
	if (tc == KEYI_WRITE) {

		advance();

		if (tc == DLMI_LPAR) {

			advance();

			if (parseIdList()) {

				if (tc == DLMI_RPAR) {

					advance();
					result = true;

				}

			}

		}

	}

	return result;

}

//--------------------------------------------------------------------------

/*
Function: parseFor
Purpose: Implements the <for> rule.
Parameters: N/A
Return: True on success, false otherwise
*/
bool Parser::parseFor() {

	bool result = false;

	if (tc == KEYI_FOR) {

		advance();

		if (parseIndexExp()) {

			if (tc == KEYI_DO) {

				cout << "Line " << numLine << ": Okay" << endl;
				advance();

				if (parseBody())
					result = true;

			}

		}

	}

	return result;

}

//--------------------------------------------------------------------------

/*
Function: parseIndexExp
Purpose: Implements the <index-exp> rule.
Parameters: N/A
Return: True on success, false otherwise
*/
bool Parser::parseIndexExp() {

	bool result = false;

	if (tc == TOKEN_ID) {

		advance();

		if (tc == DLMI_ASSIGN) {

			advance();

			if (parseExp()) {

				if (tc == KEYI_TO) {

					advance();

					if (parseExp())
						result = true;

				}

			}

		}

	}

	return result;

}

//--------------------------------------------------------------------------

/*
Function: parseBody
Purpose: Implements the <body> rule.
Parameters: N/A
Return: True on success, false otherwise
*/
bool Parser::parseBody() {

	bool result = false;

	if (tc == KEYI_BEGIN) {

		cout << "Line " << numLine << ": Okay" << endl;
		advance();

		if (parseStmtList()) {

			if (tc == KEYI_END) {

				advance();
				result = true;

			}

		}

	}

	else if (parseStmt())
		result = true;

	return result;

}

//--------------------------------------------------------------------------

/*
Function: toUpper
Purpose: This function converts a string to all uppercase characters.
Parameters: str is a reference to a string to be capitalized
Return: N/A (void)
*/
void Parser::toUpper(string & str) {

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
void Parser::trimSpaces(string & str) {

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

		}
		else {

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
void Parser::preProcess(string & str, bool & commentFlag) {

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
void Parser::detectComment(string & str, bool & commentFlag) {

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
void Parser::initST() {

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
void Parser::printST() {

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
