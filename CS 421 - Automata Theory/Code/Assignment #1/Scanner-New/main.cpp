#include <iostream>
#include <fstream>
#include <string>

#include "Constants.h"
#include "SymbolTable.h"

using namespace std;




// TO DO: Migrate to Scanner class
// TO DO: Comments





//--------------------------------------------------------------------------

void toUpper(string & str);
void trimSpaces(string & str);
void preProcess(string & str, bool & commentFlag);
void detectComment(string & str, bool & commentFlag);

bool isAlpha(char c);
bool isDigit(char c);
bool isDelimiter(char c);

int scan(const string & s, int & ptr, int & numErrors);
int recognizeKeyword(const string & str, int ptrPos, int & delta);
int recognizeOperator(const string & str, int ptrPos, int & delta);
bool recognizeIdentifier(const string & str, int ptrPos, int & delta);
int recognizeDelimiter(const string & str, int ptrPos, int & delta);
bool recognizeInteger(const string & str, int ptrPos, int & delta);
int match(const string & str, const string & token, int cPtr, int lPtr);

void initST();
void printST();

//--------------------------------------------------------------------------

SymbolTable ST(ST_ENTRIES);

//--------------------------------------------------------------------------

int main() {

	ifstream file("prog2.txt");
	string line, buffer;
	int numLines = 0;
	int numErrors = 0;
	bool commentFlag = false;



	int ptr;
	int len;

	initST();


	if (file.is_open()) {

		while (getline(file, line)) {

			toUpper(line);
			buffer = line;
			
			preProcess(buffer, commentFlag);
			len = buffer.length();

			if (!commentFlag && len > 0) {

				numLines++;
				cout << numLines << "\t" << line << endl;


				//**************************************************************


				ptr = 0;
				while (ptr < len)
					scan(buffer, ptr, numErrors);


				//**************************************************************


				if (DEBUG_MODE)
					cout << endl;

			}

		}

		file.close();

	}
	
	cout << DASHES << endl << endl;
	cout << numLines << " Line(s)" << endl << numErrors << " Error(s)" << endl << endl;

	if (DEBUG_MODE)
		printST();
	
	getchar();
	
	return 0;

}

//--------------------------------------------------------------------------

void toUpper(string & str) {

	int len = str.length();

	for (int i = 0; i < len; i++)
		str[i] = char(toupper(str[i]));

}

//--------------------------------------------------------------------------

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

void preProcess(string & str, bool & commentFlag) {

	trimSpaces(str);
	detectComment(str, commentFlag);

}

//--------------------------------------------------------------------------

void detectComment(string & str, bool & commentFlag) {

	int len = str.length();

	// A bit hacky, but it works
	if ((str[0] == '(') && (str[1] == '*'))
		commentFlag = true;

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

bool isAlpha(char c) {

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

bool isDigit(char c){

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

int scan(const string & s, int & ptr, int & numErrors) {

	int delta = 0;
	int code = 0;
	int len;
	char name[NAME_LEN + 1];
	string temp;
	Record entry;

	if ((code = recognizeKeyword(s, ptr, delta)) > 0) {
		//
	}

	else if ((code = recognizeOperator(s, ptr, delta)) > 0) {
		//
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
		//
	}

	else if (recognizeInteger(s, ptr, delta)) {
		code = TOKEN_INT;
	}
		
	//Unrecognized token
	else {

		if (s[ptr] != SPACE) {
			code = TOKEN_UNKNOWN;
			numErrors++;
		}
				
		delta = 1;

	}

	temp = s.substr(ptr, delta);
	len = temp.length();

	if ((temp != " ") && (code != TOKEN_UNKNOWN) && (code != TOKEN_INVALID)) {
		
		// We could use strcpy, but here we are guaranteed extra null bytes
		for (int i = 0; i <= NAME_LEN; i++) {
			if (i < len)
				name[i] = temp[i];
			else
				name[i] = '\0';
		}

		// Avoiding duplicates
		if (ST.Search(name) < 0) {

			//entry = { 0, "", code };
            entry.key = 0;
            for (int i = 0; i <= NAME_LEN; i++)
                (entry.name)[i] = name[i];
            entry.code = code;
			//strcpy_s(entry.name, name);

			if (ST.Insert(entry) < 0) {

				//error

			}

		}

	}
	
	if ((DEBUG_MODE) && (temp != " ")) {
		cout << '\t' << code << '\t' << temp;
		if (code == TOKEN_UNKNOWN)
			cout << "\t<-- UNRECOGNIZED TOKEN";
		else if (code == TOKEN_INVALID)
			cout << "\t<-- INVALID IDENTIFIER";
		cout << endl;
	}
		
	ptr += delta;

    return code;

}









int recognizeKeyword(const string & str, int ptrPos, int & delta) {

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
			if ((delta = match(str, KEY_ENDP, cPtr, lPtr)) > 0)
				result = KEYI_ENDP;
			else if ((delta = match(str, KEY_END, cPtr, lPtr)) > 0)
				result = KEYI_END;
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

int recognizeOperator(const string & str, int ptrPos, int & delta) {

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

bool recognizeIdentifier(const string & str, int ptrPos, int & delta) {

	int cPtr = ptrPos;
	int lPtr = ptrPos;
	char c;
	bool result = false;

	delta = 0;
	c = str[cPtr];

	if (isAlpha(c) || c == UNDERSCORE) {

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

int recognizeDelimiter(const string & str, int ptrPos, int & delta) {

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

bool recognizeInteger(const string & str, int ptrPos, int & delta) {

	int cPtr = ptrPos;
	int lPtr = ptrPos;
	char c;
	bool result = false;

	c = str[cPtr];

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



int match(const string & str, const string & token, int cPtr, int lPtr) {

	unsigned int countMatch = 0;
	unsigned int len = token.length();
	char c = str[cPtr];
	int result = 0;

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

	if ((countMatch > 0) && (countMatch == len) && 
        (isDelimiter(c) || c == '\0' || c == '\n'))
		result = countMatch;
	
	return result;

}




void initST() {

	for (int i = 0; i < NUM_KEYWORDS; i++)
		ST.Insert(KEYWORDS[i]);

}

void printST() {

	int code;

	cout << DASHES << endl;
	cout << "          SYMBOL TABLE CONTENTS          " << endl;
	cout << DASHES << endl;

	for (int i = 0; i < ST.getTableSize(); i++) {

		cout << i + 1 << "\t" << ST[i] << "\t";
		code = ST.getRecordCode(ST[i]);

		if (code >= 1)
			cout << code;

		cout << endl;

	}

}


bool isDelimiter(char c) {

    for (int i = 0; i < NUM_DELIMITERS; i++)
        if (c == DELIMITERS[i])
            return true;

    return false;

}
