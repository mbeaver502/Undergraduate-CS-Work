#ifndef CONSTANTS_H
#define CONSTANTS_H

#include <iostream>

/*
	Set to true to see console output from scanner et al.
	Set to false to hide console output from scanner et al.
*/
const bool DEBUG_MODE = true;

//--------------------------------------------------------------------------

const int NAME_LEN = 16;

struct Record {

	int key;
	char name[NAME_LEN + 1];
	int code;

};

//--------------------------------------------------------------------------

const int NUM_ALPHA = 26;
const int NUM_DIGITS = 10;
const int NUM_KEYWORDS = 21;
const int NUM_DELIMITERS = 9;
const int ST_ENTRIES = 75;
const std::string DASHES = "-----------------------------------------";

//--------------------------------------------------------------------------

const char ALPHA[NUM_ALPHA] = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I',
								'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S',
								'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
const char DIGITS[NUM_DIGITS] = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
const char DELIMITERS[NUM_DELIMITERS] = { ' ', ';', ':', ',', '+', '-', '*', '(', ')' };

const char SPACE		= ' ';
const char UNDERSCORE	= '_';

//--------------------------------------------------------------------------

const std::string KEY_PROG		= "PROGRAM";
const std::string KEY_VAR		= "VAR";
const std::string KEY_BEGIN		= "BEGIN";
const std::string KEY_END		= "END";
const std::string KEY_ENDP		= "END.";
const std::string KEY_INT		= "INTEGER";
const std::string KEY_FOR		= "FOR";
const std::string KEY_READ		= "READ";
const std::string KEY_WRITE		= "WRITE";
const std::string KEY_TO		= "TO";
const std::string KEY_DO		= "DO";
const char DLM_SEMI				= ';';
const char DLM_COLON			= ':';
const char DLM_COMMA			= ',';
const std::string DLM_ASSIGN	= ":=";
const char OP_PLUS				= '+';
const char OP_MINUS				= '-';
const char OP_MULT				= '*';
const std::string OP_DIV		= "DIV";
const char DLM_LPAR				= '(';
const char DLM_RPAR				= ')';

//--------------------------------------------------------------------------

const int TOKEN_INVALID = -2;
const int TOKEN_UNKNOWN	= -1;
const int KEYI_PROG		= 1;
const int KEYI_VAR		= 2;
const int KEYI_BEGIN	= 3;
const int KEYI_END		= 4;
const int KEYI_ENDP		= 5;
const int KEYI_INT		= 6;
const int KEYI_FOR		= 7;
const int KEYI_READ		= 8;
const int KEYI_WRITE	= 9;
const int KEYI_TO		= 10;
const int KEYI_DO		= 11;
const int DLMI_SEMI		= 12;
const int DLMI_COLON	= 13;
const int DLMI_COMMA	= 14;
const int DLMI_ASSIGN	= 15;
const int OPI_PLUS		= 16;
const int OPI_MINUS		= 17;
const int OPI_MULT		= 18;
const int OPI_DIV		= 19;
const int DLMI_LPAR		= 20;
const int DLMI_RPAR		= 21;
const int TOKEN_ID		= 22;
const int TOKEN_INT		= 23;

//--------------------------------------------------------------------------

// Out of sheer laziness...
const Record KEYWORDS[NUM_KEYWORDS] = { 
								{ 0, "PROGRAM", KEYI_PROG }, {0, "VAR", KEYI_VAR },
								{ 0, "BEGIN", KEYI_BEGIN }, {0, "END", KEYI_END },
								{ 0, "END.", KEYI_ENDP }, { 0, "INTEGER", KEYI_INT },
								{ 0, "FOR", KEYI_FOR }, { 0, "READ", KEYI_READ },
								{ 0, "WRITE", KEYI_WRITE }, { 0, "TO", KEYI_TO },
								{ 0, "DO", KEYI_DO }, { 0, ";", DLMI_SEMI },
								{ 0, ":", DLMI_COLON }, { 0, ",", DLMI_COMMA },
								{ 0, ":=", DLMI_ASSIGN }, { 0, "+", OPI_PLUS },
								{ 0, "-", OPI_MINUS }, { 0, "*", OPI_MULT },
								{ 0, "DIV", OPI_DIV }, { 0, "(", DLMI_LPAR },
								{ 0, ")", DLMI_RPAR } 
								};

#endif
